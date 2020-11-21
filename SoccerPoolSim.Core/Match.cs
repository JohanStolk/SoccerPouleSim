using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    public class Match
    {
        public ITeam Team1 { get; }
        public ITeam Team2 { get; }

        public int GoalsTeam1 { get; set; }
        public int GoalsTeam2 { get; set; }

        public Match(ITeam team1, ITeam team2)
        {
            Team1 = team1;
            Team2 = team2;
        }
    }
}
