using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    public interface IPool
    {
        string Name { get; init; }
        List<ITeam> Teams { get; }
        List<Match> Matches { get; }
        List<PoolResult> Results { get; }
        void GenerateMatches();

        void GenerateResults();
        void PrintResults();
        void PrintMatches();
    }
}
