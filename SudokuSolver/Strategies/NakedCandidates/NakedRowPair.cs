using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies.NakedCandidates
{
    public sealed class NakedRowPairStrategy : NakedPairStrategy
    {
        public override string Name
        {
            get { return "Naked Row Pair"; }
        }

        protected override Func<SudokuPuzzle, int, IEnumerable<SudokuSquare>> ReadUnitHandler
        {
            get
            {
                return (p, i) => p.ReadRow(i);
            }
        }
    }
}
