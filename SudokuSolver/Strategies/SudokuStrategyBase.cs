using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    public abstract class SudokuStrategyBase : ISudokuStrategy
    {
        public abstract string Name { get; }

        IEnumerable<SudokuStrategyResult> ISudokuStrategy.Query(SudokuPuzzle puzzle)
        {
            if (puzzle == null)
                throw new ArgumentNullException("puzzle");
            if (puzzle.IsCompleted)
                throw new InvalidOperationException("You cannot query a strategy result on a completed or invalid puzzle.");

            return PerformQuery(puzzle);
        }

        protected abstract IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle);

        protected int[] GetDistinctCandidates(IEnumerable<SudokuSquare> squares)
        {
            return squares.Where(s => !s.IsValueSet)
                          .SelectMany(s => s.Candidates)
                          .Distinct()
                          .ToArray();
        }

        protected IDictionary<int, SudokuSquare[]> GetSquaresByCandidate(IEnumerable<SudokuSquare> squares)
        {
            int[] unitCandidates = GetDistinctCandidates(squares);
            var candidatesToSquares = unitCandidates.ToDictionary(c => c, c => squares.Where(s => s.Candidates.Contains(c)).ToArray());
            return candidatesToSquares;
        }
    }
}
