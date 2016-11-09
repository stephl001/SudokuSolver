using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedTripleStrategy : NakedCandidatesStrategyBase
    {
        public override string Name
        {
            get { return "Naked Triple"; }
        }

        protected override int NakedCandidatesCount
        {
            get { return 3; }
        }
    }
}
