using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    /// <summary>
    /// Poule with name
    /// </summary>
    public class Poule : IPoule
    {
        public string Name { get; init; }
        public List<ITeam> Teams { get; } = new();

        public List<Match> Matches { get; } = new();

        public void Sim()
        {

        }
        public void GenerateMatches()
        {
            Matches.Clear();
            for (int i = 0; i < Teams.Count; i++)
            {
                for (int j = i + 1; j < Teams.Count; j++)
                {
                    if ((i & 1) == 0)
                    {
                        Matches.Add(new Match { Team1 = Teams[i], Team2 = Teams[j] });
                    }
                    else
                    {
                        Matches.Add(new Match { Team1 = Teams[j], Team2 = Teams[i] });
                    }
                }
            }
        }
    }
}
