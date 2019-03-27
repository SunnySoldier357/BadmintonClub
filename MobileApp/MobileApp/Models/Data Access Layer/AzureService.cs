using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MobileApp.Models.DB;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MobileApp.Models.Data_Access_Layer
{
    public class AzureService
    {
        // Static Properties
        public static AzureService DefaultService = new AzureService();

        // Public Properties
        public MobileServiceClient Client { get; set; }

        // Private Properties
        private IMobileServiceSyncTable<BlogPost> blogPostTable;

        // Constructors
        public AzureService() =>
            Task.Run(async () => await InitialiseAsync()).Wait();

        // Public Methods
        public async Task<List<BlogPost>> GetBlogPostsAsync()
            => await blogPostTable.ToListAsync();

        public async Task InitialiseAsync()
        {
            string appUrl = Constants.AppUrl;
            Client = new MobileServiceClient(appUrl);

            string path = Path.Combine(MobileServiceClient.DefaultDatabasePath,
                Constants.LocalDBName);
            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<BlogPost>();

            await Client.SyncContext.InitializeAsync(store);

            blogPostTable = Client.GetSyncTable<BlogPost>();
        }

        public async Task SyncDataTablesAsync()
        {
            // Device is offline, skip!
            if (!CrossConnectivity.Current.IsConnected)
                return;

            try
            {
                // Device is online, continue...
                await Client.SyncContext.PushAsync();

                await blogPostTable.PullAsync("allBLogPost", blogPostTable.CreateQuery());
            }
            catch (MobileServicePushFailedException e)
            {
                // Simple error / conflict handling
                if (e.PushResult != null)
                {
                    foreach (var error in e.PushResult.Errors)
                    {
                        if (error.OperationKind == MobileServiceTableOperationKind.Update &&
                            error.Result != null)
                        {
                            // Update failed, reverting to server's copy
                            await error.CancelAndUpdateItemAsync(error.Result);
                        }
                        else
                        {
                            // Discard local changes
                            await error.CancelAndDiscardItemAsync();
                        }

                        Debug.WriteLine("Error executing sync operation, Item: {0} ({1}). Operation discarded.",
                            error.TableName, error.Item["id"]);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error!" + e.Message);
            }
        }
    }
}