using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies
{
    public sealed class HiddenBoxSingleStrategy : HiddenSingleStrategy
    {
        public override string Name { get; } = "Hidden Box Single";

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadGroupHandler
        {
            get
            {
                return (p, i) => p.ReadBox(i);
            }
        }
    }
}
