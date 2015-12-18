using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Beezer.Enums;
using Beezer.Model;
using Beezer.Repository;
using Beezer.Services;
using HtmlAgilityPack;
using Meta.Numerics.Statistics;
using Newtonsoft.Json;

namespace Beezer
{
    class Program
    {
        private static readonly StatlineRepository _statlineRepository = new StatlineRepository();
        private static readonly GameRepository _gameRepository = new GameRepository();
        private static readonly PredictorService _predictorService = new PredictorService();
        private static readonly PredictionRepository _predictionRepository = new PredictionRepository();

        static void Main(string[] args)
        {
            Console.WriteLine("Puck drop!");
            //GetTeamStats();
            //GetTeamStandings();
            Console.WriteLine("End of the first!");
            //GetSchedule();
            Console.WriteLine("That'll do us for the second.");
            //PredictNextGames();
            CrystalBall();
            // fetch best bet prices and providers for predicted winners
            Console.WriteLine("That's the horn!");
            //ShowPredictionPerformance();
            Thread.Sleep(420);
        }

        /// <summary>
        /// Discover what is relevant by retroactively predicting and measuring, rather than using correlation
        /// </summary>
        private static void CrystalBall()
        {
            var lastGames = _gameRepository.GetLastResultedGames(100);
            var predictions = new List<Prediction>();
            var homeAwayPredictions = new List<Prediction>();
            var positiveStats = new Statline().PositiveStats;
            var negativeStats = new Statline().NegativeStats;

            foreach (var game in lastGames)
            {
                // get team statlines for date before game date
                var gameStatlines = _statlineRepository.GetStatlinesForGame(game);

                Game game1 = game;
                var home = gameStatlines.Single(s => s.Team == game1.HomeTeam);
                var away = gameStatlines.Single(s => s.Team == game1.AwayTeam);
                var actualWinner = game1.HomeScore > game1.AwayScore ? home.Team : away.Team;

                // REVISIT: Extend variables to have positive/negative. Write a comparison function that checks these
                foreach (var variable in positiveStats)
                {
                    predictions.Add(new Prediction
                    {
                        GameId = game.Id,
                        PredictedWinner = home.GetByName(variable.Name) > away.GetByName(variable.Name) ? home.Team : away.Team,
                        ActualWinner = actualWinner,
                    });
                }

                foreach (var variable in negativeStats)
                {
                    predictions.Add(new Prediction
                    {
                        GameId = game.Id,
                        PredictedWinner = home.GetByName(variable.Name) < away.GetByName(variable.Name) ? home.Team : away.Team,
                        ActualWinner = actualWinner,
                    });
                }

                var homeAwayPrediction = new Prediction
                {
                    GameId = game.Id,
                    PredictedWinner = home.HomeWins > away.AwayWins ? home.Team : away.Team,
                    ActualWinner = actualWinner,
                };

                predictions.Add(homeAwayPrediction);
                homeAwayPredictions.Add(homeAwayPrediction);
            }

            Console.WriteLine("Crystal ball performance: {0}/{1} = {2}%",
                predictions.Count(p => p.PredictedWinner == p.ActualWinner),
                predictions.Count,
                Math.Round((double) (predictions.Count(p => p.PredictedWinner == p.ActualWinner) / predictions.Count * 100), 2)
            );

            Console.WriteLine("Crystal ball home & away performance: {0}/{1} = {2}%",
                homeAwayPredictions.Count(p => p.PredictedWinner == p.ActualWinner),
                homeAwayPredictions.Count,
                Math.Round((double)(homeAwayPredictions.Count(p => p.PredictedWinner == p.ActualWinner) / homeAwayPredictions.Count * 100), 2)
            );
        }

        private static void ShowPredictionPerformance()
        {
            var performance = _predictionRepository.GetPredictionPerformance();
            Console.WriteLine("Performance: {0}%", Math.Round(performance * 100, 2));
        }

