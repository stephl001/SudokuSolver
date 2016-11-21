using SudokuSolver.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SudokuSolver
{
    public sealed class Solver : ISudokuPuzzleSolver
    {
        private static readonly ISudokuStrategy[] _strategies = LoadStrategies();

        private static ISudokuStrategy[] LoadStrategies()
        {
            return Assembly.GetExecutingAssembly()
                           .GetExportedTypes()
                           .Where(t => !t.IsAbstract && (t.GetInterface("ISudokuStrategy") != null))
                           .Select(t => (ISudokuStrategy)Activator.CreateInstance(t))
                           .ToArray();
        }

        public Solver()
        {
            LoadedStrategies = _strategies.Length;
        }

        public int LoadedStrategies { get; }

        public IEnumerable<SolverResult> Solve(SudokuPuzzle puzzle)
        {
            if (puzzle == null)
                throw new ArgumentNullException(nameof(puzzle));
            if (!puzzle.IsValid)
                throw new ArgumentException("Entry puzzle is invalid.", nameof(puzzle));

            SudokuPuzzle actualPuzzle = puzzle;
            while (!actualPuzzle.IsCompleted)
            {
                SudokuStrategyResult result = _strategies.SelectMany(s => s.Query(actualPuzzle))
                                                         .OrderBy(r => r.Result)
                                                         .ThenByDescending(r => r.AffectedSquares.Count())
                                                         .FirstOrDefault();
                if (result == null)
                    yield break;

                actualPuzzle = ApplyResult(actualPuzzle, result);
                yield return new SolverResult(result, actualPuzzle);
            }
        }

        private SudokuPuzzle ApplyResult(SudokuPuzzle puzzle, SudokuStrategyResult result)
        {
            if (result.Result == StrategyResultOutcome.ValueFound)
                return puzzle.SetValue(result.AffectedSquares.Single());

            return puzzle.ClearCandidates(result.AffectedSquares, result.Candidates.ToArray());
        }
    }
}
