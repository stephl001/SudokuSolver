using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuSquareTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationX()
        {
            var s = new SudokuSquare(-1, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationY()
        {
            var s = new SudokuSquare(0, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationY()
        {
            var s = new SudokuSquare(0, 9, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationX()
        {
            var s = new SudokuSquare(9, 5, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationValue()
        {
            var s = new SudokuSquare(3, 5, 9);
        }

        [TestMethod]
        public void TestValidCreationValue()
        {
            var s = new SudokuSquare(3, 5, 4);
            Assert.IsTrue(s.IsValueSet);
            Assert.AreEqual(4, s.Value);
            Assert.IsTrue(s.Candidates.Length == 0);
            Assert.AreEqual(3, s.X);
            Assert.AreEqual(5, s.Y);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidatesX()
        {
            var s = new SudokuSquare(-1, 0, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidatesY()
        {
            var s = new SudokuSquare(0, -1, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationCandidatesY()
        {
            var s = new SudokuSquare(0, 9, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationCandidatesX()
        {
            var s = new SudokuSquare(9, 5, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidCreationCandidates()
        {
            var s = new SudokuSquare(3, 5, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationTooMuchCandidates()
        {
            var s = new SudokuSquare(3, 5, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidateValueTooSmall()
        {
            var s = new SudokuSquare(3, 5, new int[] { 2, 4, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidateValueTooBig()
        {
            var s = new SudokuSquare(3, 5, new int[] { 2, 4, 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCreationCandidateDuplicateValues()
        {
            var s = new SudokuSquare(3, 5, new int[] { 2, 4, 5, 2 });
        }

        [TestMethod]
        public void TestValidCreationCandidates()
        {
            var candidates = new int[] { 6, 8, 9 };
            var s = new SudokuSquare(3, 5, candidates);
            Assert.IsFalse(s.IsValueSet);
            Assert.AreEqual(0, s.Value);
            Assert.IsTrue(s.Candidates.SequenceEqual(candidates));
            Assert.AreEqual(3, s.X);
            Assert.AreEqual(5, s.Y);
        }

        [TestMethod]
        public void TestToString()
        {
            var s = new SudokuSquare(3, 5, 4);
            Assert.AreEqual($"Position: (3, 5), Value: 4, Candidates: {{}}", s.ToString());

            s = new SudokuSquare(3, 5, new int[] { 4, 8, 9 });
            Assert.AreEqual($"Position: (3, 5), Value: 0, Candidates: {{4, 8, 9}}", s.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestClearCandidatesWithValue()
        {
            var s = new SudokuSquare(2, 2, 3);
            var s2 = s.ClearCandidates(new int[] { 1, 2 });
        }

        [TestMethod]
        public void TestEqualityWithValue()
        {
            var s1 = new SudokuSquare(2, 2, 3);
            var s2 = new SudokuSquare(2, 1, 3);
            var s3 = new SudokuSquare(1, 2, 3);
            var s4 = new SudokuSquare(2, 2, 4);
            var s5 = new SudokuSquare(2, 2, 3);
            Assert.IsFalse(s1.Equals(null));
            Assert.AreEqual(s1, s5);
            Assert.AreNotEqual(s1, s2);
            Assert.AreNotEqual(s1, s3);
            Assert.AreNotEqual(s1, s4);
        }

        [TestMethod]
        public void TestHashValue()
        {
            IEnumerable<SudokuSquare> allSquares = GenerateAllPossibleSquaresWithValue();
            IEnumerable<int> allHashes = allSquares.Select(s => s.GetHashCode()).Distinct();

            Assert.AreEqual(allHashes.Count(), allSquares.Count());

            var s1 = new SudokuSquare(2, 2, 3);
            var s2 = new SudokuSquare(2, 2, 3);
            Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
        }

        [TestMethod]
        public void TestHashCandidates()
        {
            IEnumerable<SudokuSquare> allSquares = GenerateAllPossibleCandidatesForAllPossibleSquare();
            IEnumerable<int> allHashes = allSquares.Select(s => s.GetHashCode()).Distinct();

            Assert.AreEqual(allHashes.Count(), allSquares.Count());
        }

        private IEnumerable<SudokuSquare> GenerateAllPossibleSquaresWithValue()
        {
            for (int x=0; x<9; x++)
            {
                for (int y=0; y<9; y++)
                {
                    for (int v=0; v<9; v++)
                    {
                        yield return new SudokuSquare(x, y, v);
                    }
                }
            }
        }

        private IEnumerable<SudokuSquare> GenerateAllPossibleCandidatesForAllPossibleSquare()
        {
            List<int[]> permutations = new List<int[]>();
            GetAllPossiblePermutations(new int[] { }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, ref permutations);

            for (int x=0; x<9; x++)
            {
                for (int y=0; y<9; y++)
                {
                    foreach (int[] p in permutations)
                    {
                        yield return new SudokuSquare(x, y, p);
                    }
                }
            }
        }

        private void GetAllPossiblePermutations(int[] currentPermutation, int[] remainingNumbers, ref List<int[]> permutations)
        {
            foreach (int n in remainingNumbers)
            {
                int[] newPermutation = new int[currentPermutation.Length + 1];
                Array.Copy(currentPermutation, newPermutation, currentPermutation.Length);
                newPermutation[currentPermutation.Length] = n;
                permutations.Add(newPermutation);

                int[] newRemainingNumbers = remainingNumbers.Where(nb => nb > n).ToArray();
                GetAllPossiblePermutations(newPermutation, newRemainingNumbers, ref permutations);
            }
        }

        [TestMethod]
        public void TestHashCandidatesUnordered()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 3, 7, 8 });
            var s2 = new SudokuSquare(2, 2, new int[] { 3, 1, 8, 7 });
            Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
        }

        [TestMethod]
        public void TestEqualityWithCandidates()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            var s2 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            var s3 = new SudokuSquare(2, 2, new int[] { 1, 6, 5, 8 });
            var s4 = new SudokuSquare(2, 2, 4);
            Assert.AreNotEqual(s1, s4);
            Assert.AreEqual(s1, s2);
            Assert.AreEqual(s1, s3);
        }
    }
}
