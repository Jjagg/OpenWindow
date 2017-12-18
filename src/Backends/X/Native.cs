// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

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