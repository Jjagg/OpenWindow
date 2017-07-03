// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    public class OpenGLWindowSettings
    {
        /// <summary>
        /// To create a window suitable for rendering with OpenGL, set this to <code>true</code>.
        /// When set to false, the other settings are unused. Defaults to <code>false</code>.
        /// </summary>
        public bool EnableOpenGl { get; set; }

        /// <summary>
        /// Set to <code>true</code> to enable double buffering. Defaults to <code>true</code>.
        /// </summary>
        public bool DoubleBuffer { get; set; } = true;

        /// <summary>
        /// Number of bits for the red channel. Defaults to 8.
        /// </summary>
        public int RedSize { get; set; } = 8;

        /// <summary>
        /// Number of bits for the green channel. Defaults to 8.
        /// </summary>
        public int GreenSize { get; set; } = 8;

        /// <summary>
        /// Number of bits for the blue channel. Defaults to 8.
        /// </summary>
        public int BlueSize { get; set; } = 8;

        /// <summary>
        /// Number of bits for the alpha channel. Defaults to 0.
        /// </summary>
        public int AlphaSize { get; set; }

        /// <summary>
        /// Size of the depth buffer in bits. Defaults to 16.
        /// </summary>
        public int DepthSize { get; set; } = 16;

        /// <summary>
        /// Size of the stencil buffer in bits. Defaults to 0.
        /// </summary>
        public int StencilSize { get; set; }

        /// <summary>
        /// Number of samples for MSAA (anti-aliasing). Set to 0 to disable MSAA. Defaults to 0 (no MSAA).
        /// </summary>
        public int MultiSampleCount { get; set; }
    }
}