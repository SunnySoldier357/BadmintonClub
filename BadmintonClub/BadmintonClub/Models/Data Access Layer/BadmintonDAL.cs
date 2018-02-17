using System.Data;
using System.Data.SqlClient;

namespace BadmintonClub.Models.Data_Access_Layer
{
    public class BadmintonDAL
    {
        // Private Properties
        private SqlConnection connection;

        // Public Properties
        public DataTable BlogPostsDT { get; private set; }
        public DataTable MatchesDT { get; private set; }
        public DataTable UsersDT { get; private set; }

        // Constructors
        public BadmintonDAL()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                UserID = "sunny",
                Password = "BadmintonClub2018",
                DataSource = "badmintonclubdb.database.windows.net",
                InitialCatalog = "BadmintonClubDB"
            };

            connection = new SqlConnection(builder.ConnectionString);

            BlogPostsDT = getDT(Table.BlogPosts);
            MatchesDT = getDT(Table.Matches);
            UsersDT = getDT(Table.Users);
        }

        // Public Methods
        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public User GetUser(int userID)
        {
            foreach (DataRow row in UsersDT.Rows)
            {
                if ((int)row[0] == userID)
                    return new User(row);
            }
            return null;
        }

        // Private Methods
        private DataTable getDT(Table t)
        {
            connection.Open();
            DataTable dt = new DataTable();

            string table = "dbo.";
            switch (t)
            {
                case Table.BlogPosts:
                    table += "BlogPosts";
                    break;
                case Table.Matches:
                    table += "Matches";
                    break;
                case Table.Users:
                    table += "Users";
                    break;
            }

            SqlCommand command = new SqlCommand(string.Format("SELECT * FROM {0}", table), connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            connection.Close();
            return dt;
        }

        // Nested Enums
        private enum Table
        {
            BlogPosts,
            Matches,
            Users
        }
    }
}
