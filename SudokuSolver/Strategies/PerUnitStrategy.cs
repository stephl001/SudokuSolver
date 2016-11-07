using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies
{
    public abstract class PerUnitStrategy : SudokuStrategyBase
    {
        protected abstract Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadUnitHandler
        {
            get;
        }
    }
}
