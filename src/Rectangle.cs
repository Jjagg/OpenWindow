// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace OpenWindow
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public Point Position => new Point(X, Y);
        public Point Size => new Point(Width, Height);

        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
                return false;

            var that = (Rectangle) obj;
            return Equals(that);
        }

        public bool Equals(Rectangle that)
        {
            return this == that;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.X == b.X &&
                   a.Y == b.Y &&
                   a.Width == b.Width &&
                   a.Height == b.Height;
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Width}, {Height}";
        }
    }
}
