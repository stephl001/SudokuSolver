using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies
{
    public sealed class HiddenRowSingleStrategy : HiddenSingleStrategy
    {
        public override string Name { get; } = "Hidden Row Single";

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadGroupHandler
        {
            get
            {
                return (p, i) => p.ReadRow(i);
            }
        }
    }
}
