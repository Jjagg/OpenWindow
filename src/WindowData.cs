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
