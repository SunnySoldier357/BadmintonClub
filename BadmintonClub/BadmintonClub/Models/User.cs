using System.Collections.Generic;

namespace BadmintonClub.Models
{
    public class User
    {
        // Public Properties
        public bool IsCompetitive { get; set; }

        public int ClearanceLevel { get; set; } // 0-Member   1-Admin/Board   2-Master
        public int GamesPlayed { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesWon { get; set; }
        public int PointsAgainst { get; set; }
        public int PointsFor { get; set; }
        public int PointsInCurrentSeason { get; set; }

        public string FirstName { get; set; }
        public string Id { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public double WinPercentage { get { return GamesPlayed == 0 ? double.NaN : GamesWon / GamesPlayed * 100; } }

        [Newtonsoft.Json.JsonIgnore]
        public int GamesLost { get { return GamesPlayed - GamesWon; } }
        [Newtonsoft.Json.JsonIgnore]
        public int PointDifference { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<Match> Matches { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        [Newtonsoft.Json.JsonIgnore]
        public string TitleDisplay { get { return Title + (Title.ToLower().Contains("of") ? " for" : " of") + " Badminton Club"; } }

        // Constructors
        public User() : this("Default", "Default", "Member", 0) { }

        public User(string firstName, string lastName, string title, int clearanceLevel)
        {
            FirstName = firstName;
            LastName = lastName;
            Title = title;
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
            return ClearanceLevel == 2;
        }

        public bool IsAdmin()
        {
            return ClearanceLevel >= 1;
        }

        // DEBUG PURPOSES
        public override string ToString()
        {
            return string.Format("Name: {0}, ID: {1}, Title: {2}", FullName, Id, TitleDisplay);
        }
    }
}