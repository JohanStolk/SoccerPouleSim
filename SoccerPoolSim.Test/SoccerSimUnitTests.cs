using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoccerPoolSim.Core;

namespace SoccerSim.Test
{
    [TestClass]
    public class SoccerSimUnitTests
    {
        [TestMethod]
        public IPool TestGenerateMatches()
        {
            IPool pool = Pool.GenerateEK88Group2();
            Assert.IsTrue(pool.Teams.Count == 4);
            Assert.IsTrue(pool.Matches.Count == 0);
            pool.GenerateMatches();
            Assert.IsTrue(pool.Matches.Count == 6);
            return pool;
        }
        [TestMethod]
        public void TestCompareMutualResult()
        {
            IPool pool = TestGenerateMatches();

            //  pool.CompareMutualResult();
        }
        [TestMethod]
        public void TestGenerateResults()
        {
            IPool pool = TestGenerateMatches();

            pool.GenerateResults();
            Assert.IsTrue(pool.Results.Count == pool.Teams.Count);
            foreach (PoolResult result in pool.Results)
            {
                Assert.IsTrue(result.Played == (pool.Teams.Count - 1));
            }
        }
    }
}
