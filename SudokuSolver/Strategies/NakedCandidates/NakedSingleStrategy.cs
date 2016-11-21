using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedSingleStrategy : SudokuStrategyBase
    {
        public override string Name { get; } = "Naked Single";

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            SudokuSquare square = puzzle.ReadAllSquares().FirstOrDefault(s => !s.IsValueSet && (s.Candidates.Count == 1));
            if (square == null)
                yield break;

            var valueSquare = new SudokuSquare(square.Row, square.Column, square.Candidates.Single());
            yield return SudokuStrategyResult.FromValue(valueSquare, Name);
        }
    }
}
