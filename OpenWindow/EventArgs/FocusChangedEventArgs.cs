// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow.EventArgs
{
    public class FocusChangedEventArgs : System.EventArgs
    {
        public readonly bool HasFocus;

        public FocusChangedEventArgs(bool hasFocus)
        {
            HasFocus = hasFocus;
        }
    }
}
