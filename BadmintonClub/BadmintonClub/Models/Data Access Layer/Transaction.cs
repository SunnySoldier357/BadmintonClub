using System.Threading.Tasks;

namespace BadmintonClub.Models.Data_Access_Layer
{
    public class Transaction
    {
        // Public Properties
        public dynamic Arguments;

        public TransactionType TransactType;

        // Constructors
        public Transaction(dynamic arguments, TransactionType transactionType)
        {
            Arguments = arguments;
            TransactType = transactionType;
        }

        // Public Methods
        public async Task<dynamic> RunTransaction(AzureService azureService)
        {
            switch (TransactType)
            {
                case TransactionType.AddBlogPost:
                    return await azureService.AddBlogPostAsync(Arguments.BlogTitle, Arguments.BodyOfPost);

                case TransactionType.GetBlogPosts:
                    return await azureService.GetBlogPostsAsync();

                case TransactionType.AddMatch:
                    return await azureService.AddMatchAsync(int.Parse(Arguments.PlayerScore),
                        int.Parse(Arguments.OpponentScore), 
                        Arguments.PlayerName, 
                        Arguments.OpponentName,
                        Arguments.IsSeasonMatch,
                        Arguments.StartNewSeason);

                case TransactionType.GetMatches:
                    return await azureService.GetMatchesAsync();

                case TransactionType.GetSeasonData:
                    return await azureService.GetSeasonDataAsync();

                case TransactionType.GetUsers:
                    return await azureService.GetUsersAsync();

                case TransactionType.SyncUserMatches:
                    return null;

                default:
                    return null;
            }
        }
    }
}