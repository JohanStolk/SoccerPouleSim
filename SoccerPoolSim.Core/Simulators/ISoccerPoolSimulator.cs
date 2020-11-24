using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// interface for all simulators
    /// </summary>
    public interface ISoccerPoolSimulator
    {
        /// <summary>
        /// Name of the simulator
        /// </summary>
        string Name { get; }

        /// <summary>
        /// simulate the pool by generating match results
        /// </summary>
        /// <param name="pool">the pool to simulate</param>
        void Simulate(Pool pool);
    }
}
