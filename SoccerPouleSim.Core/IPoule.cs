using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    public interface IPoule
    {
        string Name { get; init; }
        List<ITeam> Teams { get; }
        List<Match> Matches { get; }
        List<PouleResult> Results { get; }
        void GenerateMatches();

        void Sim();
        void GenerateResults();
    }
}
