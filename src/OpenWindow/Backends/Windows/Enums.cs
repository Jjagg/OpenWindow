using System;

namespace OpenWindow.Backends.Windows
{
    [Flags]
    internal enum WindowStyleEx : uint
    {
        None = 0
    }

    internal enum WindowMessage : uint
    {
        Destroy         = 0x0002,
        Size            = 0x0005,
        Activate        = 0x0006,
        SetFocus        = 0x0007,
        KillFocus       = 0x0008,
        Close           = 0x0010,
        Quit            = 0x0012,
        GetMinMaxInfo   = 0x0024,
        InputLangChange = 0x0051,
        NcActivate      = 0x0086,
        NcMouseMove     = 0x00A0,
        KeyDown         = 0x0100,
        KeyUp           = 0x0101,
        Char            = 0x0102,
        SysKeyDown      = 0x0104,
        SysKeyUp        = 0x0105,
        UniChar         = 0x0109,
        MouseMove       = 0x0200,
        LButtonDown     = 0x0201,
        LButtonUp       = 0x0202,
        MButtonDown     = 0x0207,
        MButtonUp       = 0x0208,
        RButtonDown     = 0x0204,
        RButtonUp       = 0x0205,
        MouseWheel      = 0x020A,
        MouseHWheel     = 0x020E,
        XButtonDown     = 0x020B,
        XButtonUp       = 0x020C,
        Sizing          = 0x0214,
        EnterSizeMove   = 0x0231,
        ExitSizeMove    = 0x0232,
        MouseLeave      = 0x02A3,
    }

    [Flags]
    internal enum PfdFlags
    {
        DoubleBuffer  = 1,
        DrawToWindow  = 1 << 2,
        SupportOpengl = 1 << 5
    }

    internal enum PfdType : byte
    {
        Rgba = 0
    }

    internal enum ShowWindowCommand
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window
        /// for the first time.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3,
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        ShowMaximized = 3,
        /// <summary>
        /// Displays a window in its most recent size and position. This value
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level
        /// window in the Z order.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,
        /// <summary>
        /// Displays the window in its current size and position. This value is
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the
        /// window is not activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the SW_* value specified in the
        /// STARTUPINFO structure passed to the CreateProcess function by the
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,
        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
        /// that owns the window is not responding. This flag should only be
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }

    internal enum Cursor
    {
        Arrow = 32512,
        Hand = 32649,
        IBeam = 32513,
        Wait = 32514
    }

    internal enum SystemMetric
    {
        CMonitors = 80,
    }

    internal enum WglPiAttributes : int
    {
        NumberPixelFormats          = 0x2000, 
        DrawToWindow                = 0x2001,
        DrawToBitmap                = 0x2002,
        Acceleration                = 0x2003,
        NeedPalette                 = 0x2004,
        NeedSystemPalette           = 0x2005,
        SwapLayerBuffers            = 0x2006,
        SwapMethod                  = 0x2007,
        NumberOverlays              = 0x2008,
        NumberUnderlays             = 0x2009,
        Transparent                 = 0x200A,
        TransparentRedValue         = 0x2037,
        TransparentGreenValue       = 0x2038,
        TransparentBlueValue        = 0x2039,
        TransparentAlphaValue       = 0x203A,
        TransparentIndexValue       = 0x203B,
        ShareDepth                  = 0x200C,
        ShareStencil                = 0x200D,
        ShareAccum                  = 0x200E,
        SupportGdi                  = 0x200F,
        SupportOpengl               = 0x2010,
        DoubleBuffer                = 0x2011,
        Stereo                      = 0x2012,
        PixelType                   = 0x2013,
        ColorBits                   = 0x2014,
        RedBits                     = 0x2015,
        RedShift                    = 0x2016,
        GreenBits                   = 0x2017,
        GreenShift                  = 0x2018,
        BlueBits                    = 0x2019,
        BlueShift                   = 0x201A,
        AlphaBits                   = 0x201B,
        AlphaShift                  = 0x201C,
        AccumBits                   = 0x201D,
        AccumRedBits                = 0x201E,
        AccumGreenBits              = 0x201F,
        AccumBlueBits               = 0x2020,
        AccumAlphaBits              = 0x2021,
        DepthBits                   = 0x2022,
        StencilBits                 = 0x2023,
        AuxBuffers                  = 0x2024,
        SampleBuffers               = 0x2041,
        Samples                     = 0x2042,
    }

    internal enum KeyMapType : uint
    {
        VkToSc = 0,
        ScToVk = 1,
        VkToChar = 2,
        ScToVkEx = 3,
        VkToScEx = 4
    }
}
