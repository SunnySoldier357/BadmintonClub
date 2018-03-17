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
        // Static Properties
        public static AzureService DefaultService { get; set; } = DependencyService.Get<AzureService>();

        // Private Properties
        private IMobileServiceSyncTable<BlogPost> blogPostTable;
        private IMobileServiceSyncTable<Match> matchTable;
        private IMobileServiceSyncTable<SeasonData> seasonDataTable;
        private IMobileServiceSyncTable<User> userTable;

        // Public Properties
        public MobileServiceClient Client { get; set; }

        // Constructor
        public AzureService()
        {
            string appUrl = "https://badmintonclub.azurewebsites.net";
            Client = new MobileServiceClient(appUrl);

            string path = "badmintonclubrecords.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);
            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<BlogPost>();
            store.DefineTable<Match>();
            store.DefineTable<SeasonData>();
            store.DefineTable<User>();

            Client.SyncContext.InitializeAsync(store);

            blogPostTable = Client.GetSyncTable<BlogPost>();
            matchTable = Client.GetSyncTable<Match>();
            seasonDataTable = Client.GetSyncTable<SeasonData>();
            userTable = Client.GetSyncTable<User>();
        }

        // Public Methods
        public async Task<BlogPost> AddBlogPostAsync(string title, string bodyOfPost)
        {
            BlogPost blogpost = new BlogPost()
            {
                Title = title,
                BodyOfPost = bodyOfPost,
                UserID = (Application.Current as App).SignedInUser.Id,
                Publisher = (Application.Current as App).SignedInUser,
                DateTimePublished = DateTime.Now
            };

            await blogPostTable.InsertAsync(blogpost);

            return blogpost;
        }

        public async Task<Match> AddMatchAsync(int playerScore, int opponentScore, string playerName, string opponentName, bool isSeasonMatch)
        {
            int seasonNumber = await GetSeasonNumberAsync();

            Match match = new Match()
            {
                PlayerScore = playerScore,
                OpponentScore = opponentScore,
                OpponentID = await GetUserIdFromNameAsync(opponentName),
                PlayerID = await GetUserIdFromNameAsync(playerName),
                SeasonNumber = seasonNumber
            };

            await matchTable.InsertAsync(match);

            // Updating player's statistics according to Match results
            match.Player = await userTable.LookupAsync(match.PlayerID);
            if (isSeasonMatch)
            {
                var query = from season in seasonDataTable
                            where season.UserID == match.PlayerID
                            select season;
                match.Player.UserSeasonData = (await seasonDataTable.ReadAsync(query)).FirstOrDefault();
                match.Player.UserSeasonData.AddMatch(match, true);
            }
            match.Player.AddMatch(match, true);

            match.Opponent = await userTable.LookupAsync(match.OpponentID);
            if (isSeasonMatch)
            {
                var query = from season in seasonDataTable
                            where season.UserID == match.OpponentID
                            select season;
                match.Opponent.UserSeasonData = (await seasonDataTable.ReadAsync(query)).FirstOrDefault();

                match.Opponent.UserSeasonData.AddMatch(match, false);
            }
            match.Opponent.AddMatch(match, false);

            if (isSeasonMatch)
            {
                await seasonDataTable.UpdateAsync(match.Player.UserSeasonData);
                await seasonDataTable.UpdateAsync(match.Opponent.UserSeasonData);
            }
            await userTable.UpdateAsync(match.Player);
            await userTable.UpdateAsync(match.Opponent);

            if (match.PlayerID.Equals((Application.Current as App).SignedInUserId) ||
                match.OpponentID.Equals((Application.Current as App).SignedInUserId))
                (Application.Current as App).SignedInUser.Matches.Add(match);
            return match;
        }

        public async Task<User> AddUserAsync(string firstName, string lastName, string password, bool competitive)
        {
            User user = new User()
            {
                Title = "Member",
                FirstName = firstName,
                LastName = lastName,
                ClearanceLevel = 0,
                Password = password,
                IsCompetitive = competitive
            };

            await userTable.InsertAsync(user);

            await seasonDataTable.InsertAsync(new SeasonData(user.Id)
            {
                SeasonNumber = await GetSeasonNumberAsync()
            });

            return user;
        }

        public async Task<bool> DoesUserExistAsync(string fullName)
        {
            var query = from user in userTable
                        where (user.FirstName + " " + user.LastName) == fullName
                        select user;

            var users = await userTable.ReadAsync(query);

            return users.Count() == 1;
        }

        public async Task<List<BlogPost>> GetBlogPostsAsync()
        {
            var data = await blogPostTable.ToListAsync();

            foreach (var item in data)
            {
                var result = await userTable.LookupAsync(item.UserID);
                item.Publisher = result;
            }

            return data;
        }

        public async Task<IEnumerable<Match>> GetMatchesAsync()
        {
            await SyncAllDataTablesAsync();

            var data = await matchTable.ToEnumerableAsync();

            return data;
        }

        public async Task<List<SeasonData>> GetSeasonDataAsync()
        {
            var data = await seasonDataTable.ToListAsync();

            foreach (var item in data)
            {
                var result = await userTable.LookupAsync(item.UserID);
                item.Player = result;
            }

            return data.Where(s => s.Player.IsCompetitive).ToList();
        }

        public async Task<int> GetSeasonNumberAsync()
        {
            var matches = await matchTable.ToEnumerableAsync();

            return matches.Count() == 0 ? 1 : matches.Max(match => match.SeasonNumber);
        }

        public async Task<User> GetUserFromIdAsync(string id)
        {
            await SyncAllDataTablesAsync();
            return await userTable.LookupAsync(id);
        }

        public async Task<string> GetUserIdFromNameAsync(string name)
        {
            var query = from user in userTable
                        where (user.FirstName + " " + user.LastName) == name
                        select user.Id;

            var result = await userTable.ReadAsync(query);
            return result.First();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            await SyncAllDataTablesAsync();

            var data = await userTable
                .OrderByDescending(user => user.FirstName)
                .ToEnumerableAsync();
            return data;
        }

        public async Task<bool> LoginAsync(string fullName, string password)
        {
            var query = from user in userTable
                        where (user.FirstName + " " + user.LastName) == fullName
                                && user.Password == password
                        select user;

            var users = await userTable.ReadAsync(query);

            if (users.Count() == 1)
            {
                User first = users.First();
                (Application.Current as App).SignedInUser = first;
                (Application.Current as App).SignedInUserId = first.Id;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Syncs the current tables in SQLite to the remote Azure SQL Database.
        /// </summary>
        /// <param name="syncBools">
        /// A boolean array of specifically size 4 that is used to decide which tables are synced.
        /// The order of the booleans are BlogPost, Match, SeasonData, and User.
        /// </param>
        /// <returns></returns>
        public async Task SyncDataTablesAsync(bool[] syncBools)
        {
            try
            {
                await Client.SyncContext.PushAsync();

                if (syncBools[0])
                    await blogPostTable.PullAsync("allBlogPost", blogPostTable.CreateQuery());
                if (syncBools[1])
                    await matchTable.PullAsync("allMatch", matchTable.CreateQuery());
                if (syncBools[2])
                    await seasonDataTable.PullAsync("allSeasonData", seasonDataTable.CreateQuery());
                if (syncBools[3])
                    await userTable.PullAsync("allUser", userTable.CreateQuery());

                if ((Application.Current as App).SignedInUser == null || syncBools[1])
                {
                    (Application.Current as App).SignedInUser = await userTable.LookupAsync((Application.Current as App).SignedInUserId);

                    var query = from match in matchTable
                                where match.OpponentID == (Application.Current as App).SignedInUserId ||
                                      match.PlayerID == (Application.Current as App).SignedInUserId
                                select match;

                    (Application.Current as App).SignedInUser.Matches.Clear();
                    var data = await matchTable.ReadAsync(query);
                    foreach (var item in data)
                    {
                        item.Player = await userTable.LookupAsync(item.PlayerID);
                        item.Opponent = await userTable.LookupAsync(item.OpponentID);
                        (Application.Current as App).SignedInUser.Matches.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task SyncAllDataTablesAsync()
        {
            try
            {
                // Device is offline, skip!
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                // Device is online, continue...
                await Client.SyncContext.PushAsync();
                await blogPostTable.PullAsync("allBlogPost", blogPostTable.CreateQuery());
                await matchTable.PullAsync("allMatch", matchTable.CreateQuery());
                await seasonDataTable.PullAsync("allSeasonData", seasonDataTable.CreateQuery());
                await userTable.PullAsync("allUser", userTable.CreateQuery());

                if (!string.IsNullOrEmpty((Application.Current as App).SignedInUserId))
                {
                    (Application.Current as App).SignedInUser = await userTable.LookupAsync((Application.Current as App).SignedInUserId);

                    var query = from match in matchTable
                                where match.OpponentID == (Application.Current as App).SignedInUserId ||
                                      match.PlayerID == (Application.Current as App).SignedInUserId
                                select match;

                    (Application.Current as App).SignedInUser.Matches.Clear();
                    var data = await matchTable.ReadAsync(query);
                    foreach (var item in data)
                    {
                        item.Player = await userTable.LookupAsync(item.PlayerID);
                        item.Opponent = await userTable.LookupAsync(item.OpponentID);
                        (Application.Current as App).SignedInUser.Matches.Add(item);
                    }
                }

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
