using Microsoft.Azure.Mobile.Server;

namespace badmintonclubService.DataObjects
{
    public class User : EntityData
    {
        // Public Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }

        public int ClearanceLevel { get; set; } // 0-Member   1-Admin/Board   2-Master

        public int GamesPlayed { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesWon { get; set; }

        public int PointsAgainst { get; set; }
        public int PointsFor { get; set; }

        public string Password { get; set; }

        public bool IsCompetitive { get; set; }
    }
}