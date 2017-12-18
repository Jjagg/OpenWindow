// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace WaylandSharpGen
{
    internal abstract class WlObject
    {
        public IntPtr Pointer { get; }

        protected WlObject(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }
}
