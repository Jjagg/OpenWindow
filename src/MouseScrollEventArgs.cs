using System;

namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.MouseScroll"/> event.
    /// </summary>
    public struct MouseScrollEventArgs
    {
        /// <summary>
        /// The amount scrolled horizontally. Positive is to the right, negative is to the left.
        /// </summary>
        public float X;

        /// <summary>
        /// The amount scrolled vertically. Positive is forward (away from the user), negative is backward (to the user).
        /// </summary>
        public float Y;

        internal MouseScrollEventArgs(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
