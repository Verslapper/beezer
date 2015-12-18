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
                Rank = statline.Rank,
                PlusMinus = statline.PlusMinus,
                HomeWins = statline.HomeWins,
                HomeLosses = statline.HomeLosses,
                HomeOvertimeLosses = statline.HomeOvertimeLosses,
                AwayWins = statline.AwayWins,
                AwayLosses = statline.AwayLosses,
                AwayOvertimeLosses  = statline.AwayOvertimeLosses,
                ShootoutWins = statline.ShootoutWins,
                ShootoutLosses = statline.ShootoutWins,
                WinsInLast10 = statline.WinsInLast10,
                LossesInLast10  = statline.LossesInLast10,
                OvertimeLossesInLast10 = statline.OvertimeLossesInLast10,
                StreakType = (StreakType)Enum.Parse(typeof(StreakType), statline.StreakType.ToString()),
                StreakLength = statline.StreakLength,
            }).ToList();
        }

        public Statline GetMostRecentStatlineFor(Team team)
        {
            var statline = _db.SingleOrDefault<StatlineDTO>("select top 1 * from Statline where Team = @0 order by date desc", (int)team);

            if (statline == null)
            {
                return null;
            }

            return new Statline
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
            };
        }

        public void Update(Statline statline)
        {
            var dto = _db.SingleOrDefault<StatlineDTO>("where Team = @0 and Date = @1", (int)statline.Team, statline.Date);

            if (dto != null)
            {
                dto.Date = statline.Date;
                dto.FaceoffWinPercentage = statline.FaceoffWinPercentage;
                dto.GamesPlayed = statline.GamesPlayed;
                dto.GoalsAgainst = statline.GoalsAgainst;
                dto.GoalsAgainstPerGame = statline.GoalsAgainstPerGame;
                dto.GoalsFor = statline.GoalsFor;
                dto.GoalsForPerGame = statline.GoalsForPerGame;
                dto.Points = statline.Points;
                dto.Losses = statline.Losses;
                dto.Wins = statline.Wins;
                dto.OvertimeLosses = statline.OvertimeLosses;
                dto.RegularPlusOvertimeWins = statline.RegularPlusOvertimeWins;
                dto.PowerPlayPercentage = statline.PowerPlayPercentage;
                dto.PenaltyKillPercentage = statline.PenaltyKillPercentage;
                dto.PointPercentage = statline.PointPercentage;
                dto.SeasonId = statline.SeasonId;
                dto.ShotsForPerGame = statline.ShotsForPerGame;
                dto.ShotsAgainstPerGame = statline.ShotsAgainstPerGame;
                dto.Team = (int) statline.Team;

                // REVISIT: Turn these fields into nullable fields
                if (statline.Rank > 0)
                {
                    dto.Rank = statline.Rank;
                    dto.PlusMinus = statline.PlusMinus;
                    dto.HomeWins = statline.HomeWins;
                    dto.HomeLosses = statline.HomeLosses;
                    dto.HomeOvertimeLosses = statline.HomeOvertimeLosses;
                    dto.AwayWins = statline.AwayWins;
                    dto.AwayLosses = statline.AwayLosses;
                    dto.AwayOvertimeLosses = statline.AwayOvertimeLosses;
                    dto.ShootoutWins = statline.ShootoutWins;
                    dto.ShootoutLosses = statline.ShootoutLosses;
                    dto.WinsInLast10 = statline.WinsInLast10;
                    dto.LossesInLast10 = statline.LossesInLast10;
                    dto.OvertimeLossesInLast10 = statline.OvertimeLossesInLast10;
                    dto.StreakType = (int) statline.StreakType;
                    dto.StreakLength = statline.StreakLength;
                }
                _db.Save(dto);
            }

        }
    }
}
