namespace BadmintonClub.Models.Data_Access_Layer
{
    public enum TransactionType
    {
        AddBlogPost,
        GetBlogPosts,

        AddMatch,
        GetMatches,
        SyncUserMatches,

        GetSeasonData,

        AddUser,
        GetUsers,

        LogIn,
        SignUp
    }
}