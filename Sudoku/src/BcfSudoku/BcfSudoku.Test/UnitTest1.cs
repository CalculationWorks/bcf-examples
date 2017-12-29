using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BcfSudoku.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Solve2Solutions()
        {
            var sudoku = new Sudoku();
            
            sudoku.Load(
                "123456789" +
                "456789123" +
                "789123456" +
                "912345678" +
                "345678912" +
                "678912345" +
                "891234567" +
                "230000000" +
                "560000000");
            
            var results = sudoku.Solve(s => s.ToString()).ToArray();

            Assert.AreEqual(2, results.Length);

            var expected1 =
                "123456789" +
                "456789123" +
                "789123456" +
                "912345678" +
                "345678912" +
                "678912345" +
                "891234567" +
                "234567891" +
                "567891234";

            var expected2 =
                "123456789" +
                "456789123" +
                "789123456" +
                "912345678" +
                "345678912" +
                "678912345" +
                "891234567" +
                "237561894" +
                "564897231";

            Assert.IsTrue(results[0] == expected1 || results[0] == expected2);
            Assert.IsTrue(results[1] == expected1 || results[1] == expected2);
        }

        [TestMethod]
        public void Solve3000Solutions()
        {
            var sudoku = new Sudoku();

            sudoku.Load(
                "123456789" +
                "456789123" +
                "789123456" +
                "912345678" +
                "345678912" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000");

            var resultCount = sudoku.Solve(m => 0).Count();
            Assert.AreEqual(3000, resultCount);
        }

        [TestMethod]
        public void Test1Solution()
        {
            var sudoku = new Sudoku();

            sudoku.Load(
                "056300001" +
                "000000000" +
                "000000473" +
                "008904160" +
                "000050000" +
                "015608700" +
                "864000000" +
                "000000000" +
                "100009280");

            var resultCount = sudoku.Solve(m => 0).Count();
            Assert.AreEqual(1, resultCount);
        }
    }
}
