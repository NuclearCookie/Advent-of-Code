using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Datastructures
{
    public struct Point2 : IEquatable<Point2>
    {
        public int X;
        public int Y;

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
}
