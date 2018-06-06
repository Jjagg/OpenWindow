// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWindow.Backends.Windows
{
    internal class Win32WindowingService : WindowingService
    {
        internal Dictionary<IntPtr, Display> DisplayDict;
        public override Display[] Displays => DisplayDict.Values.ToArray();

        public Win32WindowingService()
        {
            KeyMap.Create();
            WndProc = ProcessWindowMessage;
        }

        protected override void Initialize()
        {
            // detect connected displays
            DisplayDict = new Dictionary<IntPtr, Display>();
            Native.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate(IntPtr handle, IntPtr hdc, ref Rect rect, IntPtr data)
                {
                    DisplayDict.Add(handle, Util.DisplayFromMonitorHandle(handle));
                    return true;
                }, IntPtr.Zero);
        }

        /// <inheritdoc />
        public override Window CreateWindow(bool show = true)
        {
            var window = new Win32Window(GlSettings, show);
            ManagedWindows.Add(window.Handle, window);
            return window;
        }

        /// <inheritdoc />
        public override Window WindowFromHandle(IntPtr handle)
        {
            return new Win32Window(handle);
        }

        /// <inheritdoc />
        public override void PumpEvents()
        {
            while (Native.PeekMessage(out var nativeMessage, IntPtr.Zero, 0, 0, 1))
            {
                Native.TranslateMessage(ref nativeMessage);
                Native.DispatchMessage(ref nativeMessage);
            }
        }

        /// <inheritdoc />
        public override void WaitEvent()
        {
            Native.GetMessage(out var nativeMessage, IntPtr.Zero, 0, 0);
            Native.TranslateMessage(ref nativeMessage);
            Native.DispatchMessage(ref nativeMessage);
        }

        // we need to keep a reference to the delegate so it is not garbage collected
        public readonly WndProc WndProc;

        private IntPtr ProcessWindowMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (TryGetWindow(hWnd, out var window))
            {
                switch (msg)
                {
                    case WindowMessage.SetFocus:
                        window._focused = true;
                        window.RaiseFocusChanged(true);
                        return IntPtr.Zero;

                    case WindowMessage.KillFocus:
                        window._focused = false;
                        window.RaiseFocusChanged(false);
                        return IntPtr.Zero;

                    case WindowMessage.KeyDown:
                    case WindowMessage.SysKeyDown:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        var key = KeyMap.Map[(int) vk];
                        var c = (char) Native.MapVirtualKey((uint) vk, KeyMapType.VkToChar);
                        var repeats = (int) (lp & 0xFFFF);
                        var repeated = ((lp >> 30) & 1) > 0;
                        window.RaiseKeyDown(key, repeats, repeated, (int) scanCode, c);
                        if (!repeated)
                            window.RaiseKeyPressed(key, (int) scanCode, c);
                        break;
                    }
                    case WindowMessage.KeyUp:
                    case WindowMessage.SysKeyUp:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        var key = KeyMap.Map[(int) vk];
                        var c = (char) Native.MapVirtualKey((uint) vk, KeyMapType.VkToChar);
                        window.RaiseKeyUp(key, (int) scanCode, c);
                        break;
                    }
                    case WindowMessage.Char:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        if (vk != VirtualKey.Escape && vk != VirtualKey.Tab && vk != VirtualKey.Back)
                            window.RaiseTextInput((char) wParam);

                        return IntPtr.Zero;
                    }

                    //case WindowMessage.NcMouseMove:
                    case WindowMessage.MouseMove:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseMoved(p);
                        break;
                    }
                    case WindowMessage.LButtonDown:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseDown(MouseButtons.Left, p);
                        break;
                    }
                    case WindowMessage.LButtonUp:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseUp(MouseButtons.Left, p);
                        break;
                    }
                    case WindowMessage.MButtonDown:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseDown(MouseButtons.Middle, p);
                        break;
                    }
                    case WindowMessage.MButtonUp:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseUp(MouseButtons.Middle, p);
                        break;
                    }
                    case WindowMessage.RButtonDown:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseDown(MouseButtons.Right, p);
                        break;
                    }
                    case WindowMessage.RButtonUp:
                    {
                        var p = MakePoint(lParam);
                        window.RaiseMouseUp(MouseButtons.Right, p);
                        break;
                    }
                    case WindowMessage.XButtonDown:
                    {
                        var p = MakePoint(lParam);
                        var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButtons.X1 : MouseButtons.X2;
                        window.RaiseMouseDown(btn, p);
                        break;
                    }
                    case WindowMessage.XButtonUp:
                    {
                        var p = MakePoint(lParam);
                        var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButtons.X1 : MouseButtons.X2;
                        window.RaiseMouseUp(btn, p);
                        break;
                    }
                    case WindowMessage.MouseLeave:
                        LogDebug("Mouse left!");
                        break;
                    case WindowMessage.MouseWheel:
                        LogDebug("Mouse wheel!");
                        break;

                    case WindowMessage.Close:
                        window.Close();
                        return IntPtr.Zero;

                    case WindowMessage.Destroy:
                        ManagedWindows.Remove(window.Handle);
                        window.RaiseClosing();
                        return IntPtr.Zero;
                }
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private Point MakePoint(IntPtr lparam)
        {
            var lp = lparam.ToInt32();
            var x = lp & 0xff;
            var y = (lp >> 16) & 0xff;
            return new Point(x, y);
        }
    }
}