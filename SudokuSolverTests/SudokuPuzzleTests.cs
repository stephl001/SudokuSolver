using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuPuzzleTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidCreation()
        {
            var puzzle = new SudokuPuzzle(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidInput1()
        {
            var puzzle = new SudokuPuzzle(new int[9, 8]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidInput2()
        {
            var puzzle = new SudokuPuzzle(new int[12, 9]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidInput3()
        {
            var puzzle = new SudokuPuzzle(new int[9, 9] {
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
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberColumn()
        {
            var pt = PuzzleTest.Load("invalidcolumn");
            var puzzle = new SudokuPuzzle(pt.Input);
            Assert.IsFalse(puzzle.IsValid);
            puzzle.ValidationErrors.All(e => e.Type == SudokuValidationType.Column);
            puzzle.ValidationErrors.All(e => e.Message.StartsWith("The column contains"));
            puzzle.ValidationErrors.All(e => e.FaultySquares.Count >= 2);
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberRow()
        {
            var pt = PuzzleTest.Load("invalidrow");
            var puzzle = new SudokuPuzzle(pt.Input);
            Assert.IsFalse(puzzle.IsValid);
            puzzle.ValidationErrors.All(e => e.Type == SudokuValidationType.Row);
            puzzle.ValidationErrors.All(e => e.Message.StartsWith("The row contains"));
            puzzle.ValidationErrors.All(e => e.FaultySquares.Count >= 2);
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberBox()
        {
            var pt = PuzzleTest.Load("invalidbox");
            var puzzle = new SudokuPuzzle(pt.Input);
            Assert.IsFalse(puzzle.IsValid);
            puzzle.ValidationErrors.All(e => e.Type == SudokuValidationType.Box);
            puzzle.ValidationErrors.All(e => e.Message.StartsWith("The box contains"));
            puzzle.ValidationErrors.All(e => e.FaultySquares.Count >= 2);
        }

        [TestMethod]
        public void TestValidInput()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            Assert.IsTrue(puzzle.IsValid);
            Assert.AreEqual(9, puzzle.Height);
            Assert.AreEqual(9, puzzle.Width);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadRowOutOrRange1()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadRow(-1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadRowOutOrRange2()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadRow(10).ToArray();
        }

        [TestMethod]
        public void TestReadRowPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            Assert.IsTrue(puzzle.ReadRow(0).Select(s => s.Value).SequenceEqual(new int[] { 9, 0, 1, 7, 3, 0, 8, 2, 5 }));
            Assert.IsTrue(puzzle.ReadRow(8).Select(s => s.Value).SequenceEqual(new int[] { 0, 4, 0, 6, 5, 8, 1, 3, 2 }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadColumnOutOrRange1()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadColumn(-1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadColumnOutOrRange2()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadColumn(10).ToArray();
        }

        [TestMethod]
        public void TestReadcolumnPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            Assert.IsTrue(puzzle.ReadColumn(0).Select(s => s.Value).SequenceEqual(new int[] { 9, 5, 2, 8, 4, 6, 0, 1, 0 }));
            Assert.IsTrue(puzzle.ReadColumn(8).Select(s => s.Value).SequenceEqual(new int[] { 5, 9, 4, 0, 6, 1, 8, 0, 2 }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadBoxOutOrRange1()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadBox(-1).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadBoxOutOrRange2()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            puzzle.ReadBox(10).ToArray();
        }

        [TestMethod]
        public void TestReadBoxPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzle = new SudokuPuzzle(pt.Input);
            var test = puzzle.ReadBox(0).Select(s => s.Value).ToArray();
            Assert.IsTrue(puzzle.ReadBox(0).Select(s => s.Value).SequenceEqual(new int[] { 9, 0, 1, 5, 3, 4, 2, 7, 0 }));
            Assert.IsTrue(puzzle.ReadBox(4).Select(s => s.Value).SequenceEqual(new int[] { 0, 4, 0, 3, 0, 5, 0, 2, 0 }));
            Assert.IsTrue(puzzle.ReadBox(8).Select(s => s.Value).SequenceEqual(new int[] { 0, 9, 8, 6, 5, 0, 1, 3, 2 }));
        }

        [TestMethod]
        public void TestCompletedPuzzle()
        {
            var pt = PuzzleTest.Load("easy");
            var puzzleInput = new SudokuPuzzle(pt.Input);
            var puzzleSolution = new SudokuPuzzle(pt.Solution);
            Assert.IsFalse(puzzleInput.IsCompleted);
            Assert.IsTrue(puzzleSolution.IsCompleted);
        }
    }
}
