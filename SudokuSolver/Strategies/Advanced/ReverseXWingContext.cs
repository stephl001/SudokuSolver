namespace SudokuSolver.Strategies.Advanced
{
    public class ReverseXWingContext : XWingContext
    {
        internal ReverseXWingContext(SudokuPuzzle puzzle)
            : base(puzzle.ReadColumn, puzzle.ReadRow, s => s.Row)
        { }
    }
}
