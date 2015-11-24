using System;
using Beezer.Enums;

namespace Beezer.Model
{
    public class Game
    {
        public DateTime Date { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
