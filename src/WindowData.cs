namespace OpenWindow
{
    /// <summary>
    /// This is a marker interface for classes containing platform-specific data on windows.
    /// Returned by <see cref="OpenWindow.Window.GetPlatformData"/>.
    /// </summary>
    public abstract class WindowData
    {
        /// <summary>
        /// The windowing backend that this instance contains information about.
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
