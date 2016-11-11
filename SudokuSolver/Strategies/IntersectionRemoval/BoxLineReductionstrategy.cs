using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public class BoxLineReductionStrategy : PerUnitStrategy
    {
        public override string Name
        {
            get { return "Box/Line Reduction"; }
        }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            foreach (Func<int, IEnumerable<SudokuSquare>> unitHandler in GetRowColumnUnitHandlers(puzzle))
            {
                for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
                {
                    SudokuSquare[] unitCandidateSquares = unitHandler(i).Where(s => !s.IsValueSet).ToArray();
                    int[] unitCandidates = unitCandidateSquares.SelectMany(s => s.Candidates).Distinct().ToArray();
                    foreach (int c in unitCandidates)
                    {
                        SudokuSquare[] squaresForCandidate = unitCandidateSquares.Where(s => s.Candidates.Contains(c)).ToArray();
                        if ((squaresForCandidate.Select(s => s.Box).Distinct().Count() == 1) &&
                            (squaresForCandidate.Length > 1))
                        {
                            //Found it!
                            SudokuSquare[] boxSquaresWithInvalidCandidates = puzzle.ReadBox(squaresForCandidate[0].Box).Except(squaresForCandidate).Where(s => s.Candidates.Contains(c)).ToArray();
                            if (boxSquaresWithInvalidCandidates.Any())
                            {
                                //Found impossible candidates in box.
                                yield return SudokuStrategyResult.FromImpossibleCandidates(boxSquaresWithInvalidCandidates, new int[] { c });
                            }
                        }
                    }
                }
            }
        }
    }
}
