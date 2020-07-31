using System;
using System.Buffers;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal sealed class Win32Window : Window
    {
        #region Fields

        private WindowData _windowData;
        internal IntPtr Hwnd { get; }

        private string _className;

        // Handles we keep around to clean up stuff we created
        private IntPtr _lastSetIconHandle;
        private IntPtr _lastSetCursorHandle;
        // Win32 sends UTF-16 chars so we store high surrogates to join them with their paired low surrogate
        // that follows it so we can hand users the codepoint.
        private char _lastHighSurrogate;

        private IntPtr _defaultCursor;

        #endregion

        #region Constructor

        public Win32Window(OpenGlSurfaceSettings glSettings, WndProc wndProc)
            : base()
        {
            var winClass = new WndClass
            {
                style = Constants.CS_OWNDC,
                lpfnWndProc = wndProc,
                hInstance = Native.GetModuleHandle(null),
                lpszClassName = $"OpenWindow_DUMMY[{Native.GetCurrentThreadId()}]({_windowId++})"
            };

            _className = winClass.lpszClassName;

            if (Native.RegisterClass(ref winClass) == 0)
                throw GetLastException("Registering window class failed.");

            Hwnd = Native.CreateWindowEx(
                0,
                winClass.lpszClassName,
                "OpenWindow dummy window",
                0,
                0, 0, 0, 0,
                IntPtr.Zero,
                IntPtr.Zero,
                winClass.hInstance,
                IntPtr.Zero);

            _windowData = new Win32WindowData(Hwnd);

            InitOpenGl(glSettings);
        }

        public Win32Window(WindowingService ws, WndProc wndProc, in WindowCreateInfo wci)
            : base(ws, false, wci)
        {
            RegisterNewWindowClass(wndProc);

            // Don't immediately show the window because we might need to recreate it if multisampling is enabled.
            var style = GetWindowStyle(wci.Decorated, wci.Resizable, visible: false);

            var x = wci.X;
            var y = wci.Y;
            var width = wci.Width;
            var height = wci.Height;

            // Win32 sets the non-client rect of the window, but we
            // mean the client size, so we need to adjust.
            if (style != Constants.WS_OVERLAPPED)
            {
                var rect = new Rect(x, y, x + width, y + height);
                Native.AdjustWindowRect(ref rect, style, false);
                x = rect.Left;
                y = rect.Top;
                width = rect.Width;
                height = rect.Height;
            }

            var handle = Native.CreateWindowEx(
                WindowStyleEx.None,
                _className,
                wci.Title,
                style,
                x,
                y,
                width,
                height,
                IntPtr.Zero,
                IntPtr.Zero,
                (IntPtr) Native.GetModuleHandle(null),
                IntPtr.Zero);

            var wi = WindowInfo.Create();
            var result = Native.GetWindowInfo(handle, ref wi);

            if (handle == IntPtr.Zero)
            {
                if (!Native.UnregisterClass(_className, IntPtr.Zero))
                    throw GetLastException("Failed to unregister window class!");
                throw GetLastException("Failed to create window.");
            }

            Hwnd = handle;

            var glSettings = ws.GlSettings;
            if (glSettings.EnableOpenGl)
            {
                InitOpenGl(glSettings);

                // FIXME: use the actual ms count, not the preferred
                if (glSettings.MultiSampleCount > 1)
                {
                    // we need to recreate the window to support multisampling
                    Native.DestroyWindow(Hwnd);
                    Hwnd = Native.CreateWindowEx(
                        WindowStyleEx.None,
                        _className,
                        string.Empty,
                        style,
                        x,
                        y,
                        width,
                        height,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        (IntPtr) Native.GetModuleHandle(null),
                        IntPtr.Zero);
                    InitOpenGl(glSettings);
                }
            }
            else
            {
                GlSettings = OpenGlSurfaceSettings.Disabled;
            }

            _windowData = new Win32WindowData(Hwnd);

            // Now we can show the window
            Native.ShowWindow(Hwnd, ShowWindowCommand.Show);
        }

        private void InitOpenGl(OpenGlSurfaceSettings s)
        {
            var hdc = IntPtr.Zero;
            var hglrc = IntPtr.Zero;
            try
            {
                var pfd = new PixelFormatDescriptor();
                pfd.nSize = (short) Marshal.SizeOf<PixelFormatDescriptor>();
                pfd.nVersion = 1;
                pfd.dwFlags = PfdFlags.DrawToWindow | PfdFlags.SupportOpengl;

                if (s.DoubleBuffer)
                    pfd.dwFlags |= PfdFlags.DoubleBuffer;

                pfd.iPixelType = PfdType.Rgba;

                pfd.cRedBits = (byte) s.RedSize;
                pfd.cGreenBits = (byte) s.GreenSize;
                pfd.cBlueBits = (byte) s.BlueSize;
                pfd.cAlphaBits = (byte) s.AlphaSize;
                pfd.cColorBits = (byte) (s.RedSize + s.GreenSize + s.BlueSize);

                pfd.cDepthBits = (byte) s.DepthSize;
                pfd.cStencilBits = (byte) s.StencilSize;

                hdc = Native.GetDC(Hwnd);
                if (hdc == IntPtr.Zero)
                {
                    WindowingService.LogWarning(
                        "Failed to get a device context. Window was not initialized with OpenGL support.");
                    return;
                }

                var iPixelFormat = Native.ChoosePixelFormat(hdc, ref pfd);
                if (wglChoosePixelFormatARB == null)
                {
                    Native.SetPixelFormat(hdc, iPixelFormat, ref pfd);

                    hglrc = Native.wglCreateContext(hdc);
                    Native.wglMakeCurrent(hdc, hglrc);

                    wglChoosePixelFormatARB = LoadWglExtension<wglChoosePixelFormatArbDelegate>("wglChoosePixelFormatARB");
                }

                var actualMsCount = 0;
                if (wglChoosePixelFormatARB != null)
                {
                    const int fullAcceleration = 0x2027;
                    int[] piattrs =
                    {
                        (int) WglPiAttributes.DrawToWindow, 1,
                        (int) WglPiAttributes.SupportOpengl, 1,
                        (int) WglPiAttributes.Acceleration, fullAcceleration,
                        (int) WglPiAttributes.DoubleBuffer, s.DoubleBuffer ? 1 : 0,
                        (int) WglPiAttributes.RedBits, pfd.cRedBits,
                        (int) WglPiAttributes.BlueBits, pfd.cBlueBits,
                        (int) WglPiAttributes.GreenBits, pfd.cGreenBits,
                        (int) WglPiAttributes.AlphaBits, pfd.cAlphaBits,
                        (int) WglPiAttributes.DepthBits, pfd.cDepthBits,
                        (int) WglPiAttributes.StencilBits, pfd.cStencilBits,
                        (int) WglPiAttributes.SampleBuffers, s.MultiSampleCount > 1 ? 1 : 0,
                        (int) WglPiAttributes.Samples, s.MultiSampleCount == 1 ? 0 : s.MultiSampleCount,
                        0
                    };

                    var pis = new int[1];
                    if (!wglChoosePixelFormatARB(hdc, piattrs, null, 1, pis, out var nformats))
                        throw GetLastException("wglChoosePixelFormatARB failed!");
                    if (nformats == 0)
                        throw new OpenWindowException("GL initialization failed: no matching pixel formats!");
                    Native.SetPixelFormat(hdc, pis[0], ref pfd);
                    // TODO can we get the actual ms count here?
                }
                else
                {
                    if (GlSettings.MultiSampleCount > 1)
                        WindowingService.LogWarning("wglChoosePixelFormatARB not supported.");
                    else
                        WindowingService.LogInfo("wglChoosePixelFormatARB not supported.");
                }

                var ppfd = new PixelFormatDescriptor();
                Native.DescribePixelFormat(hdc, iPixelFormat, (uint) Marshal.SizeOf<PixelFormatDescriptor>(), ref ppfd);

                GlSettings = new OpenGlSurfaceSettings
                {
                    EnableOpenGl = true,
                    DoubleBuffer = (ppfd.dwFlags & PfdFlags.DoubleBuffer) != 0,
                    RedSize = pfd.cRedBits,
                    GreenSize = pfd.cGreenBits,
                    BlueSize = pfd.cBlueBits,
                    AlphaSize = pfd.cAlphaBits,
                    DepthSize = pfd.cDepthBits,
                    StencilSize = pfd.cStencilBits,
                    MultiSampleCount = actualMsCount
                };
            }
            finally
            {
                if (hglrc != IntPtr.Zero) Native.wglDeleteContext(hglrc);
                if (hdc != IntPtr.Zero) Native.ReleaseDC(Hwnd, hdc);
            }
        }

        #endregion

        #region Window Properties

        /// <inheritdoc />
        public override Point Position
        {
            get
            {
                if (!Native.GetWindowRect(Hwnd, out var rect))
                    throw GetLastException("Failed to get window bounds.");
                return rect.Position;
            }
            set
            {
                if (!Native.SetWindowPos(Hwnd, IntPtr.Zero, value.X, value.Y, 0, 0, Constants.SWP_NOSIZE | Constants.SWP_NOZORDER))
                    throw GetLastException("Setting window position failed.");
            }
        }

        /// <inheritdoc />
        public override Size ClientSize
        {
            get
            {
                if (!Native.GetClientRect(Hwnd, out var rect))
                    throw GetLastException("Failed to get window client rectangle.");
                return rect.Size;
            }
            set
            {
                var rect = new Rect(0, 0, value.Width, value.Height);
                var style = GetWindowStyle();
                if (!Native.AdjustWindowRect(ref rect, style, false))
                    throw GetLastException("Failed to set client rectangle.");
                if (!Native.SetWindowPos(Hwnd, IntPtr.Zero, 0, 0, rect.Width, rect.Height, Constants.SWP_NOMOVE | Constants.SWP_NOZORDER))
                    throw GetLastException("Setting window position failed.");
            }
        }

        /// <inheritdoc />
        public override Rectangle ClientBounds
        {
            get
            {
                if (!Native.GetClientRect(Hwnd, out var rect))
                    throw GetLastException("Failed to get window client rectangle.");
                var pt = Point.Zero;
                if (!Native.ClientToScreen(Hwnd, ref pt))
                    throw GetLastException("ClientToScreen failed.");
                return new Rectangle(pt, rect.Size);
            }
            set
            {
                Rect rect = value;
                var style = GetWindowStyle();
                if (!Native.AdjustWindowRect(ref rect, style, false))
                    throw GetLastException("Failed to set client rectangle.");

                if (!Native.SetWindowPos(Hwnd, IntPtr.Zero, rect.Left, rect.Top, rect.Width, rect.Height, Constants.SWP_NOZORDER))
                    throw GetLastException("Failed to set window bounds.");
            }
        }

        #endregion

        #region Window Functions

        /// <inheritdoc />
        public override Display GetContainingDisplay()
        {
            var displayHandle = Native.MonitorFromWindow(Hwnd, Constants.MonitorDefaultToNearest);
            var display = Service.Displays.FirstOrDefault(d => d.Handle == displayHandle);
            if (display == null)
                throw new InvalidOperationException("Containing display for a window was not a known display! This should not happen!");
            return display;
        }

        /// <inheritdoc />
        public override WindowData GetPlatformData() => _windowData;

        #endregion

        #region Protected Methods

        /// <inheritdoc />
        protected override void InternalMaximize()
        {
            Native.ShowWindow(Hwnd, ShowWindowCommand.Maximize);
        }

        /// <inheritdoc />
        protected override void InternalMinimize()
        {
            Native.ShowWindow(Hwnd, ShowWindowCommand.Minimize);
        }

        /// <inheritdoc />
        protected override void InternalRestore()
        {
            Native.ShowWindow(Hwnd, ShowWindowCommand.Restore);
        }

        /// <inheritdoc />
        protected override void InternalSetTitle(string value)
        {
            if (!Native.SetWindowText(Hwnd, value))
                throw GetLastException("Failed to set window title.");
        }

        /// <inheritdoc />
        protected override void InternalSetDecorated(bool value)
        {
            UpdateStyle();
        }

        /// <inheritdoc />
        protected override void InternalSetResizable(bool value)
        {
            UpdateStyle();
        }

        /// <inheritdoc />
        protected override void InternalSetMinSize(Size value)
        {
            if (value == Size.Empty)
                return;
            var s = ClientSize;
            if (s.Width < value.Width || s.Height < value.Height)
                ClientSize = s;
        }

        /// <inheritdoc />
        protected override void InternalSetMaxSize(Size value)
        {
            if (value == Size.Empty)
                return;
            var s = ClientSize;
            if (s.Width > value.Width || s.Height > value.Height)
                ClientSize = s;
        }

        /// <inheritdoc />
        protected override void InternalSetCursorVisible(bool value)
        {
            Native.ShowCursor(value);
        }

        /// <inheritdoc />
        protected unsafe override void InternalSetIcon(ReadOnlySpan<byte> pixelData, int width, int height)
        {
            var iconHandle = CreateIcon(pixelData, width, height, isIcon: true);

            // Set the small icon (caption icon)
            var oldSmallIcon = Native.SendMessage(Hwnd, WindowMessage.SetIcon, new IntPtr(0), iconHandle);
            // Set the big icon (Alt+Tab dialog icon)
            var oldBigIcon = Native.SendMessage(Hwnd, WindowMessage.SetIcon, new IntPtr(1), iconHandle);

            // Destroy icons if we created them
            if (oldSmallIcon != IntPtr.Zero && oldSmallIcon == _lastSetIconHandle)
            {
                Native.DestroyIcon(oldSmallIcon);
            }
            else if (oldBigIcon != IntPtr.Zero && oldBigIcon == _lastSetIconHandle)
            {
                Native.DestroyIcon(oldBigIcon);
            }

            _lastSetIconHandle = iconHandle;
        }

        protected override void InternalSetCursor(ReadOnlySpan<byte> pixelData, int width, int height, int hotspotX, int hotspotY)
        {
            var cursorHandle = CreateIcon(pixelData, width, height, isIcon: false);
            var oldCursor = Native.SetCursor(cursorHandle);
            if (oldCursor == _lastSetCursorHandle)
            {
                Native.DestroyIcon(oldCursor);
            }

            _lastSetCursorHandle = cursorHandle;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Processes UTF-16 characters into UTF-32 unicode codepoints by
        /// combining surrogate pairs from subsequent calls if necessary and
        /// by dropping control characters.
        /// </summary>
        internal bool TryGetUtf32(char c, out int utf32)
        {
            const char noChar = (char) 0;

            if (char.IsHighSurrogate(c))
            {
                utf32 = noChar;
                _lastHighSurrogate = c;
            }
            else if (char.IsLowSurrogate(c))
            {
                if (_lastHighSurrogate != noChar)
                {
                    utf32 = char.ConvertToUtf32(_lastHighSurrogate, c);
                    _lastHighSurrogate = noChar;
                }
                else
                {
                    WindowingService.LogError("Got a low surrogate UTF-16 character without getting a UTF-16 high surrogate first.");
                    utf32 = noChar;
                }
            }
            else if (char.IsControl(c))
            {
                utf32 = noChar;
            }
            else
            {
                utf32 = c;
            }

            return utf32 != noChar;
        }

        internal void ResetCursor()
        {
            // FIXME We should maybe initialize the cursor to the system arrow cursor
            if (_lastSetCursorHandle != IntPtr.Zero)
            {
                Native.SetCursor(_lastSetCursorHandle);
            }
            else
            {
                if (_defaultCursor == default)
                    _defaultCursor = Native.LoadCursor(IntPtr.Zero, Cursor.Arrow);

                Native.SetCursor(_defaultCursor);
            }
        }

        #endregion

        #region Private Methods

        private static uint _windowId;
        private void RegisterNewWindowClass(WndProc wndProc)
        {
            _className = $"OpenWindow[{Native.GetCurrentThreadId()}]({_windowId++})";
            var winClass = new WndClass();
            winClass.lpszClassName = _className;

            winClass.lpfnWndProc = wndProc;
            winClass.hInstance = Native.GetModuleHandle(null);

            // If we set a cursor here we can't change the cursor later.
            // From https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setcursor#remarks:
            // "[...] make sure the class cursor for the specified window's class is set to NULL. If the class
            //  cursor is not NULL, the system restores the class cursor each time the mouse is moved."
            winClass.hCursor = IntPtr.Zero;
            if (Native.RegisterClass(ref winClass) == 0)
                throw GetLastException("Registering window class failed.");
        }

        private uint GetWindowStyle()
            => GetWindowStyle(Decorated, Resizable, visible: true);

        private uint GetWindowStyle(bool decorated, bool resizable, bool visible)
        {
            // FIXME Window size is wrong when Decorated = true and Resizable = false.
            // Client width and height turn out 4 pixels too big. Where do they come from?

            // TODO undecorated window (popup) still needs a border to be displayed
            // There are also some issues with a top level window being a popup window
            // For better borderless windows we should look at
            // - https://docs.microsoft.com/en-us/windows/win32/dwm/customframe
            // - https://github.com/rossy/borderless-window

            uint style = Constants.WS_MINIMIZEBOX | Constants.WS_SYSMENU;

            if (resizable)
            {
                style |= Constants.WS_THICKFRAME | Constants.WS_MAXIMIZEBOX;
            }

            if (decorated)
            {
                style |= Constants.WS_CAPTION;
            }
            else
            {
                style |= Constants.WS_POPUP | Constants.WS_BORDER;
            }

            if (visible)
            {
                style |= Constants.WS_VISIBLE;
            }

            return style;
        }

        private void UpdateStyle()
        {
            var ws = GetWindowStyle();
            const int GWL_STYLE = -16;
            Native.SetWindowLong(Hwnd, GWL_STYLE, (int) ws);
            Native.ShowWindow(Hwnd, ShowWindowCommand.Show);
        }

        private unsafe IntPtr CreateIcon(ReadOnlySpan<byte> pixelData, int width, int height, bool isIcon)
        {
            var pixelDataSize = width * height * 4;
            // mask pitch needs to be aligned
            // FIXME some sources say it needs to be WORD (16-bit) or DWORD (32-bit) aligned, need to double-check
            var maskDataSize = (width + 7) * height / 8;
            var iconDataArray = ArrayPool<byte>.Shared.Rent(40 + pixelDataSize + maskDataSize);
            Span<byte> iconData = iconDataArray;

            // Bitmap Info Header:
            // typedef struct tagBITMAPINFOHEADER {
            //   DWORD biSize;
            //   LONG  biWidth;
            //   LONG  biHeight;
            //   WORD  biPlanes;
            //   WORD  biBitCount;
            //   DWORD biCompression;
            //   DWORD biSizeImage;
            //   LONG  biXPelsPerMeter;
            //   LONG  biYPelsPerMeter;
            //   DWORD biClrUsed;
            //   DWORD biClrImportant;
            // }

            SpanWriter sr = iconData;
            sr.Write(40);
            sr.Write(width);
            sr.Write(height * 2);
            sr.Write((ushort) 1);
            sr.Write((ushort) 32); // bpp
            sr.Write(0); // BI_RGB
            sr.Write(pixelDataSize);
            sr.Write(0);
            sr.Write(0);
            sr.Write(0);
            sr.Write(0);

            // DIBs have a bottom-left origin so we need to flip our image data vertically
            var rowSize = 4 * width;
            for (var y = height - 1; y >= 0; y--)
            {
                var row = pixelData.Slice(rowSize * y, rowSize);
                sr.Write(row);
            }

            // We have to write a monochrome (1 bpp) bitmap mask to enable transparency
            // even though the values are ignored: https://stackoverflow.com/a/5926552
            sr.Write(0xff, maskDataSize);

            IntPtr iconHandle;
            fixed (byte* iconDataPtr = iconData)
            {
                const uint defaultSizeFlag = 0x40;
                iconHandle = Native.CreateIconFromResourceEx(iconDataPtr, (uint) iconData.Length, true, 0x00030000, 0, 0, defaultSizeFlag);
            }

            ArrayPool<byte>.Shared.Return(iconDataArray);

            if (iconHandle == IntPtr.Zero)
            {
                throw GetLastException("Failed to create icon.");
            }

            return iconHandle;
        }

        private static Exception GetLastException(string message)
        {
            var e = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            return new OpenWindowException(message, e);
        }

        #endregion

        #region Extensions

        private delegate bool wglChoosePixelFormatArbDelegate(
            IntPtr hdc,
            [In] int[] attribIList,
            [In] float[] attribFList,
            uint maxFormats,
            [Out] int[] pixelFormats,
            out uint numFormats);

        private delegate bool wglGetPixelFormatAttribivArbDelegate(
            IntPtr hdc,
            [In] int iPixelFormat,
            [In] int iLayerPlane,
            [In] uint nAttributes,
            [In] int[] piAttributes,
            [Out] int[] piValues);


        private delegate string wglGetExtensionsString(IntPtr hdc);

        private static wglChoosePixelFormatArbDelegate wglChoosePixelFormatARB;
        private static wglGetPixelFormatAttribivArbDelegate wglGetPixelFormatAttribivARB;
        private static wglGetPixelFormatAttribivArbDelegate wglGetPixelFormatAttribivEXT;

        private static T LoadWglExtension<T>(string name) where T : class
        {
            var ptr = Native.wglGetProcAddress(name);
            if (ptr == IntPtr.Zero)
                return default(T);
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        #endregion

        #region IDisposable

        protected override void ReleaseUnmanagedResources()
        {
            if (_defaultCursor != IntPtr.Zero)
            {
                if (Native.DestroyIcon(_defaultCursor))
                {
                    _defaultCursor = IntPtr.Zero;
                }
                else
                {
                    WindowingService.LogWarning("Failed to destroy arrow cursor.");
                }
            }

            if (!UserManaged)
            {
                Native.DestroyWindow(Hwnd);
                if (!Native.UnregisterClass(_className, IntPtr.Zero))
                    throw GetLastException("Failed to unregister window class!");
            }
        }

        #endregion
    }
}
