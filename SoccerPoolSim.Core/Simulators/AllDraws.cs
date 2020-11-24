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
        /// this simulator generates only draws
        /// </summary>
        public class AllDraws : SoccerPoolSimulator
        {
            /// <summary>
            /// simulate the pool by generating match results
            /// </summary>
            /// <param name="pool">the pool to simulate</param>
            public override void Simulate(Pool pool)
            {
                foreach (Match match in pool.Matches)
                {
                    float r1 = match.Team1.Rating;
                    float r2 = match.Team2.Rating;
                    // higher rating more goals but still a draw
                    match.GoalsTeam1 = match.GoalsTeam2 = random.Next(0, (int)(10 *(r1 + r2)));
                }
            }
        }
    }
}
