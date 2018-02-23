﻿using BadmintonClub.Models.Data_Access_Layer;
using BadmintonClub.ViewModels;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]
namespace BadmintonClub.Models.Data_Access_Layer
{
    public class AzureService
    {
        // Private Properties
        private IMobileServiceSyncTable<BlogPost> blogPostTable;
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
            store.DefineTable<User>();

            await Client.SyncContext.InitializeAsync(store);

            blogPostTable = Client.GetSyncTable<BlogPost>();
            userTable = Client.GetSyncTable<User>();
        }

        public async Task SyncBlogPost()
        {
            await Initialise();

            try
            {
                // Device is offline, skip!
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                // Device is online
                await Client.SyncContext.PushAsync();
                await blogPostTable.PullAsync("allBlogPost", blogPostTable.CreateQuery());
                await userTable.PullAsync("allUser", userTable.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task SyncUser()
        {

        }

        public async Task<BlogPost> AddBlogPost(string title, string bodyOfPost)
        {
            await Initialise();

            BlogPost blogpost = new BlogPost(title, DateTime.Now, bodyOfPost, UserViewModel.SignedInUser);

            await blogPostTable.InsertAsync(blogpost);
            await SyncBlogPost();

            return blogpost;
        }

        public async Task<User> AddUser(string firstName, string lastName)
        {
            await Initialise();

            User user = new User(firstName, lastName, "Member", 0);

            await userTable.InsertAsync(user);
            await SyncUser();

            return user;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPosts()
        {
            await Initialise();
            await SyncBlogPost();

            var data = await blogPostTable
                .OrderByDescending(bp => bp.DateTimePublished)
                .ToEnumerableAsync();

            return data;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await Initialise();
            await SyncUser();

            var data = await userTable
                .OrderByDescending(user => user.PointsInCurrentSeason)
                .ThenBy(user => user.FirstName)
                .ToEnumerableAsync();
            return data;
        }
    }
}
