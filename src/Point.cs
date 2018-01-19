// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace OpenWindow
{
    /// <summary>
    /// An immutable value type representing a point with an integer x and y coordinate.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        /// The Point2 at (0, 0).
        /// </summary>
        public static readonly Point Zero;

        /// <summary>
        /// X value of the point.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y value of the point.
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// Create a <see cref="Point"/> with the given X and Y values.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Component-wise add two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns><code>new Point2(p1.X + p2.X, p1.Y + p2.Y)</code></returns>
        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        /// <summary>
        /// Component-wise subtract one point from another.
        /// </summary>
        /// <param name="p1">Point to subtract from.</param>
        /// <param name="p2">Point to subtract.</param>
        /// <returns><code>new Point2(p1.X - p2.X, p1.Y - p2.Y)</code></returns>
        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// Multiply a points components by a scalar value.
        /// </summary>
        /// <param name="s">Factor to scale by.</param>
        /// <param name="p">Point to scale.</param>
        /// <returns><code>new Point2(s * p.X, s * p.Y)</code></returns>
        public static Point operator *(int s, Point p)
        {
            return new Point(s * p.X, s * p.Y);
        }

        /// <summary>
        /// Multiply a points components by a scalar value.
        /// </summary>
        /// <param name="p">Point to scale.</param>
        /// <param name="s">Factor to scale by.</param>
        /// <returns><code>new Point2(s * p.X, s * p.Y)</code></returns>
        public static Point operator *(Point p,int s)
        {
            return new Point(s * p.X, s * p.Y);
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public bool Equals(Point other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}
