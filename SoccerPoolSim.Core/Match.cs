using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    public class Match
    {
        public Team Team1 { get; }
        public Team Team2 { get; }

        public int GoalsTeam1 { get; set; }
        public int GoalsTeam2 { get; set; }

        public Match(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }

        /// <summary>
        /// serialize complete object to string for maximum debug info using Json.NET
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Match ScoreGoal(Team team)
        {
            if (team == Team1)
                GoalsTeam1++;
            else if (team == Team2)
                GoalsTeam2++;
            else
                throw new SoccerPoolSimException("couldn't find team {0} in match {1}", team.Name, this);
            return this;
        }

        public Match CancelGoal(Team team)
        {
            if (team == Team1)
            {
                if (GoalsTeam1 == 0)
                    throw new SoccerPoolSimException("no goal to cancel for team {0} in match {1}", team.Name, this);
                GoalsTeam1--;
            }
            else if (team == Team2)
            {
                if (GoalsTeam1 == 0)
                    throw new SoccerPoolSimException("no goal to cancel for team {0} in match {1}", team.Name, this);

                GoalsTeam2--;
            }
            else
                throw new SoccerPoolSimException("couldn't find team {0} in match {1}", team.Name, this);

            return this;
        }
    }
}
