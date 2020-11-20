using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    public class PouleResult
    {
        public ITeam Team { get; init; }

        public int Played { get; set; }
        public int Won { get; set; }
        public int Draw { get; set; }
        public int Lost { get; set; }

        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; } // aka "doelsaldo" (Dutch)

        public int Points { get; set; }
    }
}
