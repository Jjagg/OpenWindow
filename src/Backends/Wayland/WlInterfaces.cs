using System;
using System.Collections.Generic;

namespace OpenWindow.Backends.Wayland
{
    internal static partial class WlInterfaces
    {
        private static readonly Dictionary<string, WlInterface> Interfaces
            = new Dictionary<string, WlInterface>();

        private static void Add(string name, WlInterface iface)
        {
            Interfaces.Add(name, iface);
        }

        public static IntPtr Get(string name)
        {
            return Interfaces[name].Pointer;
        }
    }
}