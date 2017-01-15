// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    public class TextInputEventArgs : System.EventArgs
    {
        public readonly char Character;

        public TextInputEventArgs(char character)
        {
            Character = character;
        }
    }
}
