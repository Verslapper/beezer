using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Beezer.Enums;
using Beezer.Model;
using Beezer.Repository;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Beezer
{
    class Program
    {
        private static readonly StatlineRepository _statlineRepository = new StatlineRepository();
        private static readonly GameRepository _gameRepository = new GameRepository();

        static void Main(string[] args)
        {
            Console.WriteLine("Puck drop!");
            GetTeamStats();
            Console.WriteLine("End of the first!");
            GetSchedule();
            Console.WriteLine("That's the horn!");
            Thread.Sleep(420);
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

                            if (awayScore.HasValue && homeScore.HasValue)
                            {
                                game.AwayScore = awayScore;
                                game.HomeScore = homeScore;
                            }

                            // TODO: Create mode to only scan for updates since last checked date, instead of overwriting everything.
                            // Not much point checking if it exists then not adding it, not here
                            _gameRepository.Update(game);
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
            catch (Exception)
            {
                // for 404s, move along
                Console.WriteLine("Something went wrong with the team stats.");
            }
        }
    }
}
