using System;
using System.Linq.Expressions;

namespace OpenWindow.Backends.Wayland
{
    internal abstract class WlObject
    {
        public IntPtr Pointer { get; }
        public bool IsNullPtr => Pointer == IntPtr.Zero;

        public WlObject(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }
}