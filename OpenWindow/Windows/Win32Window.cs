// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using static OpenWindow.Windows.Enums;
using static OpenWindow.Windows.Structs;
using static OpenWindow.Windows.Constants;
using System.Threading;

namespace OpenWindow.Windows
{
    internal class Win32Window : Window
    {
        #region HINSTANCE

        private static IntPtr _moduleHinstance;

        static Win32Window()
        {
            var module = typeof(Window).Module;
            _moduleHinstance = Marshal.GetHINSTANCE(module);
        }

        #endregion

        #region Private Fields

        private readonly IntPtr _handle;
        private readonly string _className;

        private IntPtr hdc;

        private bool _fullscreen;
        private bool _focused;

        private string _title = string.Empty;

        #endregion

        #region Constructor

        public Win32Window()
        {
            _className = RegisterNewWindowClass();

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
                _moduleHinstance,
                IntPtr.Zero);

            if (handle == IntPtr.Zero)
                throw GetLastException();

            Native.ShowWindow(handle, ShowWindowCommand.Show);

            _handle = handle;
        }

        #endregion

        #region Window Properties

        public override IntPtr Handle => _handle;

        public override bool IsFullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                if (_fullscreen == value)
                    return;
                if (value)
                {
                    var mHandle = Native.MonitorFromWindow(_handle, MonitorDefaultToNearest);
                    MonitorInfo mInfo = new MonitorInfo();
                    mInfo.cbSize = Marshal.SizeOf(typeof(MonitorInfo));
                    if (!Native.GetMonitorInfo(mHandle, ref mInfo))
                        throw GetLastException();
                    ClientBounds = mInfo.rcMonitor;
                    _fullscreen = true;
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
            get { return _focused; }
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

        public override OwPoint Position
        {
            get { return Bounds.Position; }
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, 0, 0, SwpNoSize | SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override OwPoint Size
        {
            get { return Bounds.Size; }
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, 0, 0, 0, 0, SwpNoMove | SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override OwRectangle Bounds
        {
            get
            {
                if (!Native.GetWindowRect(_handle, out var rect))
                    throw GetLastException();
                return rect;
            }
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override OwRectangle ClientBounds
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
                if (!Native.AdjustWindowRect(ref rect, WsCaption, false))
                    throw GetLastException();
                Bounds = rect;
            }
        }

        public override string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (!Native.SetWindowText(_handle, value))
                    throw GetLastException();
                _title = value;
            }
        }

        #endregion

        #region Window Functions
        
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
        private string RegisterNewWindowClass()
        {
            var className = $"OpenWindow[{Thread.CurrentThread.ManagedThreadId}]({_windowId++})";
            var winClass = new WndClass();
            winClass.lpszClassName = className;

            var service = WindowingService.Get() as WindowsWindowingService;
            winClass.lpfnWndProc = service.WndProc;
            winClass.hInstance = _moduleHinstance;
            
            if (Native.RegisterClass(ref winClass) == 0)
                throw GetLastException();

            return className;
        }

        private static Exception GetLastException()
        {
            return Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        #endregion
    }
}
