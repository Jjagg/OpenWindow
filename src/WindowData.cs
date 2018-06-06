// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Base class for classes containing platform-specific data on windows.
    /// Returned by <see cref="OpenWindow.Window.GetPlatformData"/>.
    /// </summary>
    public class WindowData
    {
        /// <summary>
        /// The <see cref="WindowingService"/> that manages the window or <code>null</code> for a
        /// <see cref="VirtualWindow"/>.
        /// </summary>
        public WindowingService WindowingService { get; }

        /// <summary>
        /// The window that this object contains data on.
        /// </summary>
        public Window Window { get; }

        /// <summary>
        /// Create a <see cref="WindowData"/> instance.
        /// </summary>
        /// <param name="windowingService">The <see cref="WindowingService"/> that manages the window.</param>
        /// <param name="window">The window that this object contains data on.</param>
        public WindowData(WindowingService windowingService, Window window)
        {
            WindowingService = windowingService;
            Window = window;
        }
    }
}