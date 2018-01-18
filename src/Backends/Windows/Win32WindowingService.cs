// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                        var lp = ParamToInt(lParam);
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        var key = KeyMap.Map[(int) vk];
                        var c = (char) Native.MapVirtualKey((uint) vk, KeyMapType.VkToChar);
                        var repeats = lp & 0xFFFF;
                        window.RaiseKeyDown(key, repeats, (int) scanCode, c);
                        if (repeats == 0)
                            window.RaiseKeyPressed(key, (int) scanCode, c);
                        break;
                    }
                    case WindowMessage.KeyUp:
                    case WindowMessage.SysKeyUp:
                    {
                        var lp = ParamToInt(lParam);
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        var key = KeyMap.Map[(int) vk];
                        var c = (char) Native.MapVirtualKey((uint) vk, KeyMapType.VkToChar);
                        window.RaiseKeyUp(key, (int) scanCode, c);
                        break;
                    }
                    case WindowMessage.Char:
                    {
                        var lp = ParamToInt(lParam);
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        if (vk != VirtualKey.Escape && vk != VirtualKey.Tab && vk != VirtualKey.Back)
                            window.RaiseTextInput((char) wParam);

                        return IntPtr.Zero;
                    }

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

        private int ParamToInt(IntPtr param)
        {
             return (int) (param.ToInt64() & 0xFFFF0000);
        }
    }
}