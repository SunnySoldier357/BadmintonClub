namespace BadmintonClub.Models
{
    public class Match
    {
        // Public Properties
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }

        public string OpponentName { get; set; }
        public int OpponentScore { get; set; }

        public string MatchWinner { get { return PlayerScore > OpponentScore ? PlayerName : OpponentName; } }

        // Constructors
        public Match(User player, User opponent, int playerScore, int opponentScore)
        {
            PlayerName = player.FullName;
            OpponentName = opponent.FullName;
            PlayerScore = playerScore;
            OpponentScore = opponentScore;
        }
    }
}
