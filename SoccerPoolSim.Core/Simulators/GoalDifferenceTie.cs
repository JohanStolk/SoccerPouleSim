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
        /// this simulator generates only draws
        /// </summary>
        public class GoalDifferenceTie : SoccerPoolSimulator
        {
            public override void Simulate(Pool pool)
            {
                foreach (Match match in pool.Matches)
                    match.GoalsTeam1 = match.GoalsTeam2 = 2;

                Team team1 = pool.Teams[0];
                Team team2 = pool.Teams[1];
                Team team3 = pool.Teams[2];
                Match match13 = pool.FindMatch(team1, team3);
                match13.ScoreGoal(team1);
                Match match23 = pool.FindMatch(team2, team3);
                match23.CancelGoal(team3);
            }
        }
    }
}
