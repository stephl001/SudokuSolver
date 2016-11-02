using System.Collections.Generic;

namespace SudokuSolver
{
    public enum StrategyResultOutcome
    {
        NotFound,
        ValueFound,
        ImpossibleCandidatesFound
    }

    public sealed class SudokuStrategyResult
    {
        
        public StrategyResultOutcome Result { get; }

        public IEnumerable<SudokuSquare> AffectedSquares { get; }

        public int? Value { get; }

        public IEnumerable<int> Candidates { get; }
    }
}
