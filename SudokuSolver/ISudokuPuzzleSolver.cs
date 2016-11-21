using System.Collections.Generic;

namespace SudokuSolver
{
    public interface ISudokuPuzzleSolver
    {
        IEnumerable<SolverResult> Solve(SudokuPuzzle puzzle);
    }
}
