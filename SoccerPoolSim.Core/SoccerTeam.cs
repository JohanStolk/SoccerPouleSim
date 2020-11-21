using System;
using System.Collections.Generic;
using System.Text;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// Soccer team with name and rating
    /// </summary>
    public class SoccerTeam : ITeam
    {
        /// <summary>
        /// Team name e.g. "The Netherlands"
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// Team Rating [0f..1f] 0f = loser 1f = winner, higher rated teams have more chance of winning against lower rated teams
        /// </summary>
        public float Rating { get; set; } = 0.5f;

        public SoccerTeam(string name)
        {
            Name = name;
        }
    }
}
