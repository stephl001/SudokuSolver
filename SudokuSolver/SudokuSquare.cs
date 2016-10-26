using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SudokuSolver
{
    public sealed class SudokuSquare : IEquatable<SudokuSquare>
    {
        private const int MaxRange = 9;
        private const int SqrtMaxRange = 3;
        private static readonly int[] EmptyCandidates = new int[] {};
        
        private readonly int _hash;

        public SudokuSquare(int row, int column, int value)
            : this(row, column, value, EmptyCandidates)
        {            
        }

        public SudokuSquare(int row, int column, int[] candidates)
            : this(row, column, 0, candidates)
        {
        }

        private SudokuSquare(int row, int column, int value, int[] candidates)
        {
            if (row < 0 || row >= MaxRange)
                throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} must be greater or equal than 0 and lower than {MaxRange}.");
            if (column < 0 || column >= MaxRange)
                throw new ArgumentOutOfRangeException(nameof(column), $"{nameof(column)} must be greater or equal than 0 and lower than {MaxRange}.");
            if ((value > MaxRange) || (value < 0))
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be greater or equal than 0 and lower than {MaxRange}.");
            if (candidates == null)
                throw new ArgumentNullException(nameof(candidates));
            if (candidates.Length > MaxRange)
                throw new ArgumentOutOfRangeException(nameof(candidates), $"The list of candidates cannot contain more than {MaxRange} elements.");
            if (candidates.Where(c => ((c <= 0) || (c > MaxRange))).Any())
                throw new ArgumentOutOfRangeException(nameof(candidates), $"A candidate must have a value greater than 0 and smaller or equal than {MaxRange}.");
            if (candidates.Distinct().Count() != candidates.Length)
                throw new ArgumentException("A list of candidates cannot contain duplicate values.", nameof(candidates));

            Row = row;
            Column = column;
            Box = ((column / SqrtMaxRange) + ((row / SqrtMaxRange)*SqrtMaxRange));
            Value = value;

            Array.Sort(candidates);
            Candidates = Array.AsReadOnly(candidates);

            _hash = CalculateHash();

            IsEmpty = ((value == 0) && !candidates.Any());
        }

        private int CalculateHash()
        {
            int hash = 0;
            hash |= (Row << 8);
            hash |= (Column << 4);
            hash |= Value;

            if (!IsValueSet && Candidates.Any())
            {
                int partialHash = Candidates.Aggregate(0, (bits, c) => bits |= (1 << (c-1)));
                hash |= (partialHash << 16);
            }

            return hash;
        }

        public int Row { get; }

        public int Column { get; }

        public int Box { get; }

        public int Value { get; }

        public bool IsValueSet { get { return Value > 0; } }

        public bool IsEmpty { get; }

        public ReadOnlyCollection<int> Candidates { get; }

        public SudokuSquare ClearCandidates(params int[] candidatesToExclude)
        {
            candidatesToExclude = candidatesToExclude ?? new int[] { };

            if (IsValueSet)
                throw new InvalidOperationException("You cannot clear candidates on a square that has its value set.");

            return new SudokuSquare(Row, Column, Candidates.Except(candidatesToExclude).ToArray());
        }

        public SudokuSquare KeepCandidates(params int[] candidatesToKeep)
        {
            candidatesToKeep = candidatesToKeep ?? Enumerable.Range(1, MaxRange).ToArray();

            if (IsValueSet)
                throw new InvalidOperationException("You cannot keep candidates on a square that has its value set.");

            return new SudokuSquare(Row, Column, Candidates.Intersect(candidatesToKeep).ToArray());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SudokuSquare);
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override string ToString()
        {
            return $"Position: ({Row}, {Column}), Value: {Value}, Candidates: {{{string.Join(", ", Candidates.Select(c => c.ToString()))}}}";
        }

        public bool Equals(SudokuSquare other)
        {
            if (other == null)
                return false;

            if ((Row != other.Row) || (Column != other.Column))
                return false;

            if (IsValueSet != other.IsValueSet)
                return false;

            if (IsValueSet)
                return (Value == other.Value);

            return Candidates.SequenceEqual(other.Candidates);
        }
    }
}
