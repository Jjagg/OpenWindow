using System;

namespace OpenWindow
{
    /// <summary>
    /// Platform specific data for a Win32 managed window.
    /// </summary>
    public class Win32WindowData : WindowData
    {
        /// <summary>
        /// HINSTANCE of the executing module.
        /// </summary>
        public IntPtr HInstance { get; }


        /// <summary>
        /// The handle of the window.
        /// </summary>
        public IntPtr Hwnd { get; }

        internal Win32WindowData(IntPtr hinstance, IntPtr hwnd)
            : base(WindowingBackend.Win32)
        {
            HInstance = hinstance;
            Hwnd = hwnd;
        }
    }
}
