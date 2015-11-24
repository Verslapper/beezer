using System;
using Beezer.Enums;

namespace Beezer.Model
{
    public class Statline
    {
        public DateTime Date { get; set; }
        public decimal FaceoffWinPercentage { get; set; }
        public int GamesPlayed { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalsFor { get; set; }
        public decimal GoalsAgainstPerGame { get; set; }
        public decimal GoalsForPerGame { get; set; }
        public int Losses { get; set; }
        public int OvertimeLosses { get; set; }
        public decimal PenaltyKillPercentage { get; set; }
        /// <summary>
        /// Points divided by maximum number of points
        /// </summary>
        public decimal PointPercentage { get; set; }
        public int Points { get; set; }
        /// <summary>
        /// Power play percentage
        /// </summary>
        public decimal PowerPlayPercentage { get; set; }
        public int RegularPlusOvertimeWins { get; set; }
        public int SeasonId { get; set; }
        public decimal ShotsAgainstPerGame { get; set; }
        public decimal ShotsForPerGame { get; set; }
        public string TeamAbbrev { get; set; }
        public string TeamFullName { get; set; }
        public int TeamId { get; set; }
        public int Ties { get; set; }
        public int Wins { get; set; }
        public Team Team { get; set; }
    }
}
