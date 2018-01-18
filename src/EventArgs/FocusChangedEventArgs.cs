// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.FocusChanged"/> event.
    /// </summary>
    public class FocusChangedEventArgs : System.EventArgs
    {
        /// <summary>
        /// <code>true</code> if the <see cref="Window"/> got keyboard focus,
        /// <code>false</code> if it lost keyboard focus.
        /// </summary>
        public readonly bool HasFocus;

        internal FocusChangedEventArgs(bool hasFocus)
        {
            HasFocus = hasFocus;
        }
    }
}
