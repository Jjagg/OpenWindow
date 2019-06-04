using System;

namespace OpenWindow
{
    /// <summary>
    /// Platform specific data for a Wayland display connection.
    /// </summary>
    public class WaylandWindowingServiceData : WindowingServiceData
    {
        /// <summary>
        /// The Wayland display proxy.
        /// </summary>
        public IntPtr WlDisplay { get; }

        /// <summary>
        /// The Wayland registry proxy.
        /// </summary>
        public IntPtr WlRegistry { get; }

        /// <summary>
        /// The globals that were present at the time of calling <see cref="WindowService.GetPlatformData"/>.
        /// </summary>
        public WaylandGlobal[] Globals { get; }

        /// <summary>
        /// The EGL display connection.
        /// Note that this will be <see>IntPtr.Zero</see> if EGL was not initialized.
        /// <summary>
        /// <seealso cref="WindowingService.InitializeOpenGl">
        public IntPtr EGLDisplay { get; }

        internal WaylandWindowingServiceData(IntPtr wlDisplay, IntPtr wlRegistry, WaylandGlobal[] globals, IntPtr eglDisplay)
            : base(WindowingBackend.Wayland)
        {
            WlDisplay = wlDisplay;
            WlRegistry = wlRegistry;
            Globals = globals;
            EGLDisplay = eglDisplay;
        }
    }
}
