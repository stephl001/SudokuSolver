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
            XWingContext[] contexts = new XWingContext[] { new XWingContext(puzzle), new ReverseXWingContext(puzzle) };
            foreach (XWingContext ctx in contexts)
            {
                var groupedLockedCells = GetLockedCandidates(ctx.ReadUnitHandler, ctx.LockedCandidatesCount)
                    .GroupBy(tuple => (new string(tuple.Item2.OrderBy(ctx.UnitSelector).Select(s => (char)('0' + ctx.UnitSelector(s))).ToArray())) + tuple.Item1)
                    .Where(g => g.Count() == ctx.LockedCandidatesCount);
                foreach (var g in groupedLockedCells)
                {
                    int unit1 = g.Key[0] - '0';
                    int unit2 = g.Key[1] - '0';
                    SudokuSquare[] wingSquares = g.SelectMany(tuple => tuple.Item2).ToArray();
                    int lockedCandidate = g.First().Item1;
                    var squaresWithimpossibleCandidates = ctx.OppositeReadUnitHandler(unit1).Concat(ctx.OppositeReadUnitHandler(unit2)).Except(wingSquares).Where(s => s.Candidates.Contains(lockedCandidate)).ToArray();
                    if (squaresWithimpossibleCandidates.Any())
                    {
                        yield return SudokuStrategyResult.FromImpossibleCandidates(squaresWithimpossibleCandidates, new int[] { lockedCandidate });
                    }
                }
            }
        }

        private IEnumerable<Tuple<int,SudokuSquare[]>> GetLockedCandidates(Func<int, IEnumerable<SudokuSquare>> readUnitHandler, int nbLockedCandidates)
        {
            for (int i = 0; i < SudokuPuzzle.MaxValue; i++)
            {
                SudokuSquare[] squaresWithCandidates = readUnitHandler(i).Where(s => !s.IsValueSet).ToArray();
                int[] unitCandidates = squaresWithCandidates.SelectMany(s => s.Candidates).Distinct().ToArray();
                var candidatesToSquares = unitCandidates.ToDictionary(c => c, c => squaresWithCandidates.Where(s => s.Candidates.Contains(c)).ToArray());
                foreach (var kvp in candidatesToSquares.Where(kvp => kvp.Value.Length == nbLockedCandidates))
                {
                    yield return new Tuple<int, SudokuSquare[]>(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
