using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public abstract class HiddenSingleStrategy : PerUnitStrategy
    {
        protected override SudokuStrategyResult PerformQuery(SudokuPuzzle puzzle, CancellationToken cancelationToken)
        {
            for (int i=0; i<SudokuPuzzle.MaxValue; i++)
            {
                IEnumerable<int> allCandidates = ReadUnitHandler(puzzle, i).SelectMany(s => s.Candidates);
                var group = allCandidates.GroupBy(c => c).Where(g => g.Count() == 1).FirstOrDefault();
                if (group != null)
                {
                    int hiddenCandidate = group.Key;
                    SudokuSquare targetSquare = ReadUnitHandler(puzzle, i).Single(s => s.Candidates.Contains(hiddenCandidate));
                    var newSquare = new SudokuSquare(targetSquare.Row, targetSquare.Column, hiddenCandidate);

                    return SudokuStrategyResult.FromValue(newSquare);
                }
            }

            return SudokuStrategyResult.NotFound;
        }
    }
}
