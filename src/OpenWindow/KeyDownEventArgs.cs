using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.KeyDown"/> event.
    /// </summary>
    public struct KeyDownEventArgs
    {
        /// <summary>
        /// The virtual <see cref="Key"/> that was pressed or released.
        /// </summary>
        public readonly Key Key;

        /// <summary>
        /// The scancode of the physicial key that was pressed or released.
        /// </summary>
        public readonly ScanCode ScanCode;

        internal KeyDownEventArgs(Key key, ScanCode scanCode)
        {
            Key = key;
            ScanCode = scanCode;
        }
    }
}
