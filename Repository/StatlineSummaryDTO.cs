using System.Collections.Generic;

namespace Beezer.Repository
{
    internal class StatlineSummaryDTO
    {
        public List<StatlineDTO> data { get; set; }
        public int total { get; set; }
    }
}
