using SoccerPoolSim.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoccerPoolSim.Cmd
{
    class Program
    {
        /// <summary>
        /// commandline test environment for quick prototyping
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Pool pool = Pool.GenerateEK88Group2(); 

           // pool.Teams.Add(new Team("San Marino") { Rating = 0 });
           // pool.Teams.Add(new Team("San Marino 2") { Rating = 0 });
           // pool.Teams.Add(new Team("San Marino 3") { Rating = 0 });

            pool.GenerateMatches();

            // test all known simulators
            List<Type> simulators = SoccerSimTools.FindAllDerivedTypes<SoccerPoolSimulator>();
            foreach (Type simulatorType in simulators)
                Simulate(SoccerSimTools.CreateInstanceOfType<SoccerPoolSimulator>(simulatorType), pool);            
        }

        /// <summary>
        /// test a given simulator and output the results
        /// </summary>
        /// <param name="simulator"></param>
        /// <param name="pool"></param>
        static void Simulate (ISoccerPoolSimulator simulator, Pool pool)
        {
            Console.WriteLine("\nusing simulator: " + simulator.Name);
            simulator.Simulate(pool);
            pool.PrintMatches();
            pool.GenerateResults();
            pool.PrintResults();
        }
    }
}
