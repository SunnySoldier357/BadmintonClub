using Microsoft.WindowsAzure.MobileServices;
using Plugin.Connectivity;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BadmintonClub.Models.Data_Access_Layer
{
    public class AzureTransaction
    {
        // Private Properties
        private AzureService azureService;

        private bool addingItem;

        private Transaction[] transactions;

        // Constructor
        public AzureTransaction(params Transaction[] transactions)
        {
            azureService = AzureService.DefaultService;

            addingItem = false;

            this.transactions = transactions;
        }

        // Public Methods
        public async Task<dynamic> ExecuteAsync()
        {
            dynamic result = null;

            // Syncing necessary data tables before running transactions
            bool[] syncTables = new bool[4];
            if (CrossConnectivity.Current.IsConnected)
            {
                foreach (Transaction transaction in transactions)
                {
                    switch (transaction.transactionType)
                    {
                        case TransactionType.AddBlogPost:
                            addingItem = true;
                            break;

                        case TransactionType.GetBlogPosts:
                            syncTables[0] = true;
                            syncTables[3] = true;
                            break;

                        case TransactionType.AddMatch:
                            addingItem = true;
                            break;

                        case TransactionType.GetMatches:
                            syncTables[1] = true;
                            break;

                        case TransactionType.GetSeasonData:
                            syncTables[2] = true;
                            break;

                        case TransactionType.AddUser:
                            addingItem = true;
                            break;

                        case TransactionType.GetUsers:
                            syncTables[3] = true;
                            break;

                        default:
                            break;
                    }
                }

                await azureService.SyncDataTablesAsync(syncTables);
            }

            foreach (Transaction transaction in transactions)
            {
                Debug.WriteLine("Theer is a transaction...");
                result = await transaction.RunTransaction(azureService);
                foreach (var item in result)
                {
                    Debug.WriteLine((item as BlogPost).Publisher.ToString());
                }
            }

            if (addingItem)
                await azureService.Client.SyncContext.PushAsync();

            return result;
        }
    }
}
