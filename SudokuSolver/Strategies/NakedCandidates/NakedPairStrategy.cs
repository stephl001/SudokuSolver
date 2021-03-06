﻿using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedPairStrategy : NakedCandidatesStrategyBase
    {
        public override string Name
        {
            get { return "Naked Pair"; }
        }

        protected override int NakedCandidatesCount
        {
            get { return 2; }
        }
    }
}
