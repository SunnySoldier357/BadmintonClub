using System.Collections.Generic;
using System.Data;

namespace BadmintonClub.Models
{
    public class User
    {
        // Private Properties
        private int clearanceLevel;
        private string title;

        // Public Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public string Title { get { return title; }
            set { title = value + (value.ToLower().Contains("of") ? " for" : " of") + " Badminton Club"; } }

        // 0-Member   1-Admin/Board   2-Master
        public int ClearanceLevel { get { return clearanceLevel; }
            set { clearanceLevel = (value == 0 || value == 1 | value == 2) ? value : 0; } } 

        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GamesLost { get { return GamesPlayed - GamesWon; } }
        public double WinPercentage { get { return GamesPlayed == 0 ? double.NaN : GamesWon / GamesPlayed * 100; } }
        public int PointsInCurrentSeason { get; set; }

        public List<Match> Matches { get; private set; }

        // Constructors
        public User() : this("Default", "Default", "Default", 0) { }

        public User(DataRow row)
        {
            FirstName = row["firstName"].ToString();
            LastName = row["lastName"].ToString();
            Title = row["title"].ToString();
            ClearanceLevel = (int)row["clearanceLevel"];
            GamesPlayed = (int)row["gamesPlayed"];
            GamesWon = (int)row["gamesWon"];
            PointsInCurrentSeason = (int)row["pointsInCurrentSeason"];
        }

        public User(string firstName, string lastName, string title, int clearanceLevel)
        {
            FirstName = firstName;
            LastName = lastName;
            Title = title + (title.ToLower().Contains("of") ? " for" : " of") + " Badminton Club";
            ClearanceLevel = clearanceLevel;
        }

        // Public Methods
        public void AddMatch(User opponent, int playerScore, int userScore)
        {
            Match temp = new Match(this, opponent, playerScore, userScore);
            GamesPlayed++;
            if (temp.MatchWinner.Equals(FullName))
                GamesWon++;
        }

        // Getters
        public bool IsMaster()
        {
            return clearanceLevel == 2;
        }

        public bool IsAdmin()
        {
            return clearanceLevel >= 1;
        }
    }
}
