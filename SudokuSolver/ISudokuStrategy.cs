namespace SudokuSolver
{
    public interface ISudokuStrategy
    {
        string Name { get; }

        SudokuStrategyResult Query(SudokuPuzzle puzzle);
    }
}
