using System.Threading.Tasks;

namespace BadmintonClub.Models.Data_Access_Layer
{
    public class Transaction
    {
        // Private Properties
        private dynamic arguments;

        // Public Properties
        public TransactionType transactionType;

        // Constructors
        public Transaction(dynamic arguments, TransactionType transactionType)
        {
            this.arguments = arguments;
            this.transactionType = transactionType;
        }

        // Public Methods
        public async Task<dynamic> RunTransaction(AzureService azureService)
        {
            switch (transactionType)
            {
                case TransactionType.AddBlogPost:
                    return await azureService.AddBlogPostAsync(arguments.BlogTitle, arguments.BodyOfPost);

                case TransactionType.GetBlogPosts:
                    return await azureService.GetBlogPostsAsync();

                case TransactionType.AddMatch:
                    return await azureService.AddMatchAsync(int.Parse(arguments.PlayerScore),
                        int.Parse(arguments.OpponentScore), arguments.PlayerName, arguments.OpponentName);

                case TransactionType.GetMatches:
                    return await azureService.GetMatchesAsync();

                case TransactionType.GetSeasonData:
                    return await azureService.GetSeasonDataAsync();

                case TransactionType.AddUser:
                    return await azureService.AddUserAsync(arguments.FirstName, arguments.LastName,
                        arguments.Password);

                case TransactionType.GetUsers:
                    return await azureService.GetUsersAsync();

                case TransactionType.SyncUserMatches:
                    return null;

                case TransactionType.LogIn:


                default:
                    return null;
            }
        }
    }
}
