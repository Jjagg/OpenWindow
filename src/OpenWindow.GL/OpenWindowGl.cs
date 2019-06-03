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

        static OpenWindowGl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _impl = new WglInterface();
            }
            else
            {
                _impl = new EglInterface();
            }

            _impl.Initialize();
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
        /// Activate an OpenGL context to draw to a window surface on the calling thread.
        /// </summary>
        public static bool MakeCurrent(Window window, IntPtr ctx) => _impl.MakeCurrentImpl(window.GetPlatformData(), ctx);

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
