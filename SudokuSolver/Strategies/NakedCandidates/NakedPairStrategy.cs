using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedPairStrategy : PerUnitStrategy
    {
        public override string Name
        {
            get { return "Naked Pair"; }
        }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            foreach (Func<int, IEnumerable<SudokuSquare>> unitHandler in GetUnitHandlers(puzzle))
            {
                for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
                {
                    SudokuSquare[] unitCandidateSquares = unitHandler(i).Where(s => !s.IsValueSet).ToArray();
                    var groupCandidates = unitCandidateSquares.Where(s => s.Candidates.Count == 2).GroupBy(s => $"{s.Candidates[0]}{s.Candidates[1]}");
                    var group = groupCandidates.Where(g => g.Count() == 2).FirstOrDefault();
                    if (group != null)
                    {
                        yield return SudokuStrategyResult.FromOnlyPossibleCandidates(group, group.First().Candidates);
                    }
                }
            }
        }
    }
}
