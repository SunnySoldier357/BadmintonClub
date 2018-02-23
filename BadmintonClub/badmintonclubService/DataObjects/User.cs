using Microsoft.Azure.Mobile.Server;

namespace badmintonclubService.DataObjects
{
    public class User : EntityData
    {
        public int ClearanceLevel { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int PointsInCurrentSeason { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}