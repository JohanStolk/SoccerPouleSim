using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoccerPoolSim.Core;

namespace SoccerSim.Test
{
    [TestClass]
    public class SoccerSimUnitTests
    {
        /// <summary>
        /// helper for start scenarios in tests
        /// </summary>
        /// <returns></returns>
        public IPool GeneratePoolWithMatches()
        {
            IPool pool = Pool.GenerateEK88Group2();
            pool.GenerateMatches();
            return pool;
        }
        /// <summary>
        /// test generating matches 4 teams => 4*3/2 = 6 matches
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestGenerateMatches()
        {
            IPool pool = Pool.GenerateEK88Group2();
            Assert.IsTrue(pool.Teams.Count == 4, "ek88 group 2 should have 4 teams, not " + pool.Teams.Count);
            Assert.IsTrue(pool.Matches.Count == 0, "after generating the group no matches should be present, found " + pool.Matches.Count);
            pool.GenerateMatches();
            Assert.IsTrue(pool.Matches.Count == 6, "ek88 group 2 should have 6 matches, not " + pool.Matches.Count);
        }
        /// <summary>
        /// test generating more matches 4.. 14 teams =>
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestGenerateMoreMatches()
        {
            IPool pool = Pool.GenerateEK88Group2();
            Assert.IsTrue(pool.Teams.Count == 4, "ek88 group 2 should have 4 teams, not " + pool.Teams.Count);
            Assert.IsTrue(pool.Matches.Count == 0, "after generating the group no matches should be present, found " + pool.Matches.Count);
            for (int i = 0; i < 10; i++)
            {
                pool.GenerateMatches();
                int expectedMatchesCount = (pool.Teams.Count) * (pool.Teams.Count - 1)/2;
                Assert.IsTrue(pool.Matches.Count == expectedMatchesCount, "expected matches {0} but found {1} in {2}", expectedMatchesCount, pool.Matches.Count, pool);
                pool.Teams.Add(new SoccerTeam("Test team " + i));
            }
        }
        [TestMethod]
        public void TestCompareMutualResult()
        {
            IPool pool = GeneratePoolWithMatches();

            //Assert.ThrowsException<NullReferenceException>(() => cust = CustomerRepository.GetCustomer(""));
            //  pool.CompareMutualResult();
        }
        [TestMethod]
        public void TestGenerateResults()
        {
            IPool pool = GeneratePoolWithMatches();

            pool.GenerateResults();
            Assert.IsTrue(pool.Results.Count == pool.Teams.Count, "expected # results {0} but found {1} in {2}", pool.Teams.Count, pool.Results.Count, pool);
            foreach (PoolResult result in pool.Results)
            {
                Assert.IsTrue(result.Played == (pool.Teams.Count - 1), "expected # matches played {0} for team {1} but found {2} in {3}", pool.Teams.Count - 1, result.Team, result.Played, pool);
            }
        }
    }
}
