using System;

namespace MobileApp.Models.DB
{
    public class Statistics
    {
        // Public Properties
        public Guid Id { get; set; }

        public int GamesPlayed => GamesDrawn + GamesLost + GamesWon;
        public int GamesDrawn { get; set; }
        public int GamesLost { get; set; }
        public int GamesWon { get; set; }
        public int PointsAgainst { get; set; }
        public int PointDifference => PointsFor - PointsAgainst;
        public int PointsFor { get; set; }

        // Constructors
        public Statistics() { }

        public Statistics(Statistics statistics)
        {
            GamesDrawn = statistics.GamesDrawn;
            GamesLost = statistics.GamesLost;
            GamesWon = statistics.GamesWon;
            PointsAgainst = statistics.PointsAgainst;
            PointsFor = statistics.PointsFor;
        }

        // Public Methods
        public void Update(Statistics statistics)
        {
            GamesDrawn = statistics.GamesDrawn;
            GamesLost = statistics.GamesLost;
            GamesWon = statistics.GamesWon;
            PointsAgainst = statistics.PointsAgainst;
            PointsFor = statistics.PointsFor;
        }

        public void UpdateStats(Match match)
        {
            if (match.IsDraw)
                GamesDrawn++;
            else if (match.IsPlayer ? match.IsPlayerWinner : !match.IsPlayerWinner)
                GamesWon++;
            else
                GamesLost++;

            PointsFor += match.IsPlayer ? match.PlayerScore : match.OpponentScore;
            PointsAgainst += match.IsPlayer ? match.OpponentScore : match.PlayerScore;
        }
    }
}