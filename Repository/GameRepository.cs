﻿using System;
using System.Collections.Generic;
using System.Linq;
using Beezer.Enums;
using Beezer.Model;
using PetaPoco;

namespace Beezer.Repository
{
    public class GameRepository
    {
        private readonly Database _db;

        public GameRepository()
        {
            _db = new Database("Beezer.ConnectionString");
        }

        public void Update(Game game)
        {
            var existingGame = _db.SingleOrDefault<GameDTO>("where Date = @0 and AwayTeam = @1 and HomeTeam = @2",
                                                            game.Date, game.AwayTeam, game.HomeTeam);

            if (existingGame == null)
            {
                _db.Insert(new GameDTO
                {
                    Date = game.Date,
                    AwayTeam = (int)game.AwayTeam,
                    HomeTeam = (int)game.HomeTeam,
                    AwayScore = game.AwayScore,
                    HomeScore = game.HomeScore
                });
            }
            else
            {
                existingGame.AwayScore = game.AwayScore;
                existingGame.HomeScore = game.HomeScore;
                _db.Save(existingGame);
            }
        }

        public int? GetGameId(Game game)
        {
            if (game.Id != 0)
            {
                return game.Id;
            }

            var dto = _db.SingleOrDefault<int?>("select Id from Game where Date = @0 and AwayTeam = @1 and HomeTeam = @2",
                                                            game.Date, game.AwayTeam, game.HomeTeam);

            return dto.HasValue ? dto.Value : (int?)null;
        }

        public List<Game> GetNextGames()
        {
            // These are the next gameday of games. Assumes we are up-to-date with results.
            var dtos = _db.Fetch<GameDTO>("where date = (select min(date) from Game where homescore is null)");

            return dtos.Select(dto => new Game
            {
                Id = dto.Id,
                Date = dto.Date,
                AwayTeam = (Team) Enum.Parse(typeof (Team), dto.AwayTeam.ToString()),
                HomeTeam = (Team) Enum.Parse(typeof (Team), dto.HomeTeam.ToString()),
            }).ToList();
        }

        public List<Game> GetLastResultedGames(int gamesToReturn)
        {
            var dtos = _db.Fetch<GameDTO>("select top " + gamesToReturn + " * from Game where HomeScore is not null order by Date desc");

            return dtos.Select(dto => new Game
            {
                Id = dto.Id,
                Date = dto.Date,
                AwayTeam = (Team)Enum.Parse(typeof(Team), dto.AwayTeam.ToString()),
                HomeTeam = (Team)Enum.Parse(typeof(Team), dto.HomeTeam.ToString()),
                AwayScore = dto.AwayScore,
                HomeScore = dto.HomeScore,
            }).ToList();
        }
    }
}
