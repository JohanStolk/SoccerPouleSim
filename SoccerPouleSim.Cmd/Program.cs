using SoccerPouleSim.Core;
using System;

namespace SoccerPouleSim.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            ITeam a = new SoccerTeam { Name = "The Netherlands", Rating = 0.9f };
            ITeam b = new SoccerTeam { Name = "Soviet Union", Rating = 0.9f };
            ITeam c = new SoccerTeam { Name = "Republic of Ireland", Rating = 0.3f };
            ITeam d = new SoccerTeam { Name = "England", Rating = 0.6f };

            IPoule poule = new Poule { Name = "Group 2" };
            poule.Teams.Add(a);
            poule.Teams.Add(b);
            poule.Teams.Add(c);
            poule.Teams.Add(d);

            poule.GenerateMatches();

            foreach (Match match in poule.Matches)
                Console.WriteLine(match.Team1.Name + " - " + match.Team2.Name);

        }
    }
}
