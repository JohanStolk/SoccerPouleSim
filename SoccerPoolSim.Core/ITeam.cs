using System;
using System.Collections.Generic;
using System.Text;

namespace SoccerPoolSim.Core
{
    public interface ITeam
    {
        string Name { get; init; }
        float Rating { get; set; }
    }
}
