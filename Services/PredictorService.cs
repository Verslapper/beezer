using System;
using System.Collections.Generic;
using Beezer.Model;
using Beezer.Repository;
using Meta.Numerics.Statistics;

namespace Beezer.Services
{
    public class PredictorService
    {
        private readonly StatlineRepository _statlineRepository;

        public PredictorService(StatlineRepository statlineRepository)
        {
            _statlineRepository = statlineRepository;
        }

        public void FindCorrelationsViaLinearRegression()
        {
            var statlines = _statlineRepository.GetStatlines();

            const int COLUMNS = 9;
            var sample = new MultivariateSample(COLUMNS);
            foreach (var statline in statlines)
            {
                sample.Add(
                    (double)statline.GoalsForPerGame,
                    (double)statline.GoalsAgainstPerGame,
                    (double)statline.PointPercentage,
                    (double)statline.PowerPlayPercentage,
                    (double)statline.PenaltyKillPercentage,
                    statline.RegularPlusOvertimeWins,
                    (double)statline.FaceoffWinPercentage,
                    (double)statline.ShotsForPerGame,
                    (double)statline.ShotsAgainstPerGame
                );
            }

            var fitResult = sample.LinearRegression(0);

            Console.WriteLine(@"Columns are:
                        (double)statline.GoalsForPerGame,
                        (double)statline.GoalsAgainstPerGame,
                        (double)statline.PointPercentage,
                        (double)statline.PowerPlayPercentage,
                        (double)statline.PenaltyKillPercentage,
                        statline.RegularPlusOvertimeWins,
                        (double)statline.FaceoffWinPercentage,
                        (double)statline.ShotsForPerGame,
                        (double)statline.ShotsAgainstPerGame");

            // Here's where we see if there's relevance or not. If there's one or two clear, independent ones, then we go multiple regression.
            var significantCorrelations = new List<Correlation>();
            var borderlineCorrelations = new List<Correlation>();
            for (var i = 0; i < COLUMNS; i++)
            {
                for (var j = i + 1; j < COLUMNS; j++)
                {
                    if (i != j)
                    {
                        var coefficient = fitResult.CorrelationCoefficient(i, j);

                        if (coefficient >= 0.8 || coefficient <= -0.8)
                        {
                            significantCorrelations.Add(new Correlation { Coefficient = coefficient, ColumnA = i, ColumnB = j });
                            Console.WriteLine("----> SIGNIFICANT correlation between cols {0} and {1}: {2}", i, j, coefficient);
                        }
                        else if (coefficient >= 0.6 || coefficient <= -0.6)
                        {
                            borderlineCorrelations.Add(new Correlation { Coefficient = coefficient, ColumnA = i, ColumnB = j });
                            Console.WriteLine("--> Borderline correlation between cols {0} and {1}: {2}", i, j, coefficient);
                        }

                        Console.WriteLine("Correlation between cols {0} and {1}: {2}", i, j, coefficient);
                    }
                }
            }

            // convert column number back to a readable name (build a dictionary to start with)
            // return correlations
        }
    }
}
