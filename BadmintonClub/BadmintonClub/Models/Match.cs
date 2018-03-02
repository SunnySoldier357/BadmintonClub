namespace BadmintonClub.Models
{
    public class Match
    {
        // Public Properties
        public int OpponentScore { get; set; }
        public int PlayerScore { get; set; }

        public string Id { get; set; }
        public string OpponentID { get; set; }
        public string PlayerID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public User Opponent { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public User Player { get; set; }

        // Constructor
        public Match() : this("0", "0", 0, 0) { }

        public Match(string playerID, string opponentID, int playerScore, int opponentScore)
        {
            PlayerID = playerID;
            OpponentID = opponentID;
            PlayerScore = playerScore;
            OpponentScore = opponentScore;
        }

        // Getters
        public bool IsDraw()
        {
            return PlayerScore == OpponentScore;
        }

        public bool IsPlayerWinner()
        {
            return PlayerScore > OpponentScore;
        }
    }
}