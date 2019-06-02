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
        /// </summary>
        public static IntPtr GetProcAddress(string func) => _impl.GetProcAddressImpl(func);

        /// <summary>
        /// </summary>
        public static bool SetVSync(VSyncState state) => _impl.SetVSyncImpl(state);

        /// <summary>
        /// </summary>
        public static VSyncState GetVSync() => _impl.GetVSyncImpl();

        /// <summary>
        /// Create an OpenGL context.
        /// </summary>
        public static IntPtr CreateContext(Window window) => _impl.CreateContextImpl(window.GetPlatformData());

        /// <summary>
        /// </summary>
        public static bool MakeCurrent(Window window, IntPtr ctx) => _impl.MakeCurrentImpl(window.GetPlatformData(), ctx);

        /// <summary>
        /// </summary>
        public static IntPtr GetCurrentContext() => _impl.GetCurrentContextImpl();

        /// <summary>
        /// </summary>
        public static bool DestroyContext(IntPtr ctx) => _impl.DestroyContextImpl(ctx);

        /// <summary>
        /// </summary>
        public static bool SwapBuffers(Window window) => _impl.SwapBuffersImpl(window.GetPlatformData());
    }

    internal abstract class WindowingGlInterface
    {
        internal virtual void Initialize() { }
        internal abstract IntPtr GetProcAddressImpl(string func);
        internal abstract bool SetVSyncImpl(VSyncState state);
        internal abstract VSyncState GetVSyncImpl();
        internal abstract IntPtr CreateContextImpl(WindowData wdata);
        internal abstract bool MakeCurrentImpl(WindowData wdata, IntPtr ctx);
        internal abstract IntPtr GetCurrentContextImpl();
        internal abstract bool DestroyContextImpl(IntPtr ctx);
        internal abstract bool SwapBuffersImpl(WindowData wdata);
    }
}
