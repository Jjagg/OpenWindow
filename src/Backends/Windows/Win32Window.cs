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

        private readonly IntPtr _handle;

        private bool _focused;

        private string _className;
        private string _title = string.Empty;
        private bool _borderless;
        private bool _resizable;

        #endregion

        #region Constructor

        public Win32Window(OpenGLWindowSettings glSettings)
        {
            RegisterNewWindowClass();

            var handle = Native.CreateWindowEx(
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

            if (handle == IntPtr.Zero)
            {
                Native.UnregisterClass(_className, IntPtr.Zero);
                throw GetLastException("Failed to create window.");
            }

            _handle = handle;

            if (glSettings.EnableOpenGl)
            {
                InitOpenGl(glSettings);

                // FIXME: use the actual ms count, not the preferred
                if (glSettings.MultiSampleCount > 1)
                {
                    // we need to recreate the window to have a multisample window
                    Native.DestroyWindow(_handle);
                    _handle = Native.CreateWindowEx(
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
                GlSettings = new OpenGLWindowSettings();

            Native.ShowWindow(_handle, ShowWindowCommand.Normal);
        }

        private void InitOpenGl(OpenGLWindowSettings s)
        {
            var hdc = IntPtr.Zero;
            var hglrc = IntPtr.Zero;
            try
            {
                var pfd = new PixelFormatDescriptor();
                pfd.nSize = (short) Marshal.SizeOf(typeof(PixelFormatDescriptor));
                pfd.nVersion = 1;
                const int PFD_DRAW_TO_WINDOW = 4;
                const int PFD_SUPPORT_OPENGL = 32;
                pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL;

                const int PFD_DOUBLE_BUFFER = 1;
                if (s.DoubleBuffer)
                    pfd.dwFlags |= PFD_DOUBLE_BUFFER;

                const int PFD_TYPE_RGBA = 0;
                pfd.iPixelType = PFD_TYPE_RGBA;

                pfd.cRedBits = (byte) s.RedSize;
                pfd.cGreenBits = (byte) s.GreenSize;
                pfd.cBlueBits = (byte) s.BlueSize;
                pfd.cAlphaBits = (byte) s.AlphaSize;
                pfd.cColorBits = (byte) (s.RedSize + s.GreenSize + s.BlueSize);

                pfd.cDepthBits = (byte) s.DepthSize;
                pfd.cStencilBits = (byte) s.StencilSize;

                hdc = Native.GetDC(_handle);
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
                    WindowingService.Log(GlSettings.MultiSampleCount > 1 ? Logger.Level.Warning : Logger.Level.Info,
                        "wglChoosePixelFormatARB not supported.");

                var ppfd = new PixelFormatDescriptor();
                Native.DescribePixelFormat(hdc, iPixelFormat, (uint) Marshal.SizeOf(typeof(PixelFormatDescriptor)),
                    ref ppfd);

                GlSettings = new OpenGLWindowSettings
                {
                    EnableOpenGl = true,
                    DoubleBuffer = (ppfd.dwFlags & PFD_DOUBLE_BUFFER) != 0,
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
                if (hdc != IntPtr.Zero) Native.ReleaseDC(_handle, hdc);
            }
        }

        #endregion

        #region Window Properties

        public override IntPtr Handle => _handle;

        public override bool Resizable
        {
            get => _resizable;
            set
            {
                _resizable = value;
                UpdateStyle();
            }
        }

        public override bool Borderless
        {
            get => _borderless;
            set
            {
                _borderless = value;
                UpdateStyle();
            }
        }

        public override bool IsFocused
        {
            get => _focused;
            set
            {
                if (value == _focused)
                    return;

                if (Native.SetActiveWindow(_handle) == IntPtr.Zero)
                    throw GetLastException("Failed to focus window.");

                _focused = value;
                RaiseFocusChanged(_focused);
            }
        }

        public override Point Position
        {
            get => Bounds.Position;
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, 0, 0, Constants.SWP_NOSIZE | Constants.SWP_NOZORDER))
                    throw GetLastException("Setting window position failed.");
            }
        }

        public override Point Size
        {
            get => Bounds.Size;
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, 0, 0, value.X, value.Y, Constants.SWP_NOMOVE | Constants.SWP_NOZORDER))
                    throw GetLastException("Setting window position failed.");
            }
        }

        public override Rectangle Bounds
        {
            get
            {
                if (!Native.GetWindowRect(_handle, out var rect))
                    throw GetLastException("Failed to get window bounds.");
                return rect;
            }
            set
            {
                if (Bounds == value)
                    return;
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, Constants.SWP_NOZORDER))
                    throw GetLastException("Failed to set window bounds.");
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                if (!Native.GetClientRect(_handle, out var rect))
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

        public override string Title
        {
            get => _title;
            set
            {
                if (!Native.SetWindowText(_handle, value))
                    throw GetLastException("Failed to set window title.");
                _title = value;
            }
        }

        #endregion

        #region Window Functions

        public override Display GetContainingDisplay()
        {
            var displayHandle = Native.MonitorFromWindow(Handle, Constants.MonitorDefaultToNearest);
            var service = (Win32WindowingService) WindowingService.Get();
            if (!service.DisplayDict.ContainsKey(displayHandle))
                throw new InvalidOperationException("Containing display for a window was not a known display! This should not happen!");
            return service.DisplayDict[displayHandle];
        }

        public override void Close()
        {
            Native.PostMessage(_handle, WindowMessage.Close, IntPtr.Zero, IntPtr.Zero);
        }

        public override byte[] GetKeyboardState()
        {
            var result = new byte[256];
            if (!Native.GetKeyboardState(result))
                throw GetLastException("Getting keyboard state failed.");

            return result;
        }

        public override bool IsDown(VirtualKey key)
        {
            return Native.GetKeyState(key) < 0;
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

            if (Borderless)
                style |= Constants.WS_POPUP | Constants.WS_SYSMENU;
            else
                style |= DefaultWs;

            if (Resizable)
                style |= Constants.WS_THICKFRAME | Constants.WS_MAXIMIZEBOX;

            return style;
        }

        private void UpdateStyle()
        {
            var ws = GetWindowStyle();
            const int GWL_STYLE = -16;
            Native.SetWindowLong(_handle, GWL_STYLE, ws);
            Native.ShowWindow(_handle, ShowWindowCommand.Show);
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
            return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
        }

        #endregion

        #region IDisposable

        protected override void ReleaseUnmanagedResources()
        {
            Native.UnregisterClass(_className, IntPtr.Zero);
        }

        #endregion
    }

    internal delegate void glBlendFuncSeparateDelegate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha,
        uint dfactorAlpha);
}
