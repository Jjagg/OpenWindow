// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace OpenWindow
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OwRectangle
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public OwPoint Position => new OwPoint(X, Y);
        public OwPoint Size => new OwPoint(Width, Height);

        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public OwRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OwRectangle))
                return false;

            var that = (OwRectangle) obj;
            return Equals(that);
        }

        public bool Equals(OwRectangle that)
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

        public static bool operator ==(OwRectangle a, OwRectangle b)
        {
            return a.X == b.X &&
                   a.Y == b.Y &&
                   a.Width == b.Width &&
                   a.Height == b.Height;
        }

        public static bool operator !=(OwRectangle a, OwRectangle b)
        {
            return !(a == b);
        }
    }
}
