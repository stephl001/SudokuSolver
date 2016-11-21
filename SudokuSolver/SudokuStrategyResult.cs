using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public enum StrategyResultOutcome
    {
        ValueFound,
        ImpossibleCandidatesFound
    }

    public sealed class SudokuStrategyResult
    {
        private SudokuStrategyResult(SudokuSquare s, string strategyName)
            : this(new SudokuSquare[] { s }, new int[] { }, StrategyResultOutcome.ValueFound, strategyName)
        {
        }

        private SudokuStrategyResult(IEnumerable<SudokuSquare> squares, IEnumerable<int> candidates, StrategyResultOutcome outcome, string strategyName)
        {
            AffectedSquares = Array.AsReadOnly(squares.ToArray());
            Candidates = Array.AsReadOnly(candidates.ToArray());
            Result = outcome;
            StrategyName = strategyName;
        }

        public string StrategyName { get; }

        public StrategyResultOutcome Result { get; }

        public IEnumerable<SudokuSquare> AffectedSquares { get; } = Enumerable.Empty<SudokuSquare>();

        public IEnumerable<int> Candidates { get; } = Enumerable.Empty<int>();

        public static SudokuStrategyResult FromValue(SudokuSquare s, string strategyName)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.IsValueSet)
                throw new ArgumentException("Argument must be a value square.", nameof(s));

            return new SudokuStrategyResult(s, strategyName);
        }

        public static SudokuStrategyResult FromImpossibleCandidates(IEnumerable<SudokuSquare> squares, IEnumerable<int> candidates, string strategyName)
        {
            if (squares == null)
                throw new ArgumentNullException(nameof(squares));
            if (candidates == null)
                throw new ArgumentNullException(nameof(candidates));

            int[] localCandidates = candidates.ToArray();
            if (localCandidates.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(candidates), "You cannot provide an empty collection of candidates.");

            return new SudokuStrategyResult(squares, localCandidates, StrategyResultOutcome.ImpossibleCandidatesFound, strategyName);
        }
    }
}
