using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// Pool with name
    /// </summary>
    public class Pool : IPool
    {
        public string Name { get; init; } = "Unnamed Pool";
        public List<ITeam> Teams { get; } = new();
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

        internal int CompareMutualResult(PoolResult x, PoolResult y)
        {
            // if these requirements are not met we shouldn't compare mutual results
            if (x.Points != y.Points)
                throw new SoccerPoolSimException.PointsNotEqual(x, y);
            if (x.GoalDifference != y.GoalDifference)
                throw new SoccerPoolSimException.GoalDifferenceNotEqual(x, y);
            if (x.GoalsFor != y.GoalsFor)
                throw new SoccerPoolSimException.GoalsForNotEqual(x, y);
            if (x.GoalsAgainst != y.GoalsAgainst)
                throw new SoccerPoolSimException.GoalsAgainstNotEqual(x, y);

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

            Dictionary<ITeam, PoolResult> fastLookUp = new();
            foreach (ITeam team in Teams)
            {
                PoolResult result = new PoolResult(team);
                fastLookUp[team] = result;
                Results.Add(result);
            }

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


        public void PrintResults()
        {
            foreach (PoolResult result in Results)
            {
                Console.WriteLine("{0,30} Pos {9,2} Pld {1,2} W {2} D {3} L {4} GF {5,2} GA {6,2} GD {7,3} Pts {8,3} {10}",
                    result.Team.Name, result.Played, result.Won, result.Draw, result.Lost,
                    result.GoalsFor, result.GoalsAgainst, result.GoalDifference.ToString("+#;-#;0"), 
                    result.Points, result.Position, result.IsTie ? "tie":"");
            }
        }

        public void PrintMatches()
        {
            foreach (Match match in Matches)
                Console.WriteLine(match.Team1.Name + " - " + match.Team2.Name + " " + match.GoalsTeam1 + "-" + match.GoalsTeam2);
        }
    }
}
