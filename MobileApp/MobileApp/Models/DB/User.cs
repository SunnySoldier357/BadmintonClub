using System;
using System.Collections.Generic;

namespace MobileApp.Models.DB
{
    public class User
    {
        // Public Properties
        public bool IsAdmin => ClearanceLevel == ClearanceLevel.Admin || 
            ClearanceLevel == ClearanceLevel.Master;

        public ClearanceLevel ClearanceLevel { get; set; }

        public Guid Id { get; set; }

        public List<Match> Matches { get; set; }

        public Season Season { get; set; }

        public Statistics Statistics { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }

        public string FullName => string.Format("{0} {1}", FirstName, LastName);
        public string TitleDisplay =>
            Title + (Title.ToLower().Contains("of") ? " for" : " of") + " Badminton Club";

        public string WinPercentage =>
            Statistics.GamesPlayed == 0 ? "0 %" : Math.Round(
                (double)Statistics.GamesWon / (double)Statistics.GamesPlayed * 100.0, 
                2, MidpointRounding.AwayFromZero).ToString() + " %";

        // Constructors
        public User() : this(null, null, "Member", 0) { }

        public User(User user)
            : this(user.FirstName, user.LastName, user.Title, user.ClearanceLevel) { }

        public User(string firstName, string lastName, string title, ClearanceLevel clearanceLevel)
        {
            ClearanceLevel = clearanceLevel;

            Matches = new List<Match>();

            FirstName = firstName;
            LastName = lastName;
            Title = title;
        }

        // Public Methods
        public void UpdateStats(Match match) => Statistics.UpdateStats(match);

        public void Update(User user)
        {
            ClearanceLevel = user.ClearanceLevel;
            Statistics.Update(user.Statistics);
        }
    }
}