        private static void PredictNextGames()
        {
            var statlines = _statlineRepository.GetStatlines();
            var variables = statlines.First().KeyStats; // get mapping

            var sample = new MultivariateSample(variables.Count);
            var statsOfVariables = new double[variables.Count];

            foreach (var statline in statlines)
            {
                for (var i = 0; i < variables.Count; i++)
                {
                    statsOfVariables[i] = statline.GetByName(variables[i].Name);
                }
                sample.Add(statsOfVariables);
            }

            var correlations = _predictorService.FindCorrelationsViaLinearRegression(sample, variables.Count);

            var nextGames = _gameRepository.GetNextGames();
            foreach (var game in nextGames)
            {
                Console.WriteLine("{0} at {1}", game.AwayTeam, game.HomeTeam);

                var gameStatlines = _statlineRepository.GetStatlinesForGame(game);

                Game game1 = game;
                var home = gameStatlines.Single(s => s.Team == game1.HomeTeam);
                var away = gameStatlines.Single(s => s.Team == game1.AwayTeam);

                // Here we can choose the independent variable (e.g. 0). Assumes larger Column A is desirable.
                //var correlationsVsIndependentVariable = correlations.Where(c => c.ColumnA == 0);
                var bestCorrelationTypeAvailable = CorrelationType.Significant;
                var significantCorrelations = correlations.Count(c => c.CorrelationType == CorrelationType.Significant);
                if (significantCorrelations == 0)
                {
                    bestCorrelationTypeAvailable = CorrelationType.Borderline;
                }
                
                var correlationsVsIndependentVariable = correlations.Where(c => c.CorrelationType == bestCorrelationTypeAvailable);
                var predictions = new List<bool>();
                foreach (var correlation in correlationsVsIndependentVariable)
                {
                    var independentVariable = home.GetName(correlation.ColumnA, variables);
                    var dependentVariable = home.GetName(correlation.ColumnB, variables);

                    bool homeWins;
                    if (correlation.Coefficient < 0)
                    {
                        homeWins = home.GetById(correlation.ColumnB, variables) < away.GetById(correlation.ColumnB, variables);
                    }
                    else
                    {
                        homeWins = home.GetById(correlation.ColumnB, variables) > away.GetById(correlation.ColumnB, variables);
                    }

                    Console.WriteLine("{0} to have {1} {2} based on {3} {4}",
                        homeWins ? home.Team : away.Team, "more", independentVariable, correlation.Coefficient < 0 ? "lower" : "higher", dependentVariable);

                    predictions.Add(homeWins);
                }

                // If all predictions reach the same conclusion
                if (predictions.Count > 0 && (predictions.Count == predictions.Count(p => p) || predictions.Count == predictions.Count(p => !p)))
                {
                    var winner = predictions.First() ? home : away;
                    Console.WriteLine("----> Our predicted winner is {0} <----", winner.Team);
                    _predictionRepository.Log(game1, winner);
                }
                else
                {
                    Console.WriteLine("Too close to call {0} at {1}", away.Team, home.Team);
                }
            }
        }

