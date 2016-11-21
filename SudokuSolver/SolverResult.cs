namespace SudokuSolver
{
    public sealed class SolverResult
    {
        internal SolverResult(SudokuStrategyResult result, SudokuPuzzle newPuzzle)
        {
            StrategyResult = result;
            NewPuzzle = newPuzzle;
        }

        public SudokuStrategyResult StrategyResult { get; }

        public SudokuPuzzle NewPuzzle { get; }
    }
}
