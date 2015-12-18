using System;
using System.Collections.Generic;
using System.Linq;
using Beezer.Enums;

namespace Beezer.Model
{
    public class Statline
    {
        // This is the mapping. I can't put it in a mapper class because we need the relevant column name AND its value.
        private readonly List<Variable> _keyStats = new List<Variable>
        {
            new Variable { Id = 0, Name = "GoalsForPerGame"},
            new Variable { Id = 1, Name = "GoalsAgainstPerGame"},
            new Variable { Id = 2, Name = "PointPercentage"},
            new Variable { Id = 3, Name = "PowerPlayPercentage"},
            new Variable { Id = 4, Name = "PenaltyKillPercentage"},
            new Variable { Id = 5, Name = "FaceoffWinPercentage"},
            new Variable { Id = 6, Name = "ShotsForPerGame"},
            new Variable { Id = 7, Name = "ShotsAgainstPerGame"},
            new Variable { Id = 8, Name = "Rank"},
            new Variable { Id = 9, Name = "PlusMinus"},
            new Variable { Id = 10, Name = "WinsInLast10"},
        };

        private readonly List<Variable> _positiveStats = new List<Variable>
        {
            new Variable { Id = 0, Name = "GoalsForPerGame"},
            new Variable { Id = 1, Name = "Streak"},
            new Variable { Id = 2, Name = "PointPercentage"},
            new Variable { Id = 3, Name = "PowerPlayPercentage"},
            new Variable { Id = 4, Name = "PenaltyKillPercentage"},
            new Variable { Id = 5, Name = "FaceoffWinPercentage"},
            new Variable { Id = 6, Name = "ShotsForPerGame"},
            new Variable { Id = 7, Name = "AwayWins"},
            new Variable { Id = 8, Name = "ShootoutWins"},
            new Variable { Id = 9, Name = "PlusMinus"},
            new Variable { Id = 10, Name = "WinsInLast10"},
            new Variable { Id = 11, Name = "Wins"},
            new Variable { Id = 12, Name = "GoalsFor"},
            new Variable { Id = 13, Name = "HomeWins"},
            new Variable { Id = 14, Name = "Points"},
            new Variable { Id = 15, Name = "RegularPlusOvertimeWins"},
        };

        private readonly List<Variable> _negativeStats = new List<Variable>
        {
            new Variable { Id = 0, Name = "OvertimeLossesInLast10"},
            new Variable { Id = 1, Name = "GoalsAgainstPerGame"},
            new Variable { Id = 2, Name = "ShotsAgainstPerGame"},
            new Variable { Id = 3, Name = "Rank"},
            new Variable { Id = 4, Name = "Losses"},
            new Variable { Id = 5, Name = "OvertimeLosses"},
            new Variable { Id = 6, Name = "GoalsAgainst"},
            new Variable { Id = 7, Name = "HomeLosses"},
            new Variable { Id = 8, Name = "HomeOvertimeLosses"},
            new Variable { Id = 9, Name = "AwayLosses"},
            new Variable { Id = 10, Name = "AwayOvertimeLosses"},
            new Variable { Id = 11, Name = "ShootoutLosses"},
            new Variable { Id = 12, Name = "LossesInLast10"},
        };

        public List<Variable> KeyStats
        {
            get
            {
                // No need to initialise everytime.
                return _keyStats;
            }
        }

        /// <summary>
        /// Teams want these to be as high as possible
        /// </summary>
        public List<Variable> PositiveStats
        {
            get
            {
                return _positiveStats;
            }
        }

        /// <summary>
        /// Teams want these to be as low as possible
        /// </summary>
        public List<Variable> NegativeStats
        {
            get
            {
                return _negativeStats;
            }
        }

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
        public Team Team { get; set; }

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
        public StreakType StreakType { get; set; }
        public int StreakLength { get; set; }

        public double GetByName(string name)
        {
            switch (name)
            {
                case "GoalsForPerGame":
                    return (double) GoalsForPerGame;
                case "GoalsAgainstPerGame":
                    return (double) GoalsAgainstPerGame;
                case "PointPercentage":
                    return (double) PointPercentage;
                case "PowerPlayPercentage":
                    return (double) PowerPlayPercentage;
                case "PenaltyKillPercentage":
                    return (double) PenaltyKillPercentage;
                case "FaceoffWinPercentage":
                    return (double) FaceoffWinPercentage;
                case "ShotsForPerGame":
                    return (double) ShotsForPerGame;
                case "ShotsAgainstPerGame":
                    return (double) ShotsAgainstPerGame;
                case "Rank":
                    return Rank;
                case "PlusMinus":
                    return PlusMinus;
                case "WinsInLast10":
                    return WinsInLast10;
                case "Wins":
                    return Wins;
                case "Losses":
                    return Losses;
                case "OvertimeLosses":
                    return OvertimeLosses;
                case "Points":
                    return Points;
                case "RegularPlusOvertimeWins":
                    return RegularPlusOvertimeWins;
                case "GoalsFor":
                    return GoalsFor;
                case "GoalsAgainst":
                    return GoalsAgainst;
                case "HomeWins":
                    return HomeWins;
                case "HomeLosses":
                    return HomeLosses;
                case "HomeOvertimeLosses":
                    return HomeOvertimeLosses;
                case "AwayWins":
                    return AwayWins;
                case "AwayLosses":
                    return AwayLosses;
                case "AwayOvertimeLosses":
                    return AwayOvertimeLosses;
                case "ShootoutWins":
                    return ShootoutWins;
                case "ShootoutLosses":
                    return ShootoutLosses;
                case "LossesInLast10":
                    return LossesInLast10;
                case "OvertimeLossesInLast10":
                    return OvertimeLossesInLast10;
                case "StreakType":
                    return (int)StreakType;
                case "StreakLength":
                    return StreakLength;
                case "Streak": // trying this out to provide a meaningful comparable field. not sure if right place to do it
                    return StreakType == StreakType.Win ? StreakLength : StreakLength * -1;
                default:
                    return 0;
            }
        }

        public double GetById(int id, List<Variable> statGroup)
        {
            return GetByName(statGroup.Single(s => s.Id == id).Name);
        }

        public string GetName(int id, List<Variable> statGroup)
        {
            return statGroup.Single(s => s.Id == id).Name;
        }
    }
}
