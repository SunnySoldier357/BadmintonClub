using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace badmintonclubService.DataObjects
{
    public class User : EntityData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Title { get; set; }

        // 0-Member   1-Admin/Board   2-Master
        public int ClearanceLevel { get; set; }

        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }

        public int PointsInCurrentSeason { get; set; }

        public ICollection<Match> Matches { get; set; }
    }
}