// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using OpenWindow.Common;
using static OpenWindow.Windows.Enums;
using static OpenWindow.Windows.Structs;
using static OpenWindow.Windows.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenWindow.Windows
{
    internal class Win32Window : Window
    {
        #region HINSTANCE

        private static IntPtr _moduleHinstance;

        static Win32Window()
        {
            var module = typeof(OpenWindow).Module;
            _moduleHinstance = Marshal.GetHINSTANCE(module);
        }

        #endregion

        private static Dictionary<IntPtr, Win32Window> _windows = new Dictionary<IntPtr, Win32Window>();

        #region Create

        private static short WindowNumber;

        public static new Win32Window Create(int x, int y, int width, int height)
        {
            var windowName = $"OpenWindow[{Thread.CurrentThread.ManagedThreadId}]({WindowNumber++})";
            var window = new Win32Window();

            var winClass = new WndClass();
            winClass.lpszClassName = windowName;
            winClass.lpfnWndProc = window.WndProcDelegate;
            winClass.hInstance = _moduleHinstance;

            ushort identifier;
            identifier = Native.RegisterClass(ref winClass);
            if (identifier == 0)
                throw GetLastException();

            var rect = (Rect) new Rectangle(x, y, width, height);
            if (!Native.AdjustWindowRect(ref rect, WsCaption, false))
                throw GetLastException();
            var niceRect = (Rectangle) rect;

            var handle = Native.CreateWindowEx(
                WindowStyleEx.None,
                windowName,
                string.Empty,
                WindowStyle.WS_OVERLAPPEDWINDOW,
                niceRect.X,
                niceRect.Y,
                niceRect.Width,
                niceRect.Height,
                IntPtr.Zero,
                IntPtr.Zero,
                _moduleHinstance,
                IntPtr.Zero);

            if (handle == IntPtr.Zero)
                throw GetLastException();

            Native.ShowWindow(handle, ShowWindowCommand.Show);

            window.SetHandle(handle);
            return window;
        }

        #endregion

        #region Private Fields

        private IntPtr _handle;
        private IntPtr hdc;

        private bool _fullscreen;
        private bool _focused;

        #endregion

        #region Constructor

        private Win32Window()
        {
            WndProcDelegate = ProcessWindowMessage;
        }

        private void SetHandle(IntPtr handle)
        {
            _handle = handle;
            _windows.Add(handle, this);
        }

        #endregion

        #region Window Implementation

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
                if (Native.SetActiveWindow(_handle) == IntPtr.Zero)
                    throw GetLastException();
            }
        }

        public override Point Position
        {
            get { return Bounds.Position; }
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, 0, 0, SwpNoSize | SwpNoZOrder))
                    throw GetLastException();
            }
        }

        public override Point Size
        {
            get { return Bounds.Size; }
            set
            {
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, 0, 0, 0, 0, SwpNoMove | SwpNoZOrder))
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
                if (!Native.SetWindowPos(_handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, SwpNoZOrder))
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
                if (!Native.AdjustWindowRect(ref rect, WsCaption, false))
                    throw GetLastException();
                Bounds = rect;
            }
        }

        public override Message GetMessage()
        {
            var code = Native.PeekMessage(out var nativeMessage, IntPtr.Zero, 0, 0, 1);

            if (nativeMessage.message == WindowMessage.Destroy)
                Native.PostQuitMessage(0);

            if (nativeMessage.message != WindowMessage.Quit)
            {
                Native.TranslateMessage(ref nativeMessage);
                Native.DispatchMessage(ref nativeMessage);
            }

            return nativeMessage.ToMessage();
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

        #region Private Helper Methods

        public WndProc WndProcDelegate;

        private IntPtr ProcessWindowMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.Destroy:
                    _windows.Remove(hWnd);
                    if (!_windows.Any())
                        Native.PostQuitMessage(0);
                    break;
                case WindowMessage.Activate:
                    if (_windows.TryGetValue(hWnd, out var window))
                        window._focused = (short)wParam != WaInactive;
                    break;
                case WindowMessage.KeyDown:
                    // TODO
                    break;
                case WindowMessage.KeyUp:
                    // TODO
                    break;
                case WindowMessage.Char:
                    RaiseTextInput((char) wParam);
                    break;
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private static Exception GetLastException()
        {
            return Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        #endregion
    }
}
