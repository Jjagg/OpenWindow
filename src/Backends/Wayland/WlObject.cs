using System;

namespace OpenWindow.Backends.Wayland
{
    internal class WlObject
    {
        public IntPtr Pointer { get; }

        public WlObject(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }
}