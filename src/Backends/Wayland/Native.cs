using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static class Egl
    {
        [DllImport("libEGL.so", EntryPoint = "eglGetDisplay")]
        public static extern IntPtr GetDisplay(int nativeDisplayType);
        [DllImport("libEGL.so", EntryPoint = "eglInitialize")]
        public static extern bool Initialize(IntPtr display, out int major, out int minor);
        [DllImport("libEGL.so", EntryPoint = "eglTerminate")]
        public static extern bool Terminate(IntPtr display);
        [DllImport("libEGL.so", EntryPoint = "eglGetConfigs")]
        public static extern bool GetConfigs(IntPtr display, IntPtr configs, int configSize, out int numConfig);
        [DllImport("libEGL.so", EntryPoint = "eglGetConfigs")]
        public static extern bool GetConfigs(IntPtr display, ref IntPtr configs, int configSize, out int numConfig);
    }

    internal static class WlEgl
    {

        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_create")]
        public static extern IntPtr WindowCreate(IntPtr surface, int width, int height);

        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_destroy")]
        public static extern void WindowDestroy(IntPtr egl_window);
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_resize")]
        public static extern void WindowResize(IntPtr egl_window, int width, int height, int dx, int dy);
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_get_attached_size")]
        public static extern void WindowGetAttachedSize(IntPtr egl_window, out int width, out int height);
    }
}
