namespace BadmintonClub.Models
{
    public class SeasonData
    {
        // Public Properties
        public int GamesPlayed { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesWon { get; set; }
        public int PointsAgainst { get; set; }
        public int PointsFor { get; set; }
        public int PointsInCurrentSeason { get; set; }
        public int SeasonNumber { get; set; }

        public string Id { get; set; }
        public string UserID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public double WinPercentage { get { return GamesPlayed == 0 ? 0 : GamesWon / GamesPlayed * 100; } }

        [Newtonsoft.Json.JsonIgnore]
        public int GamesLost { get { return GamesPlayed - GamesWon - GamesDrawn; } }
        [Newtonsoft.Json.JsonIgnore]
        public int PointDifference { get { return PointsFor - PointsAgainst; } }

        [Newtonsoft.Json.JsonIgnore]
        public User Player { get; set; }

        // Constructor
        public SeasonData() : this("") { }

        public SeasonData(string userID)
        {
            UserID = userID;

            GamesPlayed = 0;
            GamesDrawn = 0;
            GamesWon = 0;
            PointsAgainst = 0;
            PointsFor = 0;
        }

        // Public Methods
        public void AddMatch(Match match, bool isPlayer)
        {
            GamesPlayed++;

            if (match.IsDraw())
            {
                GamesDrawn++;
                PointsInCurrentSeason++;
            }
            else if (isPlayer ? match.IsPlayerWinner() : !match.IsPlayerWinner())
            {
                GamesWon++;
                PointsInCurrentSeason += 3;
            }

            PointsFor += isPlayer ? match.PlayerScore : match.OpponentScore;
            PointsAgainst += isPlayer ? match.OpponentScore : match.PlayerScore;
        }
    }
}
