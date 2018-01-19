// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.MouseMoved"/> event.
    /// </summary>
    public class MouseMovedEventArgs : EventArgs
    {
        /// <summary>
        /// The position of the mouse.
        /// </summary>
        public readonly Point Position;

        internal MouseMovedEventArgs(Point position)
        {
            Position = position;
        }
    }
}