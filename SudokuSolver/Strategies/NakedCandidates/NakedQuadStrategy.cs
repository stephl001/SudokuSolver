using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedQuadStrategy : NakedCandidatesStrategyBase
    {
        public override string Name
        {
            get { return "Naked Quad"; }
        }

        protected override int NakedCandidatesCount
        {
            get { return 4; }
        }
    }
}
