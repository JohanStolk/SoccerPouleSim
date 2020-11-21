using SoccerPoolSim.Core;
using System;
using System.Linq;

namespace SoccerPoolSim.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            ITeam a = new SoccerTeam("The Netherlands") { Rating = 1f };
            ITeam b = new SoccerTeam("Soviet Union") { Rating = 0.9f };
            ITeam c = new SoccerTeam("Republic of Ireland") { Rating = 0f };
            ITeam d = new SoccerTeam("England") { Rating = 0.6f };

            IPool pool = new Pool { Name = "Group 2" };
            pool.Teams.Add(a);
            pool.Teams.Add(b);
            pool.Teams.Add(c);
            pool.Teams.Add(d);

            pool.GenerateMatches();
            pool.Sim();
            foreach (Match match in pool.Matches)
                Console.WriteLine(match.Team1.Name + " - " + match.Team2.Name + " " + match.GoalsTeam1 + "-" + match.GoalsTeam2);

            pool.GenerateResults();
            foreach (PoolResult result in pool.Results)
            {
                Console.WriteLine("{0,30} Pld {1,2} W {2} D {3} L {4} GF {5,2} GA {6,2} GD {7,3} Pts {8,3}",
                    result.Team.Name, result.Played, result.Won, result.Draw, result.Lost, result.GoalsFor, result.GoalsAgainst, result.GoalDifference.ToString("+#;-#;0"), result.Points);
            }
        }
    }
}
