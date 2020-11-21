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
        public List<PouleResult> Results { get; private set; } = new();

        Random random = new();

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

        public void Sim()
        {
            foreach (Match match in Matches)
            {
                float r1 = match.Team1.Rating;
                float r2 = match.Team2.Rating;
                float diff = r2 - r1; // diff [0f..1f] 0 if teams equal, -1/1 if max difference

                // the bigger the difference the more change of high score goals (13 as a limit)
                // plus some random goals to prevent 0-0 if equal ratings
                int goals = random.Next(0, (int)(Math.Abs(diff) * 13)) + random.Next(0, 4);

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

        internal int CompareEqualPointsAndGoalDifferenceTie(PouleResult x, PouleResult y)
        {
            if (x.Points != y.Points)
                throw new SoccerSimException("Teams do not have the same points {0} : {1} pts, {2} : {3} pts", x.Team.Name, x.Points, y.Team.Name, y.Points);
            if (x.GoalDifference != y.GoalDifference)
                throw new SoccerSimException("Teams do not have the same GoalDifference {0} : {1} GD, {2} : {3} GD", x.Team.Name, x.GoalDifference, y.Team.Name, y.GoalDifference);
            if (x.GoalsFor != y.GoalsFor)
                throw new SoccerSimException("Teams do not have the same GoalsFor {0} : {1} GD, {2} : {3} GD", x.Team.Name, x.GoalsFor, y.Team.Name, y.GoalsFor);
            if (x.GoalsAgainst != y.GoalsAgainst)
                throw new SoccerSimException("Teams do not have the same GoalsAgainst {0} : {1} GD, {2} : {3} GD", x.Team.Name, x.GoalsAgainst, y.Team.Name, y.GoalsAgainst);


            return 0;
        }

        public void GenerateResults()
        {
            Results.Clear();

            Dictionary<ITeam, PouleResult> fastLookUp = new();
            foreach (ITeam team in Teams)
            {
                PouleResult result = new PouleResult { Team = team };
                fastLookUp[team] = result;
                Results.Add(result);
            }

            foreach (Match match in Matches)
            {
                PouleResult pouleResult1 = fastLookUp[match.Team1];
                PouleResult pouleResult2 = fastLookUp[match.Team2];

                pouleResult1.Played++;
                pouleResult2.Played++;

                pouleResult1.GoalsFor += match.GoalsTeam1;
                pouleResult1.GoalsAgainst += match.GoalsTeam2;

                pouleResult2.GoalsFor += match.GoalsTeam2;
                pouleResult2.GoalsAgainst += match.GoalsTeam1;

                if (match.GoalsTeam1 == match.GoalsTeam2)
                {
                    pouleResult1.Draw++;
                    pouleResult2.Draw++;
                    pouleResult1.Points++;
                    pouleResult2.Points++;
                }
                else if (match.GoalsTeam1 > match.GoalsTeam2)
                {
                    pouleResult1.Won++;
                    pouleResult2.Lost++;
                    pouleResult1.Points += 2;
                }
                else
                {
                    pouleResult2.Won++;
                    pouleResult1.Lost++;
                    pouleResult2.Points += 2;
                }
            }
            //            Results = Results.OrderByDescending(r => r.Points).ToList();
            Results.Sort(new PouleResult.Comparer(this));
        }
    }
}
