using System;
using PetaPoco;

namespace Beezer.Repository
{
    [TableName("Game"), PrimaryKey("Id")]
    public class GameDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeam { get; set; }
        public int AwayTeam { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
