using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public static class SudokuPuzzleExtensions
    {
        public static IEnumerable<SudokuSquare> ReadBuddiesWithValue(this SudokuPuzzle puzzle, SudokuSquare square)
        {
            return puzzle.ReadBuddies(square).Where(s => s.IsValueSet);
        }

        public static IEnumerable<SudokuSquare> ReadBuddiesWithoutValue(this SudokuPuzzle puzzle, SudokuSquare square)
        {
            return puzzle.ReadBuddies(square).Where(s => !s.IsValueSet);
        }

        public static IEnumerable<int> ReadBuddiesValues(this SudokuPuzzle puzzle, SudokuSquare square)
        {
            return ReadBuddiesWithValue(puzzle, square).Select(s => s.Value).Distinct();
        }

        public static IEnumerable<int> ReadBuddiesCandidates(this SudokuPuzzle puzzle, SudokuSquare square)
        {
            return puzzle.ReadBuddies(square)
                         .Where(s => !s.IsValueSet)
                         .SelectMany(s => s.Candidates)
                         .Distinct();
        }
    }
}
