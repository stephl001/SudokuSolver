using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Strategies;
using FluentAssertions;
using SudokuSolver;
using System.Linq;
using SudokuSolver.Strategies.NakedCandidates;
using SudokuSolver.Strategies.HiddenCandidates;
using System.Collections.Generic;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuStrategiesTests : SudokuTests
    {
        [TestMethod]
        public void TestArgumentsStrategy()
        {
            ISudokuStrategy strategy = new NakedSingleStrategy();

            Action act = () => strategy.Query(null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("puzzle");

            act = () => strategy.Query(HardPuzzleSolution);
            act.ShouldThrow<InvalidOperationException>().WithMessage("You cannot query a strategy result on a completed or invalid puzzle.");
        }

        [TestMethod]
        public void TestNakedSingle()
        {
            ISudokuStrategy strategy = new NakedSingleStrategy();
            strategy.Name.Should().Be("Naked Single");
            
            IEnumerable<SudokuStrategyResult> results = strategy.Query(NoNakedSinglePuzzle);
            results.Should().BeEmpty();

            results = strategy.Query(HardPuzzle);
            results.Should().HaveCount(1);
            results.Single().Result.Should().Be(StrategyResultOutcome.ValueFound);
            results.Single().AffectedSquares.Should().HaveCount(1);
            results.Single().AffectedSquares.Single().Should().Be(new SudokuSquare(2, 3, 4));
        }

        [TestMethod]
        public void TestHiddenSingle()
        {
            ISudokuStrategy strategy = new HiddenSingleStrategy();
            strategy.Name.Should().Be("Hidden Single");
            
            IEnumerable<SudokuStrategyResult> results = strategy.Query(NoHiddenSinglePuzzle);
            results.Should().BeEmpty();
            
            results = strategy.Query(HiddenSinglePuzzle);
            results.Should().HaveCount(10);
            results.First().Result.Should().Be(StrategyResultOutcome.ValueFound);
            results.First().AffectedSquares.Should().HaveCount(1);
            results.First().AffectedSquares.Single().Should().Be(new SudokuSquare(1, 3, 7));
        }

        [TestMethod]
        public void TestNakedPair()
        {
            ISudokuStrategy strategy = new NakedPairStrategy();
            strategy.Name.Should().Be("Naked Pair");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(NakedPairPuzzle);
            results.Should().HaveCount(8);
            results.First().Result.Should().Be(StrategyResultOutcome.OnlyPossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(2);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(0, 1, new int[] { 1, 6 }),
                new SudokuSquare(0, 2, new int[] { 1, 6 })
            });
        }
    }
}
