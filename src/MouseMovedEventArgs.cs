using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.MouseMoved"/> event.
    /// </summary>
    public struct MouseMovedEventArgs
    {
        /// <summary>
        /// The x coordinate of the mouse in the window client area.
        /// </summary>
        public int X;

        /// <summary>
        /// The y coordinate of the mouse in the window client area.
        /// </summary>
        public int Y;

        /// <summary>
        /// The position of the mouse in the window client area.
        /// </summary>
        public Point Position => new Point(X, Y);

        internal MouseMovedEventArgs(int x, int y)
        {
            X = x;
            Y =y ;
        }
    }
}
