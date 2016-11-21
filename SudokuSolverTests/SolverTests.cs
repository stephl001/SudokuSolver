using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using FluentAssertions;
using System.Linq;
using System;

namespace SudokuSolverTests
{
    [TestClass]
    public class SolverTests : SudokuTests
    {
        [TestMethod]
        public void TestInvalidCreation()
        {
            var solver = new Solver();
            Action act = () => solver.Solve(null).ToArray();
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("puzzle");

            var invalidPuzzle = new SudokuPuzzle(new int[9, 9] {
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9}
            });
            act = () => solver.Solve(invalidPuzzle).ToArray();
            act.ShouldThrow<ArgumentException>().WithMessage("Entry puzzle is invalid*Parameter name: puzzle");
        }

        [TestMethod]
        public void TestCreation()
        {
            var solver = new Solver();
            solver.LoadedStrategies.Should().Be(11);
        }

        [TestMethod]
        public void TestSolveEasy()
        {
            var solver = new Solver();
            var finalPuzzle = solver.Solve(GetPuzzle("easy")).Last().NewPuzzle;
            finalPuzzle.IsCompleted.Should().BeTrue();
            GetPuzzleSolution("easy").Should().Be(finalPuzzle);
        }

        [TestMethod]
        public void TestSolveHard()
        {
            var solver = new Solver();
            var finalPuzzle = solver.Solve(GetPuzzle("hard")).Last().NewPuzzle;
            finalPuzzle.IsCompleted.Should().BeTrue();
            GetPuzzleSolution("hard").Should().Be(finalPuzzle);
        }
    }
}
