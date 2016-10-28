using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuSquareTests
    {
        [TestMethod]
        public void TestInvalidCreation()
        {
            Action act = () => new SudokuSquare(-1, 0, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("row");
                
            act = () => new SudokuSquare(0, -1, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("column");

            act = () => new SudokuSquare(9, 0, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("row");

            act = () => new SudokuSquare(0, 9, 0);
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("column");

            act = () => new SudokuSquare(3, 5, -1);
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("value");

            act = () => new SudokuSquare(3, 5, 10);
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void TestValidCreationValue()
        {
            var s = new SudokuSquare(3, 5, 0);
            s.IsValueSet.Should().BeFalse();

            for (int val = 1; val <= 9; val++)
            {
                s = new SudokuSquare(3, 5, val);
                s.IsValueSet.Should().BeTrue();
                val.Should().Be(s.Value);
                s.Candidates.Should().BeEmpty();
                s.Row.Should().Be(3);
                s.Column.Should().Be(5);
            }
        }

        [TestMethod]
        public void TestInvalidCreationCandidates()
        {
            Action act = () => new SudokuSquare(3, 5, null);
            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.Should().Be("candidates");

            act = () => new SudokuSquare(-1, 0, new int[] { 1 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("row");

            act = () => new SudokuSquare(0, -1, new int[] { 1 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("column");

            act = () => new SudokuSquare(0, 9, new int[] { 1 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("column");

            act = () => new SudokuSquare(9, 0, new int[] { 1 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("row");

            act = () => new SudokuSquare(3, 5, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("candidates");

            act = () => new SudokuSquare(3, 5, new int[] { 2, 4, 0 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("candidates");

            act = () => new SudokuSquare(3, 5, new int[] { 2, 4, 10 });
            act.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("candidates");

            act = () => new SudokuSquare(3, 5, new int[] { 2, 4, 5, 2 });
            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("candidates");
        }

        [TestMethod]
        public void TestValidCreationCandidates()
        {
            var candidates = new int[] { 6, 8, 9 };
            var s = new SudokuSquare(3, 5, candidates);
            s.IsValueSet.Should().BeFalse();
            s.Value.Should().Be(0);
            s.Candidates.Should().BeEquivalentTo(candidates);
            s.Row.Should().Be(3);
            s.Column.Should().Be(5);
        }

        [TestMethod]
        public void TestToString()
        {
            var s = new SudokuSquare(3, 5, 4);
            s.ToString().Should().Be("Position: (3, 5), Value: 4, Candidates: {}");

            s = new SudokuSquare(3, 5, new int[] { 4, 8, 9 });
            s.ToString().Should().Be("Position: (3, 5), Value: 0, Candidates: {4, 8, 9}");
        }

        [TestMethod]
        public void TestClearCandidatesWithValue()
        {
            var s = new SudokuSquare(2, 2, 3);

            Action act = () => s.ClearCandidates(new int[] { 1, 2 });
            act.ShouldThrow<InvalidOperationException>()
                .WithMessage("You cannot clear candidates on a square that has its value set.");
        }

        [TestMethod]
        public void TestKeepCandidatesWithValue()
        {
            var s = new SudokuSquare(2, 2, 3);

            Action act = () => s.KeepCandidates(new int[] { 1, 2 });
            act.ShouldThrow<InvalidOperationException>()
                .WithMessage("You cannot keep candidates on a square that has its value set.");
        }

        [TestMethod]
        public void TestEqualityWithValue()
        {
            var s1 = new SudokuSquare(2, 2, 3);
            var s2 = new SudokuSquare(2, 1, 3);
            var s3 = new SudokuSquare(1, 2, 3);
            var s4 = new SudokuSquare(2, 2, 4);
            var s5 = new SudokuSquare(2, 2, 3);
            s1.Equals(null).Should().BeFalse();
            s1.Should().Be(s5);
            s1.Should().NotBe(s2);
            s1.Should().NotBe(s3);
            s1.Should().NotBe(s4);
        }

        [TestMethod]
        public void TestHashValue()
        {
            IEnumerable<SudokuSquare> allSquares = GenerateAllPossibleSquaresWithValue();
            IEnumerable<int> allHashes = allSquares.Select(s => s.GetHashCode()).Distinct();

            allHashes.Should().HaveSameCount(allSquares);

            var s1 = new SudokuSquare(2, 2, 3);
            var s2 = new SudokuSquare(2, 2, 3);
            s1.GetHashCode().Should().Be(s2.GetHashCode());
        }

        [TestMethod]
        public void TestHashCandidates()
        {
            IEnumerable<SudokuSquare> allSquares = GenerateAllPossibleCandidatesForAllPossibleSquare();
            IEnumerable<int> allHashes = allSquares.Select(s => s.GetHashCode()).Distinct();

            allHashes.Should().HaveSameCount(allSquares);
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
            s1.GetHashCode().Should().Be(s2.GetHashCode());
        }

        [TestMethod]
        public void TestEqualityWithCandidates()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            var s2 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            var s3 = new SudokuSquare(2, 2, new int[] { 1, 6, 5, 8 });
            var s4 = new SudokuSquare(2, 2, 4);
            s1.Should().NotBe(s4);
            s1.Should().Be(s2);
            s1.Should().Be(s3);
        }

        [TestMethod]
        public void TestClearCandidates()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            SudokuSquare newSquare = s1.ClearCandidates(new int[] { 5, 6 });
            SudokuSquare newSquare2 = s1.ClearCandidates(new int[] { 3, 9 });

            newSquare.Should().Be(new SudokuSquare(2, 2, new int[] { 1, 8 }));
            newSquare2.Should().Be(s1);

            //Clearing null candidates is same as clearing nothing.
            s1.ClearCandidates(null).Should().Be(s1);
        }

        [TestMethod]
        public void TestKeepCandidates()
        {
            var s1 = new SudokuSquare(2, 2, new int[] { 1, 5, 6, 8 });
            SudokuSquare newSquare = s1.KeepCandidates(new int[] { 5, 6 });
            SudokuSquare newSquare2 = s1.KeepCandidates(new int[] { 1, 7, 9 });
            SudokuSquare newSquare3 = s1.KeepCandidates(new int[] { 2, 7, 9 });

            newSquare.Should().Be(new SudokuSquare(2, 2, new int[] { 5, 6 }));
            newSquare2.Should().Be(new SudokuSquare(2, 2, new int[] { 1 }));
            newSquare3.Should().Be(new SudokuSquare(2, 2, new int[] { }));

            //Keeping null candidates is same as keeping everything.
            s1.KeepCandidates(null).Should().Be(s1);
        }

        [TestMethod]
        public void TestEmptySquare()
        {
            var s1 = new SudokuSquare(2, 1, 0);
            var s2 = new SudokuSquare(2, 1, new int[] { });

            s1.Should().Be(s2);
            s1.IsEmpty.Should().BeTrue();

            var s3 = new SudokuSquare(3, 4, new int[] { 2, 5, 8, 9 });
            s3.IsEmpty.Should().BeFalse();
            s3.KeepCandidates(1).IsEmpty.Should().BeTrue();
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
                    s.Box.Should().Be(expectedBoxIndexes[x, y]);
                }
            }
        }
    }
}
