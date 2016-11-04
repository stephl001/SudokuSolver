using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Strategies;
using FluentAssertions;
using SudokuSolver;
using System.Linq;

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
            
            SudokuStrategyResult result = strategy.Query(NoNakedSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.NotFound);

            result = strategy.Query(HardPuzzle);
            result.Result.Should().Be(StrategyResultOutcome.ValueFound);
            result.AffectedSquares.Should().HaveCount(1);
            result.AffectedSquares.Single().Should().Be(new SudokuSquare(2, 3, 4));
        }

        [TestMethod]
        public void TestHiddenBoxSingle()
        {
            ISudokuStrategy strategy = new HiddenBoxSingleStrategy();
            strategy.Name.Should().Be("Hidden Box Single");
            
            SudokuStrategyResult result = strategy.Query(NoHiddenBoxSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.NotFound);
            
            result = strategy.Query(HiddenBoxSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.ValueFound);
            result.AffectedSquares.Should().HaveCount(1);
            result.AffectedSquares.Single().Should().Be(new SudokuSquare(1, 3, 7));
        }

        [TestMethod]
        public void TestHiddenRowSingle()
        {
            ISudokuStrategy strategy = new HiddenRowSingleStrategy();
            strategy.Name.Should().Be("Hidden Row Single");

            SudokuStrategyResult result = strategy.Query(NoHiddenBoxSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.NotFound);

            result = strategy.Query(HiddenBoxSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.ValueFound);
            result.AffectedSquares.Should().HaveCount(1);
            result.AffectedSquares.Single().Should().Be(new SudokuSquare(1, 3, 7));
        }

        [TestMethod]
        public void TestHiddenColumnSingle()
        {
            ISudokuStrategy strategy = new HiddenColumnSingleStrategy();
            strategy.Name.Should().Be("Hidden Column Single");

            SudokuStrategyResult result = strategy.Query(NoHiddenBoxSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.NotFound);

            result = strategy.Query(HiddenBoxSinglePuzzle);
            result.Result.Should().Be(StrategyResultOutcome.ValueFound);
            result.AffectedSquares.Should().HaveCount(1);
            result.AffectedSquares.Single().Should().Be(new SudokuSquare(8, 4, 6));
        }
    }
}
