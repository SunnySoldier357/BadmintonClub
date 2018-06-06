using Microsoft.WindowsAzure.MobileServices;
using Plugin.Connectivity;
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

            await azureService.InitialiseAsync();

            // Syncing necessary data tables before running transactions
            bool[] syncTables = new bool[4];
            if (CrossConnectivity.Current.IsConnected)
            {
                foreach (Transaction transaction in transactions)
                {
                    switch (transaction.TransactType)
                    {
                        case TransactionType.AddBlogPost:
                        case TransactionType.AddUser:
                            addingItem = true;
                            break;

                        case TransactionType.AddMatch:
                            addingItem = true;
                            syncTables[1] = true;
                            syncTables[2] = true;
                            syncTables[3] = true;
                            break;

                        case TransactionType.GetBlogPosts:
                            syncTables[0] = true;
                            syncTables[3] = true;
                            break;

                        case TransactionType.GetMatches:
                        case TransactionType.SyncUserMatches:
                            syncTables[1] = true;
                            break;

                        case TransactionType.GetSeasonData:
                            syncTables[2] = true;
                            syncTables[3] = true;
                            break;

                        case TransactionType.GetUsers:
                            syncTables[3] = true;
                            break;

                        case TransactionType.SignUp:
                            syncTables[1] = true;
                            syncTables[3] = true;
                            addingItem = true;
                            break;

                        case TransactionType.LogIn:
                            syncTables[3] = true;
                            break;

                        default:
                            break;
                    }
                }

                await azureService.SyncDataTablesAsync(syncTables);
            }

            result = new object[transactions.Length];
            for (int i = 0; i < transactions.Length; i++)
            {
                if (transactions[i].TransactType == TransactionType.SignUp)
                {
                    result = new object[2];
                    Transaction transaction = transactions[0];
                    result[0] = null;
                    if (!(await azureService.DoesUserExistAsync(transaction.Arguments.FirstName + " " + transaction.Arguments.LastName)))
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            result[0] = await azureService.AddUserAsync(transaction.Arguments.FirstName, 
                                transaction.Arguments.LastName,
                                transaction.Arguments.Password,
                                transaction.Arguments.IsCompetitive);
                        }
                        else
                            result[1] = "Device is offline. Sign-up is only available when device is online.";
                    }
                    else
                        result[1] = "User already exists! Please sign in!";
                }
                else if (transactions[i].TransactType == TransactionType.LogIn)
                {
                    result = new object[2];
                    Transaction transaction = transactions[0];
                    result[0] = null;
                    if (await azureService.DoesUserExistAsync(transaction.Arguments.FullName))
                    {
                        if (await azureService.LoginAsync(transaction.Arguments.FullName, transaction.Arguments.Password))
                            result[0] = bool.TrueString;
                        else
                            result[1] = "The name or password entered was incorrect.";
                    }
                    else
                        result[1] = "User does not exist. Please sign up!";
                }
                else
                    result[i] = await transactions[i].RunTransaction(azureService);
            }

            if (addingItem)
                await azureService.Client.SyncContext.PushAsync();

            return result;
        }
    }
}