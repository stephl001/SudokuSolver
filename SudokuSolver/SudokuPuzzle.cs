﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuPuzzle
    {
        private const int MaxRange = 9;
        private const int SqrtMaxRange = 3;

        public const int MinValue = 1;
        public const int MaxValue = MaxRange;
        public static ReadOnlyCollection<int> PossibleValues = Array.AsReadOnly(Enumerable.Range(MinValue, MaxRange).ToArray());

        private readonly SudokuSquare[,] _squares = new SudokuSquare[MaxRange, MaxRange];
        
        public SudokuPuzzle(int[,] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.GetUpperBound(0) != (MaxRange - 1))
                throw new ArgumentOutOfRangeException(nameof(input), "Array must be 9x9");
            if (input.GetUpperBound(1) != (MaxRange - 1))
                throw new ArgumentOutOfRangeException(nameof(input), "Array must be 9x9");

            FillSquares(input);
            SetupCandidates();

            ReadOnlyCollection<SudokuValidationError> validationErrors = GetValidationErrors();
            ValidationErrors = validationErrors;
            IsValid = (validationErrors.Count == 0);
            IsCompleted = IsValid && ReadAllSquares().All(s => s.IsValueSet);
        }

        private void SetupCandidates()
        {
            foreach (SudokuSquare unsetSquare in ReadAllSquares().Where(s => !s.IsValueSet))
            {
                IEnumerable<int> candidates = PossibleValues.Except(this.ReadBuddiesValues(unsetSquare));
                _squares[unsetSquare.Row, unsetSquare.Column] = new SudokuSquare(unsetSquare.Row, unsetSquare.Column, candidates.ToArray());
            }
        }

        private ReadOnlyCollection<SudokuValidationError> GetValidationErrors()
        {
            List<SudokuValidationError> errors = new List<SudokuValidationError>();

            errors.AddRange(GetRowValidationErrors());
            errors.AddRange(GetColumnValidationErrors());
            errors.AddRange(GetBoxValidationErrors());

            return Array.AsReadOnly(errors.ToArray());
        }

        private IEnumerable<SudokuValidationError> GetRowValidationErrors()
        {
            return GetValidationErrors(ReadRow, squares => new SudokuRowValidationError(squares));
        }

        private IEnumerable<SudokuValidationError> GetColumnValidationErrors()
        {
            return GetValidationErrors(ReadColumn, squares => new SudokuColumnValidationError(squares));
        }

        private IEnumerable<SudokuValidationError> GetBoxValidationErrors()
        {
            return GetValidationErrors(ReadBox, squares => new SudokuBoxValidationError(squares));
        }

        private IEnumerable<SudokuValidationError> GetValidationErrors(Func<int, IEnumerable<SudokuSquare>> squaresFetcher, Func<IEnumerable<SudokuSquare>, SudokuValidationError> errorCreationHandler)
        {
            return Enumerable.Range(0, MaxRange).Aggregate(new List<SudokuValidationError>(), (list, i) =>
            {
                list.AddRange(squaresFetcher(i).Where(s => s.IsValueSet)
                                .GroupBy(s => s.Value)
                                .Where(g => g.Count() > 1)
                                .Select(errorCreationHandler));
                return list;
            });
        }

        private void FillSquares(int[,] input)
        {
            for (int row = 0; row < MaxRange; row++)
            {
                for (int column = 0; column < MaxRange; column++)
                {
                    _squares[row, column] = new SudokuSquare(row, column, input[row, column]);
                }
            }
        }

        public IEnumerable<SudokuValidationError> ValidationErrors { get; }

        public IEnumerable<SudokuSquare> ReadRow(int rowIndex)
        {
            if ((rowIndex < 0) || (rowIndex >= MaxRange))
                throw new ArgumentOutOfRangeException(nameof(rowIndex));

            for (int c=0; c<MaxRange; c++)
            {
                yield return _squares[rowIndex, c];
            }
        }

        public IEnumerable<SudokuSquare> ReadColumn(int columnIndex)
        {
            if ((columnIndex < 0) || (columnIndex >= MaxRange))
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            for (int r = 0; r < MaxRange; r++)
            {
                yield return _squares[r, columnIndex];
            }
        }

        public IEnumerable<SudokuSquare> ReadAllSquares()
        {
            for (int row=0; row<MaxRange; row++)
            {
                for (int column=0; column<MaxRange; column++)
                {
                    yield return _squares[row, column];
                }
            }
        }

        public IEnumerable<SudokuSquare> ReadBuddies(SudokuSquare square)
        {
            if (square == null)
                throw new ArgumentNullException(nameof(square));

            return ReadRow(square.Row)
                .Concat(ReadColumn(square.Column))
                .Concat(ReadBox(square.Box))
                .Distinct()
                .Except(new[] { square });
        }

        public IEnumerable<SudokuSquare> ReadBox(int boxIndex)
        {
            if ((boxIndex < 0) || (boxIndex >= MaxRange))
                throw new ArgumentOutOfRangeException(nameof(boxIndex));

            for (int b=0; b<MaxRange; b++)
            {
                int x = (b / SqrtMaxRange) + ((boxIndex / SqrtMaxRange) * SqrtMaxRange);
                int y = (b % SqrtMaxRange) + ((boxIndex % SqrtMaxRange) * SqrtMaxRange);

                yield return _squares[x, y];
            }
        }

        public SudokuPuzzle SetSquare(SudokuSquare square)
        {
            throw new NotImplementedException();
        }

        public SudokuSquare GetSquare(int row, int column)
        {
            if (row < 0 || row >= MaxRange)
                throw new ArgumentOutOfRangeException("row");
            if (column < 0 || column >= MaxRange)
                throw new ArgumentOutOfRangeException("column");

            return _squares[row, column];
        }

        public int Width { get { return MaxRange; } }

        public int Height { get { return MaxRange; } }

        public bool IsCompleted { get; }

        public bool IsValid { get; }
    }
}