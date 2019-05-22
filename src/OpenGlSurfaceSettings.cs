namespace OpenWindow
{
    /// <summary>
    /// Surface settings to request when creating a window. This must be properly configured
    /// if you plan on drawing to this window with OpenGL.
    /// </summary>
    public class OpenGlSurfaceSettings
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
        /// Number of samples for MSAA (anti-aliasing). Set to 1 to disable MSAA. Defaults to 1 (no MSAA).
        /// </summary>
        public int MultiSampleCount { get; set; } = 1;
    }
}
