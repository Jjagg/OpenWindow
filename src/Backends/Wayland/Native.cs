using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    public static class Native
    {
        [DllImport("libegl.so")]
        public static extern IntPtr EglGetDisplay(int nativeDisplayType);
        [DllImport("egl")]
        public static extern bool EglInitialize(IntPtr display, out int major, out int minor);
        [DllImport("egl")]
        public static extern bool EglTerminate(IntPtr display);
        [DllImport("egl")]
        public static extern bool EglGetConfigs(IntPtr display, out IntPtr configs, int configSize, out int numConfig);

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
