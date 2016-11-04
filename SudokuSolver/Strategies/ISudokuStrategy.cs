using System.Threading;

namespace SudokuSolver.Strategies
{
    public interface ISudokuStrategy
    {
        string Name { get; }

        SudokuStrategyResult Query(SudokuPuzzle puzzle, CancellationToken cancellationToken);
    }
}
