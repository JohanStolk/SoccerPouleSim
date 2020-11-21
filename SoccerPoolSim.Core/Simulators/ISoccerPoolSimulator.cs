﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    public interface ISoccerPoolSimulator
    {
        string Name { get; }

        void Simulate(IPool pool);
    }
}
