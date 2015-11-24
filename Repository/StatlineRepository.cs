using System;
using Beezer.Model;
using PetaPoco;

namespace Beezer.Repository
{
    public class StatlineRepository
    {
        private readonly Database _db;

        public StatlineRepository()
        {
            _db = new Database("Beezer.ConnectionString");
        }

        public void Add(Statline statline)
        {
            _db.Insert(new StatlineDTO
            {
                Date = statline.Date,
                FaceoffWinPercentage = statline.FaceoffWinPercentage,
                GamesPlayed = statline.GamesPlayed,
                GoalsAgainst = statline.GoalsAgainst,
                GoalsAgainstPerGame = statline.GoalsAgainstPerGame,
                GoalsFor = statline.GoalsFor,
                GoalsForPerGame = statline.GoalsForPerGame,
                Points = statline.Points,
                Losses = statline.Losses,
                Wins = statline.Wins,
                OvertimeLosses = statline.OvertimeLosses,
                RegularPlusOvertimeWins = statline.RegularPlusOvertimeWins,
                PowerPlayPercentage = statline.PowerPlayPercentage,
                PenaltyKillPercentage = statline.PenaltyKillPercentage,
                PointPercentage = statline.PointPercentage,
                SeasonId = statline.SeasonId,
                ShotsForPerGame = statline.ShotsForPerGame,
                ShotsAgainstPerGame = statline.ShotsAgainstPerGame,
                Team = (int)statline.Team,
            });
        }
    }
}
