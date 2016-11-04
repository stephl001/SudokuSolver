using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenColumnSingleStrategy : HiddenSingleStrategy
    {
        public override string Name { get; } = "Hidden Column Single";

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadUnitHandler
        {
            get
            {
                return (p, i) => p.ReadColumn(i);
            }
        }
    }
}
