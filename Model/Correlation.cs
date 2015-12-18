using Beezer.Enums;

namespace Beezer.Model
{
    public class Correlation
    {
        public CorrelationType CorrelationType { get; set; }
        public double Coefficient { get; set; }
        public int ColumnA { get; set; }
        public int ColumnB { get; set; }
    }
}
