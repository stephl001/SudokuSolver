using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolverTests
{
    public sealed class PuzzleTest
    {
        private PuzzleTest(int[,] input, int[,] solution)
        {
            Input = input;
            Solution = solution;
        }

        internal int[,] Input { get; }

        internal int[,] Solution { get; }

        internal static PuzzleTest Load(string resourceName)
        {
            int[,] input = new int[9, 9];
            int[,] solution = null;

            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    FillPuzzleArray(input, reader);
                    if (reader.ReadLine() != null)
                    {
                        solution = new int[9, 9];
                        FillPuzzleArray(solution, reader);
                    }
                }
            }

            return new PuzzleTest(input, solution);
        }

        private static void FillPuzzleArray(int[,] input, StreamReader reader)
        {
            for (int iLine = 0; iLine < 9; iLine++)
            {
                string line = reader.ReadLine();
                var lineNumbers = line.Select(c => char.IsNumber(c) ? (c - '0') : 0).ToArray();
                for (int iCol = 0; iCol < 9; iCol++)
                {
                    input[iLine, iCol] = lineNumbers[iCol];
                }
            }
        }
    }
}
