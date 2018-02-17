using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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

        public void OpenConnection()
        {
            connection.Open();
        }

        // Private Methods
        private DataTable getDT(Table t)
        {
            connection.Open();
            DataTable dt = new DataTable();

            string table = "";
            switch (t)
            {
                case Table.BlogPosts:
                    table = "dbo.BlogPosts";
                    break;
                case Table.Matches:
                    table = "dbo.Matches";
                    break;
                case Table.Users:
                    table = "dbo.Users";
                    break;
            }
            SqlCommand command = new SqlCommand(string.Format("SELECT * FROM {0}", table), 
                connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            connection.Close();
            return dt;
        }

        // Private Constructs
        private enum Table
        {
            BlogPosts,
            Matches,
            Users
        }
    }
}
