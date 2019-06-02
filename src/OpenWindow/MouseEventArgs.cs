using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.MouseDown"/> and <see cref="Window.MouseUp"/> events.
    /// </summary>
    public struct MouseEventArgs
    {
        /// <summary>
        /// The affected mouse button.
        /// </summary>
        public readonly MouseButtons Button;

        /// <summary>
        /// The x coordinate of the mouse position.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// The y coordinate of the mouse position.
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// The position of the mouse.
        /// </summary>
        public Point Position => new Point(X, Y);

        internal MouseEventArgs(MouseButtons buttons, int x, int y)
        {
            Button = buttons;
            X = x;
            Y = y;
        }

    }
}
