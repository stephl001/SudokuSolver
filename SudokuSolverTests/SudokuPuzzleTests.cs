using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using FluentAssertions;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuPuzzleTests
    {
        [TestMethod]
        public void TestInvalidCreation()
        {
            Action act = () => new SudokuPuzzle(null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("input");

            act = () => new SudokuPuzzle(new int[9, 8]);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("input");

            act = () => new SudokuPuzzle(new int[12, 9]);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("input");
        }

        [TestMethod]
        public void TestInvalidInput3()
        {
            Action act = () => new SudokuPuzzle(new int[9, 9] {
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,10}, //<== 10 is not a valid entry
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9}
            });

            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberColumn()
        {
            var pt = PuzzleTest.Load("invalidcolumn");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.IsValid.Should().BeFalse();
            var columnErrors = puzzle.ValidationErrors.Where(e => e.Type == SudokuValidationType.Column).ToArray();
            columnErrors.Should().HaveCount(3);
            columnErrors.All(e => e.Message.StartsWith("The column contains")).Should().BeTrue();
            columnErrors.All(e => e.FaultySquares.Count >= 2).Should().BeTrue();
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberRow()
        {
            var pt = PuzzleTest.Load("invalidrow");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.IsValid.Should().BeFalse();
            var rowErrors = puzzle.ValidationErrors.Where(e => e.Type == SudokuValidationType.Row).ToArray();
            rowErrors.Should().HaveCount(1);
            rowErrors.All(e => e.Message.StartsWith("The row contains")).Should().BeTrue();
            rowErrors.All(e => e.FaultySquares.Count >= 2).Should().BeTrue();
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberBox()
        {
            var pt = PuzzleTest.Load("invalidbox");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.IsValid.Should().BeFalse();
            puzzle.ValidationErrors.All(e => e.Type == SudokuValidationType.Box).Should().BeTrue();
            puzzle.ValidationErrors.All(e => e.Message.StartsWith("The box contains")).Should().BeTrue();
            puzzle.ValidationErrors.All(e => e.FaultySquares.Count >= 2).Should().BeTrue();
        }

        [TestMethod]
        public void TestValidInput()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.IsValid.Should().BeTrue();
            puzzle.Height.Should().Be(9);
            puzzle.Width.Should().Be(9);
        }

        [TestMethod]
        public void TestReadOutOfRange()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);

            Action act = () => puzzle.ReadRow(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIndex");

            act = () => puzzle.ReadRow(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIndex");

            act = () => puzzle.ReadColumn(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIndex");

            act = () => puzzle.ReadColumn(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIndex");

            act = () => puzzle.ReadBox(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("boxIndex");

            act = () => puzzle.ReadBox(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("boxIndex");
        }

        [TestMethod]
        public void TestReadRowPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadRow(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 0, 1, 7, 3, 0, 8, 2, 5 });
            puzzle.ReadRow(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 4, 0, 6, 5, 8, 1, 3, 2 });
        }

        [TestMethod]
        public void TestReadColumnPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadColumn(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 5, 2, 8, 4, 6, 0, 1, 0 });
            puzzle.ReadColumn(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 5, 9, 4, 0, 6, 1, 8, 0, 2 });
        }

        [TestMethod]
        public void TestReadBoxPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            
            puzzle.ReadBox(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 0, 1, 5, 3, 4, 2, 7, 0 });
            puzzle.ReadBox(4).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 4, 0, 3, 0, 5, 0, 2, 0 });
            puzzle.ReadBox(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 9, 8, 6, 5, 0, 1, 3, 2 });
        }
        
        [TestMethod]
        public void TestCompletedPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzleInput = new SudokuPuzzle(pt.Input);
            var puzzleSolution = new SudokuPuzzle(pt.Solution);
            puzzleInput.IsCompleted.Should().BeFalse();
            puzzleSolution.IsCompleted.Should().BeTrue();
        }
        /*
        [TestMethod]
        public void TestCandidatesPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzleInput = new SudokuPuzzle(pt.Input);
            SudokuSquare[] unsetSquares = puzzleInput.ReadAllSquares().Where(s => !s.IsValueSet).ToArray();
            Assert.AreEqual(22, unsetSquares.Length);
            foreach (SudokuSquare s in unsetSquares)
            {
                puzzleInput.Read
            }
        }*/
    }
}
