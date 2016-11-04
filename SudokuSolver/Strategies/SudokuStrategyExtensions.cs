using System.Threading;

namespace SudokuSolver.Strategies
{
    public static class SudokuStrategyExtensions
    {
        public static SudokuStrategyResult Query(this ISudokuStrategy strategy, SudokuPuzzle puzzle)
        {
            return strategy.Query(puzzle, CancellationToken.None);
        }
    }
}
