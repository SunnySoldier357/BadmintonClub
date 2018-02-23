using Microsoft.Azure.Mobile.Server;

namespace badmintonclubService.DataObjects
{
    public class Match : EntityData
    {
        public int OpponentScore { get; set; }
        public int PlayerScore { get; set; }

        public string OpponentID { get; set; }
        public string PlayerID { get; set; }
    }
}