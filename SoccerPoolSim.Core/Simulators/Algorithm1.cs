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
        public class Algorithm1 : SoccerPoolSimulator
        {
            public override void Simulate(IPool pool)
            {
                foreach (Match match in pool.Matches)
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
        }
    }
}
