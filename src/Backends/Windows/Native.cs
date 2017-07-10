// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal static class Native
    {
        #region Messages

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetMessage(out Msg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(out Msg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool TranslateMessage([In] ref Msg lpMsg);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr DispatchMessage([In] ref Msg lpmsg);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void PostQuitMessage(int nExitCode);

        #endregion

        #region Window Class

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort RegisterClass([In] ref WndClass lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterClass([In] string className, [In] IntPtr hinstance);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, WindowMessage uMsg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Window Operations

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
            WindowStyleEx dwExStyle,
            [MarshalAs(UnmanagedType.LPStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPStr)] string lpWindowName,
            uint dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean AdjustWindowRect(ref Rect lpRect, UInt32 dwStyle, bool bMenu);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString = null);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SetWindowLong([In] IntPtr hWnd, [In] int nIndex, [In] uint dwNewLong);

        #endregion

        #region Device Context

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        // TODO IDisposable and cleanup
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        #endregion

        #region Monitor

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr deviceContext, IntPtr clipRect, MonitorEnumDelegate callback, IntPtr data);
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        #endregion

        #region Input

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short GetKeyState(VirtualKey nVirtKey);

        #endregion

        #region gdi

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int ChoosePixelFormat(IntPtr hdc, ref PixelFormatDescriptor ppfd);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PixelFormatDescriptor ppfd);

        [DllImport("gdi32.dll")]
        public static extern int DescribePixelFormat(IntPtr hdc, int iPixelFormat, uint nBytes, ref PixelFormatDescriptor ppfd);

        #endregion

        #region Misc

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetModuleHandle(string name);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hinstance, Cursor cursor);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetSystemMetrics(SystemMetric metric);

        #endregion

    }
}
