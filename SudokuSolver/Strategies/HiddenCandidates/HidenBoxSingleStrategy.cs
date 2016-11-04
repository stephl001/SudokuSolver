﻿using System;
using System.Collections.Generic;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenBoxSingleStrategy : HiddenSingleStrategy
    {
        public override string Name { get; } = "Hidden Box Single";

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadUnitHandler
        {
            get
            {
                return (p, i) => p.ReadBox(i);
            }
        }
    }
}