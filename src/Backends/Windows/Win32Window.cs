// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal class Win32Window : Window
    {
        #region Static

        private static readonly IntPtr ModuleHinstance = new IntPtr(Native.GetModuleHandle(null));

        private const uint DefaultWs = Constants.WS_OVERLAPPED | Constants.WS_CAPTION |
                                       Constants.WS_SYSMENU | Constants.WS_MINIMIZEBOX;

        #endregion

        #region Private Fields

        private string _className;

        #endregion

        #region Constructor

        public Win32Window(IntPtr handle)
            : base(true)
        {
            Handle = handle;
            // TODO init properties
        }

        public Win32Window(OpenGlSurfaceSettings glSettings, bool show)
            : base(false)
        {
            RegisterNewWindowClass();

            var handle = Native.CreateWindowEx(
                WindowStyleEx.None,
                _className,
                Title,
                DefaultWs,
                0,
                0,
                100,
                100,
                IntPtr.Zero,
                IntPtr.Zero,
                ModuleHinstance,
                IntPtr.Zero);

            if (handle == IntPtr.Zero)
            {
                Native.UnregisterClass(_className, IntPtr.Zero);
                throw GetLastException("Failed to create window.");
            }

            Handle = handle;

            if (glSettings.EnableOpenGl)
            {
                InitOpenGl(glSettings);

                // FIXME: use the actual ms count, not the preferred
                if (glSettings.MultiSampleCount > 1)
                {
                    // we need to recreate the window to have a multisample window
                    Native.DestroyWindow(Handle);
                    Handle = Native.CreateWindowEx(
                        WindowStyleEx.None,
                        _className,
                        string.Empty,
                        DefaultWs,
                        0,
                        0,
                        100,
                        100,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        ModuleHinstance,
                        IntPtr.Zero);
                    InitOpenGl(glSettings);
                }
            }
            else
                GlSettings = new OpenGlSurfaceSettings();

            if (show)
                Native.ShowWindow(Handle, ShowWindowCommand.Normal);
        }

        private void InitOpenGl(OpenGlSurfaceSettings s)
        {
            var hdc = IntPtr.Zero;
            var hglrc = IntPtr.Zero;
            try
            {
                var pfd = new PixelFormatDescriptor();
                pfd.nSize = (short) Marshal.SizeOf(typeof(PixelFormatDescriptor));
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

                hdc = Native.GetDC(Handle);
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
                    if (!wglChoosePixelFormatARB.Invoke(hdc, piattrs, null, 1, pis, out var nformats))
                        throw GetLastException("wglChoosePixelFormatARB failed!");
                    if (nformats == 0)
                        throw new OpenWindowException("GL initialization failed: no matching pixel formats!");
                    Native.SetPixelFormat(hdc, pis[0], ref pfd);
                    // todo can we get the actual ms count here?
                }
                else
                {
                    if (GlSettings.MultiSampleCount > 1)
                        WindowingService.LogWarning("wglChoosePixelFormatARB not supported.");
                    else
                        WindowingService.LogInfo("wglChoosePixelFormatARB not supported.");
                }

                var ppfd = new PixelFormatDescriptor();
                Native.DescribePixelFormat(hdc, iPixelFormat, (uint) Marshal.SizeOf(typeof(PixelFormatDescriptor)), ref ppfd);

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
                if (hdc != IntPtr.Zero) Native.ReleaseDC(Handle, hdc);
            }
        }

        #endregion

        #region Window Properties

        /// <inheritdoc />
        public override Point Position
        {
            get => Bounds.Position;
            set
            {
                if (!Native.SetWindowPos(Handle, IntPtr.Zero, value.X, value.Y, 0, 0, Constants.SWP_NOSIZE | Constants.SWP_NOZORDER))
                    throw GetLastException("Setting window position failed.");
            }
        }

        /// <inheritdoc />
        public override Size Size
        {
            get => Bounds.Size;
            set
            {
                if (!Native.SetWindowPos(Handle, IntPtr.Zero, 0, 0, value.Width, value.Height, Constants.SWP_NOMOVE | Constants.SWP_NOZORDER))
                    throw GetLastException("Setting window position failed.");
            }
        }

        /// <inheritdoc />
        public override Rectangle Bounds
        {
            get
            {
                if (!Native.GetWindowRect(Handle, out var rect))
                    throw GetLastException("Failed to get window bounds.");
                return rect;
            }
            set
            {
                if (Bounds == value)
                    return;
                if (!Native.SetWindowPos(Handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, Constants.SWP_NOZORDER))
                    throw GetLastException("Failed to set window bounds.");
            }
        }

        /// <inheritdoc />
        public override Rectangle ClientBounds
        {
            get
            {
                if (!Native.GetClientRect(Handle, out var rect))
                    throw GetLastException("Failed to get window client rectangle.");
                return rect;
            }
            set
            {
                Rect rect = value;
                var style = GetWindowStyle();
                if (!Native.AdjustWindowRect(ref rect, style, false))
                    throw GetLastException("Failed to set client rectangle.");
                Bounds = rect;
            }
        }

        #endregion

        #region Window Functions

        /// <inheritdoc />
        public override Display GetContainingDisplay()
        {
            var displayHandle = Native.MonitorFromWindow(Handle, Constants.MonitorDefaultToNearest);
            var service = (Win32WindowingService) WindowingService.Get();
            if (!service.DisplayDict.ContainsKey(displayHandle))
                throw new InvalidOperationException("Containing display for a window was not a known display! This should not happen!");
            return service.DisplayDict[displayHandle];
        }

        /// <inheritdoc />
        public override bool IsDown(Key key)
        {
            var vk = KeyMap.InvMap[(int) key];
            return Native.GetKeyState(vk) < 0;
        }

        public override KeyMod GetKeyModifiers()
        {
            var ctrl = Native.GetKeyState(VirtualKey.Control) < 0 ? KeyMod.Control : 0;
            var shift = Native.GetKeyState(VirtualKey.Shift) < 0 ? KeyMod.Shift : 0;
            var alt = Native.GetKeyState(VirtualKey.Alt) < 0 ? KeyMod.Alt : 0;
            return ctrl | shift | alt;
        }

        /// <inheritdoc />
        public override bool IsCapsLockOn()
        {
            return KeyEnabled(VirtualKey.CapsLock);
        }

        /// <inheritdoc />
        public override bool IsNumLockOn()
        {
            return KeyEnabled(VirtualKey.NumLock);
        }

        /// <inheritdoc />
        public override bool IsScrollLockOn()
        {
            return KeyEnabled(VirtualKey.ScrollLock);
        }

        /// <inheritdoc />
        public override MouseState GetMouseState()
        {
            var btns = MouseButtons.None;
            if (Native.GetKeyState(VirtualKey.LButton) < 0)
                btns |= MouseButtons.Left;
            if (Native.GetKeyState(VirtualKey.MButton) < 0)
                btns |= MouseButtons.Middle;
            if (Native.GetKeyState(VirtualKey.RButton) < 0)
                btns |= MouseButtons.Right;
            if (Native.GetKeyState(VirtualKey.XButton1) < 0)
                btns |= MouseButtons.X1;
            if (Native.GetKeyState(VirtualKey.XButton2) < 0)
                btns |= MouseButtons.X2;

            if (!Native.GetCursorPos(out var position))
                throw GetLastException("Failed to get cursor position.");

            return new MouseState(btns, position);
        }

        /// <inheritdoc />
        public override void SetCursorPosition(int x, int y)
        {
            if (!Native.SetCursorPos(x, y))
                throw GetLastException("Failed to set cursor position.");
        }

        /// <inheritdoc />
        public override WindowData GetPlatformData()
        {
            return new Win32WindowData(ModuleHinstance, WindowingService.Get(), this);
        }

        #endregion

        #region Private Methods

        private static uint _windowId;
        private void RegisterNewWindowClass()
        {
            _className = $"OpenWindow[{Native.GetCurrentThreadId()}]({_windowId++})";
            var winClass = new WndClass();
            winClass.lpszClassName = _className;

            var service = (Win32WindowingService) WindowingService.Get();
            winClass.lpfnWndProc = service.WndProc;
            winClass.hInstance = ModuleHinstance;

            winClass.hCursor = Native.LoadCursor(IntPtr.Zero, Cursor.Arrow);

            if (Native.RegisterClass(ref winClass) == 0)
                throw GetLastException("Registering window class failed.");
        }

        private uint GetWindowStyle()
        {
            uint style = 0;

            if (Decorated)
                style |= DefaultWs;
            else
                style |= Constants.WS_POPUP | Constants.WS_SYSMENU;

            if (Resizable)
                style |= Constants.WS_THICKFRAME | Constants.WS_MAXIMIZEBOX;

            return style;
        }

        private void UpdateStyle()
        {
            var ws = GetWindowStyle();
            const int GWL_STYLE = -16;
            Native.SetWindowLong(Handle, GWL_STYLE, ws);
            Native.ShowWindow(Handle, ShowWindowCommand.Show);
        }

        private static Exception GetLastException(string message)
        {
            var e = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            return new OpenWindowException(message, e);
        }

        private bool KeyEnabled(VirtualKey key)
        {
            return (Native.GetKeyState(key) & 0x1) > 0;
        }

        public void TrackMouseLeave()
        {
            var tme = TrackMouseEvent.CreateLeave(Handle);
            if (!Native.TrackMouseEvent(ref tme))
                throw GetLastException("TrackMouseEvent failed.");
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
            return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc />
        protected override void InternalSetVisible(bool value)
        {
            Native.ShowWindow(Handle, value ? ShowWindowCommand.Show: ShowWindowCommand.Hide);
        }

        /// <inheritdoc />
        protected override void InternalMaximize()
        {
            Native.ShowWindow(Handle, ShowWindowCommand.Maximize);
        }

        /// <inheritdoc />
        protected override void InternalMinimize()
        {
            Native.ShowWindow(Handle, ShowWindowCommand.Minimize);
        }

        /// <inheritdoc />
        protected override void InternalRestore()
        {
            Native.ShowWindow(Handle, ShowWindowCommand.Restore);
        }

        /// <inheritdoc />
        protected override void InternalSetTitle(string value)
        {
            if (!Native.SetWindowText(Handle, value))
                throw GetLastException("Failed to set window title.");
        }

        /// <inheritdoc />
        protected override void InternalSetBorderless(bool value)
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
            var s = Size;
            if (s.Width < value.Width || s.Height < value.Height)
                Size = s;
        }

        /// <inheritdoc />
        protected override void InternalSetMaxSize(Size value)
        {
            if (value == Size.Empty)
                return;
            var s = Size;
            if (s.Width > value.Width || s.Height > value.Height)
                Size = s;
        }

        /// <inheritdoc />
        protected override void InternalSetCursorVisible(bool value)
        {
            Native.ShowCursor(value);
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        protected override void ReleaseUnmanagedResources()
        {
            if (!UserManaged)
            {
                Native.UnregisterClass(_className, IntPtr.Zero);
                Native.DestroyWindow(Handle);
            }
        }

        #endregion
    }
}
