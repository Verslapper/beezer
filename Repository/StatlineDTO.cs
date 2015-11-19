namespace Beezer.Repository
{
    internal class StatlineDTO
    {
        public decimal faceoffWinPctg { get; set; }
        public int gamesPlayed { get; set; }
        public int goalsAgainst { get; set; }
        public int goalsFor { get; set; }
        public decimal goalsAgainstPerGame { get; set; }
        public decimal goalsForPerGame { get; set; }
        public int losses { get; set; }
        public int otLosses { get; set; }
        public decimal pkPctg { get; set; }
        public decimal pointPctg { get; set; }
        public int points { get; set; }
        /// <summary>
        /// Power play percentage
        /// </summary>
        public decimal ppPctg { get; set; }
        public int regPlusOtWins { get; set; }
        public int seasonId { get; set; }
        public decimal shotsAgainstPerGame { get; set; }
        public decimal shotsForPerGame { get; set; }
        public string teamAbbrev { get; set; }
        public string teamFullName { get; set; }
        public int teamId { get; set; }
        public int ties { get; set; }
        public int wins { get; set; }
    }
}
