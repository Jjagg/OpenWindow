using System;
using System.Runtime.InteropServices;

namespace OpenWindow.GL
{
    /// <summary>
    /// Abstraction for interfaces between OpenGL and windowing systems.
    /// Abstracts differences between WGL and EGL to provide an API that's easy to use with OpenWindow.
    /// </summary>
    public static class OpenWindowGl
    {
        private static WindowingGlInterface _impl;

        /// <summary>
        /// A call to this method is required before calling any other method in this class.
        /// </summary>
        public static void Initialize(WindowingService service)
        {
            if (service.Backend == WindowingBackend.Win32)
            {
                _impl = new WglInterface(service);
            }
            else if (service.Backend == WindowingBackend.Wayland)
            {
                _impl = new EglInterface(service);
            }
            else
            {
                throw new NotSupportedException("Only WGL (for Windows) and EGL (for Wayland) are implemented for now.");
            }
        }

        /// <summary>
        /// Get a pointer to an OpenGL or WGL/EGL function.
        /// </summary>
        public static IntPtr GetProcAddress(string func) => _impl.GetProcAddressImpl(func);

        /// <summary>
        /// Set VSync state.
        /// </summary>
        public static bool SetVSync(VSyncState state) => _impl.SetVSyncImpl(state);

        /// <summary>
        /// Get the VSync state.
        /// </summary>
        public static VSyncState GetVSync() => _impl.GetVSyncImpl();

        /// <summary>
        /// Create an OpenGL context.
        /// </summary>
        public static IntPtr CreateContext(Window window) => _impl.CreateContextImpl(window.GetPlatformData());

        /// <summary>
        /// Create an OpenGL context with a minimum OpenGL version.
        /// </summary>
        public static IntPtr CreateContext(Window window, int major, int minor) => _impl.CreateContextImpl(window.GetPlatformData(), major, minor);

        /// <summary>
        /// Activate an OpenGL context to draw to a window surface on the calling thread.
        /// </summary>
        public static bool MakeCurrent(Window window, IntPtr ctx) => _impl.MakeCurrentImpl(window?.GetPlatformData(), ctx);

        /// <summary>
        /// Get the current OpenGL context or <c>IntPtr.Zero</c> if none is current.
        /// </summary>
        public static IntPtr GetCurrentContext() => _impl.GetCurrentContextImpl();

        /// <summary>
        /// Destroy an OpenGL context.
        /// </summary>
        public static bool DestroyContext(IntPtr ctx) => _impl.DestroyContextImpl(ctx);

        /// <summary>
        /// When double buffering is enabled, swap the front and back buffer.
        /// </summary>
        /// <seealso cref="OpenGlSurfaceSettings.DoubleBuffer"/>
        /// <seealso cref="SetVSync(VSyncState)"/>
        public static bool SwapBuffers(Window window) => _impl.SwapBuffersImpl(window.GetPlatformData());
    }
}
