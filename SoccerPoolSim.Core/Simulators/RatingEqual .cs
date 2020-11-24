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
        /// this simulator generates results where the # goals depend on the rating so equal rating has equal results
        /// </summary>
        public class RatingEqual : SoccerPoolSimulator
        {
            /// <summary>
            /// simulate the pool by generating match results
            /// </summary>
            /// <param name="pool">the pool to simulate</param>
            public override void Simulate(Pool pool)
            {
                foreach (Match match in pool.Matches)
                {
                    match.GoalsTeam1 = (int)(5 * match.Team1.Rating);
                    match.GoalsTeam2 = (int)(5 * match.Team2.Rating);
                }
            }
        }
    }
}
