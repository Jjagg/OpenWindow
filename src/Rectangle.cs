// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace OpenWindow
{
    /// <summary>
    /// Value type representing a rectangle with integer coordinates.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Rectangle with position and size of (0, 0).
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle();

        /// <summary>
        /// Rectangle with position (0, 0) and size (1, 1)
        /// </summary>
        public static readonly Rectangle Unit = new Rectangle(0, 0, 1, 1);

        /// <summary>
        /// X coordinate of the rectangle.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y coordinate of the rectangle.
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// Width of the rectangle.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Height of the rectangle.
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Top of the rectangle. Same as <see cref="Y"/>.
        /// </summary>
        public int Top => Y;

        /// <summary>
        /// Bottom of the rectangle. Equal to <code><see cref="Y"/> + <see cref="Height"/></code>.
        /// </summary>
        public int Bottom => Y + Height;

        /// <summary>
        /// Left of the rectangle. Same as <see cref="X"/>.
        /// </summary>
        public int Left => X;

        /// <summary>
        /// Right of the rectangle. Equal to <code><see cref="X"/> + <see cref="Width"/></code>.
        /// </summary>
        public int Right => X + Width;

        /// <summary>
        /// Location of the top left corner of the rectangle.
        /// </summary>
        public Point TopLeft => new Point(Left, Top);

        /// <summary>
        /// Location of the top right corner of the rectangle.
        /// </summary>
        public Point TopRight => new Point(Right, Top);

        /// <summary>
        /// Location of the bottom right corner of the rectangle.
        /// </summary>
        public Point BottomRight => new Point(Right, Bottom);

        /// <summary>
        /// Location of the bottom left corner of the rectangle.
        /// </summary>
        public Point BottomLeft => new Point(Left, Bottom);

        /// <summary>
        /// Location of the top left corner of the rectangle. Same as <see cref="TopLeft"/>.
        /// </summary>
        public Point Position => new Point(Left, Top);

        /// <summary>
        /// Center of the rectangle.
        /// </summary>
        public Point Center => new Point(X + Width / 2, Y + Height / 2);

        /// <summary>
        /// Size of the rectangle.
        /// </summary>
        public Size Size => new Size(Width, Height);

        /// <summary>
        /// Half of the size of the rectangle.
        /// </summary>
        public Size HalfExtents => new Size(Width / 2, Height / 2);

        /// <summary>
        /// Create a rectangle.
        /// </summary>
        /// <param name="x">X coordinate of the rectangle.</param>
        /// <param name="y">Y coordinate of the rectangle.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Create a new rectangle.
        /// </summary>
        /// <param name="pos">Coordinates of the top left point of the rectangle.</param>
        /// <param name="size">Size of the rectangle.</param>
        public Rectangle(Point pos, Size size)
            : this(pos.X, pos.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Get the corners of the rectangle. Order is top left, top right, bottom right, bottom left.
        /// </summary>
        public IEnumerable<Point> GetPoints()
        {
            yield return TopLeft;
            yield return TopRight;
            yield return BottomRight;
            yield return BottomLeft;
        }

        /// <summary>
        /// Create a rectangle with the same center, but expanded by <paramref name="v"/> at all sides.
        /// </summary>
        /// <param name="v">Amount to inflate the rectangle at the four sides.</param>
        /// <remarks>A negative value can be passed. This create a shrinked rectangle.</remarks>
        public Rectangle Inflate(int v)
        {
            return Inflate(v, v);
        }

        /// <summary>
        /// Create a rectangle with the same center, but expanded by <paramref name="h"/> at the horizontal sides
        /// and by <paramref name="v"/> at the vertical sides.
        /// </summary>
        /// <param name="h">Amount to inflate the rectangle at the left and right sides.</param>
        /// <param name="v">Amount to inflate the rectangle at the top and bottom sides.</param>
        /// <remarks>A negative value can be passed. This create a shrinked rectangle.</remarks>
        public Rectangle Inflate(int h, int v)
        {
            var halfH = h / 2;
            var halfV = v / 2;
            return new Rectangle(X - halfH, Y - halfV, Width + h, Height + v);
        }

        /// <summary>
        /// Create a rectangle with the same size as the current rectangle, but with a different position.
        /// </summary>
        /// <param name="pos">The new coordinates of the top left point of the rectangle.</param>
        public Rectangle WithPosition(Point pos)
        {
            return new Rectangle(pos.X, pos.Y, Width, Height);
        }

        /// <summary>
        /// Create a rectangle with the same size as the current rectangle, but with a different position.
        /// </summary>
        /// <param name="x">The new x coordinate of the top left point of the rectangle.</param>
        /// <param name="y">The new y coordinate of the top left point of the rectangle.</param>
        public Rectangle WithPosition(int x, int y)
        {
            return new Rectangle(x, y, Width, Height);
        }

        /// <summary>
        /// Create a rectangle with the same position as the current rectangle, but with a different size.
        /// </summary>
        /// <param name="size">The new size of the rectangle.</param>
        public Rectangle WithSize(Size size)
        {
            return new Rectangle(X, Y, size.Width, size.Height);
        }

        /// <summary>
        /// Create a rectangle with the same position as the current rectangle, but with a different size.
        /// </summary>
        /// <param name="width">The new width of the rectangle.</param>
        /// <param name="height">The new height of the rectangle.</param>
        public Rectangle WithSize(int width, int height)
        {
            return new Rectangle(X, Y, width, height);
        }

        /// <summary>
        /// Create a rectangle.
        /// </summary>
        /// <param name="tl">Top left of the rectangle.</param>
        /// <param name="br">Bottom right of the rectangle.</param>
        public static Rectangle FromExtremes(Point tl, Point br)
        {
            return new Rectangle(tl, br - tl);
        }

        /// <summary>
        /// Create a rectangle.
        /// </summary>
        /// <param name="center">Center of the rectangle.</param>
        /// <param name="halfExtents">Half of the size of the rectangle.</param>
        public static Rectangle FromHalfExtents(Point center, Size halfExtents)
        {
            return new Rectangle(center - halfExtents, halfExtents * 2);
        }

        public void Deconstruct(out int x, out int y, out int width, out int height)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
        }

        public void Deconstruct(out Point position, out Size size)
        {
            position = TopLeft;
            size = Size;
        }

        public bool Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rectangle && Equals((Rectangle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                return hashCode;
            }
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }
}
