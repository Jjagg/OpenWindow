// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Represents a connected display/monitor.
    /// </summary>
    public class Display
    {
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
        // TODO does this make sense for all platforms?
        public Rectangle WorkingArea { get; }

        // TODO does this make sense for all platforms?
        public bool IsPrimary { get; }

        internal Display(string name, Rectangle bounds, Rectangle workingArea, bool isPrimary)
        {
            Name = name;
            Bounds = bounds;
            WorkingArea = workingArea;
            IsPrimary = isPrimary;
        }
    }
}
