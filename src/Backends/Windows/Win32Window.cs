// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal class Win32Window : Window
    {
        #region HINSTANCE

        private static readonly IntPtr ModuleHinstance = new IntPtr(Native.GetModuleHandle(null));

        #endregion

        #region Private Fields

        private readonly IntPtr _handle;

        private bool _fullscreen;
        private bool _focused;

        private string _className;
        private string _title = string.Empty;

        #endregion

        #region Constructor

        public Win32Window(OpenGLWindowSettings glSettings)
        {
            RegisterNewWindowClass();

            var handle = Native.CreateWindowEx(
                WindowStyleEx.None,
                _className,
                string.Empty,
                WindowStyle.WS_OVERLAPPEDWINDOW,
                0,
                0,
                0,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                ModuleHinstance,
                IntPtr.Zero);

            if (handle == IntPtr.Zero)
            {
                Native.UnregisterClass(_className, IntPtr.Zero);
                throw GetLastException();
            }

            Native.ShowWindow(handle, ShowWindowCommand.Show);

            _handle = handle;

            if (glSettings.EnableOpenGl)
                InitOpenGl(glSettings);
        }

        private void InitOpenGl(OpenGLWindowSettings s)
        {
            var hdc = GetDeviceContext();

            var pfd = new PixelFormatDescriptor();
            pfd.nSize = (short) Marshal.SizeOf<PixelFormatDescriptor>();
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

            var iPixelFormat = Native.ChoosePixelFormat(hdc, ref pfd);
            Native.SetPixelFormat(hdc, iPixelFormat, ref pfd);

            var ppfd = new PixelFormatDescriptor();
            Native.DescribePixelFormat(hdc, iPixelFormat, (uint) Marshal.SizeOf<PixelFormatDescriptor>(), ref ppfd);

            ReleaseDeviceContext(hdc);

            GlSettings = new OpenGLWindowSettings
            {
                EnableOpenGl = true,
                DoubleBuffer = (ppfd.dwFlags & PFD_DOUBLE_BUFFER) != 0,
                RedSize = pfd.cRedBits,
                GreenSize = pfd.cGreenBits,
                BlueSize = pfd.cBlueBits,
                AlphaSize = pfd.cAlphaBits,
                DepthSize = pfd.cDepthBits,
                StencilSize = pfd.cStencilBits
            };

            // TODO: Check for WGL_ARB_pixel_format to set MSAA if possible
        }

        #endregion

        #region Window Properties

        public override IntPtr Handle => _handle;

        public override bool IsFullscreen
        {
            get => _fullscreen;
            set
            {
                if (_fullscreen == value)
                    return;
                if (value)
                {
                    var mHandle = Native.MonitorFromWindow(_handle, Constants.MonitorDefaultToNearest);
                    MonitorInfo mInfo = new MonitorInfo();
                    mInfo.cbSize = Marshal.SizeOf<MonitorInfo>();
                    if (!Native.GetMonitorInfo(mHandle, ref mInfo))
                        throw GetLastException();
                    ClientBounds = mInfo.monitorRect;
                    _fullscreen = true;
                    // TODO
                }
                else
                {
                    // TODO
                    _fullscreen = false;
                }
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
                    throw GetLastException();

                _focused = value;
                RaiseFocusChanged(_focused);
            }
        }

        public override Point Position
        {
            get => Bounds.Position;
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, 0, 0, Constants.SwpNoSize | Constants.SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override Point Size
        {
            get => Bounds.Size;
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, 0, 0, value.X, value.Y, Constants.SwpNoMove | Constants.SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override Rectangle Bounds
        {
            get
            {
                if (!Native.GetWindowRect(_handle, out var rect))
                    throw GetLastException();
                return rect;
            }
            set
            {
                if (Bounds == value)
                    return;
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, Constants.SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                if (!Native.GetClientRect(_handle, out var rect))
                    throw GetLastException();
                return rect;
            }
            set
            {
                Rect rect = value;
                if (!Native.AdjustWindowRect(ref rect, Constants.WsCaption, false))
                    throw GetLastException();
                Bounds = rect;
            }
        }

        public override string Title
        {
            get => _title;
            set
            {
                if (!Native.SetWindowText(_handle, value))
                    throw GetLastException();
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
                throw GetLastException();

            return result;
        }

        public override bool IsDown(VirtualKey key)
        {
            return Native.GetKeyState(key) < 0;
        }

        public override IntPtr GetDeviceContext()
        {
            var hdc = Native.GetDC(_handle);
            if (hdc == IntPtr.Zero)
                throw GetLastException();
            return hdc;
        }

        public override void ReleaseDeviceContext(IntPtr deviceContext)
        {
            if (!Native.ReleaseDC(_handle, deviceContext))
                throw GetLastException();
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
                throw GetLastException();
        }

        private static Exception GetLastException()
        {
            return Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        #endregion

        protected override void ReleaseUnmanagedResources()
        {
            Native.UnregisterClass(_className, IntPtr.Zero);
        }
    }
}
