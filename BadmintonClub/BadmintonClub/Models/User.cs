using System;
using System.Collections.Generic;
using System.Text;

namespace BadmintonClub.Models
{
    public class User
    {
        // Private Properties
        private int clearanceLevel;

        // Public Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Title { get; set; }
        public int ClearanceLevel { get { return clearanceLevel; }
            set { clearanceLevel = (value == 0 || value == 1 | value == 2) ? value : 0; } } // 0-Member 1-Admin/Board 2-Master

        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GamesLost { get; private set; }
        public double WinPercentage { get { return GamesPlayed == 0 ? double.NaN : GamesWon / GamesPlayed * 100; } }
        public int PointsInCurrentSeason { get; set; }
        public List<Match> Matches { get; private set; }

        // Constructors
        public User() : this("Default", "Default", "Default", 0) { }

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
            else
                GamesLost++;
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

        public override string ToString()
        {
            return string.Format("[Name: {0}, Title: {1}, Clearance Level: {2}]", FullName, Title, ClearanceLevel);
        }
    }
}
