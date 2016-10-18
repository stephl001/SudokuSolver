using System;
using System.Linq;

namespace SudokuSolver
{
    public sealed class SudokuSquare : IEquatable<SudokuSquare>
    {
        private const int MaxRange = 9;
        private static readonly int[] EmptyCandidates = new int[] {};

        public SudokuSquare(int x, int y, int value)
            : this(x, y, value, EmptyCandidates)
        {            
        }

        public SudokuSquare(int x, int y, int[] candidates)
            : this(x, y, 0, candidates)
        {
        }

        private SudokuSquare(int x, int y, int value, int[] candidates)
        {
            if (x < 0 || x >= MaxRange)
                throw new ArgumentOutOfRangeException($"{nameof(x)} must be greater or equal than 0 and lower than {MaxRange}.");
            if (y < 0 || y >= MaxRange)
                throw new ArgumentOutOfRangeException($"{nameof(y)} must be greater or equal than 0 and lower than {MaxRange}.");
            if (value >= MaxRange)
                throw new ArgumentOutOfRangeException($"{nameof(value)} must be greater or equal than 0 and lower than {MaxRange}.");
            if (candidates == null)
                throw new ArgumentNullException(nameof(candidates));
            if (candidates.Length > MaxRange)
                throw new ArgumentOutOfRangeException($"The list of candidates cannot contain more than {MaxRange} elements.");
            if (candidates.Where(c => ((c <= 0) || (c > MaxRange))).Any())
                throw new ArgumentOutOfRangeException($"A candidate must have a value greater than 0 and smaller or equal than {MaxRange}.");
            if (candidates.Distinct().Count() != candidates.Length)
                throw new ArgumentException("A list of candidates cannot duplicate values.");

            X = x;
            Y = y;
            Value = value;
            Candidates = candidates;
        }

        public int X { get; }

        public int Y { get; }

        public int Value { get; }

        public bool IsValueSet { get { return Value > 0; } }

        public int[] Candidates { get; }

        public SudokuSquare ClearCandidates(int[] candidates)
        {
            throw new NotImplementedException();
        }

        public SudokuSquare KeepCandidates(int[] candidates)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Position: ({X}, {Y}), Value: {Value}, Candidates: {{{string.Join(", ", Candidates.Select(c => c.ToString()))}}}";
        }

        public bool Equals(SudokuSquare other)
        {
            throw new NotImplementedException();
        }
    }
}
