using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    public abstract partial class SoccerPoolSimulator : ISoccerPoolSimulator
    {
        private static Random random = new();

        public string Name => GetType().Name; 

        public abstract void Simulate(IPool pool);
    }
}
