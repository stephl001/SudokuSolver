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
            new SudokuSquare(-1, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationY()
        {
            new SudokuSquare(0, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationY()
        {
            new SudokuSquare(0, 9, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationX()
        {
            new SudokuSquare(9, 5, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationValue()
        {
            new SudokuSquare(3, 5, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationNegativeValue()
        {
            new SudokuSquare(3, 5, -1);
        }

        [TestMethod]
        public void TestValidCreationValue()
        {
            var s = new SudokuSquare(3, 5, 0);
            Assert.IsFalse(s.IsValueSet);

            for (int val = 1; val <= 9; val++)
            {
                s = new SudokuSquare(3, 5, val);
                Assert.IsTrue(s.IsValueSet);
                Assert.AreEqual(val, s.Value);
                Assert.IsTrue(s.Candidates.Count == 0);
                Assert.AreEqual(3, s.X);
                Assert.AreEqual(5, s.Y);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidatesX()
        {
            new SudokuSquare(-1, 0, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidatesY()
        {
            new SudokuSquare(0, -1, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationCandidatesY()
        {
            new SudokuSquare(0, 9, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidRangeCreationCandidatesX()
        {
            new SudokuSquare(9, 5, new int[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidCreationCandidates()
        {
            new SudokuSquare(3, 5, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationTooMuchCandidates()
        {
            new SudokuSquare(3, 5, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidateValueTooSmall()
        {
            new SudokuSquare(3, 5, new int[] { 2, 4, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidCreationCandidateValueTooBig()
        {
            new SudokuSquare(3, 5, new int[] { 2, 4, 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCreationCandidateDuplicateValues()
        {
            new SudokuSquare(3, 5, new int[] { 2, 4, 5, 2 });
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
            s.ClearCandidates(new int[] { 1, 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestKeepCandidatesWithValue()
        {
            var s = new SudokuSquare(2, 2, 3);
            s.KeepCandidates(new int[] { 1, 2 });
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

        [TestMethod]
        public void TestClearCandidates()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            SudokuSquare newSquare = s1.ClearCandidates(new int[] { 5, 6 });
            SudokuSquare newSquare2 = s1.ClearCandidates(new int[] { 3, 9 });

            Assert.AreEqual(new SudokuSquare(2, 2, new int[] { 1, 8 }), newSquare);
            Assert.AreEqual(s1, newSquare2);

            //Clearing null candidates is same as clearing nothing.
            Assert.AreEqual(s1, s1.ClearCandidates(null));
        }

        [TestMethod]
        public void TestKeepCandidates()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            SudokuSquare newSquare = s1.KeepCandidates(new int[] { 5, 6 });
            SudokuSquare newSquare2 = s1.KeepCandidates(new int[] { 1, 7, 9 });
            SudokuSquare newSquare3 = s1.KeepCandidates(new int[] { 2, 7, 9 });

            Assert.AreEqual(new SudokuSquare(2, 2, new int[] { 5, 6 }), newSquare);
            Assert.AreEqual(new SudokuSquare(2, 2, new int[] { 1 }), newSquare2);
            Assert.AreEqual(new SudokuSquare(2, 2, new int[] { }), newSquare3);

            //Keeping null candidates is same as keeping everything.
            Assert.AreEqual(s1, s1.KeepCandidates(null));
        }

        [TestMethod]
        public void TestEmptySquare()
        {
            var s1 = new SudokuSquare(2, 1, 0);
            var s2 = new SudokuSquare(2, 1, new int[] { });

            Assert.AreEqual(s1, s2);
            Assert.IsTrue(s1.IsEmpty);

            var s3 = new SudokuSquare(3, 4, new int[] { 2, 5, 8, 9 });
            Assert.IsFalse(s3.IsEmpty);
            Assert.IsTrue(s3.KeepCandidates(1).IsEmpty);
        }

        [TestMethod]
        public void TestValidBoxIndex()
        {
            int[,] expectedBoxIndexes = new int[,] {
                { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
                { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
                { 0, 0, 0, 1, 1, 1, 2, 2, 2 },
                { 3, 3, 3, 4, 4, 4, 5, 5, 5 },
                { 3, 3, 3, 4, 4, 4, 5, 5, 5 },
                { 3, 3, 3, 4, 4, 4, 5, 5, 5 },
                { 6, 6, 6, 7, 7, 7, 8, 8, 8 },
                { 6, 6, 6, 7, 7, 7, 8, 8, 8 },
                { 6, 6, 6, 7, 7, 7, 8, 8, 8 } };

            for (int x=0; x<9; x++)
            {
                for (int y=0; y<9; y++)
                {
                    var s = new SudokuSquare(x, y, 0);
                    Assert.AreEqual(expectedBoxIndexes[x, y], s.Box);
                }
            }
        }
    }
}
