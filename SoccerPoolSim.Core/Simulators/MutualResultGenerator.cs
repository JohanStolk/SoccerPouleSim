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
        /// this simulator generates TODO 
        /// </summary>
        public class MutualResultGenerator : SoccerPoolSimulator
        {
            public override void Simulate(IPool pool)
            {
                foreach (Match match in pool.Matches)
                {
                    float r1 = match.Team1.Rating;
                    float r2 = match.Team2.Rating;
                    match.GoalsTeam1 += (int)(10 * r1);
                    match.GoalsTeam2 += (int)(10 * r2);

                }
            }
        }
    }
}
