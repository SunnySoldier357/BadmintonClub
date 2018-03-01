using BadmintonClub.Models.Data_Access_Layer;
using BadmintonClub.ViewModels;
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
    public class AzureService
    {
        // Private Properties
        private IMobileServiceSyncTable<BlogPost> blogPostTable;
        private IMobileServiceSyncTable<Match> matchTable;
        private IMobileServiceSyncTable<User> userTable;

        public MobileServiceClient Client = null;

        // Public Methods
        public async Task Initialise()
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

        public async Task<BlogPost> AddBlogPost(string title, string bodyOfPost)
        {
            await Initialise();

            BlogPost blogpost = new BlogPost()
            {
                Title = title,
                BodyOfPost = bodyOfPost,
                UserID = UserViewModel.SignedInUser.Id,
                DateTimePublished = DateTime.Now
            };

            await blogPostTable.InsertAsync(blogpost);
            await SyncAllDataTables();

            return blogpost;
        }

        public async Task<Match> AddMatch(int playerScore, int opponentScore, string playerID, string opponentID)
        {
            await Initialise();

            Match match = new Match()
            {
                PlayerScore = playerScore,
                OpponentScore = opponentScore,
                OpponentID = opponentID,
                PlayerID = playerID
            };

            await matchTable.InsertAsync(match);
            await SyncAllDataTables();

            return match;
        }

        public async Task<User> AddUser(string firstName, string lastName)
        {
            await Initialise();

            User user = new User(firstName, lastName, "Member", 0);

            await userTable.InsertAsync(user);
            await SyncAllDataTables();

            return user;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPosts()
        {
            await Initialise();
            await SyncAllDataTables();

            IEnumerable<BlogPost> data = await blogPostTable
                                         .OrderByDescending(bp => bp.DateTimePublished)
                                         .ToEnumerableAsync();

            return data;
        }

        public async Task<IEnumerable<Match>> GetMatches()
        {
            await Initialise();
            await SyncAllDataTables();

            IEnumerable<Match> data = await matchTable.ToEnumerableAsync();

            return data;
        }

        public async Task<User> GetUserFromId(string id)
        {
            await Initialise();
            await SyncAllDataTables();

            return await userTable.LookupAsync(id);
        }

        public async Task<string> GetUserIdFromName(string name)
        {
            await Initialise();
            await SyncAllDataTables();

            var userid = from user in userTable
                         where user.FullName == name
                         select user.Id;
            // Problem
            return userid.Query.First();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await Initialise();
            await SyncAllDataTables();

            var data = await userTable
                .OrderByDescending(user => user.PointsInCurrentSeason)
                .ThenBy(user => user.FirstName)
                .ToEnumerableAsync();
            return data;
        }

        public async Task SyncAllDataTables()
        {
            await Initialise();

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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
