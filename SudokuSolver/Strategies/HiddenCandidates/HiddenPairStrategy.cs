using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenPairStrategy : PerUnitStrategy
    {
        public override string Name
        {
            get { return "Hidden Pair"; }
        }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            foreach (Func<int, IEnumerable<SudokuSquare>> unitHandler in GetUnitHandlers(puzzle))
            {
                for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
                {
                    SudokuSquare[] squaresWithCandidates = unitHandler(i).Where(s => !s.IsValueSet).ToArray();
                    int[] allCandidates = squaresWithCandidates.SelectMany(s => s.Candidates).Distinct().ToArray();
                    Dictionary<int,SudokuSquare[]> candidateToSquares = allCandidates.ToDictionary(c => c, c => squaresWithCandidates.Where(s => s.Candidates.Contains(c)).ToArray());
                    var potentialEntries = candidateToSquares.Where(kvp => kvp.Value.Length == 2).ToArray();
                    if (potentialEntries.Length < 2)
                        continue;

                    //Find if there exist two entries with the same squares for 2 different candidates.
                    //If so, we just found hidden candidates.
                    var entriesWithHiddenCandidates = potentialEntries.Combinations(2, false).FirstOrDefault(kvps => kvps.SelectMany(kvp => kvp.Value).Distinct().Count() == 2);
                    if (entriesWithHiddenCandidates == null)
                        continue;

                    int[] hiddenCandidates = entriesWithHiddenCandidates.Select(kvp => kvp.Key).ToArray();
                    int[] actualCandidates = entriesWithHiddenCandidates.SelectMany(kvp => kvp.Value).SelectMany(s => s.Candidates).Distinct().ToArray();
                    int[] impossibleCandidates = actualCandidates.Except(hiddenCandidates).ToArray();
                    if (impossibleCandidates.Length == 0)
                        continue;

                    yield return SudokuStrategyResult.FromImpossibleCandidates(entriesWithHiddenCandidates.First().Value, impossibleCandidates);
                }
            }
        }
    }
}
