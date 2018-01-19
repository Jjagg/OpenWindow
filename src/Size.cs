// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// An immutable value type representing a point with an integer x and y coordinate.
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// A Size of (0, 0).
        /// </summary>
        public static readonly Size Empty = new Size();

        /// <summary>
        /// Width of the size.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Height of the size.
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Create a <see cref="Size"/> with the given Width and Height.
        /// </summary>
        /// <param name="width">Width value.</param>
        /// <param name="height">Height value.</param>
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Component-wise add two sizes.
        /// </summary>
        /// <param name="p1">First size.</param>
        /// <param name="p2">Second size.</param>
        /// <returns><code>new Size(p1.Width + p2.Width, p1.Height + p2.Height)</code></returns>
        public static Size operator +(Size p1, Size p2)
        {
            return new Size(p1.Width + p2.Width, p1.Height + p2.Height);
        }

        /// <summary>
        /// Component-wise subtract one point from another.
        /// </summary>
        /// <param name="p1">Point to subtract from.</param>
        /// <param name="p2">Point to subtract.</param>
        /// <returns><code>new Size(p1.Width - p2.Width, p1.Height - p2.Height)</code></returns>
        public static Size operator -(Size p1, Size p2)
        {
            return new Size(p1.Width - p2.Width, p1.Height - p2.Height);
        }

        /// <summary>
        /// Multiply a points components by a scalar value.
        /// </summary>
        /// <param name="s">Factor to scale by.</param>
        /// <param name="p">Point to scale.</param>
        /// <returns><code>new Size(s * p.Width, s * p.Height)</code></returns>
        public static Size operator *(int s, Size p)
        {
            return new Size(s * p.Width, s * p.Height);
        }

        /// <summary>
        /// Multiply a points components by a scalar value.
        /// </summary>
        /// <param name="p">Point to scale.</param>
        /// <param name="s">Factor to scale by.</param>
        /// <returns><code>new Size(s * p.Width, s * p.Height)</code></returns>
        public static Size operator *(Size p,int s)
        {
            return new Size(s * p.Width, s * p.Height);
        }

        /// <summary>
        /// Implicitly convert a <see cref="Point"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="p">Point to convert.</param>
        /// <returns>new Size(p.X, p.Y)</returns>
        public static implicit operator Size(Point p)
        {
            return new Size(p.X, p.Y);
        }

        public void Deconstruct(out int x, out int y)
        {
            x = Width;
            y = Height;
        }

        public bool Equals(Size other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Size) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Size left, Size right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{Width} {Height}";
        }
    }
}