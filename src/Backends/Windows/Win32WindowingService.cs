using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal class Win32WindowingService : WindowingService
    {
        private List<Display> _displays;
        public override ReadOnlyCollection<Display> Displays => new ReadOnlyCollection<Display>(_displays);
        public override Display PrimaryDisplay => _displays.FirstOrDefault(d => d.IsPrimary);

        public Win32WindowingService()
        {
            WndProc = ProcessWindowMessage;
        }

        protected override void Initialize()
        {
            UpdateKeyMap();
            InitializeDisplays();
        }

        private void UpdateKeyMap()
        {
            // TODO Set the scan code to virtual key code map based on the keyboard layout
            //      also call this method when keyboard layout changes.
        }

        private void InitializeDisplays()
        {
            _displays = new List<Display>();
            Native.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate(IntPtr handle, IntPtr hdc, ref Rect rect, IntPtr data)
                {
                    _displays.Add(Util.DisplayFromMonitorHandle(handle));
                    return true;
                }, IntPtr.Zero);
        }

        /// <inheritdoc />
        public override Window CreateWindow()
        {
            var window = new Win32Window(GlSettings);
            ManagedWindows.Add(window.Hwnd, window);
            return window;
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
                var wwindow = (Win32Window) window;
                switch (msg)
                {
                    case WindowMessage.SetFocus:
                        wwindow._focused = true;
                        wwindow.RaiseFocusChanged(true);
                        return IntPtr.Zero;

                    case WindowMessage.KillFocus:
                        wwindow._focused = false;
                        wwindow.RaiseFocusChanged(false);
                        return IntPtr.Zero;
                    case WindowMessage.Size:
                    {
                        var wp = wParam.ToInt32();
                        if (wp == 2)
                            wwindow.RaiseMaximized();
                        else if (wp == 1)
                            wwindow.RaiseMinimized();
                        wwindow.RaiseResize();
                        return IntPtr.Zero;
                    }
                    case WindowMessage.EnterSizeMove:
                        wwindow.RaiseResizeStart();
                        break;
                    case WindowMessage.ExitSizeMove:
                        wwindow.RaiseResizeEnd();
                        break;
                    case WindowMessage.GetMinMaxInfo:
                    {
                        Debug.WriteLine("GetMinMaxInfo");
                        if (wwindow.MinSize != Size.Empty)
                        {
                            Marshal.WriteInt32(lParam, 24, wwindow.MinSize.Width);
                            Marshal.WriteInt32(lParam, 28, wwindow.MinSize.Height);
                        }
                        if (wwindow.MaxSize != Size.Empty)
                        {
                            Marshal.WriteInt32(lParam, 32, wwindow.MaxSize.Width);
                            Marshal.WriteInt32(lParam, 36, wwindow.MaxSize.Height);
                        }

                        return IntPtr.Zero;
                    }
                    case WindowMessage.KeyDown:
                    case WindowMessage.SysKeyDown:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        var repeated = ((lp >> 30) & 1) > 0;
                        SetWinKey(scanCode, true);
                        break;
                    }
                    case WindowMessage.KeyUp:
                    case WindowMessage.SysKeyUp:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        SetWinKey(scanCode, false);
                        break;
                    }
                    case WindowMessage.UniChar:
                    {
                        var c = wParam.ToInt32();
                        if (c == Constants.UNICODE_NOCHAR)
                            return (IntPtr) 1;

                        // FIXME check what characters to (not) raise for
                        window.RaiseTextInput(c);

                        return IntPtr.Zero;
                    }
                    case WindowMessage.Char:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        // FIXME check what characters to (not) raise for
                        var vk = Native.MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                        window.RaiseTextInput((char) wParam);

                        return IntPtr.Zero;
                    }

                    //case WindowMessage.NcMouseMove:
                    case WindowMessage.MouseMove:
                    {
                        ExtractCoords(lParam, out var x, out var y);
                        SetMousePosition(x, y);
                        break;
                    }
                    case WindowMessage.LButtonDown:
                    {
                        SetMouseButton(MouseButtons.Left, true);
                        break;
                    }
                    case WindowMessage.LButtonUp:
                    {
                        SetMouseButton(MouseButtons.Left, false);
                        break;
                    }
                    case WindowMessage.MButtonDown:
                    {
                        SetMouseButton(MouseButtons.Middle, true);
                        break;
                    }
                    case WindowMessage.MButtonUp:
                    {
                        SetMouseButton(MouseButtons.Middle, false);
                        break;
                    }
                    case WindowMessage.RButtonDown:
                    {
                        SetMouseButton(MouseButtons.Right, true);
                        break;
                    }
                    case WindowMessage.RButtonUp:
                    {
                        SetMouseButton(MouseButtons.Right, false);
                        break;
                    }
                    case WindowMessage.XButtonDown:
                    {
                        var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButtons.X1 : MouseButtons.X2;
                        SetMouseButton(btn, true);
                        break;
                    }
                    case WindowMessage.XButtonUp:
                    {
                        var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButtons.X1 : MouseButtons.X2;
                        SetMouseButton(btn, false);
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
                        ManagedWindows.Remove(wwindow.Hwnd);
                        window.RaiseClosing();
                        return IntPtr.Zero;
                }
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void SetWinKey(uint sc, bool down)
        {
            var owSc = TranslateWinScanCode(sc);
            SetKey(owSc, down);
        }

        private ScanCode TranslateWinScanCode(uint sc)
        {
            return ScanCode.Unknown;
        }

        private void ExtractCoords(IntPtr lparam, out int x, out int y)
        {
            var lp = lparam.ToInt32();
            x = lp & 0xff;
            y = (lp >> 16) & 0xff;
        }
    }
}
