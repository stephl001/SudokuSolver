using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies.Advanced
{
    public class XWingContext
    {
        internal XWingContext(SudokuPuzzle puzzle)
            : this(puzzle.ReadRow, puzzle.ReadColumn, s => s.Column)
        { }

        protected XWingContext(Func<int, IEnumerable<SudokuSquare>> readUnitHandler, Func<int, IEnumerable<SudokuSquare>> oppositeReadUnitHandler, Func<SudokuSquare, int> unitSelector)
        {
            ReadUnitHandler = readUnitHandler;
            OppositeReadUnitHandler = oppositeReadUnitHandler;
            UnitSelector = unitSelector;
        }

        public int LockedCandidatesCount { get; } = 2;

        public Func<int, IEnumerable<SudokuSquare>> ReadUnitHandler { get; }

        public Func<int, IEnumerable<SudokuSquare>> OppositeReadUnitHandler { get; }

        public Func<SudokuSquare, int> UnitSelector { get; }
    }
}
