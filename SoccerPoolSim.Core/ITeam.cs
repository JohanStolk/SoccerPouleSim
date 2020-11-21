using System;
using System.Collections.Generic;
using System.Text;

namespace SoccerPouleSim.Core
{
    public interface ITeam
    {
        string Name { get; init; }
        float Rating { get; set; }
    }
}
