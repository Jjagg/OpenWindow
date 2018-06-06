// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow
{
    /// <summary>
    /// Mouse buttons used for reporting mouse/touch pad input.
    /// </summary>
    [Flags]
    public enum MouseButtons
    {
        None = 0,
        /// <summary>
        /// Left mouse button.
        /// </summary>
        Left = 1,
        /// <summary>
        /// Middle mouse button.
        /// </summary>
        Middle = 2,
        /// <summary>
        /// Right mouse button.
        /// </summary>
        Right = 3,
        /// <summary>
        /// X1 mouse button. Usually the thumb button closest to you.
        /// </summary>
        X1 = 4,
        /// <summary>
        /// X2 mouse button. Usually the thumb button farthest to you.
        /// </summary>
        X2 = 5
    }
}