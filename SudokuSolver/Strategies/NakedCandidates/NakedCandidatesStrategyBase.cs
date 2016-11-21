using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public abstract class NakedCandidatesStrategyBase : PerUnitStrategy
    {
        protected abstract int NakedCandidatesCount { get; }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            foreach (Func<int, IEnumerable<SudokuSquare>> unitHandler in GetUnitHandlers(puzzle))
            {
                for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
                {
                    SudokuSquare[] unitCandidateSquares = unitHandler(i).Where(s => !s.IsValueSet).ToArray();
                    SudokuSquare[] potentialSquares = unitCandidateSquares.Where(s => (s.Candidates.Count > 1) && (s.Candidates.Count <= NakedCandidatesCount)).ToArray();
                    if (potentialSquares.Length < NakedCandidatesCount)
                        continue;

                    IEnumerable<SudokuSquare> nakedDoubles = potentialSquares.Combinations(NakedCandidatesCount, false).FirstOrDefault(seq => seq.SelectMany(s => s.Candidates).Distinct().Count() == NakedCandidatesCount);
                    if (nakedDoubles == null)
                        continue;

                    var foundCandidates = nakedDoubles.SelectMany(s => s.Candidates).Distinct().ToArray();
                    var affectedSquares = unitCandidateSquares.Except(nakedDoubles).Where(s => s.Candidates.Intersect(foundCandidates).Any()).ToArray();
                    if (affectedSquares.Any())
                        yield return SudokuStrategyResult.FromImpossibleCandidates(affectedSquares, foundCandidates, Name);
                }
            }
        }
    }
}
