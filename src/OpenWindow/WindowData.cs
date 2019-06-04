namespace OpenWindow
{
    /// <summary>
    /// Contains platform-specific data for native windows.
    /// </summary>
    /// <seealso cref="OpenWindow.Window.GetPlatformData"/>.
    public abstract class WindowData
    {
        /// <summary>
        /// The windowing backend of the <see cref="Window"/>.
        /// This property can be checked before casting to the platform specific implementation.
        /// </summary>
        public WindowingBackend Backend { get; }

        /// <summary>
        /// Create a new <see cref="WindowData"/> instance.
        /// </summary>
        protected WindowData(WindowingBackend backend)
        {
            Backend = backend;
        }
    }
}
