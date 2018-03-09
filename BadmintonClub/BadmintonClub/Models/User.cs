using MvvmHelpers;
using System;
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

        public string FirstName { get; set; }
        public string Id { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string WinPercentage { get { return GamesPlayed == 0 ? "0 %" : 
            Math.Round((double)GamesWon / (double)GamesPlayed * 100.0, 2, MidpointRounding.AwayFromZero).ToString() 
            + " %"; } }

        [Newtonsoft.Json.JsonIgnore]
        public int GamesLost { get { return GamesPlayed - GamesWon - GamesDrawn; } }
        [Newtonsoft.Json.JsonIgnore]
        public int PointDifference { get { return PointsFor - PointsAgainst; } }

        [Newtonsoft.Json.JsonIgnore]
        public ObservableRangeCollection<Match> Matches { get; set; }

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
            Matches = new ObservableRangeCollection<Match>();
        }

        // Public Methods
        public void AddMatch(Match match, bool IsPlayer)
        {
            GamesPlayed++;

            if (match.IsDraw())
            {
                GamesDrawn++;
                //PointsInCurrentSeason++;
            }
            else if (IsPlayer ? match.IsPlayerWinner() : !match.IsPlayerWinner())
            {
                GamesWon++;
                //PointsInCurrentSeason += 3;
            }

            PointsFor += IsPlayer ? match.PlayerScore : match.OpponentScore;
            PointsAgainst += IsPlayer ? match.OpponentScore : match.PlayerScore;

            Matches.Add(match);
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