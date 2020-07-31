using System;

namespace OpenWindow
{
    /// <summary>
    /// Represents a connected display/monitor.
    /// </summary>
    public class Display
    {
        /// <summary>
        /// Handle to the native display object.
        /// The meaning of this value varies by windowing backend.
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// An identifier for the display.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Desktop bounds of the display.
        /// </summary>
        public Rectangle Bounds { get; internal set; }

        /// <summary>
        /// Working area for the display
        /// </summary>
        public Rectangle WorkingArea { get; internal set; }

        /// <summary>
        /// <c>true</c> if this display is the primary display, <c>false</c> otherwise.
        /// </summary>
        public bool IsPrimary { get; internal set; }

        internal Display(IntPtr handle)
        {
            Handle = handle;
        }

        internal Display(IntPtr handle, string name, Rectangle bounds, Rectangle workingArea, bool isPrimary)
        {
            Handle = handle;
            Name = name;
            Bounds = bounds;
            WorkingArea = workingArea;
            IsPrimary = isPrimary;
        }
    }
}
