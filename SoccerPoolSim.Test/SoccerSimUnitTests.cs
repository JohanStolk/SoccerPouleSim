using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoccerPoolSim.Core;

namespace SoccerSim.Test
{
    /// <summary>
    /// unit test class
    /// </summary>
    [TestClass]
    public class SoccerSimUnitTests
    {
        /// <summary>
        /// helper for start scenarios in tests
        /// </summary>
        /// <returns>Pool</returns>
        public Pool GeneratePoolWithMatches()
        {
            Pool pool = Pool.GenerateEK88Group2();
            pool.GenerateMatches();
            return pool;
        }
        /// <summary>
        /// test generating matches 4 teams => 4*3/2 = 6 matches
        /// </summary>
        [TestMethod]
        public void TestGenerateMatches()
        {
            Pool pool = Pool.GenerateEK88Group2();
            Assert.IsTrue(pool.Teams.Count == 4, "ek88 group 2 should have 4 teams, not " + pool.Teams.Count);
            Assert.IsTrue(pool.Matches.Count == 0, "after generating the group no matches should be present, found " + pool.Matches.Count);
            pool.GenerateMatches();
            Assert.IsTrue(pool.Matches.Count == 6, "ek88 group 2 should have 6 matches, not " + pool.Matches.Count);
        }
        /// <summary>
        /// test generating more matches 4.. 14 teams
        /// </summary>
        [TestMethod]
        public void TestGenerateMoreMatches()
        {
            Pool pool = Pool.GenerateEK88Group2();
            Assert.IsTrue(pool.Teams.Count == 4, "ek88 group 2 should have 4 teams, not " + pool.Teams.Count);
            Assert.IsTrue(pool.Matches.Count == 0, "after generating the group no matches should be present, found " + pool.Matches.Count);
            for (int i = 0; i < 10; i++)
            {
                pool.GenerateMatches();
                int expectedMatchesCount = (pool.Teams.Count) * (pool.Teams.Count - 1)/2;
                Assert.IsTrue(pool.Matches.Count == expectedMatchesCount, "expected matches {0} but found {1} in {2}", expectedMatchesCount, pool.Matches.Count, pool);
                pool.Teams.Add(new Team("Test team " + i));
            }
        }
        /// <summary>
        /// test the CompareMutualResult for expected exceptions
        /// </summary>
        [TestMethod]
        public void TestCompareMutualResult()
        {
            Pool pool = GeneratePoolWithMatches();

            Assert.ThrowsException<SoccerPoolSimException.PointsNotEqual>(() =>
                pool.CompareMutualResult(new PoolResult(pool.Teams[0]) { Points = 2 }, new PoolResult(pool.Teams[1]) { Points = 1 }));

            Assert.ThrowsException<SoccerPoolSimException.GoalDifferenceNotEqual>(() =>
                pool.CompareMutualResult(new PoolResult(pool.Teams[0]) { GoalsFor = 2 }, new PoolResult(pool.Teams[1]) { GoalsFor = 1 }));

            Assert.ThrowsException<SoccerPoolSimException.GoalsForNotEqual>(() =>
                pool.CompareMutualResult(new PoolResult(pool.Teams[0]) { GoalsFor = 2, GoalsAgainst = 1 }, new PoolResult(pool.Teams[1]) { GoalsFor = 1 }));
        }

        /// <summary>
        /// test the GenerateResults() method
        /// </summary>
        [TestMethod]
        public void TestGenerateResults()
        {
            Pool pool = GeneratePoolWithMatches();

            pool.GenerateResults();
            Assert.IsTrue(pool.Results.Count == pool.Teams.Count, "expected # results {0} but found {1} in {2}", pool.Teams.Count, pool.Results.Count, pool);

            PoolResult? previousResult = null;
            foreach (PoolResult result in pool.Results)
            {
                Assert.IsTrue(result.Played == (pool.Teams.Count - 1), "expected # matches played {0} for team {1} but found {2} in {3}", pool.Teams.Count - 1, result.Team, result.Played, pool);
                if (previousResult != null)
                {
                    int x = pool.CompareMutualResult(previousResult, result);
                    Assert.IsTrue(x <= -23, "sorting result {0} should be > 0 when comparing 2 results in a sorted collection, results: {1} and {2}", x, previousResult, result);
                }
                previousResult = result;
            }
        }
    }
}
