namespace OpenWindow.GL
{
    /// <summary>
    /// Indicates if buffer swaps are synchronized with vertical blanking.
    /// </summary>
    public enum VSyncState
    {
        /// <summary>
        /// Buffer swaps are immediate. Might cause tearing.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Buffer swaps are synchronized with vertical blanking.
        /// </summary>
        On = 1,
        /// <summary>
        /// Enable VSync when frame rate is higher than display sync rate and disable when it is lower.
        /// Adaptive VSync is not widely supported.
        /// </summary>
        Adaptive = -1
    }
}
