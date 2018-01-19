// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    /// <summary>
    /// Supported Windowing service backends.
    /// </summary>
    public enum WindowingServiceType
    {
        /// <summary>
        /// Windows. Uses the Win32 APIs.
        /// </summary>
        Win32,
        /// <summary>
        /// Linux. Uses the X window protocol.
        /// </summary>
        X,
        /// <summary>
        /// Linux. Uses the Wayland window protocol.
        /// </summary>
        Wayland,
        /// <summary>
        /// MacOS. Uses the Quartz API.
        /// </summary>
        Quartz
    }
}