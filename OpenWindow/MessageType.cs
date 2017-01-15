// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    public enum MessageType
    {
        /// <summary>
        /// Signals that the native message is not handled by OpenWindow.
        /// </summary>
        Unhandled,
        /// <summary>
        /// Signals that the window should be closed.
        /// </summary>
        Closing,
        /// <summary>
        /// Signals that the window is getting destroyed.
        /// </summary>
        Destroy
    }
}