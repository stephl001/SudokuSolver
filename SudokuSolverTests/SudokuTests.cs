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
        private static SudokuPuzzle _noHiddenBoxSinglePuzzle;
        private static SudokuPuzzle _hiddenBoxSinglePuzzle;
        //private static SudokuPuzzle _hard2PuzzleSolution;

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

            pt = PuzzleTest.Load("nohiddenboxsingle");
            _noHiddenBoxSinglePuzzle = new SudokuPuzzle(pt.Input);
            _noHiddenBoxSinglePuzzle.IsValid.Should().BeTrue();

            pt = PuzzleTest.Load("hiddenboxsingle");
            _hiddenBoxSinglePuzzle = new SudokuPuzzle(pt.Input);
            _hiddenBoxSinglePuzzle.IsValid.Should().BeTrue();
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

        protected SudokuPuzzle NoHiddenBoxSinglePuzzle
        {
            get { return _noHiddenBoxSinglePuzzle; }
        }

        protected SudokuPuzzle HiddenBoxSinglePuzzle
        {
            get { return _hiddenBoxSinglePuzzle; }
        }
    }
}
