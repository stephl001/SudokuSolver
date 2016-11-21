using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenSingleStrategy : PerUnitStrategy
    {
        public override string Name
        {
            get { return "Hidden Single"; }
        }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            foreach (Func<int, IEnumerable<SudokuSquare>> unitHandler in GetUnitHandlers(puzzle))
            {
                for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
                {
                    IEnumerable<int> allCandidates = unitHandler(i).SelectMany(s => s.Candidates);
                    var group = allCandidates.GroupBy(c => c).Where(g => g.Count() == 1).FirstOrDefault();
                    if (group != null)
                    {
                        int hiddenCandidate = group.Key;
                        SudokuSquare targetSquare = unitHandler(i).Single(s => s.Candidates.Contains(hiddenCandidate));
                        var newSquare = new SudokuSquare(targetSquare.Row, targetSquare.Column, hiddenCandidate);

                        yield return SudokuStrategyResult.FromValue(newSquare, Name);
                    }
                }
            }
        }
    }
}
