﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Strategies;
using FluentAssertions;
using SudokuSolver;
using System.Linq;
using SudokuSolver.Strategies.NakedCandidates;
using SudokuSolver.Strategies.HiddenCandidates;
using System.Collections.Generic;
using SudokuSolver.Strategies.Advanced;

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

            act = () => strategy.Query(GetPuzzleSolution("hard"));
            act.ShouldThrow<InvalidOperationException>().WithMessage("You cannot query a strategy result on a completed or invalid puzzle.");
        }

        [TestMethod]
        public void TestNakedSingle()
        {
            ISudokuStrategy strategy = new NakedSingleStrategy();
            strategy.Name.Should().Be("Naked Single");
            
            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("nonakedsingle")).ToArray();
            results.Should().BeEmpty();

            results = strategy.Query(GetPuzzle("hard"));
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
            
            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("nohiddensingle")).ToArray();
            results.Should().BeEmpty();
            
            results = strategy.Query(GetPuzzle("hiddensingle"));
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

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("nakedpair")).ToArray();
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

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("nakedtriple")).ToArray();
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

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("nakedquads")).ToArray();
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

        [TestMethod]
        public void TestHiddenPair()
        {
            ISudokuStrategy strategy = new HiddenPairStrategy();
            strategy.Name.Should().Be("Hidden Pair");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("hiddenpair")).ToArray();
            results.Should().HaveCount(2);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(2);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(0, 7, new int[] { 2, 3, 4, 5, 6, 7, 9 }),
                new SudokuSquare(0, 8, new int[] { 3, 4, 5, 6, 7, 9 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 2, 3, 4, 5, 9 });
        }

        [TestMethod]
        public void TestHiddenTriple()
        {
            ISudokuStrategy strategy = new HiddenTripleStrategy();
            strategy.Name.Should().Be("Hidden Triple");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("hiddentriple")).ToArray();
            results.Should().HaveCount(2);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(3);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(0, 3, new int[] { 2, 4, 5, 6, 7, 8 }),
                new SudokuSquare(0, 6, new int[] { 2, 4, 6, 9 }),
                new SudokuSquare(0, 8, new int[] { 2, 4, 5, 7, 8, 9 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 4, 7, 8, 9 });
        }

        [TestMethod]
        public void TestHiddenQuad()
        {
            ISudokuStrategy strategy = new HiddenQuadStrategy();
            strategy.Name.Should().Be("Hidden Quad");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("hiddenquad")).ToArray();
            results.Should().HaveCount(3);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(4);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(3, 3, new int[] { 1, 3, 4, 6, 7, 8, 9 }),
                new SudokuSquare(3, 5, new int[] { 3, 4, 6, 7, 8, 9 }),
                new SudokuSquare(5, 3, new int[] { 1, 3, 4, 7, 8, 9 }),
                new SudokuSquare(5, 5, new int[] { 3, 4, 5, 7, 8, 9 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 3, 5, 7, 8 });
        }

        [TestMethod]
        public void TestPointingCandidates()
        {
            ISudokuStrategy strategy = new PointingCandidatesStrategy();
            strategy.Name.Should().Be("Pointing Candidates");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("pointingpairs")).ToArray();
            results.Should().HaveCount(9);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(3);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(1, 6, new int[] { 2, 3, 6, 7, 9 }),
                new SudokuSquare(1, 7, new int[] { 2, 3, 5, 8, 9 }),
                new SudokuSquare(1, 8, new int[] { 2, 3, 6, 7, 8 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 2 });
        }

        [TestMethod]
        public void TestBoxLineReduction()
        {
            ISudokuStrategy strategy = new BoxLineReductionStrategy();
            strategy.Name.Should().Be("Box/Line Reduction");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("boxlinereduction")).ToArray();
            results.Should().HaveCount(4);
            results.Last().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.Last().AffectedSquares.Should().HaveCount(4);
            results.Last().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(1, 6, new int[] { 1, 2, 4, 7 }),
                new SudokuSquare(1, 8, new int[] { 1, 4, 5, 7 }),
                new SudokuSquare(2, 6, new int[] { 2, 4 }),
                new SudokuSquare(2, 8, new int[] { 4, 5, 9 })
            });
            results.Last().Candidates.ShouldBeEquivalentTo(new int[] { 4 });
        }

        [TestMethod]
        public void TestXWing()
        {
            ISudokuStrategy strategy = new XWingStrategy();
            strategy.Name.Should().Be("X-Wing");
            
            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("xwing")).ToArray();
            results.Should().HaveCount(1);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(8);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(0, 3, new int[] { 2, 3, 4, 7, 8 }),
                new SudokuSquare(4, 3, new int[] { 2, 7, 8, 9 }),
                new SudokuSquare(7, 3, new int[] { 3, 7, 8 }),
                new SudokuSquare(3, 7, new int[] { 2, 3, 5, 7 }),
                new SudokuSquare(4, 7, new int[] { 2, 3, 5, 7, 9 }),
                new SudokuSquare(7, 7, new int[] { 3, 7, 8 }),
                new SudokuSquare(8, 3, new int[] { 3, 4, 7, 8, 9 }),
                new SudokuSquare(8, 7, new int[] { 3, 7, 8, 9 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 7 });
        }

        [TestMethod]
        public void TestReverseXWing()
        {
            ISudokuStrategy strategy = new XWingStrategy();
            
            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("reversexwing")).ToArray();
            results.Should().HaveCount(2);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(8);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(4, 0, new int[] { 1, 3, 4, 5, 6 }),
                new SudokuSquare(4, 2, new int[] { 1, 2, 3, 4, 5, 6 }),
                new SudokuSquare(4, 6, new int[] { 2, 3, 4, 5, 8 }),
                new SudokuSquare(4, 8, new int[] { 2, 3, 8 }),
                new SudokuSquare(8, 2, new int[] { 3, 5, 6, 7, 8 }),
                new SudokuSquare(8, 3, new int[] { 2, 3, 4, 5, 6, 8 }),
                new SudokuSquare(8, 5, new int[] { 3, 4, 5, 6, 7, 8 }),
                new SudokuSquare(8, 8, new int[] { 2, 3, 6, 8 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 3 });
        }

        [TestMethod]
        public void TestSwordfish()
        {
            ISudokuStrategy strategy = new SwordfishStrategy();
            strategy.Name.Should().Be("Swordfish");

            IEnumerable<SudokuStrategyResult> results = strategy.Query(GetPuzzle("swordfish")).ToArray();
            results.Should().HaveCount(1);
            results.First().Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            results.First().AffectedSquares.Should().HaveCount(9);
            results.First().AffectedSquares.ShouldBeEquivalentTo(new SudokuSquare[]
            {
                new SudokuSquare(4, 2, new int[] { 1, 2, 4, 5, 7 }),
                new SudokuSquare(5, 2, new int[] { 4, 5, 7 }),
                new SudokuSquare(6, 2, new int[] { 4, 5, 6, 8 }),
                new SudokuSquare(4, 6, new int[] { 4, 5, 7, 8, 9 }),
                new SudokuSquare(5, 6, new int[] { 4, 5, 7, 9 }),
                new SudokuSquare(6, 6, new int[] { 4, 5, 6, 8 }),
                new SudokuSquare(1, 8, new int[] { 1, 4, 5, 7 }),
                new SudokuSquare(4, 8, new int[] { 4, 5, 7, 8 }),
                new SudokuSquare(6, 8, new int[] { 4, 5, 8 })
            });
            results.First().Candidates.ShouldBeEquivalentTo(new int[] { 4 });
        }
    }
}
