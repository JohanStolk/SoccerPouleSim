﻿using System;
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
        public class AllEqual : SoccerPoolSimulator
        {
            public override void Simulate(IPool pool)
            {
                foreach (Match match in pool.Matches)
                {
                    match.GoalsTeam1 = match.GoalsTeam2 = 2;
                }
            }
        }
    }
}
