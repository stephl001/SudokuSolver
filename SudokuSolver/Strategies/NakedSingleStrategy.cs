using System;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies
{
    public sealed class NakedSingleStrategy : SudokuStrategyBase
    {
        public override string Name { get; } = "Naked Single";

        protected override SudokuStrategyResult PerformQuery(SudokuPuzzle puzzle, CancellationToken cancelationToken)
        {
            SudokuSquare square = puzzle.ReadAllSquares().FirstOrDefault(s => !s.IsValueSet && (s.Candidates.Count == 1));
            if (square == null)
                return SudokuStrategyResult.NotFound;

            var valueSquare = new SudokuSquare(square.Row, square.Column, square.Candidates.Single());
            return SudokuStrategyResult.FromValue(valueSquare);
        }
    }
}