        private static void GetSchedule()
        {
            try
            {
                var url = string.Format("http://www.nhl.com/ice/schedulebyseason.htm");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                using (Stream respStream = response.GetResponseStream())
                {
                    if (respStream != null)
                    {
                        var encoding = Encoding.GetEncoding("utf-8");
                        var streamReader = new StreamReader(respStream, encoding);

                        var content = streamReader.ReadToEnd();
                        
                        var doc = new HtmlDocument();
                        doc.LoadHtml(content);

                        foreach (var row in doc.DocumentNode.SelectNodes(".//*/table[@class='data schedTbl']/tbody/tr"))
                        {
                            var dateNode = row.SelectSingleNode("./td/div[@class='skedStartDateSite']");
                            if (dateNode == null)
                            {
                                // For special headings (e.g. "Thanksgiving Showdown", "Winter Classic")
                                continue;
                            }

                            var date = DateTime.Parse(dateNode.InnerText);

                            var awayTeamCode = string.Empty;
                            var homeTeamCode = string.Empty;
                            int? awayScore = null;
                            int? homeScore = null;

                            var teams = row.SelectNodes("./td/div[@class='teamName']/a");
                            if (teams.Count == 2) // always should be
                            {
                                awayTeamCode = teams[0].GetAttributeValue("rel", "");
                                homeTeamCode = teams[1].GetAttributeValue("rel", "");
                            }

                            var result = row.SelectNodes("./td[@class='tvInfo']/span");
                            if (result != null && result.Count == 2) // only if resulted game
                            {
                                // " MTL (5) " without the quotes
                                awayScore = Int32.Parse(Regex.Replace(result[0].InnerText, @"[^\d]", ""));
                                homeScore = Int32.Parse(Regex.Replace(result[1].InnerText, @"[^\d]", ""));
                            }

                            var game = new Game
                            {
                                Date = date,
                                AwayTeam = (Team)Enum.Parse(typeof(Team), awayTeamCode),
                                HomeTeam = (Team)Enum.Parse(typeof(Team), homeTeamCode),
                            };

                            if (awayScore.HasValue)
                            {
                                game.AwayScore = awayScore;
                                game.HomeScore = homeScore;
                            }

                            // TODO: Create mode to only scan for updates since last checked date, instead of overwriting everything.
                            // Not much point checking if it exists then not adding it, not here
                            _gameRepository.Update(game);

                            // get gameid to update prediction if necessary.
                            // REVISIT: I thought there was a swift way for PetaPoco to insert and return the ID :/
                            var gameId = _gameRepository.GetGameId(game);

                            if (awayScore.HasValue && gameId.HasValue)
                            {
                                var prediction = _predictionRepository.IsAPredictedUnresultedGame(gameId.Value);
                                if (prediction != null)
                                {
                                    prediction.ActualWinner = game.AwayScore > game.HomeScore
                                        ? game.AwayTeam
                                        : game.HomeTeam;
                                    _predictionRepository.Save(prediction);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with the schedule: {0}", e);
            }
        }

        private static void GetTeamStandings()
        {
            try
            {
                var url = string.Format("http://www.nhl.com/ice/standings.htm?season=20152016&type=LEA");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                using (Stream respStream = response.GetResponseStream())
                {
                    if (respStream != null)
                    {
                        var encoding = Encoding.GetEncoding("utf-8");
                        var streamReader = new StreamReader(respStream, encoding);

                        var content = streamReader.ReadToEnd();
                        
                        var doc = new HtmlDocument();
                        doc.LoadHtml(content);

                        foreach (var row in doc.DocumentNode.SelectNodes(".//*/table[contains(@class, 'standings')]/tbody/tr"))
                        {
                            // POS  TEAM    DIV	GP	W	L	OT	P	ROW	GF	GA	DIFF	HOME	AWAY	S/O	L10	STREAK
                            var columns = row.SelectNodes("./td");
                            var currTeam = (Team)Enum.Parse(typeof(Team), columns[1].SelectSingleNode("./a").GetAttributeValue("rel", ""));
                            var statline = _statlineRepository.GetMostRecentStatlineFor(currTeam);

                            if (statline == null)
                            {
                                continue;
                            }

                            statline.Rank = Int32.Parse(columns[0].InnerText);
                            statline.PlusMinus = columns[11].InnerText == "E" ? 0 : Int32.Parse(columns[11].InnerText);

                            // Parse W-L-OTL (e.g. 11-3-1)
                            var homeRecord = columns[12].InnerText.Split('-');
                            statline.HomeWins = Int32.Parse(homeRecord[0]);
                            statline.HomeLosses = Int32.Parse(homeRecord[1]);
                            statline.HomeOvertimeLosses = Int32.Parse(homeRecord[2]);

                            var awayRecord = columns[13].InnerText.Split('-');
                            statline.AwayWins = Int32.Parse(awayRecord[0]);
                            statline.AwayLosses = Int32.Parse(awayRecord[1]);
                            statline.AwayOvertimeLosses = Int32.Parse(awayRecord[2]);

                            var shootoutRecord = columns[14].InnerText.Split('-');
                            statline.ShootoutWins = shootoutRecord[0].Length > 0 ? Int32.Parse(shootoutRecord[0]) : 0;
                            statline.ShootoutLosses = shootoutRecord[1].Length > 1 ? Int32.Parse(shootoutRecord[1]): 0;

                            var last10Record = columns[15].InnerText.Split('-');
                            statline.WinsInLast10 = Int32.Parse(last10Record[0]);
                            statline.LossesInLast10 = Int32.Parse(last10Record[1]);
                            statline.OvertimeLossesInLast10 = Int32.Parse(last10Record[2]);

                            var streakRecord = columns[16].InnerText.Split(' ');
                            statline.StreakType = (StreakType)Enum.Parse(typeof (StreakType), streakRecord[0].Replace("Won", "Win")
                                                                                                             .Replace("Lost", "Loss")
                                                                                                             .Replace("OT", "OvertimeLoss"));
                            statline.StreakLength = Int32.Parse(streakRecord[1]);

                            _statlineRepository.Update(statline);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with the schedule: {0}", e);
            }
        }

        private static void GetTeamStats()
        {
            try
            {
                var now = DateTime.Now;
                var url = string.Format("http://www.nhl.com/stats/rest/grouped/teams/season/teamsummary?cayenneExp=seasonId=20152016%20and%20gameTypeId=2");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream respStream = response.GetResponseStream())
                {
                    if (respStream != null)
                    {
                        var encoding = Encoding.GetEncoding("utf-8");
                        var streamReader = new StreamReader(respStream, encoding);

                        var content = streamReader.ReadToEnd();
                        var dto = JsonConvert.DeserializeObject<StatlineSummaryDTO>(content);

                        foreach (var statline in dto.data)
                        {
                            _statlineRepository.Add(new Statline
                            {
                                Team = (Team)Enum.Parse(typeof(Team), statline.teamAbbrev),
                                Date = now,
                                FaceoffWinPercentage = statline.faceoffWinPctg,
                                GamesPlayed = statline.gamesPlayed,
                                GoalsAgainst = statline.goalsAgainst,
                                GoalsFor = statline.goalsFor,
                                GoalsAgainstPerGame = statline.goalsAgainstPerGame,
                                GoalsForPerGame = statline.goalsForPerGame,
                                Losses = statline.losses,
                                OvertimeLosses = statline.otLosses,
                                PenaltyKillPercentage = statline.pkPctg,
                                PointPercentage = statline.pointPctg,
                                Points = statline.points,
                                PowerPlayPercentage = statline.ppPctg,
                                RegularPlusOvertimeWins = statline.regPlusOtWins,
                                SeasonId = statline.seasonId,
                                ShotsAgainstPerGame = statline.shotsAgainstPerGame,
                                ShotsForPerGame = statline.shotsForPerGame,
                                Ties = statline.ties,
                                Wins = statline.wins,
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with the team stats: {0}", e);
            }
        }
    }
}
