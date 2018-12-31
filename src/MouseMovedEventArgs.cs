using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.MouseMove"/> event.
    /// </summary>
    public struct MouseMoveEventArgs
    {
        /// <summary>
        /// X coordinate of the mouse relative to the upper left corner of the client area.
        /// </summary>
        public int X;

        /// <summary>
        /// Y coordinate of the mouse relative to the upper left corner of the client area.
        /// </summary>
        public int Y;

        /// <summary>
        /// Position of the mouse relative to the upper left corner of the client area.
        /// </summary>
        public Point Position => new Point(X, Y);

        internal MouseMoveEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
