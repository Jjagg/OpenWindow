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