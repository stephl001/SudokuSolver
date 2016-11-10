using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenQuadStrategy : HiddenCandidatesStrategyBase
    {
        public override string Name
        {
            get { return "Hidden Quad"; }
        }

        protected override int HiddenCandidatesCount
        {
            get { return 4; }
        }
    }
}
