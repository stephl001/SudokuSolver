using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using FluentAssertions;
using System.Linq;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuStrategyResultTests : SudokuTests
    {
        [TestMethod]
        public void TestNotFound()
        {
            SudokuStrategyResult.NotFound.AffectedSquares.Should().BeEmpty();
            SudokuStrategyResult.NotFound.Result.Should().Be(StrategyResultOutcome.NotFound);
            SudokuStrategyResult.NotFound.Candidates.Should().BeEmpty();
        }

        [TestMethod]
        public void TestFoundValue()
        {
            Action act = () => SudokuStrategyResult.FromValue(null);
            act.ShouldThrow<ArgumentNullException>();

            act = () => SudokuStrategyResult.FromValue(new SudokuSquare(1, 1, new int[] { 1, 2 }));
            act.ShouldThrow<ArgumentException>().WithMessage("Argument must be a value square.*Parameter name: s");

            var square = new SudokuSquare(1, 1, 3);
            var result = SudokuStrategyResult.FromValue(square);
            result.AffectedSquares.Should().HaveCount(1);
            result.AffectedSquares.Single().Should().Be(square);
            result.Result.Should().Be(StrategyResultOutcome.ValueFound);
            SudokuStrategyResult.NotFound.Candidates.Should().BeEmpty();
        }

        [TestMethod]
        public void TestFoundImpossibleCandidates()
        {
            Action act = () => SudokuStrategyResult.FromImpossibleCandidates(null, null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("squares");

            act = () => SudokuStrategyResult.FromImpossibleCandidates(Enumerable.Empty<SudokuSquare>(), null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("candidates");

            SudokuSquare[] squares = new SudokuSquare[] { new SudokuSquare(0, 1, new int[] { 2, 5, 7 }) };
            act = () => SudokuStrategyResult.FromImpossibleCandidates(squares, new int[] { });
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("candidates");

            int[] candidates = new int[] { 2 };
            var result = SudokuStrategyResult.FromImpossibleCandidates(squares, candidates);
            result.Result.Should().Be(StrategyResultOutcome.ImpossibleCandidatesFound);
            result.AffectedSquares.ShouldBeEquivalentTo(squares);
            result.Candidates.ShouldBeEquivalentTo(candidates);
        }

        [TestMethod]
        public void TestKeepCandidates()
        {
            Action act = () => SudokuStrategyResult.FromOnlyPossibleCandidates(null, null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("squares");

            act = () => SudokuStrategyResult.FromOnlyPossibleCandidates(Enumerable.Empty<SudokuSquare>(), null);
            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("candidates");

            SudokuSquare[] squares = new SudokuSquare[] { new SudokuSquare(0, 1, new int[] { 2, 5, 7 }) };
            act = () => SudokuStrategyResult.FromOnlyPossibleCandidates(squares, new int[] { });
            act.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("candidates");
            
            int[] candidates = new int[] { 2 };
            var result = SudokuStrategyResult.FromOnlyPossibleCandidates(squares, candidates);
            result.Result.Should().Be(StrategyResultOutcome.OnlyPossibleCandidatesFound);
            result.AffectedSquares.ShouldBeEquivalentTo(squares);
            result.Candidates.ShouldBeEquivalentTo(candidates);
        }
    }
}
