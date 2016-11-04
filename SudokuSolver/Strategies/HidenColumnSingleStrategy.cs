using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies
{
    public sealed class HiddenColumnSingleStrategy : HiddenSingleStrategy
    {
        public override string Name { get; } = "Hidden Column Single";

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadGroupHandler
        {
            get
            {
                return (p, i) => p.ReadColumn(i);
            }
        }
    }
}
