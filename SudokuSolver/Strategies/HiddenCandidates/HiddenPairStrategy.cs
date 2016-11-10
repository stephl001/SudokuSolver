using LinqLib.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Strategies.HiddenCandidates
{
    public sealed class HiddenPairStrategy : HiddenCandidatesStrategyBase
    {
        public override string Name
        {
            get { return "Hidden Pair"; }
        }

        protected override int HiddenCandidatesCount
        {
            get {  return 2; }
        }
    }
}
