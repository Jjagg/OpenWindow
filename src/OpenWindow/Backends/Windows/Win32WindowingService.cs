using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal class Win32WindowingService : WindowingService
    {
        private List<Display> _displays;
        private readonly Dictionary<IntPtr, Win32Window> _managedWindows;
        private IntPtr _localeId;
        private Win32Window _mouseTrackingWindow;

        public override int WindowCount => _managedWindows.Count;
        public override ReadOnlyCollection<Display> Displays => new ReadOnlyCollection<Display>(_displays);
        public override Display PrimaryDisplay => _displays.FirstOrDefault(d => d.IsPrimary);

        // Used to store the focused window handle in case we get the WM_SETFOCUS message
        // when we create a window, before it is registered in _managedWindows.
        private IntPtr _focusedWindowHandle;

        public Win32WindowingService() : base(WindowingBackend.Win32)
        {
            _managedWindows = new Dictionary<IntPtr, Win32Window>();
            _wndProc = ProcessWindowMessage;
        }

        protected override void Initialize()
        {
            UpdateKeyMap();
            InitializeDisplays();
        }

        private void UpdateKeyMap()
        {
            var currentLocaleId = Native.GetKeyboardLayout(0);
            UpdateKeyMap(currentLocaleId);
        }

        private void UpdateKeyMap(IntPtr localeId)
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

        private WindowingServiceData _serviceData = new Win32WindowingServiceData((IntPtr) Native.GetModuleHandle(null));

        public override WindowingServiceData GetPlatformData()
        {
            return _serviceData;
        }

        /// <inheritdoc />
        public override Window CreateWindow(in WindowCreateInfo wci)
        {
            var window = new Win32Window(this, _wndProc, wci);
            _managedWindows.Add(window.Hwnd, window);
            if (_focusedWindowHandle == window.Hwnd)
            {
                SetFocus(window, true);
            }

            return window;
        }

        /// <inheritdoc />
        public override Window CreateHiddenWindow()
            => new Win32Window(GlSettings, _wndProc);

        /// <inheritdoc />
        public override void DestroyWindow(Window window)
        {
            window.Dispose();
            // window will be removed from list of windows when WM_DESTROY is sent
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

        /// <inheritdoc />
        public override KeyMod GetKeyModifiers()
        {
            var ctrl = Native.GetKeyState(VirtualKey.Control) < 0 ? KeyMod.Control : 0;
            var shift = Native.GetKeyState(VirtualKey.Shift) < 0 ? KeyMod.Shift : 0;
            var alt = Native.GetKeyState(VirtualKey.Alt) < 0 ? KeyMod.Alt : 0;
            return ctrl | shift | alt;
        }

        /// <inheritdoc />
        public override bool IsCapsLockOn()
        {
            return KeyEnabled(VirtualKey.CapsLock);
        }

        /// <inheritdoc />
        public override bool IsNumLockOn()
        {
            return KeyEnabled(VirtualKey.NumLock);
        }

        /// <inheritdoc />
        public override bool IsScrollLockOn()
        {
            return KeyEnabled(VirtualKey.ScrollLock);
        }

        private bool KeyEnabled(VirtualKey key)
        {
            return (Native.GetKeyState(key) & 0x1) > 0;
        }

        /// <inheritdoc />
        public override void SetCursorPosition(int x, int y)
        {
            if (!Native.SetCursorPos(x, y))
            {
                var e = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                throw new OpenWindowException("Failed to set cursor position.", e);
            }
        }

        // we need to keep a reference to the delegate so it is not garbage collected
        private readonly WndProc _wndProc;

        private IntPtr ProcessWindowMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            var messageStr = Enum.IsDefined(typeof(WindowMessage), msg) ? msg.ToString() : ((int) msg).ToString("X");
            //LogDebug($"WMessage: {messageStr}");

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

                    case WindowMessage.Activate:
                        break;
                    case WindowMessage.SetFocus:
                        _focusedWindowHandle = hWnd;
                        SetFocus(window, true);
                        // keyboard layout might have changed while we didn't have focus
                        UpdateKeyMap();
                        return IntPtr.Zero;
                    case WindowMessage.KillFocus:
                        if (_focusedWindowHandle == hWnd)
                        {
                            _focusedWindowHandle = IntPtr.Zero;
                        }

                        SetFocus(window, false);
                        return IntPtr.Zero;
                    case WindowMessage.InputLangChange:
                        UpdateKeyMap(lParam);
                        // we don't let this message propagate to child windows because we
                        // handle the locale in WindowingService, not Window
                        return new IntPtr(1);
                    case WindowMessage.KeyDown:
                    case WindowMessage.SysKeyDown:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (ushort) ((lp >> 16) & 0xFF);
                        var extended = ((lp >> 24) & 1) > 0;
                        var repeated = ((lp >> 30) & 1) > 0;

                        if (SetWinKey(scanCode, extended, true) && msg == WindowMessage.SysKeyDown)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }
                    case WindowMessage.KeyUp:
                    case WindowMessage.SysKeyUp:
                    {
                        var lp = lParam.ToInt64();
                        var scanCode = (ushort) ((lp >> 16) & 0xFF);
                        var extended = ((lp >> 24) & 1) > 0;

                        if (SetWinKey(scanCode, extended, false) && msg == WindowMessage.SysKeyDown)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }
                    case WindowMessage.UniChar:
                    {
                        var c = wParam.ToInt32();
                        LogInfo($"UniChar: {c.ToString("X")} ({char.ConvertFromUtf32(c)})");
                        if (c == Constants.UNICODE_NOCHAR)
                            return (IntPtr) 1;

                        // FIXME check what characters to (not) raise for
                        SendCharacter(c);

                        return IntPtr.Zero;
                    }
                    case WindowMessage.Char:
                    {
                        var c = (char) wParam.ToInt32();
                        // FIXME check what characters to (not) raise for
                        if (window.TryGetUtf32(c, out var codepoint))
                        {
                            SendCharacter(codepoint);
                        }

                        return IntPtr.Zero;
                    }
                    //case WindowMessage.NcMouseMove:
                    case WindowMessage.MouseMove:
                    {
                        TrackMouse(window);
                        ExtractCoords(lParam, out var x, out var y);
                        SetMousePosition(x, y);
                        SetMouseFocus(window, true);
                        window.ResetCursor();
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
                        if (_mouseTrackingWindow == window)
                            _mouseTrackingWindow = null;

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
            else
            {
                switch (msg)
                {
                    case WindowMessage.SetFocus:
                        // This message is sent on window creation, before we register the window in _managedWindows.
                        // We store the handle so we can tell if the window we created is the focused window.
                        _focusedWindowHandle = hWnd;
                        break;
                    case WindowMessage.KillFocus:
                        if (_focusedWindowHandle == hWnd)
                        {
                            _focusedWindowHandle = IntPtr.Zero;
                        }

                        break;
                }
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void TrackMouse(Win32Window window)
        {
            if (_mouseTrackingWindow == window)
                return;

            var tme = TrackMouseEvent.CreateLeave(window.Hwnd);
            if (!Native.TrackMouseEvent(tme))
            {
                var e = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                throw new OpenWindowException("TrackMouseEvent failed.", e);
            }

            _mouseTrackingWindow = window;
        }

        private bool SetWinKey(ushort sc, bool extended, bool down)
        {
            var owSc = TranslateWinScanCode(sc, extended);
            SetKey(owSc, down);

            var swallow = false;

            switch (owSc)
            {
                case ScanCode.F10:
                case ScanCode.LeftAlt:
                case ScanCode.RightAlt:
                    swallow = true;
                    break;
            }

            return swallow;
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
            x = lp & 0xffff;
            y = (lp >> 16) & 0xffff;
        }

        private float GetScroll(IntPtr wParam)
        {
            return ((short) (wParam.ToInt32() >> 16));
        }
    }
}
