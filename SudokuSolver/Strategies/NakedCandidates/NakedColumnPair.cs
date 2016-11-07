﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedColumnPairStrategy : NakedPairStrategy
    {
        public override string Name
        {
            get { return "Naked Column Pair"; }
        }

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadUnitHandler
        {
            get
            {
                return (p, i) => p.ReadColumn(i);
            }
        }
    }
}