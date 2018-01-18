// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.KeyDown"/> event.
    /// </summary>
    public class KeyDownEventArgs : System.EventArgs
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
        /// <code>true</code> if <see cref="RepeatCount"/> > 0, <code>false</code> otherwise.
        /// </summary>
        public bool Repeated => RepeatCount > 0;

        internal KeyDownEventArgs(Key key, int repeatCount, int scanCode, char character)
        {
            Key = key;
            RepeatCount = repeatCount;
            ScanCode = scanCode;
            Character = character;
        }
    }
}