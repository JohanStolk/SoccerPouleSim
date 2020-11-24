using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    public abstract partial class SoccerPoolSimulator : ISoccerPoolSimulator
    {
        /// <summary>
        /// this simulator generates a tie with different goal difference to test sorting
        /// </summary>
        public class GoalDifferenceTie : SoccerPoolSimulator
        {
            /// <summary>
            /// simulate the pool by generating match results
            /// </summary>
            /// <param name="pool">the pool to simulate</param>
            public override void Simulate(Pool pool)
            {
                foreach (Match match in pool.Matches)
                    match.GoalsTeam1 = match.GoalsTeam2 = 2;

                if (pool.Teams.Count < 3)
                    throw new SoccerPoolSimException("expected at least 3 teams in pool " + pool);

                Team team1 = pool.Teams[0];
                Team team2 = pool.Teams[1];
                Team team3 = pool.Teams[2];
                pool.FindMatch(team1, team3).ScoreGoal(team1);  // now team1 has a win with +1 goal
                pool.FindMatch(team2, team3).CancelGoal(team3); // now team2 also has a win but -1 goal
            }
        }
    }
}
