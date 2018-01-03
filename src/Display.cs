// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

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
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// An identifier for the display.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Desktop bounds of the display.
        /// </summary>
        public Rectangle Bounds { get; }

        /// <summary>
        /// Working area for the display
        /// </summary>
        public Rectangle WorkingArea { get; }

        /// <summary>
        /// <code>true</code> if this display is the primary display, <code>false</code> otherwise.
        /// </summary>
        public bool IsPrimary { get; }

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
