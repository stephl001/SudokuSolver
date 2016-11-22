using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Strategies.Advanced
{
    public class XWingStrategy : SudokuStrategyBase
    {
        public override string Name
        {
            get { return "X-Wing"; }
        }

        protected override IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle)
        {
            XWingContext[] contexts = new XWingContext[] { CreateContext(puzzle), CreateReverseContext(puzzle) };
            foreach (XWingContext ctx in contexts)
            {
                var groupedLockedCells = GetLockedCandidates(ctx.ReadUnitHandler, ctx.LockedCandidatesCount)
                    .GroupBy(tuple => tuple.Item1)
                    .Where(g => g.Count() == ctx.LockedCandidatesCount);
                foreach (var g in groupedLockedCells)
                {
                    //How many distinct opposite column/row?
                    int[] oppositeUnits = g.SelectMany(t => t.Item2.Select(ctx.UnitSelector)).Distinct().ToArray();
                    if (oppositeUnits.Length != ctx.LockedCandidatesCount)
                        continue;

                    SudokuSquare[] wingSquares = g.SelectMany(tuple => tuple.Item2).ToArray();
                    int lockedCandidate = g.Key;
                    var squaresWithimpossibleCandidates = oppositeUnits.SelectMany(ctx.OppositeReadUnitHandler)
                                                                       .Except(wingSquares)
                                                                       .Where(s => s.Candidates.Contains(lockedCandidate))
                                                                       .ToArray();
                    
                    if (squaresWithimpossibleCandidates.Any())
                    {
                        yield return SudokuStrategyResult.FromImpossibleCandidates(squaresWithimpossibleCandidates, new int[] { lockedCandidate }, Name);
                    }
                }
            }
        }

        private IEnumerable<Tuple<int,SudokuSquare[]>> GetLockedCandidates(Func<int, IEnumerable<SudokuSquare>> readUnitHandler, int nbLockedCandidates)
        {
            for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
            {
                SudokuSquare[] squaresWithCandidates = readUnitHandler(i).Where(s => !s.IsValueSet).ToArray();
                var candidatesToSquares = GetSquaresByCandidate(squaresWithCandidates);
                foreach (var kvp in candidatesToSquares.Where(kvp => ((kvp.Value.Length > 1) && (kvp.Value.Length <= nbLockedCandidates))))
                {
                    yield return new Tuple<int, SudokuSquare[]>(kvp.Key, kvp.Value);
                }
            }
        }

        protected virtual XWingContext CreateContext(SudokuPuzzle puzzle)
        {
            return new XWingContext(puzzle);
        }

        protected virtual XWingContext CreateReverseContext(SudokuPuzzle puzzle)
        {
            return new ReverseXWingContext(puzzle);
        }
    }
}
