namespace SudokuSolver.Strategies.Advanced
{
    public class ReverseSwordfishContext : ReverseXWingContext
    {
        internal ReverseSwordfishContext(SudokuPuzzle puzzle)
            : base(puzzle)
        { }

        public override int LockedCandidatesCount
        {
            get
            {
                return 3;
            }
        }
    }
}
