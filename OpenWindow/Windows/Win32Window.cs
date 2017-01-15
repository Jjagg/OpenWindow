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
            var windowName = $"OpenWindow{WindowNumber++}";
            var window = new Win32Window();

            var winClass = new WndClass();
            winClass.lpszClassName = windowName;
            winClass.lpfnWndProc = window.WndProcDelegate;
            winClass.hInstance = _moduleHinstance;

            ushort identifier;
            identifier = Native.RegisterClass(ref winClass);
            if (identifier == 0)
                throw GetLastException();

            var handle = Native.CreateWindowEx(
                WindowStyleEx.None,
                windowName,
                string.Empty,
                WindowStyle.WS_OVERLAPPEDWINDOW,
                x,
                y,
                width,
                height,
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

        public override bool IsFullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                if (_fullscreen)
                    return;

                var mHandle = Native.MonitorFromWindow(_handle, MonitorDefaultToNearest);
                if (!Native.GetMonitorInfo(mHandle, out var mInfo))
                    throw GetLastException();
                ClientBounds = mInfo.rcMonitor;
                _fullscreen = true;
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
                if (Native.GetWindowRect(_handle, out var rect))
                    return rect;
                throw GetLastException();
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
                if (Native.GetClientRect(_handle, out var rect))
                    return rect;
                throw GetLastException();
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
            var code = Native.GetMessage(out var nativeMessage, IntPtr.Zero, 0, 0);
            if (code == -1)
                throw GetLastException();

            if (nativeMessage.message == WindowMessage.Destroy)
                Native.PostQuitMessage(0);

            if (code != 0)
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

        #endregion

        #region Private Helper Methods

        public WndProc WndProcDelegate;

        private IntPtr ProcessWindowMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WindowMessage.Destroy)
            {
                _windows.Remove(hWnd);
                if (!_windows.Any())
                    Native.PostQuitMessage(0);
            }
            else if (msg == WindowMessage.Activate)
            {
                if (_windows.TryGetValue(hWnd, out var window))
                    window._focused = wParam.ToInt32() != WaInactive;
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        public static Exception GetLastException()
        {
            return Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        #endregion
    }
}
