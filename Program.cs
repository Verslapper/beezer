using System;
using System.IO;
using System.Net;
using System.Text;
using Beezer.Model;
using Beezer.Repository;
using Newtonsoft.Json;

namespace Beezer
{
    class Program
    {
        private static readonly StatlineRepository _statlineRepository = new StatlineRepository();

        static void Main(string[] args)
        {
            Grabbit();
        }

        private static void Grabbit()
        {
            try
            {
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
                                Date = DateTime.Now,
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
                                TeamAbbrev = statline.teamAbbrev,
                                TeamFullName = statline.teamFullName,
                                TeamId = statline.teamId,
                                Ties = statline.ties,
                                Wins = statline.wins,
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // for 404s, move along
                Console.WriteLine("well fuck.");
            }
        }
    }
}
