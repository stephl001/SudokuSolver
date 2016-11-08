using System.Collections.Generic;

namespace SudokuSolver.Strategies
{
    public interface ISudokuStrategy
    {
        string Name { get; }

        IEnumerable<SudokuStrategyResult> Query(SudokuPuzzle puzzle);
    }
}
