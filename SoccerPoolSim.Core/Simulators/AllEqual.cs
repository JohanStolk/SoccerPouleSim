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
        /// this simulator generates all equal results
        /// </summary>
        public class AllEqual : SoccerPoolSimulator
        {
            /// <summary>
            /// simulate the pool by generating match results
            /// </summary>
            /// <param name="pool">the pool to simulate</param>
            public override void Simulate(Pool pool)
            {
                // all matches 2-2 so all teams #1
                foreach (Match match in pool.Matches)
                    match.GoalsTeam1 = match.GoalsTeam2 = 2;
            }
        }
    }
}
