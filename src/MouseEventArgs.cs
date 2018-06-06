// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.MouseDown"/> and <see cref="Window.MouseUp"/> events.
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// The affected <see cref="MouseButtons"/>.
        /// </summary>
        public readonly MouseButtons Button;

        /// <summary>
        /// The position of the mouse.
        /// </summary>
        public readonly Point Position;

        internal MouseEventArgs(MouseButtons buttons, Point position)
        {
            Button = buttons;
            Position = position;
        }

    }
}