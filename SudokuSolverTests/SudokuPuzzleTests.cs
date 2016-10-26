using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuPuzzleTests
    {
        private SudokuPuzzle _easyPuzzle;
        private SudokuPuzzle _easyPuzzleSolution;
        private SudokuPuzzle _hardPuzzle;
        private SudokuPuzzle _hardPuzzleSolution;

        [TestInitialize]
        public void Setup()
        {
            var pt = PuzzleTest.Load("easy");
            _easyPuzzle = new SudokuPuzzle(pt.Input);
            _easyPuzzleSolution = new SudokuPuzzle(pt.Solution);
            _easyPuzzleSolution.ValidationErrors.Should().BeEmpty();

            pt = PuzzleTest.Load("hard");
            _hardPuzzle = new SudokuPuzzle(pt.Input);
            _hardPuzzleSolution = new SudokuPuzzle(pt.Solution);
            _hardPuzzleSolution.ValidationErrors.Should().BeEmpty();
        }

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
            _easyPuzzle.IsValid.Should().BeTrue();
            _easyPuzzle.Height.Should().Be(9);
            _easyPuzzle.Width.Should().Be(9);
        }

        [TestMethod]
        public void TestReadOutOfRange()
        {
            Action act = () => _easyPuzzle.ReadRow(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIndex");

            act = () => _easyPuzzle.ReadRow(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIndex");

            act = () => _easyPuzzle.ReadColumn(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIndex");

            act = () => _easyPuzzle.ReadColumn(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIndex");

            act = () => _easyPuzzle.ReadBox(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("boxIndex");

            act = () => _easyPuzzle.ReadBox(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("boxIndex");
        }

        [TestMethod]
        public void TestReadRowPuzzle()
        {
            _easyPuzzle.ReadRow(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 0, 1, 7, 3, 0, 8, 2, 5 });
            _easyPuzzle.ReadRow(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 4, 0, 6, 5, 8, 1, 3, 2 });
        }

        [TestMethod]
        public void TestReadColumnPuzzle()
        {
            _easyPuzzle.ReadColumn(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 5, 2, 8, 4, 6, 0, 1, 0 });
            _easyPuzzle.ReadColumn(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 5, 9, 4, 0, 6, 1, 8, 0, 2 });
        }

        [TestMethod]
        public void TestReadBoxPuzzle()
        {
            _easyPuzzle.ReadBox(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 0, 1, 5, 3, 4, 2, 7, 0 });
            _easyPuzzle.ReadBox(4).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 4, 0, 3, 0, 5, 0, 2, 0 });
            _easyPuzzle.ReadBox(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 9, 8, 6, 5, 0, 1, 3, 2 });
        }
        
        [TestMethod]
        public void TestCompletedPuzzle()
        {
            _easyPuzzle.IsCompleted.Should().BeFalse();
            _easyPuzzleSolution.IsCompleted.Should().BeTrue();
        }

        [TestMethod]
        public void TestGetSquarePuzzle()
        {
            _easyPuzzle.GetSquare(2, 3).Value.Should().Be(5);

            Action act = () => _easyPuzzle.GetSquare(-1, 2);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("row");
            act = () => _easyPuzzle.GetSquare(9, 2);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("row");
            act = () => _easyPuzzle.GetSquare(1, -1);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("column");
            act = () => _easyPuzzle.GetSquare(1, 9);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("column");
        }

        [TestMethod]
        public void TestReadBuddies()
        {
            Action act = () => _easyPuzzle.ReadBuddies(null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("square");

            foreach (SudokuSquare s in _easyPuzzle.ReadAllSquares())
            {
                _easyPuzzle.ReadBuddies(s).Should().OnlyHaveUniqueItems().And.HaveCount(20);
            }

            SudokuSquare square = _easyPuzzle.GetSquare(2, 4);
            IEnumerable<int> impossibleValues = _easyPuzzle.ReadBuddiesValues(square);
            SudokuPuzzle.PossibleValues.Except(impossibleValues).Single().Should().Be(6);
        }
        
        [TestMethod]
        public void TestCandidatesPuzzle()
        {
            _hardPuzzle.GetSquare(4, 4).Candidates.ShouldBeEquivalentTo(new int[] { 1, 4, 6, 8, 9 });
            _hardPuzzle.GetSquare(6, 2).Candidates.ShouldBeEquivalentTo(new int[] { 1, 3, 4, 6, 7, 9 });
        }
    }
}
