using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPoolSim.Core
{
    /// <summary>
    /// an exception specific to our assembly
    /// </summary>
    public class SoccerPoolSimException : Exception
    {
        /// <summary>
        /// simple message ctor
        /// </summary>
        /// <param name="message"></param>
        public SoccerPoolSimException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// string format ctor
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public SoccerPoolSimException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
        /// <summary>
        /// nested class helper for exception case where points not equal between to pool results
        /// </summary>
        public class PointsNotEqual : SoccerPoolSimException
        {
            public PointsNotEqual(PoolResult x, PoolResult y)
                : base("Teams do not have the same points {0} : {1} pts, {2} : {3} pts", x.Team.Name, x.Points, y.Team.Name, y.Points)
            {
            }
        }
        /// <summary>
        /// nested class helper for exception case where goal difference not equal between to pool results
        /// </summary>
        public class GoalDifferenceNotEqual : SoccerPoolSimException
        {
            public GoalDifferenceNotEqual(PoolResult x, PoolResult y)
                : base("Teams do not have the same GoalDifference {0} : {1} GD, {2} : {3} GD", x.Team.Name, x.GoalDifference, y.Team.Name, y.GoalDifference)
            {
            }
        }

        /// <summary>
        /// nested class helper for exception case where goals for not equal between to pool results
        /// </summary>
        public class GoalsForNotEqual : SoccerPoolSimException
        {
            public GoalsForNotEqual(PoolResult x, PoolResult y)
                : base("Teams do not have the same GoalsFor {0} : {1} GD, {2} : {3} GD", x.Team.Name, x.GoalsFor, y.Team.Name, y.GoalsFor)
            {
            }
        }

        /// <summary>
        /// nested class helper for exception case where goals against not equal between to pool results
        /// </summary>
        public class GoalsAgainstNotEqual : SoccerPoolSimException
        {
            public GoalsAgainstNotEqual(PoolResult x, PoolResult y)
                : base("Teams do not have the same GoalsAgainst {0} : {1} GD, {2} : {3} GD", x.Team.Name, x.GoalsAgainst, y.Team.Name, y.GoalsAgainst)
            {
            }
        }
    }
}
