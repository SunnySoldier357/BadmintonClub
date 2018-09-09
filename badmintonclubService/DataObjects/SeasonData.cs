using Microsoft.Azure.Mobile.Server;

namespace badmintonclubService.DataObjects
{
    public class SeasonData : EntityData
    {
        // Public Properties
        public string UserID { get; set; }

        public int GamesPlayed { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesWon { get; set; }

        public int PointsAgainst { get; set; }
        public int PointsFor { get; set; }
        public int PointsInCurrentSeason { get; set; }

        public int SeasonNumber { get; set; }
    }
}