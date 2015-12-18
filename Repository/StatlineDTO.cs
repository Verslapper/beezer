using System;
using PetaPoco;

namespace Beezer.Repository
{
    [TableName("Statline"), PrimaryKey("Id")]
    public class StatlineDTO
    {
        public int Id { get; set; }
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
        public int Ties { get; set; }
        public int Wins { get; set; }
        public int Team { get; set; }
        public int Rank { get; set; }
        public int PlusMinus { get; set; }
        public int HomeWins { get; set; }
        public int HomeLosses { get; set; }
        public int HomeOvertimeLosses { get; set; }
        public int AwayWins { get; set; }
        public int AwayLosses { get; set; }
        public int AwayOvertimeLosses { get; set; }
        public int ShootoutWins { get; set; }
        public int ShootoutLosses { get; set; }
        public int WinsInLast10 { get; set; }
        public int LossesInLast10 { get; set; }
        public int OvertimeLossesInLast10 { get; set; }
        public int StreakType { get; set; }
        public int StreakLength { get; set; }
    }
}
