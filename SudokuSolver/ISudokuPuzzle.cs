using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface ISudokuPuzzle
    {
        IEnumerable<SudokuSquare> ReadRow(int rowIndex);
        IEnumerable<SudokuSquare> ReadColumn(int columnIndex);
        IEnumerable<SudokuSquare> ReadAllSquares();
        IEnumerable<SudokuSquare> ReadBox(int boxIndex);
        ISudokuPuzzle SetSquare(SudokuSquare square);
        int Width { get; }
        int Height { get; }
        bool IsCompleted { get; }
        bool IsValid { get; }
    }
}
