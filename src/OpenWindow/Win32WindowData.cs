using System;

namespace OpenWindow
{
    /// <summary>
    /// Platform specific data for a Win32 managed window.
    /// </summary>
    public class Win32WindowData : WindowData
    {
        /// <summary>
        /// The handle of the window.
        /// </summary>
        public IntPtr Hwnd { get; }

        internal Win32WindowData(IntPtr hwnd)
            : base(WindowingBackend.Win32)
        {
            Hwnd = hwnd;
        }
    }
}
