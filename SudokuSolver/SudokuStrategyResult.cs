using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly static SudokuStrategyResult _notFound = new SudokuStrategyResult();

        private SudokuStrategyResult()
        {
        }

        private SudokuStrategyResult(SudokuSquare s)
        {
            AffectedSquares = Array.AsReadOnly(new SudokuSquare[] { s });
            Result = StrategyResultOutcome.ValueFound;
        }

        private SudokuStrategyResult(IEnumerable<SudokuSquare> squares, IEnumerable<int> candidates)
        {
            AffectedSquares = Array.AsReadOnly(squares.ToArray());
            Candidates = Array.AsReadOnly(candidates.ToArray());
            Result = StrategyResultOutcome.ImpossibleCandidatesFound;
        }

        public StrategyResultOutcome Result { get; } = StrategyResultOutcome.NotFound;

        public IEnumerable<SudokuSquare> AffectedSquares { get; } = Enumerable.Empty<SudokuSquare>();

        public IEnumerable<int> Candidates { get; } = Enumerable.Empty<int>();

        public static SudokuStrategyResult NotFound
        {
            get { return _notFound; }
        }

        public static SudokuStrategyResult FromValue(SudokuSquare s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.IsValueSet)
                throw new ArgumentException("Argument must be a value square.", nameof(s));

            return new SudokuStrategyResult(s);
        }

        public static SudokuStrategyResult FromImpossibleCandidates(IEnumerable<SudokuSquare> squares, IEnumerable<int> candidates)
        {
            if (squares == null)
                throw new ArgumentNullException(nameof(squares));
            if (candidates == null)
                throw new ArgumentNullException(nameof(candidates));

            int[] localCandidates = candidates.ToArray();
            if (localCandidates.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(candidates), "You cannot provide an empty collection of candidates.");

            return new SudokuStrategyResult(squares, localCandidates);
        }
    }
}
