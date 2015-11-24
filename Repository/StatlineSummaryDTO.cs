using System.Collections.Generic;

namespace Beezer.Repository
{
    internal class StatlineSummaryDTO
    {
        public List<NHLStatlineDTO> data { get; set; }
        public int total { get; set; }
    }
}
