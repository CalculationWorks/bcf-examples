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
        public void Solve22XSolutions()
        {
            var sudoku = new Sudoku(cross: true);

            sudoku.Load(
                "123456789" +
                "456789123" +
                "789123456" +
                "912345678" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000");

            var resultCount = sudoku.Solve(m => 0).Count();
            Assert.AreEqual(22, resultCount);
        }

        [TestMethod]
        public void Solve1340HyperSolutions()
        {
            var sudoku = new Sudoku(hyper: true);

            sudoku.Load(
                "123456789" +
                "456789123" +
                "789123456" +
                "534297861" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000");

            var resultCount = sudoku.Solve(m => 0).Count();
            Assert.AreEqual(1340, resultCount);
        }

        [TestMethod]
        public void Solve5HyperXSolutions()
        {
            var sudoku = new Sudoku(true, true);

            sudoku.Load(
                "123456789" +
                "456789123" +
                "789123456" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000");

            var resultCount = sudoku.Solve(m => 0).Count();
            Assert.AreEqual(5, resultCount);
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

        [TestMethod]
        public void GenerateRandomSolution()
        {
            var sudoku = new Sudoku(rnd: new Random());
            Assert.AreEqual(SudokuState.Solved, sudoku.GetState());
        }
        
        [TestMethod]
        public void GenerateRandomPuzzles()
        {
            foreach (var cross in new[] { false, true })
            {
                foreach (var hyper in new[] { false, true })
                {
                    foreach (var backtrackingOption in new[] { SudokuBacktrackingOption.Allowed, SudokuBacktrackingOption.Disabled, SudokuBacktrackingOption.Required })
                    {
                        for (int seed = 0; seed < 1; seed++)
                        {
                            var start = DateTime.Now;
                            var rnd = new Random(seed);
                            var solved = new Sudoku(cross, hyper, rnd);
                            var puzzle = new Sudoku(solved, rnd, backtrackingOption);

                            Console.WriteLine($"cross: {cross}; hyper: {hyper}; backtracking: {backtrackingOption}");
                            Console.WriteLine($"time taken: {DateTime.Now.Subtract(start).TotalMilliseconds}");
                            Console.WriteLine($"predefined cells: {81 - puzzle.ToString().Count(c => c == '0')}");
                            Console.Write(puzzle.Csv);
                            Console.Write(solved.Csv);
                            Console.WriteLine();

                            Assert.AreEqual(SudokuState.Valid, puzzle.GetState());
                            Assert.AreEqual(1, puzzle.Solve(x => 0).Take(2).Count());
                        }
                    }
                }
            }
        }
    }
}
