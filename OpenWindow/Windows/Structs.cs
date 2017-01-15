// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using static OpenWindow.Windows.Enums;

namespace OpenWindow.Windows
{
    internal class Structs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Msg
        {
            public IntPtr hwnd;
            public WindowMessage message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public OwPoint pt;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct WndClass
        {
            public uint style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszClassName;
        }

        public delegate IntPtr WndProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct MonitorInfo
        {
            public int cbSize;
            public Rect rcMonitor;
            public Rect rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public static implicit operator OwRectangle(Rect rect)
            {
                return new OwRectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            }

            public static implicit operator Rect(OwRectangle rect)
            {
                return new Rect
                {
                    Left = rect.Left,
                    Top = rect.Top,
                    Right = rect.Right,
                    Bottom = rect.Bottom
                };
            }
        }
    }
}
