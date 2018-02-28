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
        public User MatchWinner { get { return PlayerScore > OpponentScore ? Player : Opponent; } }
        [Newtonsoft.Json.JsonIgnore]
        public User Opponent { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public User Player { get; set; }

        // Constructor
        public Match() { }

        public Match(User player, User opponent, int playerScore, int opponentScore)
        {
            Player = player;
            Opponent = opponent;
            PlayerScore = playerScore;
            OpponentScore = opponentScore;
        }
    }
}