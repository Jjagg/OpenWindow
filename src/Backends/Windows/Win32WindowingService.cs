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
        private readonly Dictionary<IntPtr, Window> _managedWindows;
        private IntPtr _localeId;

        public override int WindowCount => _managedWindows.Count;
        public override ReadOnlyCollection<Display> Displays => new ReadOnlyCollection<Display>(_displays);
        public override Display PrimaryDisplay => _displays.FirstOrDefault(d => d.IsPrimary);

        public Win32WindowingService()
        {
            _managedWindows = new Dictionary<IntPtr, Window>();
            WndProc = ProcessWindowMessage;
        }

        protected override void Initialize()
        {
            SetKeyMap(Native.GetKeyboardLayout(0));
            InitializeDisplays();
        }

        private void SetKeyMap(IntPtr localeId)
        {
            if (_localeId == localeId)
                return;

            _localeId = localeId;

            // We have OpenWindow scancodes and keycodes and Win32 scancodes and keycodes
            // Windows lets us translate scancodes to keycodes and we manage
            // translation from Win32 scancodes to OpenWindow scancodes (and back)
            // and translation from Win32 Virtual Keys to OpenWindow keys.

            // To set up the map from OW scan codes to key codes, we iterate
            // Win32 scancodes and translate to OpenWindow scancodes on the one
            // hand and to OpenWindow keys through Win32 key codes on the other.
            // Some scan codes have different mappings depending on whether they're
            // extended keys or not, so we map the scan codes a second time
            // to catch the extended keys (even though for most keys the result
            // will be the same).

            //         VkToKey
            //  OWK <------------ WinK
            //   ^                 ^
            //   | we build        | MapVirtualKey
            //   | this            |
            //   v                 v
            //  OWS <------------> WinS (we iterate this)
            //       WinToOwScanCode
            for (uint wsc = 0; wsc < Win32KeyMaps.WinToOwScanCode.Length; wsc++)
            {
                var osc = Win32KeyMaps.WinToOwScanCode[wsc];
                if (osc != ScanCode.Unknown)
                {
                    var wvk = Native.MapVirtualKey(wsc, KeyMapType.ScToVkEx);
                    var ovk = Win32KeyMaps.VkToKey[(int) wvk];
                    if (ovk != Key.Unknown)
                        _keyboardState.Set(osc, ovk);
                }
            }

            _keyboardState.Set(ScanCode.Enter, Key.Enter);
            _keyboardState.Set(ScanCode.KpEnter, Key.KpEnter);
            _keyboardState.Set(ScanCode.LeftControl, Key.LeftControl);
            _keyboardState.Set(ScanCode.RightControl, Key.RightControl);
            _keyboardState.Set(ScanCode.LeftShift, Key.LeftShift);
            _keyboardState.Set(ScanCode.RightShift, Key.RightShift);
            _keyboardState.Set(ScanCode.LeftAlt, Key.LeftAlt);
            _keyboardState.Set(ScanCode.RightAlt, Key.RightAlt);

            _keyboardState.Set(ScanCode.Home, Key.Home);
            _keyboardState.Set(ScanCode.Up, Key.Up);
            _keyboardState.Set(ScanCode.PageUp, Key.PageUp);
            _keyboardState.Set(ScanCode.Left, Key.Left);
            _keyboardState.Set(ScanCode.Pause, Key.Pause);
            _keyboardState.Set(ScanCode.Right, Key.Right);
            _keyboardState.Set(ScanCode.End, Key.End);
            _keyboardState.Set(ScanCode.Down, Key.Down);
            _keyboardState.Set(ScanCode.PageDown, Key.PageDown);
            _keyboardState.Set(ScanCode.Insert, Key.Insert);
            _keyboardState.Set(ScanCode.Delete, Key.Delete);

            _keyboardState.Set(ScanCode.PrintScreen, Key.PrintScreen);
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
            _managedWindows.Add(window.Hwnd, window);
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
            if (_managedWindows.TryGetValue(hWnd, out var window))
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
                        SetKeyMap(lParam);
                        return IntPtr.Zero;
                    case WindowMessage.KillFocus:
                        SetFocus(window, false);
                        return IntPtr.Zero;
                    case WindowMessage.InputLangChange:
                        SetKeyMap(lParam);
                        return new IntPtr(1);
                    case WindowMessage.KeyDown:
                    case WindowMessage.SysKeyDown:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (ushort) ((lp >> 16) & 0xFF);
                        var extended = ((lp >> 24) & 1) > 0;
                        var repeated = ((lp >> 30) & 1) > 0;
                        SetWinKey(scanCode, extended, true);
                        break;
                    }
                    case WindowMessage.KeyUp:
                    case WindowMessage.SysKeyUp:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (ushort) ((lp >> 16) & 0xFF);
                        var extended = ((lp >> 24) & 1) > 0;
                        SetWinKey(scanCode, extended, false);
                        break;
                    }
                    case WindowMessage.UniChar:
                    {
                        var c = wParam.ToInt32();
                        if (c == Constants.UNICODE_NOCHAR)
                            return (IntPtr) 1;

                        // FIXME check what characters to (not) raise for
                        SendCharacter(c);

                        return IntPtr.Zero;
                    }
                    case WindowMessage.Char:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (uint) ((lp >> 16) & 0xFF);
                        // FIXME check what characters to (not) raise for
                        SendCharacter(wParam.ToInt32());

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
                        _managedWindows.Remove(wwindow.Hwnd);
                        window.RaiseClosing();
                        return IntPtr.Zero;
                }
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void SetWinKey(ushort sc, bool extended, bool down)
        {
            var owSc = TranslateWinScanCode(sc, extended);
            SetKey(owSc, down);
        }

        private ScanCode TranslateWinScanCode(uint wsc, bool extended)
        {
            var sc = ScanCode.Unknown;

            if (wsc < Win32KeyMaps.WinToOwScanCode.Length)
            {
                sc = extended ?
                    Win32KeyMaps.WinToOwScanCodeEx[wsc] :
                    Win32KeyMaps.WinToOwScanCode[wsc];
            }

            return sc;
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
