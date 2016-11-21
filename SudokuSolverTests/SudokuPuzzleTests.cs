using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuPuzzleTests : SudokuTests
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
            var puzzle = GetPuzzle("invalidcolumn");
            puzzle.IsValid.Should().BeFalse();
            var columnErrors = puzzle.ValidationErrors.Where(e => e.Type == SudokuValidationType.Column).ToArray();
            columnErrors.Should().HaveCount(3);
            columnErrors.All(e => e.Message.StartsWith("The column contains")).Should().BeTrue();
            columnErrors.All(e => e.FaultySquares.Count >= 2).Should().BeTrue();
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberRow()
        {
            var puzzle = GetPuzzle("invalidrow");
            puzzle.IsValid.Should().BeFalse();
            var rowErrors = puzzle.ValidationErrors.Where(e => e.Type == SudokuValidationType.Row).ToArray();
            rowErrors.Should().HaveCount(1);
            rowErrors.All(e => e.Message.StartsWith("The row contains")).Should().BeTrue();
            rowErrors.All(e => e.FaultySquares.Count >= 2).Should().BeTrue();
        }

        [TestMethod]
        public void TestInvalidInputRepeatedNumberBox()
        {
            var puzzle = GetPuzzle("invalidbox");
            puzzle.IsValid.Should().BeFalse();
            puzzle.ValidationErrors.All(e => e.Type == SudokuValidationType.Box).Should().BeTrue();
            puzzle.ValidationErrors.All(e => e.Message.StartsWith("The box contains")).Should().BeTrue();
            puzzle.ValidationErrors.All(e => e.FaultySquares.Count >= 2).Should().BeTrue();
        }

        [TestMethod]
        public void TestValidInput()
        {
            GetPuzzle("easy").IsValid.Should().BeTrue();
            GetPuzzle("easy").Height.Should().Be(9);
            GetPuzzle("easy").Width.Should().Be(9);
        }

        [TestMethod]
        public void TestReadOutOfRange()
        {
            Action act = () => GetPuzzle("easy").ReadRow(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIndex");

            act = () => GetPuzzle("easy").ReadRow(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIndex");

            act = () => GetPuzzle("easy").ReadColumn(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIndex");

            act = () => GetPuzzle("easy").ReadColumn(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIndex");

            act = () => GetPuzzle("easy").ReadBox(-1).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("boxIndex");

            act = () => GetPuzzle("easy").ReadBox(10).ToArray();
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("boxIndex");
        }

        [TestMethod]
        public void TestReadRowPuzzle()
        {
            GetPuzzle("easy").ReadRow(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 0, 1, 7, 3, 0, 8, 2, 5 });
            GetPuzzle("easy").ReadRow(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 4, 0, 6, 5, 8, 1, 3, 2 });
        }

        [TestMethod]
        public void TestReadColumnPuzzle()
        {
            GetPuzzle("easy").ReadColumn(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 5, 2, 8, 4, 6, 0, 1, 0 });
            GetPuzzle("easy").ReadColumn(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 5, 9, 4, 0, 6, 1, 8, 0, 2 });
        }

        [TestMethod]
        public void TestReadBoxPuzzle()
        {
            GetPuzzle("easy").ReadBox(0).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 9, 0, 1, 5, 3, 4, 2, 7, 0 });
            GetPuzzle("easy").ReadBox(4).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 4, 0, 3, 0, 5, 0, 2, 0 });
            GetPuzzle("easy").ReadBox(8).Select(s => s.Value).ShouldBeEquivalentTo(new int[] { 0, 9, 8, 6, 5, 0, 1, 3, 2 });
        }
        
        [TestMethod]
        public void TestCompletedPuzzle()
        {
            GetPuzzle("easy").IsCompleted.Should().BeFalse();
            GetPuzzleSolution("easy").IsCompleted.Should().BeTrue();
        }

        [TestMethod]
        public void TestGetSquarePuzzle()
        {
            GetPuzzle("easy").GetSquare(2, 3).Value.Should().Be(5);

            Action act = () => GetPuzzle("easy").GetSquare(-1, 2);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("row");
            act = () => GetPuzzle("easy").GetSquare(9, 2);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("row");
            act = () => GetPuzzle("easy").GetSquare(1, -1);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("column");
            act = () => GetPuzzle("easy").GetSquare(1, 9);
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("column");
        }

        [TestMethod]
        public void TestReadBuddies()
        {
            Action act = () => GetPuzzle("easy").ReadBuddies(null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("square");

            foreach (SudokuSquare s in GetPuzzle("easy").ReadAllSquares())
            {
                GetPuzzle("easy").ReadBuddies(s).Should().OnlyHaveUniqueItems().And.HaveCount(20);
            }

            SudokuSquare square = GetPuzzle("easy").GetSquare(2, 4);
            IEnumerable<int> impossibleValues = GetPuzzle("easy").ReadBuddiesValues(square);
            SudokuPuzzle.PossibleValues.Except(impossibleValues).Single().Should().Be(6);
        }
        
        [TestMethod]
        public void TestCandidatesPuzzle()
        {
            GetPuzzle("hard").GetSquare(4, 4).Candidates.ShouldBeEquivalentTo(new int[] { 1, 4, 6, 8, 9 });
            GetPuzzle("hard").GetSquare(6, 2).Candidates.ShouldBeEquivalentTo(new int[] { 1, 3, 4, 6, 7, 9 });
        }

        [TestMethod]
        public void TestSetValuePuzzleInvalidInput()
        {
            Action act = () => GetPuzzle("hard").SetValue(null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("square");

            //Setting a value with a non-valued square should fail
            act = () => GetPuzzle("hard").SetValue(new SudokuSquare(1, 2, new int[] { 1, 2 }));
            act.ShouldThrow<ArgumentException>().WithMessage("The provided square must have a value set.*Parameter name: square");

            //Setting an already set square should fail if trying to set a different value
            act = () => GetPuzzle("hard").SetValue(new SudokuSquare(1, 2, 6));
            act.ShouldThrow<ArgumentException>().WithMessage("The specified square has alerady a value set(8).*Parameter name: square");
        }

        [TestMethod]
        public void TestSetValuePuzzle()
        {
            //Setting an already set square with the exact same value should yield the same puzzle instance
            SudokuPuzzle newPuzzle = GetPuzzle("hard").SetValue(new SudokuSquare(1, 2, 8));
            newPuzzle.Should().Be(GetPuzzle("hard"));

            //Setting a new valid value should yield a new puzzle instance with updated 
            //candidates
            SudokuSquare newSquare = new SudokuSquare(4, 4, 1);
            newPuzzle = GetPuzzle("hard").SetValue(newSquare);
            newPuzzle.Should().NotBe(GetPuzzle("hard"));
            newPuzzle.IsValid.Should().BeTrue();
            newPuzzle.GetSquare(4, 4).Should().Be(newSquare);
            newPuzzle.ReadBuddiesCandidates(newSquare).Should().NotContain(1);

            //Setting a new invalid value should yield a new puzzle instance with validation errors
            newSquare = new SudokuSquare(4, 4, 3);
            newPuzzle = GetPuzzle("hard").SetValue(newSquare);
            newPuzzle.Should().NotBe(GetPuzzle("hard"));
            newPuzzle.IsValid.Should().BeFalse();
            newPuzzle.ValidationErrors.Should().HaveCount(1);
            newPuzzle.ValidationErrors.Single().Should().BeOfType(typeof(SudokuBoxValidationError));
        }

        [TestMethod]
        public void TestSetValueOnInvalidOrCompletedPuzzle()
        {
            SudokuSquare newSquare = new SudokuSquare(4, 4, 3);
            SudokuPuzzle newPuzzle = GetPuzzle("hard").SetValue(newSquare);

            //Setting a value on an invalid puzzle should raise an exception
            Action act = () => newPuzzle.SetValue(newSquare);
            act.ShouldThrow<InvalidOperationException>().WithMessage("You cannot modify an invalid or completed puzzle.");

            //Setting a value on a completed puzzle should raise an exception
            act = () => GetPuzzleSolution("hard").SetValue(newSquare);
            act.ShouldThrow<InvalidOperationException>().WithMessage("You cannot modify an invalid or completed puzzle.");
        }

        [TestMethod]
        public void TestClearCandidatesInvalidInput()
        {
            Action act = () => GetPuzzle("hard").ClearCandidates(null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("squares");
        }

        [TestMethod]
        public void TestClearCandidatesWithoutCandidatesShouldYieldSamePuzzle()
        {
            SudokuPuzzle newPuzzle = GetPuzzle("hard").ClearCandidates(new SudokuSquare[] { GetPuzzle("hard").GetSquare(0, 1) });
            newPuzzle.Should().Be(GetPuzzle("hard"));

            newPuzzle = GetPuzzle("hard").ClearCandidates(new SudokuSquare[] { GetPuzzle("hard").GetSquare(0, 1) }, null);
            newPuzzle.Should().Be(GetPuzzle("hard"));
        }

        [TestMethod]
        public void TestClearCandidatesOnValueCellShouldYieldSamePuzzle()
        {
            SudokuPuzzle newPuzzle = GetPuzzle("hard").ClearCandidates(new SudokuSquare[] { GetPuzzle("hard").GetSquare(0, 0) }, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            newPuzzle.Should().Be(GetPuzzle("hard"));
        }

        [TestMethod]
        public void TestClearCandidatesOutOfRange()
        {
            Action act = () => GetPuzzle("hard").ClearCandidates(new SudokuSquare[] { GetPuzzle("hard").GetSquare(0, 1) }, 1, 2, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Valid candidates must range between 1 and 9.*Parameter name: candidates");

            act = () => GetPuzzle("hard").ClearCandidates(new SudokuSquare[] { GetPuzzle("hard").GetSquare(0, 1) }, 1, 2, 10);
            act.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Valid candidates must range between 1 and 9.*Parameter name: candidates");
        }

        [TestMethod]
        public void TestClearRowCandidates()
        {
            GetPuzzle("hard").ReadRow(0).Where(s => s.Candidates.Contains(2)).Should().HaveCount(4);
            SudokuPuzzle newPuzzle = GetPuzzle("hard").ClearCandidates(GetPuzzle("hard").ReadRow(0), 2);
            newPuzzle.Should().NotBe(GetPuzzle("hard"));
            newPuzzle.IsValid.Should().BeTrue();
            newPuzzle.ReadRow(0).Where(s => s.Candidates.Contains(2)).Should().BeEmpty();
        }

        [TestMethod]
        public void TestClearColumnCandidates()
        {
            GetPuzzle("hard").ReadColumn(0).Where(s => s.Candidates.Contains(3)).Should().HaveCount(4);
            SudokuPuzzle newPuzzle = GetPuzzle("hard").ClearCandidates(GetPuzzle("hard").ReadColumn(0), 3);
            newPuzzle.Should().NotBe(GetPuzzle("hard"));
            newPuzzle.IsValid.Should().BeTrue();
            newPuzzle.ReadColumn(0).Where(s => s.Candidates.Contains(3)).Should().BeEmpty();
        }

        [TestMethod]
        public void TestClearBoxCandidates()
        {
            GetPuzzle("hard").ReadBox(0).Where(s => s.Candidates.Contains(7)).Should().HaveCount(6);
            SudokuPuzzle newPuzzle = GetPuzzle("hard").ClearCandidates(GetPuzzle("hard").ReadBox(0), 7);
            newPuzzle.Should().NotBe(GetPuzzle("hard"));
            newPuzzle.IsValid.Should().BeTrue();
            newPuzzle.ReadBox(0).Where(s => s.Candidates.Contains(7)).Should().BeEmpty();
        }

        [TestMethod]
        public void TestEquality()
        {
            var p1 = new SudokuPuzzle(new int[9, 9] {
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
            var p2 = new SudokuPuzzle(new int[9, 9] {
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
            ReferenceEquals(p1, p2).Should().BeFalse();
            p1.Should().Be(p2);

            p1.Should().NotBe(GetPuzzle("easy"));
            p1.GetHashCode().Should().NotBe(GetPuzzle("easy").GetHashCode());
        }
    }
}