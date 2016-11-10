using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenTripleStrategy : HiddenCandidatesStrategyBase
    {
        public override string Name
        {
            get { return "Hidden Triple"; }
        }

        protected override int HiddenCandidatesCount
        {
            get { return 3; }
        }
    }
}
