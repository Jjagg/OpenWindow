using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.KeyDown"/> event.
    /// </summary>
    public class KeyDownEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="Key"/> that was pressed or released.
        /// </summary>
        public readonly Key Key;

        /// <summary>
        /// Number of times a key down was reported.
        /// </summary>
        public readonly int RepeatCount;

        /// <summary>
        /// The scancode of the physicial key that was pressed or released. This is a platform-specific identifier.
        /// </summary>
        public readonly int ScanCode;

        /// <summary>
        /// The character corresponding to the pressed key.
        /// </summary>
        public readonly char Character;

        /// <summary>
        /// <code>true</code> if this is a repeated key press, triggered by the user holding down the key,
        /// <code>false</code> otherwise.
        /// </summary>
        public readonly bool Repeated;

        internal KeyDownEventArgs(Key key, int repeatCount, bool repeated, int scanCode, char character)
        {
            Key = key;
            RepeatCount = repeatCount;
            Repeated = repeated;
            ScanCode = scanCode;
            Character = character;
        }
    }
}
