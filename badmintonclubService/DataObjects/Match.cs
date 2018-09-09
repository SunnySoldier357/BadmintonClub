using Microsoft.Azure.Mobile.Server;

namespace badmintonclubService.DataObjects
{
    public class Match : EntityData
    {
        // Public Properties
        public string PlayerID { get; set; }
        public int PlayerScore { get; set; }

        public string OpponentID { get; set; }
        public int OpponentScore { get; set; }

        public int SeasonNumber { get; set; }
    }
}