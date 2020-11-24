using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// abstract base class for all simulators, contains nested classes with specific implementations 
    /// (in seperate files using partial class construct)
    /// </summary>
    public abstract partial class SoccerPoolSimulator : ISoccerPoolSimulator
    {
        /// <summary>
        /// get the name by reflection
        /// </summary>
        public string Name => GetType().Name;

        /// <summary>
        /// simulate the pool by generating match results
        /// </summary>
        /// <param name="pool">the pool to simulate</param>
        public abstract void Simulate(Pool pool);

        /// <summary>
        /// the static collection of all simulators, key = simulator name
        /// </summary>
        public static Dictionary<string, SoccerPoolSimulator> Simulators { get { return simulators; } }

        /// <summary>
        /// utility object to generate random numbers for simulations
        /// </summary>
        private static Random random = new();

        /// <summary>
        /// code to generate the Simulators collection using reflection for use in the website & unit tests
        /// </summary>
        private static readonly List<Type> simulatorTypes = SoccerSimTools.FindAllDerivedTypes<SoccerPoolSimulator>();
        private static readonly Dictionary<string, SoccerPoolSimulator> simulators = new();

        /// <summary>
        /// static ctor to initialize simulatorTypes collection
        /// </summary>
        static SoccerPoolSimulator()
        {
            foreach (Type simulatorType in simulatorTypes)
                simulators[simulatorType.Name] = SoccerSimTools.CreateInstanceOfType<SoccerPoolSimulator>(simulatorType);
        }
    }
}