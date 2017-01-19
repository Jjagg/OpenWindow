// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace OpenWindow
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OwPoint
    {
        public readonly int X;
        public readonly int Y;

        public OwPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OwPoint))
                return false;

            var that = (OwPoint) obj;
            return Equals(that);
        }

        public bool Equals(OwPoint that)
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
                return hash;
            }
        }

        public static bool operator ==(OwPoint a, OwPoint b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(OwPoint a, OwPoint b)
        {
            return !(a == b);
        }
    }
}
