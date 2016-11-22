namespace SudokuSolver.Strategies.Advanced
{
    public class SwordfishStrategy : XWingStrategy
    {
        public override string Name
        {
            get
            {
                return "Swordfish";
            }
        }

        protected override XWingContext CreateContext(SudokuPuzzle puzzle)
        {
            return new SwordfishContext(puzzle);
        }

        protected override XWingContext CreateReverseContext(SudokuPuzzle puzzle)
        {
            return new ReverseSwordfishContext(puzzle);
        }
    }
}
