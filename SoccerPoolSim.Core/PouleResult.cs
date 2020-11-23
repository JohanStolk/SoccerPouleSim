using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// the total result of one team in a pool
    /// </summary>
    public class PoolResult
    {
        public Team Team { get; init; }
        public int Position { get; internal set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Draw { get; set; }
        public int Lost { get; set; }

        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get { return GoalsFor - GoalsAgainst; } } // aka "doelsaldo" (Dutch)

        public int Points { get; set; }
        public bool IsTie { get; internal set; }
        public string GoalDifferenceString => GoalDifference.ToString("+#;-#;0");

        public PoolResult(Team team)
        {
            Team = team;
        }

        /// <summary>
        /// serialize complete object to string for maximum debug info using Json.NET
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// a comparer class to sort pool results
        /// implemented as a nested class because it is tightly linked to the containing class
        /// </summary>
        public class Comparer : Comparer<PoolResult>
        {
            /// <summary>
            /// the pool for which the pool results are being sorted
            /// </summary>
            private Pool pool;

            /// <summary>
            /// we need access to the Pool data in case there's a tie on points & goal difference
            /// </summary>
            /// <param name="pool"></param>
            public Comparer(Pool pool)
            {
                this.pool = pool;
            }
            /// <summary>
            /// TODO unit test this!
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public override int Compare(PoolResult ?x, PoolResult ?y)
            {
                if (x == null)
                    return -1;
                if (y == null)
                    return 1;

                if (x.Points == y.Points)
                {
                    if (x.GoalDifference == y.GoalDifference)
                    {
                        if (x.GoalsFor == y.GoalsFor)
                        {
                            if (x.GoalsAgainst == y.GoalsAgainst)
                            {
                                // the tricky case: need to compare matches when points & goal difference are the same
                                return pool.CompareMutualResult(x, y);
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
