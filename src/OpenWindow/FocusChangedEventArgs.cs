namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.FocusChange"/> event.
    /// </summary>
    public struct FocusChangedEventArgs
    {
        /// <summary>
        /// <c>true</c> if the <see cref="Window"/> got keyboard focus,
        /// <c>false</c> if it lost keyboard focus.
        /// </summary>
        public readonly bool HasFocus;

        internal FocusChangedEventArgs(bool hasFocus)
        {
            HasFocus = hasFocus;
        }
    }
}
