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
