namespace OpenWindow
{
    /// <summary>
    /// State of the mouse. Returned by <see cref="Window.GetMouseState"/>.
    /// </summary>
    public struct MouseState
    {
        /// <summary>
        /// The buttons that are down.
        /// </summary>
        public readonly MouseButtons ButtonsDown;

        /// <summary>
        /// Position of the cursor.
        /// </summary>
        public readonly Point Position;

        /// <summary>
        /// <code>true</code> if the left mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool LeftDown => (ButtonsDown & MouseButtons.Left) > 0;

        /// <summary>
        /// <code>true</code> if the middle mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool MiddleDown => (ButtonsDown & MouseButtons.Middle) > 0;

        /// <summary>
        /// <code>true</code> if the right mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool RightDown => (ButtonsDown & MouseButtons.Right) > 0;

        /// <summary>
        /// <code>true</code> if the X1 mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool X1Down => (ButtonsDown & MouseButtons.X1) > 0;

        /// <summary>
        /// <code>true</code> if the X2 mouse button is down, <code>false</code> if it is up.
        /// </summary>
        public bool X2Down => (ButtonsDown & MouseButtons.X2) > 0;

        /// <summary>
        /// Create a mouse state.
        /// </summary>
        /// <param name="buttonsDown">The <see cref="MouseButtons"/>s that are down.</param>
        /// <param name="position">The position of the cursor.</param>
        public MouseState(MouseButtons buttonsDown, Point position)
        {
            ButtonsDown = buttonsDown;
            Position = position;
        }
    }
}
