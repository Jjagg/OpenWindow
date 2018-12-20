// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static class Native
    {
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_create")]
        public static extern IntPtr EglWindowCreate(IntPtr surface, int width, int height);

        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_destroy")]
        public static extern void EglWindowDestroy(IntPtr egl_window);
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_resize")]
        public static extern void EglWindowResize(IntPtr egl_window, int width, int height, int dx, int dy); 
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_get_attached_size")]
        public static extern void EglWindowGetAttachedSize(IntPtr egl_window, out int width, out int height); 
    }
}