using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public class PointingCandidatesStrategy : SudokuStrategyBase
    {
        public override string Name
        {
            get { return "Pointing Candidates"; }
        }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
            {
                SudokuSquare[] squaresWithCandidates = puzzle.ReadBox(i).Where(s => !s.IsValueSet).ToArray();
                int[] boxCandidates = squaresWithCandidates.SelectMany(s => s.Candidates).Distinct().ToArray();
                foreach (int c in boxCandidates)
                {
                    SudokuSquare[] candidateSquares = squaresWithCandidates.Where(s => s.Candidates.Contains(c)).ToArray();
                    Func<SudokuSquare, int>[] rowColumnHandlers = new Func<SudokuSquare, int>[] { s => s.Row, s => s.Column };
                    Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>>[] rowColumnReadersHandlers = new Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>>[] { (p,ind) => p.ReadRow(ind), (p,ind) => p.ReadColumn(ind) };

                    for (int k = 0; k < 2; k++)
                    {
                        if ((candidateSquares.Select(rowColumnHandlers[k]).Distinct().Count() == 1))
                        {
                            //We have a pointing pair/triple
                            SudokuSquare[] squaresWithimpossibleCandidates = rowColumnReadersHandlers[k](puzzle, rowColumnHandlers[k](candidateSquares[0]))
                                                                                   .Where(s => !s.IsValueSet && s.Candidates.Contains(c))
                                                                                   .Except(candidateSquares)
                                                                                   .ToArray();
                            if (squaresWithimpossibleCandidates.Length == 0)
                                continue;

                            yield return SudokuStrategyResult.FromImpossibleCandidates(squaresWithimpossibleCandidates, new int[] { c }, Name);
                        }
                    }
                }
            }
        }
    }
}
