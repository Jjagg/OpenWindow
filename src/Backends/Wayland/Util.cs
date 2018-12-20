using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct ArgumentStruct
    {
        [FieldOffset(0)] public int I;
        [FieldOffset(0)] public uint U;
        [FieldOffset(0)] public IntPtr Ptr;

        public static implicit operator ArgumentStruct(int i) => new ArgumentStruct { I = i };
        public static implicit operator ArgumentStruct(uint u) => new ArgumentStruct { U = u };
        public static implicit operator ArgumentStruct(IntPtr ptr) => new ArgumentStruct { Ptr = ptr };
        public static implicit operator ArgumentStruct(WlInterface iface) => new ArgumentStruct { Ptr = iface.Pointer };
        public static implicit operator ArgumentStruct(WlObject obj) => new ArgumentStruct { Ptr = obj.Pointer };
    }

    internal sealed class WlArray : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct ArrayStruct
        {
            public uint Size;
            public uint Alloc;
            public IntPtr Data;
        }

        internal readonly IntPtr Pointer;

        public WlArray()
        {
            Pointer = Marshal.AllocHGlobal(MarshalHelpers.SizeOf<ArrayStruct>());
            ArrayInit(Pointer);
        }

        private void ReleaseUnmanagedResources()
        {
            ArrayRelease(Pointer);
            Marshal.FreeHGlobal(Pointer);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WlArray()
        {
            ReleaseUnmanagedResources();
        }

        [DllImport("libwayland-client.so")]
        private static extern void ArrayInit(IntPtr array);

        [DllImport("libwayland-client.so")]
        private static extern void ArrayRelease(IntPtr array);

        [DllImport("libwayland-client.so")]
        private static extern IntPtr ArrayAdd(IntPtr array, uint size);
    }
}