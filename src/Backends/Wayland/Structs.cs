using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static class Structs
    {
        public delegate void GlobalAdd(object data, IntPtr registry, uint id, string iface, uint version);
        public delegate void GlobalRemove(object data, IntPtr registry, uint id);
        
        public struct EventListener
        {
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public GlobalAdd GlobalAdd;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public GlobalRemove GlobalRemove;
        }

    }
}
