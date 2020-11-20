using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    /// <summary>
    /// Poule with name
    /// </summary>
    public class Poule : IPoule
    {
        public string Name { get; init; }
        public List<ITeam> Teams { get; } = new();

        public List<Match> Matches { get; } = new();
        public List<PouleResult> Results { get; } = new();

        Random random = new();
        public void Sim()
        {
            foreach (Match match in Matches)
            {
                float r1 = match.Team1.Rating;
                float r2 = match.Team2.Rating;
                float diff = r2 - r1; // diff [0f..1f] 0 if teams equal, -1/1 if max difference

                // the bigger the difference the more change of high score goals (13 as a limit)
                // plus some random goals to prevent 0-0 if equal ratings
                int goals = random.Next(0, (int)(Math.Abs(diff) * 13)) + random.Next(0, 3);

                // the higher the winner factor the more chance that the stronger team wins (max 0.5f)
                float winnerFactor = 0.5f;
                float center = 0.5f + winnerFactor*diff;
                for (int i = 0; i < goals; i++)
                {
                    if (random.NextDouble() > center)
                        match.GoalsTeam1++;
                    else
                        match.GoalsTeam2++;
                }
            }
        }
        public void GenerateMatches()
        {
            bool swap = false;
            Matches.Clear();
            for (int i = 0; i < Teams.Count; i++)
            {
                for (int j = i + 1; j < Teams.Count; j++)
                {
                    if (swap)
                        Matches.Add(new Match { Team1 = Teams[j], Team2 = Teams[i] });
                    else
                        Matches.Add(new Match { Team1 = Teams[i], Team2 = Teams[j] });

                    swap = !swap;
                }
            }
        }
    }
}
