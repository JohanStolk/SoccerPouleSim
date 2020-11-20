using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    public class Match
    {
        public ITeam Team1 { get; init; }
        public ITeam Team2 { get; init; }

        public int GoalsTeam1 { get; init; }
        public int GoalsTeam2 { get; init; }
    }
}
