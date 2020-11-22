using SoccerPoolSim.Core;
using System;
using System.Linq;

namespace SoccerPoolSim.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            ITeam a = new SoccerTeam("The Netherlands") { Rating = 0.9f };
            ITeam b = new SoccerTeam("Soviet Union") { Rating = 0.9f };
            ITeam c = new SoccerTeam("Republic of Ireland") { Rating = 0.2f };
            ITeam d = new SoccerTeam("England") { Rating = 0.6f };

            IPool pool = new Pool { Name = "Group 2" };
            pool.Teams.Add(a);
            pool.Teams.Add(b);
            pool.Teams.Add(c);
            pool.Teams.Add(d);
            pool.Teams.Add(new SoccerTeam("San Marino") { Rating = 0 });
            pool.Teams.Add(new SoccerTeam("San Marino 2") { Rating = 0 });
            pool.Teams.Add(new SoccerTeam("San Marino 3") { Rating = 0 });

            pool.GenerateMatches();

            Simulate(new SoccerPoolSimulator.Algorithm1(), pool);
            Simulate(new SoccerPoolSimulator.Draws(), pool);
            Simulate(new SoccerPoolSimulator.MutualResultGenerator(), pool);
            Simulate(new SoccerPoolSimulator.AllEqual(), pool);
            Simulate(new SoccerPoolSimulator.RatingEqual(), pool);
        }

        static void Simulate (ISoccerPoolSimulator simulator, IPool pool)
        {
            Console.WriteLine("\nusing simulator: " + simulator.Name);
            simulator.Simulate(pool);
            pool.PrintMatches();
            pool.GenerateResults();
            pool.PrintResults();

        }
    }
}
