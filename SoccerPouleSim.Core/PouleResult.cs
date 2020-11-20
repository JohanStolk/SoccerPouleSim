using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    /// <summary>
    /// the total result of one team in a poule
    /// </summary>
    public class PouleResult
    {
        public ITeam Team { get; init; }

        public int Played { get; set; }
        public int Won { get; set; }
        public int Draw { get; set; }
        public int Lost { get; set; }

        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get { return GoalsFor - GoalsAgainst; } } // aka "doelsaldo" (Dutch)

        public int Points { get; set; }

        /// <summary>
        /// a comparer class to sort poule results
        /// implemented as a nested class because it is tightly linked to the containing class
        /// </summary>
        public class Comparer : Comparer<PouleResult>
        {
            /// <summary>
            /// the poule for which the poule results are being sorted
            /// </summary>
            private Poule poule;

            /// <summary>
            /// we need access to the Poule data in case there's a tie on points & goal difference
            /// </summary>
            /// <param name="poule"></param>
            public Comparer(Poule poule)
            {
                this.poule = poule;
            }
            /// <summary>
            /// TODO unit test this!
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public override int Compare(PouleResult x, PouleResult y)
            {
                if (x.Points == y.Points)
                {
                    if (x.GoalDifference == y.GoalDifference)
                    {
                        if (x.GoalDifference == y.GoalDifference)
                        {
                            if (x.GoalDifference == y.GoalDifference)
                            {
                                // the tricky case: need to compare matches when points & goal difference are the same
                                return poule.CompareEqualPointsAndGoalDifferenceTie(x, y);
                            }
                            return x.GoalsAgainst > y.GoalsAgainst ? 1 : -1;
                        }
                        return x.GoalsFor < y.GoalsFor ? 1 : -1;
                    }
                    return x.GoalDifference < y.GoalDifference ? 1 : -1;
                }
                return x.Points < y.Points ? 1 : -1;
            }
        }
    }
}
