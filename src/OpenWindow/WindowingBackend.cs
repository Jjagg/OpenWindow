namespace OpenWindow
{
    /// <summary>
    /// Supported Windowing service backends.
    /// </summary>
    public enum WindowingBackend
    {
        /// <summary>
        /// Windows. Uses the Win32 APIs.
        /// </summary>
        Win32,
        /// <summary>
        /// Linux. Uses the X window protocol.
        /// </summary>
        X,
        /// <summary>
        /// Linux. Uses the Wayland window protocol.
        /// </summary>
        Wayland,
        /// <summary>
        /// MacOS. Uses the Quartz API.
        /// </summary>
        Quartz
    }
}
