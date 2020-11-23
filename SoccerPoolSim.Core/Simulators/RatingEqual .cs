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
        public class RatingEqual : SoccerPoolSimulator
        {
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
