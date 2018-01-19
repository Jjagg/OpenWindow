// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.KeyUp"/> and <see cref="Window.KeyPress"/> events.
    /// </summary>
    public class KeyEventArgs : System.EventArgs
    {
        /// <summary>
        /// The <see cref="Key"/> that was pressed or released.
        /// </summary>
        public readonly Key Key;

        /// <summary>
        /// The scancode of the physicial key that was pressed or released. This is a platform-specific identifier.
        /// </summary>
        public readonly int ScanCode;

        /// <summary>
        /// The character corresponding to the pressed key.
        /// </summary>
        public readonly char Character;

        internal KeyEventArgs(Key key, int scanCode, char character)
        {
            Key = key;
            ScanCode = scanCode;
            Character = character;
        }
    }
}