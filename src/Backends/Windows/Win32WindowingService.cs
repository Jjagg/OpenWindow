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
        private readonly Dictionary<IntPtr, Window> ManagedWindows;

        public override int WindowCount => ManagedWindows.Count;
        public override ReadOnlyCollection<Display> Displays => new ReadOnlyCollection<Display>(_displays);
        public override Display PrimaryDisplay => _displays.FirstOrDefault(d => d.IsPrimary);

        public Win32WindowingService()
        {
            ManagedWindows = new Dictionary<IntPtr, Window>();
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
            if (ManagedWindows.TryGetValue(hWnd, out var window))
            {
                switch (msg)
                {
                    case WindowMessage.Size:
                    {
                        var wp = wParam.ToInt32();
                        if (wp == 2)
                            window.RaiseMaximized();
                        else if (wp == 1)
                            window.RaiseMinimized();
                        window.RaiseResize();
                        return IntPtr.Zero;
                    }
                    case WindowMessage.EnterSizeMove:
                        window.RaiseResizeStart();
                        break;
                    case WindowMessage.ExitSizeMove:
                        window.RaiseResizeEnd();
                        break;
                    case WindowMessage.GetMinMaxInfo:
                    {
                        Debug.WriteLine("GetMinMaxInfo");
                        if (window.MinSize != Size.Empty)
                        {
                            Marshal.WriteInt32(lParam, 24, window.MinSize.Width);
                            Marshal.WriteInt32(lParam, 28, window.MinSize.Height);
                        }
                        if (window.MaxSize != Size.Empty)
                        {
                            Marshal.WriteInt32(lParam, 32, window.MaxSize.Width);
                            Marshal.WriteInt32(lParam, 36, window.MaxSize.Height);
                        }

                        return IntPtr.Zero;
                    }

                    case WindowMessage.SetFocus:
                        SetFocus(window, true);
                        // keyboard layout might have changed while we didn't have focus
                        UpdateKeyMap();
                        return IntPtr.Zero;
                    case WindowMessage.KillFocus:
                        SetFocus(window, false);
                        return IntPtr.Zero;
                    case WindowMessage.InputLangChange:
                        // TODO pass input locale to keymap update
                        UpdateKeyMap();
                        return IntPtr.Zero;
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
                        SetMouseFocus(window, true);
                        return IntPtr.Zero;
                    }
                    case WindowMessage.LButtonDown:
                        SetMouseButton(MouseButtons.Left, true);
                        return IntPtr.Zero;
                    case WindowMessage.LButtonUp:
                        SetMouseButton(MouseButtons.Left, false);
                        return IntPtr.Zero;
                    case WindowMessage.MButtonDown:
                        SetMouseButton(MouseButtons.Middle, true);
                        return IntPtr.Zero;
                    case WindowMessage.MButtonUp:
                        SetMouseButton(MouseButtons.Middle, false);
                        return IntPtr.Zero;
                    case WindowMessage.RButtonDown:
                        SetMouseButton(MouseButtons.Right, true);
                        return IntPtr.Zero;
                    case WindowMessage.RButtonUp:
                        SetMouseButton(MouseButtons.Right, false);
                        return IntPtr.Zero;
                    case WindowMessage.XButtonDown:
                    {
                        var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButtons.X1 : MouseButtons.X2;
                        SetMouseButton(btn, true);
                        return (IntPtr) 1;
                    }
                    case WindowMessage.XButtonUp:
                    {
                        var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButtons.X1 : MouseButtons.X2;
                        SetMouseButton(btn, false);
                        return (IntPtr) 1;
                    }
                    case WindowMessage.MouseWheel:
                    {
                        // TODO this should be scaled
                        var scroll = GetScroll(wParam);
                        SetMouseScroll(0, scroll);
                        return IntPtr.Zero;
                    }
                    case WindowMessage.MouseHWheel:
                    {
                        // TODO this should be scaled
                        var scroll = GetScroll(wParam);
                        SetMouseScroll(scroll, 0);
                        return IntPtr.Zero;
                    }
                    case WindowMessage.MouseLeave:
                        SetMouseFocus(window, false);
                        return IntPtr.Zero;

                    case WindowMessage.Close:
                        window.Close();
                        return IntPtr.Zero;

                    case WindowMessage.Destroy:
                        var wwindow = (Win32Window) window;
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

        private void ExtractCoords(IntPtr lParam, out int x, out int y)
        {
            var lp = lParam.ToInt32();
            x = lp & 0xff;
            y = (lp >> 16) & 0xff;
        }

        private float GetScroll(IntPtr wParam)
        {
            return ((short) (wParam.ToInt32() >> 16));
        }
    }
}
