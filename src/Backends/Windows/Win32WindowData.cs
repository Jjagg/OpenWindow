using System;

namespace OpenWindow.Backends.Windows
{
    /// <summary>
    /// Platform specific data for a Win32 managed window.
    /// </summary>
    public class Win32WindowData : WindowData
    {
        /// <summary>
        /// The handle of the window. Equivalent with <see cref="Window.Handle"/>.
        /// </summary>
        public IntPtr Hwnd => Window.Handle;

        /// <summary>
        /// HINSTANCE of the executing module.
        /// </summary>
        public IntPtr HInstance { get; }

        internal Win32WindowData(IntPtr hinstance, WindowingService windowingService, Window window)
            : base(windowingService, window)
        {
            HInstance = hinstance;
        }
    }
}
