using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SudokuSolverTests
{
    [TestClass]
    public abstract class SudokuTests
    {
        private static IDictionary<string, SudokuPuzzle> _allPuzzles = new Dictionary<string, SudokuPuzzle>();
        private static IDictionary<string, SudokuPuzzle> _allPuzzleSolutions = new Dictionary<string, SudokuPuzzle>();

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string resourceName in resourceNames)
            {
                PuzzleTest pt = PuzzleTest.Load(resourceName);
                SudokuPuzzle puzzle = new SudokuPuzzle(pt.Input);
                puzzle.IsValid.Should().Be(!resourceName.Contains("invalid"));
                _allPuzzles.Add(PuzzleNameFromResourceName(resourceName), puzzle);
                if (pt.Solution != null)
                {
                    puzzle = new SudokuPuzzle(pt.Solution);
                    puzzle.IsValid.Should().BeTrue();
                    _allPuzzleSolutions.Add(PuzzleNameFromResourceName(resourceName), puzzle);
                }
            }
        }

        private static string PuzzleNameFromResourceName(string resourceName)
        {
            var m = Regex.Match(resourceName, @"^SudokuSolverTests\.puzzles\.([a-zA-Z]+)\.txt$");
            return m.Groups[1].Value;
        }

        protected IEnumerable<SudokuPuzzle> GetPuzzles()
        {
            return Array.AsReadOnly(_allPuzzles.Values.ToArray());
        }

        protected SudokuPuzzle GetPuzzle(string name)
        {
            return _allPuzzles[name];
        }

        protected SudokuPuzzle GetPuzzleSolution(string name)
        {
            return _allPuzzleSolutions[name];
        }
    }
}
