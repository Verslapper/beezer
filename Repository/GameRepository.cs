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
    }
}
