namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.KeyUp"/> and <see cref="Window.KeyPress"/> events.
    /// </summary>
    public struct KeyUpEventArgs
    {
        /// <summary>
        /// The <see cref="Key"/> that was pressed or released.
        /// </summary>
        public readonly Key Key;

        /// <summary>
        /// The scancode of the physicial key that was pressed or released.
        /// </summary>
        public readonly ScanCode ScanCode;

        internal KeyUpEventArgs(Key key, ScanCode scanCode)
        {
            Key = key;
            ScanCode = scanCode;
        }
    }
}
