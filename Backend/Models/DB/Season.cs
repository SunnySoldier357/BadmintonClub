using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DB
{
    public class Season
    {
        // Public Properties
        public double WinPercentage =>
            Statistics.GamesPlayed == 0 ? 0 : Statistics.GamesWon / Statistics.GamesPlayed * 100;

        public Guid Id { get; set; }

        public int PointsInCurrentSeason => Statistics.GamesDrawn + Statistics.GamesWon * 2;
        public int SeasonNumber { get; set; }

        public List<Match> Matches { get; set; }

        [Required]
        public Statistics Statistics { get; set; }

        [Required]
        public User User { get; set; }

        // Constructor
        public Season() : this(null) { }

        public Season(Season season)
        {
            Matches = new List<Match>();

            User = season?.User ?? null;
        }

        // Public Methods
        public void Update(Season season) =>
            Statistics.Update(season.Statistics);

        public void UpdateStats(Match match) => Statistics.UpdateStats(match);
    }
}