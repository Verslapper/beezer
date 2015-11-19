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

        public Statline GetLatestStatline(int teamId)
        {
            // TODO: double DTO DTO demon
            var dto = _db.FirstOrDefault<Statline>("where teamId = @0 order by Date desc", teamId);
            if (dto != null)
            {
                return new Statline
                {
                    Wins = dto.Wins
                };
            }
            return null;
        }

        public void Add(Statline statline)
        {
            // TODO: double DTO DTO demon
            _db.Insert("Statline", "ID", statline);
        }
    }
}
