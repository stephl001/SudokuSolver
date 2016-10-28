using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver
{
    public enum SudokuValidationType
    {
        Row,
        Column,
        Box
    };

    public abstract class SudokuValidationError
    {
        protected SudokuValidationError(SudokuValidationType type, IEnumerable<SudokuSquare> faultySquares)
        {
            Type = type;
            FaultySquares = Array.AsReadOnly(faultySquares.ToArray());
            Message = BuildMessage(type, faultySquares);
        }

        private static string BuildMessage(SudokuValidationType type, IEnumerable<SudokuSquare> faultySquares)
        {
            string squares = string.Join(",", faultySquares);
            return $"The {type.ToString().ToLowerInvariant()} contains duplicate elements: {squares}";
        }
        public SudokuValidationType Type { get; }

        public ReadOnlyCollection<SudokuSquare> FaultySquares { get; }

        public string Message { get; }
    }

    public sealed class SudokuRowValidationError : SudokuValidationError
    {
        internal SudokuRowValidationError(IEnumerable<SudokuSquare> faultySquares) 
            : base(SudokuValidationType.Row, faultySquares)
        {
        }
    }

    public sealed class SudokuColumnValidationError : SudokuValidationError
    {
        internal SudokuColumnValidationError(IEnumerable<SudokuSquare> faultySquares)
            : base(SudokuValidationType.Column, faultySquares)
        {
        }
    }

    public sealed class SudokuBoxValidationError : SudokuValidationError
    {
        internal SudokuBoxValidationError(IEnumerable<SudokuSquare> faultySquares)
            : base(SudokuValidationType.Box, faultySquares)
        {
        }
    }
}
