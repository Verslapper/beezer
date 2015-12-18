using System.Collections.Generic;
using Beezer.Enums;
using Beezer.Model;
using Meta.Numerics.Statistics;

namespace Beezer.Services
{
    public class PredictorService
    {
        public List<Correlation> FindCorrelationsViaLinearRegression(MultivariateSample sample, int columns)
        {
            var fitResult = sample.LinearRegression(0);

            var correlations = new List<Correlation>();

            // The two loops are for going across every column:column combination
            for (var i = 0; i < columns; i++)
            {
                for (var j = i + 1; j < columns; j++)
                {
                    var coefficient = fitResult.CorrelationCoefficient(i, j);

                    // REVISIT: Can pass in correlation thresholds through configuration
                    if (coefficient >= 0.55 || coefficient <= -0.55)
                    {
                        correlations.Add(new Correlation
                        {
                            Coefficient = coefficient,
                            ColumnA = i,
                            ColumnB = j,
                            CorrelationType = coefficient >= 0.8 || coefficient <= -0.8
                                ? CorrelationType.Significant
                                : CorrelationType.Borderline
                        });
                    }
                }
            }

            return correlations;
        }
    }
}
