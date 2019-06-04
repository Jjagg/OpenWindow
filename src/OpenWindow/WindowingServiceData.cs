namespace OpenWindow
{
    /// <summary>
    /// Contains platform-specific data tied to the windowing system.
    /// </summary>
    public abstract class WindowingServiceData
    {
        /// <summary>
        /// The windowing backend of the <see cref="WindowingService"/>.
        /// This property can be checked before casting to the platform specific implementation.
        /// </summary>
        public WindowingBackend Backend { get; }

        /// <summary>
        /// Create a new <see cref="WindowingServiceData"/> instance.
        /// </summary>
        protected WindowingServiceData(WindowingBackend backend)
        {
            Backend = backend;
        }
    }
}
