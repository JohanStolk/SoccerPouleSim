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
        /// this simulator generates matches according to a custom algorithm 
        /// </summary>
        public class Algorithm2 : SoccerPoolSimulator
        {
            /// <summary>
            /// rare chance that stronger team scores [0.0..1.0]
            /// </summary>
            public double WinnerScoreFactor { get; set; } = 0.8;
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
                    Team weakest = r1 < r2 ? match.Team1 : match.Team2;
                    Team strongest = r1 < r2 ? match.Team2 : match.Team1;

                    // the bigger the difference the more change of goals
                    int totalGoals = random.Next(0, (int)(Math.Abs(r2 - r1) * 10));

                    for (int i = 0; i < totalGoals; i++)
                    {
                        double luckyNumber = random.NextDouble();
                        if (luckyNumber < WinnerScoreFactor)
                            match.ScoreGoal(strongest);
                        else
                            match.ScoreGoal(weakest);
                    }
                }
            }
        }
    }
}
