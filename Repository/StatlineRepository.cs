using System;
using System.Collections.Generic;
using System.Linq;
using Beezer.Enums;
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

        public List<Statline> GetStatlinesForGame(Game game)
        {
            var dtos = _db.Fetch<StatlineDTO>("select top 2 * from Statline where team in (@0,@1) order by date desc",
                                                (int) game.HomeTeam, (int) game.AwayTeam);

            return dtos.Select(statline => new Statline
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
                Team = (Team)Enum.Parse(typeof(Team), statline.Team.ToString()),
            }).ToList();
        }

        public List<Statline> GetStatlines()
        {
            // This might be the place to combine the goals for in the game after each statline
            // could be via a new column
            // could be via a join

            var dtos = _db.Fetch<StatlineDTO>("select top 30 * from Statline order by date desc");

            return dtos.Select(statline => new Statline
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
                Team = (Team)Enum.Parse(typeof(Team), statline.Team.ToString()),
            }).ToList();
        }
    }
}
