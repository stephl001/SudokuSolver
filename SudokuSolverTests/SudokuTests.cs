using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;

namespace SudokuSolverTests
{
    [TestClass]
    public abstract class SudokuTests
    {
        private static SudokuPuzzle _easyPuzzle;
        private static SudokuPuzzle _easyPuzzleSolution;
        private static SudokuPuzzle _hardPuzzle;
        private static SudokuPuzzle _hardPuzzleSolution;
        private static SudokuPuzzle _noNakedSinglePuzzle;
        private static SudokuPuzzle _noHiddenSinglePuzzle;
        private static SudokuPuzzle _hiddenSinglePuzzle;
        private static SudokuPuzzle _nakedPairPuzzle;

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var pt = PuzzleTest.Load("easy");
            _easyPuzzle = new SudokuPuzzle(pt.Input);
            _easyPuzzle.IsValid.Should().BeTrue();
            _easyPuzzleSolution = new SudokuPuzzle(pt.Solution);
            _easyPuzzleSolution.ValidationErrors.Should().BeEmpty();

            pt = PuzzleTest.Load("hard");
            _hardPuzzle = new SudokuPuzzle(pt.Input);
            _hardPuzzle.IsValid.Should().BeTrue();
            _hardPuzzleSolution = new SudokuPuzzle(pt.Solution);
            _hardPuzzleSolution.ValidationErrors.Should().BeEmpty();

            pt = PuzzleTest.Load("nonakedsingle");
            _noNakedSinglePuzzle = new SudokuPuzzle(pt.Input);
            _noNakedSinglePuzzle.IsValid.Should().BeTrue();

            pt = PuzzleTest.Load("nohiddensingle");
            _noHiddenSinglePuzzle = new SudokuPuzzle(pt.Input);
            _noHiddenSinglePuzzle.IsValid.Should().BeTrue();

            pt = PuzzleTest.Load("hiddensingle");
            _hiddenSinglePuzzle = new SudokuPuzzle(pt.Input);
            _hiddenSinglePuzzle.IsValid.Should().BeTrue();

            pt = PuzzleTest.Load("nakedpair");
            _nakedPairPuzzle = new SudokuPuzzle(pt.Input);
            _nakedPairPuzzle.IsValid.Should().BeTrue();
        }

        protected SudokuPuzzle EasyPuzzle
        {
            get { return _easyPuzzle; }
        }

        protected SudokuPuzzle EasyPuzzleSolution
        {
            get { return _easyPuzzleSolution; }
        }

        protected SudokuPuzzle HardPuzzle
        {
            get { return _hardPuzzle; }
        }

        protected SudokuPuzzle HardPuzzleSolution
        {
            get { return _hardPuzzleSolution; }
        }

        protected SudokuPuzzle NoNakedSinglePuzzle
        {
            get { return _noNakedSinglePuzzle; }
        }

        protected SudokuPuzzle NoHiddenSinglePuzzle
        {
            get { return _noHiddenSinglePuzzle; }
        }

        protected SudokuPuzzle HiddenSinglePuzzle
        {
            get { return _hiddenSinglePuzzle; }
        }

        protected SudokuPuzzle NakedPairPuzzle
        {
            get { return _nakedPairPuzzle; }
        }
    }
}
