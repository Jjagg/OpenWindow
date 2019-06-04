using System;

namespace OpenWindow
{
    /// <summary>
    /// Platform specific data for a Wayland window.
    /// </summary>
    public class WaylandWindowData : WindowData
    {
        /// <summary>
        /// The Wayland surface proxy for the surface created for the window.
        /// OpenWindow does not map a buffer to this surface.
        /// </summary>
        public IntPtr WlSurface { get; }

        /// <summary>
        /// The EGL window.
        /// Note that this will be <see>IntPtr.Zero</see> if the window was not created with OpenGL support.
        /// <summary>
        /// <seealso cref="OpenGlSurfaceSettings.EnableOpenGl">
        public IntPtr WaylandEglWindow { get; }

        /// <summary>
        /// The EGL surface for the window.
        /// Note that this will be <see>IntPtr.Zero</see> if the window was not created with OpenGL support.
        /// <summary>
        /// <seealso cref="OpenGlSurfaceSettings.EnableOpenGl">
        public IntPtr EGLSurface { get; }

        /// <summary>
        /// The EGL frame buffer configuration.
        /// Note that this will be <see>IntPtr.Zero</see> if the window was not created with OpenGL support.
        /// <summary>
        /// <seealso cref="OpenGlSurfaceSettings.EnableOpenGl">
        public IntPtr EGLConfig { get; }

        internal WaylandWindowData(IntPtr wlSurface, IntPtr eglWindow, IntPtr eglSurface, IntPtr eglConfig)
            : base(WindowingBackend.Wayland)
        {
            WlSurface = wlSurface;

            WaylandEglWindow = eglWindow;
            EGLSurface = eglSurface;
            EGLConfig = eglConfig;
        }
    }
}
