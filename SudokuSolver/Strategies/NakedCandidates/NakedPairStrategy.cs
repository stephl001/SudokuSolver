using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public abstract class NakedPairStrategy : PerUnitStrategy
    {
        protected override SudokuStrategyResult PerformQuery(SudokuPuzzle puzzle, CancellationToken cancelationToken)
        {
            for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
            {
                SudokuSquare[] unitCandidateSquares = ReadUnitHandler(puzzle, i).Where(s => !s.IsValueSet).ToArray();
                var groupCandidates = unitCandidateSquares.Where(s => s.Candidates.Count == 2).GroupBy(s => $"{s.Candidates[0]}{s.Candidates[1]}");
                var group = groupCandidates.Where(g => g.Count() == 2).FirstOrDefault();
                if (group != null)
                {
                    return SudokuStrategyResult.FromOnlyPossibleCandidates(group, group.First().Candidates);
                }
            }

            return SudokuStrategyResult.NotFound;
        }
    }
}
