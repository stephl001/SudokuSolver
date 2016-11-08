using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    public abstract class SudokuStrategyBase : ISudokuStrategy
    {
        public abstract string Name { get; }

        IEnumerable<SudokuStrategyResult> ISudokuStrategy.Query(SudokuPuzzle puzzle)
        {
            if (puzzle == null)
                throw new ArgumentNullException("puzzle");
            if (puzzle.IsCompleted)
                throw new InvalidOperationException("You cannot query a strategy result on a completed or invalid puzzle.");

            return PerformQuery(puzzle);
        }

        protected abstract IEnumerable<SudokuStrategyResult> PerformQuery(SudokuPuzzle puzzle);
    }
}
