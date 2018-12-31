namespace OpenWindow
{
    /// <summary>
    /// State of the mouse. Returned by <see cref="Window.GetMouseState"/>.
    /// </summary>
    public class MouseState
    {
        /// <summary>
        /// The window that currently has mouse focus.
        /// If mouse capture is off, this is the window that contains the mouse.
        /// This can be null.
        /// </summary>
        public Window FocusedWindow;

        /// <summary>
        /// The buttons that are down.
        /// </summary>
        public MouseButtons ButtonsDown;

        /// <summary>
        /// X coordinate of the mouse position.
        /// </summary>
        public int X;

        /// <summary>
        /// Y coordinate of the mouse position.
        /// </summary>
        public int Y;

        /// <summary>
        /// Position of the cursor.
        /// </summary>
        public Point Position
        {
            get => new Point(X, Y); 
            set { X = value.X; Y = value.Y; }
        }

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
        /// Create a mouse state with no buttons pressed and a position of (0, 0).
        /// </summary>
        public MouseState()
        {
        }

        /// <summary>
        /// Create a mouse state.
        /// </summary>
        /// <param name="buttonsDown">The <see cref="MouseButtons"/>s that are down.</param>
        /// <param name="position">The position of the cursor.</param>
        public MouseState(MouseButtons buttonsDown, Point position)
        {
            ButtonsDown = buttonsDown;
            X = position.X;
            Y = position.Y;
        }
    }
}
