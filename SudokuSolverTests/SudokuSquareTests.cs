using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using System.Linq;

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
    }
}
