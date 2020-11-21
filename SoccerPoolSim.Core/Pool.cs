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

        private Random random = new();

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

        public void Sim1()
        {
            foreach (Match match in Matches)
            {
                float r1 = match.Team1.Rating;
                float r2 = match.Team2.Rating;
                float diff = r2 - r1; // diff [0f..1f] 0 if teams equal, -1/1 if max difference

                // the bigger the difference the more change of high score goals (13 as a limit)
                // plus some random goals to prevent 0-0 if equal ratings
                int goals = random.Next(0, (int)(Math.Abs(diff) * 13)) + random.Next(0, 4);

                // the higher the winner factor the more chance that the stronger team wins (max 0.5f)
                float winnerFactor = 0.5f;
                float center = 0.5f + winnerFactor * diff;
                for (int i = 0; i < goals; i++)
                {
                    if (random.NextDouble() > center)
                        match.GoalsTeam1++;
                    else
                        match.GoalsTeam2++;
                }
            }
        }
        public void Sim()// Mutual()
        {
            foreach (Match match in Matches)
            {
                float r1 = match.Team1.Rating;
                float r2 = match.Team2.Rating;
                match.GoalsTeam1 += (int) (10 * r1);
                match.GoalsTeam2 += (int)(10 * r2);                
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
            //            Results = Results.OrderByDescending(r => r.Points).ToList();
            Results.Sort(new PoolResult.Comparer(this));
        }
    }
}
