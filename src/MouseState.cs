// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    public struct MouseState
    {
        /// <summary>
        /// The buttons that are down.
        /// </summary>
        public readonly MouseButton ButtonsDown;

        /// <summary>
        /// Position of the cursor.
        /// </summary>
        public readonly Point Position;

        /// <summary>
        /// <code>true</code> if the left mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool LeftDown => (ButtonsDown & MouseButton.Left) > 0;

        /// <summary>
        /// <code>true</code> if the middle mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool MiddleDown => (ButtonsDown & MouseButton.Middle) > 0;

        /// <summary>
        /// <code>true</code> if the right mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool RightDown => (ButtonsDown & MouseButton.Right) > 0;

        /// <summary>
        /// <code>true</code> if the X1 mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool X1Down => (ButtonsDown & MouseButton.X1) > 0;

        /// <summary>
        /// <code>true</code> if the X2 mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool X2Down => (ButtonsDown & MouseButton.X2) > 0;

        /// <summary>
        /// Create a mouse state.
        /// </summary>
        /// <param name="buttonsDown">The <see cref="MouseButton"/>s that are down.</param>
        /// <param name="position">The position of the cursor.</param>
        public MouseState(MouseButton buttonsDown, Point position)
        {
            ButtonsDown = buttonsDown;
            Position = position;
        }
    }
}