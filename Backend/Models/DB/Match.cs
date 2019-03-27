using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DB
{
    public class Match
    {
        // Public Properties
        public bool IsDraw => PlayerScore == OpponentScore;
        public bool IsPlayer => User.Id == Player.Id;
        public bool IsPlayerWinner => PlayerScore > OpponentScore;

        public Guid Id { get; set; }

        public int OpponentScore { get; set; }
        public int PlayerScore { get; set; }

        [Required]
        public User Opponent { get; set; }

        [Required]
        public User Player { get; set; }

        [Required]
        public User User { get; set; }

        // Constructor
        public Match() : this(null, null, 0, 0) { }

        public Match(Match match) :
            this(match.Player, match.Opponent, match.PlayerScore, match.OpponentScore)
        { }

        public Match(User player, User opponent, int playerScore, int opponentScore)
        {
            Player = player;
            Opponent = opponent;

            PlayerScore = playerScore;
            OpponentScore = opponentScore;
        }

        // Public Methods
        public void Update(Match updated)
        {
            OpponentScore = updated.OpponentScore;
            PlayerScore = updated.PlayerScore;

            Player = updated.Player;
            Opponent = updated.Opponent;
        }
    }
}