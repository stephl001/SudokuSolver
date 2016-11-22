namespace SudokuSolver.Strategies.Advanced
{
    public class SwordfishContext : XWingContext
    {
        internal SwordfishContext(SudokuPuzzle puzzle)
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
