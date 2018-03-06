using BadmintonClub.Models.Data_Access_Layer;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]
namespace BadmintonClub.Models.Data_Access_Layer
{
    public partial class AzureService
    {
        // Private Properties
        private IMobileServiceSyncTable<BlogPost> blogPostTable;
        private IMobileServiceSyncTable<Match> matchTable;
        private IMobileServiceSyncTable<User> userTable;

        // Public Properties
        public MobileServiceClient Client { get; private set; }

        // Public Methods
        public async Task InitialiseAsync()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            string appUrl = "https://badmintonclub.azurewebsites.net";
            Client = new MobileServiceClient(appUrl);

            string path = "badmintonclubrecords.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);
            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<BlogPost>();
            store.DefineTable<Match>();
            store.DefineTable<User>();

            await Client.SyncContext.InitializeAsync(store);

            blogPostTable = Client.GetSyncTable<BlogPost>();
            matchTable = Client.GetSyncTable<Match>();
            userTable = Client.GetSyncTable<User>();
        }

        public async Task<BlogPost> AddBlogPostAsync(string title, string bodyOfPost)
        {
            await InitialiseAsync();

            BlogPost blogpost = new BlogPost()
            {
                Title = title,
                BodyOfPost = bodyOfPost,
                UserID = (Application.Current as App).SignedInUser.Id,
                Publisher = (Application.Current as App).SignedInUser,
                DateTimePublished = DateTime.Now
            };

            await blogPostTable.InsertAsync(blogpost);
            await SyncAllDataTablesAsync();

            return blogpost;
        }

        public async Task<Match> AddMatchAsync(int playerScore, int opponentScore, string playerID, string opponentID)
        {
            await InitialiseAsync();

            Match match = new Match()
            {
                PlayerScore = playerScore,
                OpponentScore = opponentScore,
                OpponentID = opponentID,
                PlayerID = playerID
            };

            await matchTable.InsertAsync(match);

            // Updating player's statistics according to Match results
            match.Player = await userTable.LookupAsync(match.PlayerID);
            match.Player.AddMatch(match, true);

            match.Opponent = await userTable.LookupAsync(match.OpponentID);
            match.Opponent.AddMatch(match, false);

            await userTable.UpdateAsync(match.Player);
            await userTable.UpdateAsync(match.Opponent);
            await SyncAllDataTablesAsync();

            return match;
        }

        public async Task<User> AddUserAsync(string firstName, string lastName)
        {
            await InitialiseAsync();

            User user = new User(firstName, lastName, "Member", 0);

            await userTable.InsertAsync(user);
            await SyncAllDataTablesAsync();

            return user;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
        {
            await InitialiseAsync();
            await SyncAllDataTablesAsync();

            IEnumerable<BlogPost> data = await blogPostTable
                                         .OrderByDescending(bp => bp.DateTimePublished)
                                         .ToEnumerableAsync();

            return data;
        }

        public async Task<IEnumerable<Match>> GetMatchesAsync()
        {
            await InitialiseAsync();
            await SyncAllDataTablesAsync();

            var data = await matchTable.ToEnumerableAsync();

            return data;
        }

        public async Task<User> GetUserFromIdAsync(string id)
        {
            await InitialiseAsync();
            await SyncAllDataTablesAsync();

            return await userTable.LookupAsync(id);
        }

        public async Task<string> GetUserIdFromNameAsync(string name)
        {
            await InitialiseAsync();
            await SyncAllDataTablesAsync();

            var query = from user in userTable
                        where (user.FirstName + " " + user.LastName) == name
                        select user.Id;

            var result =  await userTable.ReadAsync(query);
            return result.First();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            await InitialiseAsync();
            await SyncAllDataTablesAsync();

            var data = await userTable
                .OrderByDescending(user => user.PointsInCurrentSeason)
                .ThenBy(user => user.FirstName)
                .ToEnumerableAsync();
            return data;
        }

        public async Task SyncAllDataTablesAsync()
        {
            await InitialiseAsync();

            try
            {
                // Device is offline, skip!
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                // Device is online, continue...
                await Client.SyncContext.PushAsync();
                await blogPostTable.PullAsync("allBlogPost", blogPostTable.CreateQuery());
                await matchTable.PullAsync("allMatch", matchTable.CreateQuery());
                await userTable.PullAsync("allUser", userTable.CreateQuery());

                (Application.Current as App).SignedInUser = await userTable.LookupAsync((Application.Current as App).SignedInUserId);

                await Client.SyncContext.PushAsync();
            }
            catch (MobileServicePushFailedException ex)
            {
                // Simple error/conflict handling
                if (ex.PushResult != null)
                {
                    foreach (var error in ex.PushResult.Errors)
                    {
                        if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                        {
                            // Update failed, reverting to server's copy
                            await error.CancelAndUpdateItemAsync(error.Result);
                        }
                        else
                        {
                            // Discard local changes
                            await error.CancelAndDiscardItemAsync();
                        }

                        Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.",
                            error.TableName, error.Item["id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
