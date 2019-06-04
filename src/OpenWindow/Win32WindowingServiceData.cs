using System;

namespace OpenWindow
{
    /// <summary>
    /// Platform specific data for a Windows Win32 <see cref="WindowingService"/>.
    /// </summary>
    public class Win32WindowingServiceData : WindowingServiceData
    {
        /// <summary>
        /// HINSTANCE of the executing module.
        /// </summary>
        public IntPtr HInstance { get; }

        internal Win32WindowingServiceData(IntPtr hinstance)
            : base(WindowingBackend.Win32)
        {
            HInstance = hinstance;
        }
    }
}
