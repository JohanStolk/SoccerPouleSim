using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// Pool with name
    /// </summary>
    public class Pool
    {
        public string Name { get; init; } = "Unnamed Pool";
        public List<Team> Teams { get; } = new();
        public List<Match> Matches { get; } = new();
        public List<PoolResult> Results { get; private set; } = new();

        public void GenerateMatches()
        {
            bool swap = false;
            Matches.Clear();
            for (int i = 0; i < Teams.Count; i++)
            {
                for (int j = i + 1; j < Teams.Count; j++)
                {
                    if (swap)
                        Matches.Add(new Match(Teams[j], Teams[i]));
                    else
                        Matches.Add(new Match(Teams[i], Teams[j]));

                    swap = !swap;
                }
            }
        }

        public Match FindMatch(Team team1, Team team2)
        {
            return Matches.First(m => m.Team1 == team1 && m.Team2 == team2 || m.Team1 == team2 && m.Team2 == team1);
        }

        public int CompareMutualResult(PoolResult x, PoolResult y)
        {
            // if these requirements are not met we shouldn't compare mutual results
            if (x.Points != y.Points)
                throw new SoccerPoolSimException.PointsNotEqual(x, y);
            if (x.GoalDifference != y.GoalDifference)
                throw new SoccerPoolSimException.GoalDifferenceNotEqual(x, y);
            if (x.GoalsFor != y.GoalsFor)
                throw new SoccerPoolSimException.GoalsForNotEqual(x, y);
            // the case below can never happen as GoalsAgainst must be the same in case of same GoalDifference & same GoalsFor
            // if (x.GoalsAgainst != y.GoalsAgainst)
            //    throw new SoccerPoolSimException.GoalsAgainstNotEqual(x, y);

            int mutualGoals = 0;
            foreach (Match match in Matches)
            {
                if (match.Team1 == x.Team && match.Team2 == y.Team)
                    mutualGoals += match.GoalsTeam1 - match.GoalsTeam2;
                else if (match.Team1 == y.Team && match.Team2 == x.Team)
                    mutualGoals += match.GoalsTeam2 - match.GoalsTeam1;
            }
            x.IsTie = y.IsTie = (mutualGoals == 0);
            return mutualGoals;
        }

        public void GenerateResults()
        {
            Results.Clear();

            // create a temporary hashtable to achieve fast lookup of result by team
            Dictionary<Team, PoolResult> fastLookUp = new();

            // create the results and fill the hashtable
            foreach (Team team in Teams)
            {
                PoolResult result = new PoolResult(team);
                fastLookUp[team] = result;
                Results.Add(result);
            }

            // add all data from the matches to the results
            foreach (Match match in Matches)
            {
                PoolResult poolResult1 = fastLookUp[match.Team1];
                PoolResult poolResult2 = fastLookUp[match.Team2];

                poolResult1.Played++;
                poolResult2.Played++;

                poolResult1.GoalsFor += match.GoalsTeam1;
                poolResult1.GoalsAgainst += match.GoalsTeam2;

                poolResult2.GoalsFor += match.GoalsTeam2;
                poolResult2.GoalsAgainst += match.GoalsTeam1;

                if (match.GoalsTeam1 == match.GoalsTeam2)
                {
                    poolResult1.Draw++;
                    poolResult2.Draw++;
                    poolResult1.Points++;
                    poolResult2.Points++;
                }
                else if (match.GoalsTeam1 > match.GoalsTeam2)
                {
                    poolResult1.Won++;
                    poolResult2.Lost++;
                    poolResult1.Points += 2;
                }
                else
                {
                    poolResult2.Won++;
                    poolResult1.Lost++;
                    poolResult2.Points += 2;
                }
            }

            // sort using the special comparer
            Results.Sort(new PoolResult.Comparer(this));

            // set the positions according to whether ties have been detected or not
            int position = 1;
            int tiePosition = 1;
            bool previousTie = false;
            foreach (PoolResult result in Results)
            {
                result.Position = result.IsTie && previousTie ? tiePosition : position;
                previousTie = result.IsTie;
                position++;
                if (!result.IsTie)
                    tiePosition = position;
            }
        }

        /// <summary>
        /// serialize complete object to string for maximum debug info using Json.NET
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AsJSON();
        }
        public void PrintResults()
        {
            foreach (PoolResult result in Results)
            {
                Console.WriteLine("{0,30} Pos {9,2} Pld {1,2} W {2} D {3} L {4} GF {5,2} GA {6,2} GD {7,3} Pts {8,3} {10}",
                    result.Team.Name, result.Played, result.Won, result.Draw, result.Lost,
                    result.GoalsFor, result.GoalsAgainst, result.GoalDifferenceString,
                    result.Points, result.Position, result.IsTie ? "tie" : "");
            }
        }

        public void PrintMatches()
        {
            foreach (Match match in Matches)
                Console.WriteLine(match.Team1.Name + " - " + match.Team2.Name + " " + match.GoalsTeam1 + "-" + match.GoalsTeam2);
        }

        public static Pool GenerateEK88Group1()
        {
            Pool pool = new Pool { Name = "EK 88 Group 1" };
            pool.Teams.Add(new Team("West Germany") { Rating = 0.8f });
            pool.Teams.Add(new Team("Italy") { Rating = 0.8f });
            pool.Teams.Add(new Team("Spain") { Rating = 0.5f });
            pool.Teams.Add(new Team("Denmark") { Rating = 0.2f });
            return pool;
        }
        public static Pool GenerateEK88Group2()
        {
            Pool pool = new Pool { Name = "EK 88 Group 2" };
            pool.Teams.Add(new Team("The Netherlands") { Rating = 0.9f });
            pool.Teams.Add(new Team("Soviet Union") { Rating = 0.9f });
            pool.Teams.Add(new Team("Republic of Ireland") { Rating = 0.2f });
            pool.Teams.Add(new Team("England") { Rating = 0.6f });
            return pool;
        }

        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
        public string AsJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, jsonSerializerSettings);
        }
        public static Pool FromJSON(string json)
        {
            Pool? pool = JsonConvert.DeserializeObject<Pool>(json, jsonSerializerSettings);
            if (pool == null)
                throw new SoccerPoolSimException("couldn't DeserializeObject<Pool> from " + json);
            return pool;
        }

        public void Save(string path)
        {
            using (StreamWriter sw = File.CreateText(path))
                sw.WriteLine(AsJSON());
        }
        public static Pool Load(string path)
        {
            using (StreamReader sr = File.OpenText(path))
                return FromJSON(sr.ReadToEnd());
        }
    }
}
