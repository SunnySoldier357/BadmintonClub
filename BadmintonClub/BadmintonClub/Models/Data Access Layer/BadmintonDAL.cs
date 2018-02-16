using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BadmintonClub.Models.Data_Access_Layer
{
    public class BadmintonDAL
    {
        // Private Properties
        private SqlConnection connection;

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
        }
    }
}
