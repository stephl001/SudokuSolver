using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies
{
    public abstract class PerUnitStrategy : SudokuStrategyBase
    {
        protected IEnumerable<Func<int, IEnumerable<SudokuSquare>>> GetUnitHandlers(SudokuPuzzle puzzle)
        {
            yield return i => puzzle.ReadRow(i);
            yield return i => puzzle.ReadColumn(i);
            yield return i => puzzle.ReadBox(i);
        }
    }
}
