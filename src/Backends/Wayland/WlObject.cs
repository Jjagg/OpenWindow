using System;

namespace OpenWindow.Backends.Wayland
{
    internal class WlObject
    {
        public IntPtr Pointer { get; }
        public bool IsNullPtr => Pointer == IntPtr.Zero;

        public WlObject(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }
}