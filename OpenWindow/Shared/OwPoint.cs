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
    }
}
