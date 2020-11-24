using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// abstract base class for all simulators, contains nested classes with specific implementations
    /// </summary>
    public abstract partial class SoccerPoolSimulator : ISoccerPoolSimulator
    {
        /// <summary>
        /// this simulator generates equal goal difference and goals-for so mutual result must be checked for sorting 
        /// </summary>
        public class MutualResultGenerator : SoccerPoolSimulator
        {
            /// <summary>
            /// simulate the pool by generating match results
            /// </summary>
            /// <param name="pool">the pool to simulate</param>
            public override void Simulate(Pool pool)
            {
                foreach (Match match in pool.Matches)
                    match.GoalsTeam1 = match.GoalsTeam2 = 2;

                if (pool.Teams.Count < 4)
                    throw new SoccerPoolSimException("expected at least 4 teams in pool " + pool);

                Team team1 = pool.Teams[0];
                Team team2 = pool.Teams[1];
                Team team3 = pool.Teams[2];
                Team team4 = pool.Teams[3];
                pool.FindMatch(team1, team2).ScoreGoal(team1);  // mutual result, team 1 is the winner, now team2 lost one, team1 won one, compensate below 
                pool.FindMatch(team1, team3).CancelGoal(team1); // now team1 also has a loss
                pool.FindMatch(team3, team2).ScoreGoal(team2);  // now team2 also has a win
                pool.FindMatch(team4, team2).CancelGoal(team2); // now the goal difference is equal again
                pool.FindMatch(team4, team2).CancelGoal(team4); // but result must stay a draw
                // lower goal difference for other teams:
                pool.FindMatch(team3, team4).CancelGoal(team4).CancelGoal(team3);
            }
        }
    }
}
