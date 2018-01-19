// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.TextInput"/> event.
    /// </summary>
    public class TextInputEventArgs : System.EventArgs
    {
        /// <summary>
        /// The character input.
        /// </summary>
        public readonly char Character;

        internal TextInputEventArgs(char character)
        {
            Character = character;
        }
    }
}
