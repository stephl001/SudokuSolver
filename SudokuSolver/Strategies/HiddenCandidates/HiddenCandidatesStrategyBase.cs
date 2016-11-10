using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public abstract class HiddenCandidatesStrategyBase : PerUnitStrategy
    {
        protected abstract int HiddenCandidatesCount { get; }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            foreach (Func<int, IEnumerable<SudokuSquare>> unitHandler in GetUnitHandlers(puzzle))
            {
                for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
                {
                    SudokuSquare[] squaresWithCandidates = unitHandler(i).Where(s => !s.IsValueSet).ToArray();
                    int[] allCandidates = squaresWithCandidates.SelectMany(s => s.Candidates).Distinct().ToArray();
                    Dictionary<int, SudokuSquare[]> candidateToSquares = allCandidates.ToDictionary(c => c, c => squaresWithCandidates.Where(s => s.Candidates.Contains(c)).ToArray());
                    var potentialEntries = candidateToSquares.Where(kvp => (kvp.Value.Length > 1) && (kvp.Value.Length <= HiddenCandidatesCount)).ToArray();
                    if (potentialEntries.Length < HiddenCandidatesCount)
                        continue;

                    //Find if there exist three entries with the same squares for 3 different candidates.
                    //If so, we just found hidden candidates.
                    var entriesWithHiddenCandidates = potentialEntries.Combinations(HiddenCandidatesCount, false).FirstOrDefault(kvps => kvps.SelectMany(kvp => kvp.Value).Distinct().Count() == HiddenCandidatesCount);
                    if (entriesWithHiddenCandidates == null)
                        continue;

                    int[] hiddenCandidates = entriesWithHiddenCandidates.Select(kvp => kvp.Key).ToArray();
                    int[] actualCandidates = entriesWithHiddenCandidates.SelectMany(kvp => kvp.Value).SelectMany(s => s.Candidates).Distinct().ToArray();
                    int[] impossibleCandidates = actualCandidates.Except(hiddenCandidates).ToArray();
                    if (impossibleCandidates.Length == 0)
                        continue;

                    var squaresWithHiddenCandidates = entriesWithHiddenCandidates.SelectMany(kvp => kvp.Value).Distinct();
                    yield return SudokuStrategyResult.FromImpossibleCandidates(squaresWithHiddenCandidates, impossibleCandidates);
                }
            }
        }
    }
}
