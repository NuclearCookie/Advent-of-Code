using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Datastructures
{
    public struct Point2 : IEquatable<Point2>
    {
        public int X;
        public int Y;

        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Operators

        public static Point2 operator +(Point2 a, Point2 b)
        {
            return new Point2 { X = a.X + b.X, Y = a.Y + b.Y };
        }

        public static Point2 operator -(Point2 a, Point2 b)
        {
            return new Point2 { X = a.X - b.X, Y = a.Y - b.Y };
        }

        public static Point2 operator *(int a, Point2 b)
        {
            return new Point2 { X = a * b.X, Y = a * b.Y };
        }

        public static bool operator ==(Point2 a, Point2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point2 a, Point2 b)
        {
            return !(a == b);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Point2 point && Equals(point);
        }

        #endregion

        #region Interface

        public bool Equals(Point2 other)
        {
            return this == other;
        }

        #endregion

        public int GetManhattanCoordinates()
        {
            return Math.Abs(X) + Math.Abs(Y);
        }

        public int GetManhattanDistance(Point2 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
    }

    public struct Bounds : IEquatable<Bounds>
    {
        public int Min;
        public int Max;

        #region Operators

        public static Bounds operator +(Bounds a, Bounds b)
        {
            return new Bounds { Min = a.Min + b.Min, Max = a.Max + b.Max };
        }

        public static Bounds operator *(int a, Bounds b)
        {
            return new Bounds { Min = a * b.Min, Max = a * b.Max };
        }

        public static bool operator ==(Bounds a, Bounds b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }

        public static bool operator !=(Bounds a, Bounds b)
        {
            return !(a == b);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"Min: {Min}, Max: {Max}";
        }

        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Bounds bounds && Equals(bounds);
        }

        #endregion

        #region Interface

        public bool Equals(Bounds other)
        {
            return this == other;
        }

        #endregion

        public bool Contains(int value)
        {
            return value >= Min && value <= Max;
        }
    }

    // :TODO: implement IEnumerable etc?
    public class Array2D<T>
    {
        private T[] _internalArray;
        private int _rows;
        private int _cols;

        public T this[int i]
        {
            get => _internalArray[i];
            set => _internalArray[i] = value;
        }
        public T this[Point2 coord]
        {
            get => this[coord.Y, coord.X];
            set => this[coord.Y, coord.X] = value;
        }
        public T this[int row, int col]
        {
            get => GetElementAt(row, col);
            set => _internalArray[RowColumnToIndex(row, col)] = value;
        }

        public int Length => _rows * _cols;
        public int RowLength => _rows;
        public int ColumnLength => _cols;

        public Array2D(int rows, int cols)
        {
            _internalArray = new T[rows * cols];
            _rows = rows;
            _cols = cols;
        }

        public Array2D(int rows, int cols, T[] initialData) : this(rows, cols)
        {
            Debug.Assert(initialData.Length == rows * cols);
            Array.Copy(initialData, _internalArray, initialData.Length);
        }

        public Array2D(T[][] initialData)
        {
            _rows = initialData.Length;
            _cols = initialData[0].Length;
            _internalArray = new T[_rows * _cols];
            for (int i = 0; i < _rows; ++i)
            {
                SetRow(i, initialData[i]);
            }
        }
        public void SetCol(int col, T[] colData)
        {
            Debug.Assert(colData.Length == _rows);
            for(int row = 0; row < colData.Length; ++row)
            {
                this[row, col] = colData[row];
            }
        }


        public void SetRow(int row, T[] rowData)
        {
            Debug.Assert(rowData.Length == _cols);
            Array.Copy(rowData, 0, _internalArray, row* _cols, _cols);
        }

        public void GetRow(int row, T[] outputBuffer)
        {
            Debug.Assert(row >= 0 && row < _rows && outputBuffer.Length == _cols);
            Array.Copy(_internalArray, row * _cols, outputBuffer, 0, _cols);
        }

        public T[] GetRow(int row)
        {
            T[] outputBuffer = new T[_cols];
            GetRow(row, outputBuffer);
            return outputBuffer;
        }

        public void GetColumn(int column, T[] outputBuffer)
        {
            Debug.Assert(column >= 0 && column < _cols && outputBuffer.Length == _rows);
            for(int i = 0; i < outputBuffer.Length; ++i)
            {
                outputBuffer[i] = _internalArray[RowColumnToIndex(i, column)];
            }
        }

        public T[] GetColumn(int column)
        {
            T[] outputBuffer = new T[_rows];
            GetColumn(column, outputBuffer);
            return outputBuffer;
        }

        public T GetElementAt(int row, int col)
        {
            return _internalArray[RowColumnToIndex(row, col)];
        }

        public Point2 IndexToRowColumn(int index)
        {
            return new Point2 {  Y = index / _cols, X = index % _cols };
        }

        public void IndexToRowColumn(int index, out int row, out int col)
        {
            row = index / _cols;
            col = index % _cols;
        }

        public int RowColumnToIndex(Point2 coord)
        {
            Debug.Assert(coord.Y < _rows && coord.X < _cols);
            return coord.X + (coord.Y * _cols);
        }

        public int RowColumnToIndex(int row, int col)
        {
            Debug.Assert(row < _rows && col < _cols);
            return col + (row * _cols);
        }

        public int GetNeighbourIndices(Point2 index, int[] outputBuffer)
        {
            return GetNeighbourIndices(index.Y, index.X, outputBuffer);
        }

        public int GetNeighbourIndices(int index, int[] outputBuffer)
        {
            IndexToRowColumn(index, out var row, out var col);
            return GetNeighbourIndices(row, col, outputBuffer);
        }

        public int GetNeighbourIndices(int row, int col, int[] outputBuffer)
        {
            var index = 0;
            // only direct neighbours
            if (outputBuffer.Length >= 4)
            {
                if (row > 0)
                {
                    outputBuffer[index++] = RowColumnToIndex(row - 1, col);
                }
                if (row < _rows - 1)
                {
                    outputBuffer[index++] = RowColumnToIndex(row + 1, col);
                }
                if (col > 0)
                {
                    outputBuffer[index++] = RowColumnToIndex(row, col - 1);
                }
                if (col < _cols - 1)
                {
                    outputBuffer[index++] = RowColumnToIndex(row, col + 1);
                }
            }
            if (outputBuffer.Length >= 8) // also diagonal
            {
                if (row > 0 && col > 0)
                {
                    outputBuffer[index++] = RowColumnToIndex(row - 1, col - 1);
                }
                if (row < _rows - 1 && col < _cols - 1)
                {
                    outputBuffer[index++] = RowColumnToIndex(row + 1, col + 1);
                }
                if (row < _rows - 1 && col > 0)
                {
                    outputBuffer[index++] = RowColumnToIndex(row + 1, col - 1);
                }
                if (row > 0 && col < _cols - 1)
                {
                    outputBuffer[index++] = RowColumnToIndex(row -1, col + 1);
                }
            }
            return index;
        }

        public int GetNeighbours(Point2 coords, T[] outputBuffer)
        {
            return GetNeighbours(coords.Y, coords.X, outputBuffer); 
        }

        public int GetNeighbours(int index, T[] outputBuffer)
        {
            IndexToRowColumn(index, out int row, out int col);
            return GetNeighbours(row, col, outputBuffer);
        }

        public int GetNeighbours(int row, int col, T[] outputBuffer)
        {
            var index = 0;
            // only direct neighbours
            if (outputBuffer.Length >= 4)
            {
                if (row > 0)
                {
                    outputBuffer[index++] = GetElementAt(row - 1, col);
                }
                if (row < _rows - 1)
                {
                    outputBuffer[index++] = GetElementAt(row + 1, col);
                }
                if (col > 0)
                {
                    outputBuffer[index++] = GetElementAt(row, col - 1);
                }
                if (col < _cols - 1)
                {
                    outputBuffer[index++] = GetElementAt(row, col + 1);
                }
            }
            if (outputBuffer.Length >= 8) // also diagonal
            {
                if (row > 0 && col > 0)
                {
                    outputBuffer[index++] = GetElementAt(row - 1, col - 1);
                }
                if (row < _rows - 1 && col < _cols - 1)
                {
                    outputBuffer[index++] = GetElementAt(row + 1, col + 1);
                }
                if (row < _rows - 1 && col > 0)
                {
                    outputBuffer[index++] = GetElementAt(row + 1, col - 1);
                }
                if (row < _rows - 1 && col < _cols - 1)
                {
                    outputBuffer[index++] = GetElementAt(row -1, col + 1);
                }
            }
            return index;
        }

        public void Transpose()
        {
            var transposed = new Array2D<T>(ColumnLength, RowLength);
            for(int row = 0; row < RowLength; row++)
            {
                transposed.SetCol(row, GetRow(row));
            }
            _internalArray = transposed._internalArray;
            _cols = transposed._cols;
            _rows = transposed._rows;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int row = 0; row < _rows; row++)
            {
                for(int col = 0; col < _cols; col++)
                {
                    stringBuilder.Append(GetElementAt(row,col));
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string ToString(Func<T, string> formatter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int row = 0; row < _rows; row++)
            {
                for(int col = 0; col < _cols; col++)
                {
                    stringBuilder.Append(formatter(GetElementAt(row,col)));
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
    }
}
