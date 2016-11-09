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
            results.Should().HaveCount(5);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(3);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(0, 3, new int[] { 1, 2, 5 }),
                new SudokuSquare(0, 4, new int[] { 1, 2, 5, 6, 7 }),
                new SudokuSquare(0, 5, new int[] { 2, 5, 6, 7 })
            });
        }

        [TestMethod]
        public void TestNakedTriple()
        {
            ISudokuStrategy strategy = new NakedTripleStrategy();
            strategy.Name.Should().Be("Naked Triple");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(NakedTriplePuzzle);
            results.Should().HaveCount(1);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(5);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(4, 0, new int[] { 4, 5, 6, 7, 9 }),
                new SudokuSquare(4, 2, new int[] { 1, 5, 6, 7, 9 }),
                new SudokuSquare(4, 6, new int[] { 1, 3, 5, 8, 9 }),
                new SudokuSquare(4, 7, new int[] { 3, 4, 5, 6, 8, 9 }),
                new SudokuSquare(4, 8, new int[] { 1, 5, 6, 8 })
            });
        }

        [TestMethod]
        public void TestNakedQuad()
        {
            ISudokuStrategy strategy = new NakedQuadStrategy();
            strategy.Name.Should().Be("Naked Quad");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(NakedQuadPuzzle);
            results.Should().HaveCount(1);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(4);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(0, 1, new int[] { 1, 2, 4, 5 }),
                new SudokuSquare(0, 2, new int[] { 2, 4, 5, 7 }),
                new SudokuSquare(1, 2, new int[] { 3, 5, 6, 7, 8 }),
                new SudokuSquare(2, 2, new int[] { 3, 4, 6 })
            });
        }
    }
}
