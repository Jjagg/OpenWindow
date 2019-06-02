using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.X
{
    internal static class Native
    {
        [DllImport("libxcb", EntryPoint = "xcb_connect")]
        public static extern IntPtr Connect(string displayname, IntPtr screenp);
        
        [DllImport("libxcb", EntryPoint = "xcb_connection_has_error")]
        public static extern bool ConnectionHasError(IntPtr connection);
        
        [DllImport("libxcb", EntryPoint = "xcb_disconnect")]
        public static extern void Disconnect(IntPtr c);
    }
}
