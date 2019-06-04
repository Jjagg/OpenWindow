using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

using OpenWindow.Backends.Wayland.Managed;

namespace OpenWindow.Backends.Wayland
{
    internal unsafe class WaylandWindowingService : WindowingService
    {
        private static EGLDisplay* _eglDisplay;
        private static int _eglMajor;
        private static int _eglMinor;

        private List<WaylandWindow> _windows;
        private List<Display> _displays;
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }
        public override int WindowCount => _windows.Count;

        private bool _wlShellAvailable;
        public WlDisplay _wlDisplay;
        public WlRegistry _wlRegistry;
        private XdgWmBase _xdgWmBase;
        private WlCompositor _wlCompositor;
        private WlShm _wlShm;
        private WlSeat _wlSeat;
        private WlPointer _wlPointer;
        private WlKeyboard _wlKeyboard;
        private WlTouch _wlTouch;
        private ZxdgDecorationManagerV1 _xdgDecorationManager;
        private WpViewporter _wpViewporter;

        private xkb_context* _xkbContext;
        private xkb_keymap* _xkbKeymap;
        private xkb_state* _xkbState;

        internal List<WaylandGlobal> Globals;

        internal WaylandWindowingService() : base(WindowingBackend.Wayland)
        {
            _windows = new List<WaylandWindow>();
            _displays = new List<Display>();
            Globals = new List<WaylandGlobal>();
        }

        private bool _globalsDirty;
        private WaylandGlobal[] _cachedGlobals;

        protected override void Initialize()
        {
            _xkbContext = XkbCommon.xkb_context_new();
            if (_xkbContext == null)
                throw new OpenWindowException("Failed to create xkbcommon context.");

            LogDebug("Connecting to display...");

            _wlDisplay = WlDisplay.Connect();
            if (_wlDisplay.IsNull)
            {
                var error = WaylandClient.wl_display_get_error(null);
                throw new OpenWindowException($"Failed to connect to Wayland display ({error}).");
            }
            _wlDisplay.SetListener(DisplayErrorCallback, null);

            LogDebug("Connected to display.");

            WaylandBindings.Load();
            XdgShellBindings.Load();
            XdgDecorationUnstableV1Bindings.Load();
            ViewporterBindings.Load();

            _wlRegistry = _wlDisplay.GetRegistry();

            if (_wlRegistry.IsNull)
                throw new OpenWindowException("Failed to get Wayland registry.");

            LogDebug("Got registry.");
            
            _wlRegistry.SetListener(RegistryGlobalCallback, RegistryGlobalRemoveCallback);

            LogDebug("Initiating first display roundtrip.");
            _wlDisplay.Roundtrip();

            LogDebug("Initiating second display roundtrip.");
            _wlDisplay.Roundtrip();

            if (_wlCompositor.IsNull)
                throw new OpenWindowException("Server did not advertise a compositor.");
            if (_xdgWmBase.IsNull)
            {
                if (_wlShellAvailable)
                    LogError("Server did not advertise xdg_wm_base, but it advertised a wl_shell. wl_shell is deprecated and not supported by OpenWindow.");
                throw new OpenWindowException("Server did not advertise xdg_wm_base.");
            }
        }

        private void DisplayErrorCallback(void* data, wl_display* display, uint objectId, uint code, byte* messagePtr)
        {
            var message = Util.Utf8ToString(messagePtr);
            LogError($"Irrecoverable error reported by Wayland server: ({message})");
            // todo get error from enum in iface type
            throw new OpenWindowException($"Irrecoverable error reported by Wayland server: {message}");
        }

        private void RegistryGlobalCallback(void* data, wl_registry* registry, uint name, byte* ifaceUtf8, uint version)
        {
            // TODO we should only bind with the required version
            // TODO we should expose interface names in Utf8 format so we can do a direct compare without marshalling
            var iface = Util.Utf8ToString(ifaceUtf8);
            LogDebug($"Registry global announce for type '{iface}' v{version}.");

            // we store and expose all globals so users can bind whatever they want
            var global = new WaylandGlobal(iface, name, version);
            Globals.Add(global);
            _globalsDirty = true;

            switch (iface)
            {
                case WaylandBindings.wl_shell_name:
                    _wlShellAvailable = true;
                    break;
                case WaylandBindings.wl_output_name:
                    LogDebug($"Binding WlOutput.");
                    var output = _wlRegistry.Bind<wl_output>(name, WlOutput.Interface, version);
                    AddDisplay(output);
                    LogInfo($"Display connected with id {name}.");
                    break;
                case WaylandBindings.wl_compositor_name:
                    LogDebug($"Binding WlCompositor.");
                    _wlCompositor = _wlRegistry.Bind<wl_compositor>(name, WlCompositor.Interface, version);
                    break;
                case WaylandBindings.wl_seat_name:
                    LogDebug($"Binding WlSeat.");
                    _wlSeat = _wlRegistry.Bind<wl_seat>(name, WlSeat.Interface, version);
                    _wlSeat.SetListener(SeatCapabilitiesCallback, SeatNameCallback);
                    break;
                case XdgShellBindings.xdg_wm_base_name:
                    LogDebug($"Binding XdgWmBase.");
                    _xdgWmBase = _wlRegistry.Bind<xdg_wm_base>(name, XdgWmBase.Interface, version);
                    _xdgWmBase.SetListener(XdgWmBasePingCallback);
                    break;
                case XdgDecorationUnstableV1Bindings.zxdg_decoration_manager_v1_name:
                    _xdgDecorationManager = _wlRegistry.Bind<zxdg_decoration_manager_v1>(name, ZxdgDecorationManagerV1.Interface, version);
                    break;
                case ViewporterBindings.wp_viewporter_name:
                    _wpViewporter = _wlRegistry.Bind<wp_viewporter>(name, WpViewporter.Interface, version);
                    break;
            }
        }

        private void RegistryGlobalRemoveCallback(void* data, wl_registry* registry, uint name)
        {
            for (var i = Globals.Count; i >= 0; i--)
            {
                if (Globals[i].Name == name)
                {
                    Globals.RemoveAt(i);
                    _globalsDirty = true;
                    break;
                }
            }
        }

        public override void InitializeOpenGl()
        {
            EnsureEGLInitialized();
        }

        public override WindowingServiceData GetPlatformData()
        {
            if (_globalsDirty)
            {
                _cachedGlobals = Globals.ToArray();
                _globalsDirty = false;
            }

            return new WaylandWindowingServiceData((IntPtr) _wlDisplay.Pointer, (IntPtr) _wlRegistry.Pointer, _cachedGlobals, (IntPtr) _eglDisplay);
        }

        #region Outputs

        private void AddDisplay(WlOutput output)
        {
            // keep track of the output and listen for configuration events
            output.SetListener(
                OutputGeometryCallback,
                OutputModeCallback,
                OutputDoneCallback,
                OutputScaleCallback);
            _displays.Add(new Display((IntPtr) output.Pointer));
        }

        private Display GetDisplay(wl_output* handle)
        {
            Display display = null;
            for (var i = 0; i < _displays.Count; i++)
            {
                if (_displays[i].Handle == (IntPtr) handle)
                {
                    display = _displays[i];
                    break;
                }
            }

            return display;
        }

        private void OutputGeometryCallback(void* data, wl_output* output, int x, int y, int physicalWidth,
            int physicalHeight, wl_output_subpixel subpixelEnum, byte* make, byte* model, wl_output_transform transformEnum)
        {
            var display = GetDisplay(output);

            if (display == null)
                throw new OpenWindowException("Got an Output Geometry event for unknown output.");

            // TODO check this is in the right coordinate space (unscaled or scaled)
            display.Bounds = display.Bounds.WithPosition(x, y);
            // TODO document how this name is assigned
            display.Name = Util.Utf8ToString(make) + " - " + Util.Utf8ToString(model);
        }

        private void OutputModeCallback(void* data, wl_output* output, wl_output_mode modeEnum, int width, int height, int refresh)
        {
            var display = GetDisplay(output);

            if (display == null)
                throw new OpenWindowException("Got an Output Geometry event for unknown output.");

            if (modeEnum.HasFlag(wl_output_mode.Current))
                display.Bounds = display.Bounds.WithSize(width, height);
            // TODO expose refresh rate of the output? Probably quite nice to have, but should check other platforms for support.
            // TODO supported display modes of the output - should this exist in a pure windowing lib? Seems like this is graphics territory.
        }

        private void OutputScaleCallback(void* data, wl_output* output, int factor)
        {
            // TODO high dpi stuff
        }

        private void OutputDoneCallback(void* data, wl_output* output)
        {
            // we don't really need to handle this explicitly
        }

        #endregion

        #region Seat

        private void SeatNameCallback(void* data, wl_seat* proxy, byte* name)
        {
            LogDebug("Got seat name " + Util.Utf8ToString(name));
        }

        private void SeatCapabilitiesCallback(void* data, wl_seat* proxy, wl_seat_capability capabilities)
        {
            var hasKeyboard = capabilities.HasFlag(wl_seat_capability.Keyboard);
            var hasPointer = capabilities.HasFlag(wl_seat_capability.Pointer);
            var hasTouch = capabilities.HasFlag(wl_seat_capability.Touch);

            if (hasKeyboard && _wlKeyboard.IsNull)
            {
                _wlKeyboard = _wlSeat.GetKeyboard();
                LogDebug("Got keyboard.");
                _wlKeyboard.SetListener(KeymapCallback, KeyboardEnterCallback, KeyboardLeaveCallback,
                    KeyboardKeyCallback, KeyboardModifiersCallback, KeyboardRepeatInfoCallback);
            }
            if (hasPointer && _wlPointer.IsNull)
            {
                _wlPointer = _wlSeat.GetPointer();
                LogDebug("Got pointer.");
                _wlPointer.SetListener(PointerEnterCallback, PointerLeaveCallback, PointerMotionCallback,
                    PointerButtonCallback, PointerAxisCallback,
                    PointerFrameCallback, PointerAxisSourceCallback, PointerAxisStopCallback, PointerAxisDiscreteCallback);

            }
            if (hasTouch && _wlTouch.IsNull)
            {
                _wlTouch = _wlSeat.GetTouch();
                LogDebug("Got touch.");
            }

            if (!hasKeyboard && !_wlKeyboard.IsNull)
            {
                _wlKeyboard.FreeListener();
                _wlKeyboard = WlKeyboard.Null;
                LogDebug("Lost keyboard.");
            }
            if (!hasPointer && !_wlPointer.IsNull)
            {
                _wlPointer.FreeListener();
                _wlPointer = WlPointer.Null;
                LogDebug("Lost mouse.");
            }
            if (!hasTouch && !_wlTouch.IsNull)
            {
                _wlTouch.FreeListener();
                _wlTouch = WlTouch.Null;
                LogDebug("Lost touch.");
            }
        }

        #region Keyboard

        private void KeymapCallback(void* data, wl_keyboard* proxy, wl_keyboard_keymap_format format, int fd, uint size)
        {
            if (format != wl_keyboard_keymap_format.XkbV1)
            {
                Libc.close(fd);
                throw new NotImplementedException("Only xkbcommon compatible keymaps are currently supported.");
            }

            var kbdStr = Libc.mmap(null, size, Libc.PROT_READ, Libc.MAP_PRIVATE, fd, 0);
            if (kbdStr == null)
            {
                Libc.close(fd);
                return;
            }

            var newKeymap = XkbCommon.xkb_keymap_new_from_string(_xkbContext, kbdStr, XkbCommon.XKB_KEYMAP_FORMAT_TEXT_V1);

            Libc.munmap(kbdStr, size);
            Libc.close(fd);

            if (newKeymap == null)
                throw new OpenWindowException("Failed to create xkb keymap.");

            var newState = XkbCommon.xkb_state_new(newKeymap);
            if (newState == null)
            {
                XkbCommon.xkb_keymap_unref(newKeymap);
                throw new OpenWindowException("Failed to create xkb state.");
            }

            if (_xkbKeymap != null) XkbCommon.xkb_keymap_unref(_xkbKeymap);
            if (_xkbState != null) XkbCommon.xkb_state_unref(_xkbState);

            _xkbKeymap = newKeymap;
            _xkbState = newState;

            UpdateKeymap();
        }

        private void UpdateKeymap()
        {
            for (var lsc = 0; lsc < WaylandKeyMaps.LinuxToOwScanCode.Length; lsc++)
            {
                var osc = WaylandKeyMaps.LinuxToOwScanCode[lsc];
                if (osc == ScanCode.Unknown)
                    continue;

                var sym = XkbCommon.xkb_state_key_get_one_sym(_xkbState, (uint) (lsc + 8));
                var owk = WaylandKeyMaps.XkbToOwKey(sym);

                if (owk == Key.Unknown)
                    continue;

                _keyboardState.Set(osc, owk);
            }

            _keyboardState.Set(ScanCode.PrintScreen, Key.PrintScreen);
        }

        private void KeyboardEnterCallback(void* data, wl_keyboard* proxy, uint serial, wl_surface* surface, wl_array* keys) => WlSetFocus(surface, true);
        private void KeyboardLeaveCallback(void* data, wl_keyboard* proxy, uint serial, wl_surface* surface) => WlSetFocus(surface, false);

        private void WlSetFocus(wl_surface* surface, bool newFocus)
        {
            var w = GetWindowBySurface(surface);
            if (w != null)
                SetFocus(w, newFocus);
            else
                LogWarning("Could not find window by surface. The window might have been destroyed after the event was sent from the server.");
        }

        private void KeyboardKeyCallback(void* data, wl_keyboard* proxy, uint serial, uint time, uint lsc, wl_keyboard_key_state state)
        {
            if (lsc >= WaylandKeyMaps.LinuxToOwScanCode.Length)
                return;

            var osc = WaylandKeyMaps.LinuxToOwScanCode[lsc];
            SetKey(osc, state == wl_keyboard_key_state.Pressed);

            // this returns zero if the key press generates more than 1 character in UTF32
            var utf32 = XkbCommon.xkb_state_key_get_utf32(_xkbState, lsc + 8);
            if (utf32 != 0)
            {
                SendCharacter(utf32);
            }
            else
            {
                // TODO how large should this be?
                const int strBufSize = 8;
                byte* strBuf = stackalloc byte[strBufSize];

                var size = XkbCommon.xkb_state_key_get_utf8(_xkbState, lsc + 8, strBuf, strBufSize);
                // add the null terminator
                strBuf[size] = 0;

                // We send text in UTF-32 i.e. no more than 32 bits at a time
                var offset = 0;
                while (ReadUtf32FromUtf8(strBuf, ref offset, out utf32))
                    SendCharacter(utf32);
            }
        }

        private bool ReadUtf32FromUtf8(byte* str, ref int offset, out int utf32)
        {
            if (str[offset] == 0)
            {
                utf32 = 0;
                return false;
            }

            if ((str[offset] & 0b1000_0000) == 0)
            {
                utf32 = str[offset] & 0b0111_1111;
                offset++;
            }
            else if ((str[offset] & 0b1110_0000) == 0b1100_0000)
            {
                utf32 = str[offset] & 0b0001_1111 << 6 |
                        str[offset + 1] & 0b0011_1111;
                offset += 2;
            }
            else if ((str[offset] & 0b1111_0000) == 0b1110_0000)
            {
                utf32 = str[offset] & 0b0000_1111 << 12 |
                        str[offset + 1] & 0b0011_1111 << 6 |
                        str[offset + 2] & 0b0011_1111;
                offset += 3;
            }
            else
            {
                utf32 = str[offset] & 0b0000_0111 << 18 |
                        str[offset + 1] & 0b0011_1111 << 12 |
                        str[offset + 2] & 0b0011_1111 << 6 |
                        str[offset + 3] & 0b0011_1111;
                offset += 4;
            }

            return true;
        }

        private void KeyboardModifiersCallback(void* data, wl_keyboard* proxy, uint serial, uint mods_depressed, uint mods_latched, uint mods_locked, uint group)
        {
            XkbCommon.xkb_state_update_mask(_xkbState, mods_depressed, mods_latched, mods_locked, 0, 0, group);
        }

        private void KeyboardRepeatInfoCallback(void* data, wl_keyboard* proxy, int rate, int delay)
        {
        }

        #endregion Keyboard

        #region Pointer

        private void PointerEnterCallback(void* data, wl_pointer* proxy, uint serial, wl_surface* surface, wl_fixed surface_x, wl_fixed surface_y)
        {
            // TODO call set_cursor here
            // TODO check if mouse capture prevents this callback
            WlSetMouseFocus(surface, true);
        }

        private void PointerLeaveCallback(void* data, wl_pointer* proxy, uint serial, wl_surface* surface)
        {
            WlSetMouseFocus(surface, false);
        }

        private void WlSetMouseFocus(wl_surface* surface, bool value)
        {
            var w = GetWindowBySurface(surface);
            if (w == null)
                return;

            SetMouseFocus(w, value);
        }

        private void PointerMotionCallback(void* data, wl_pointer* proxy, uint time, wl_fixed surface_x, wl_fixed surface_y)
        {
            var x = surface_x.ToInt();
            var y = surface_y.ToInt();
            SetMousePosition(x, y);
        }

        private void PointerButtonCallback(void* data, wl_pointer* proxy, uint serial, uint time, uint button, wl_pointer_button_state state)
        {
            // Key codes:
            // https://github.com/torvalds/linux/blob/master/include/uapi/linux/input-event-codes.h
            const uint BTN_LEFT    = 0x110;
            const uint BTN_RIGHT   = 0x111;
            const uint BTN_MIDDLE  = 0x112;
            const uint BTN_SIDE    = 0x113; // X1
            const uint BTN_EXTRA   = 0x114; // X2
            //const uint BTN_FORWARD = 0x115;
            //const uint BTN_BACK    = 0x116;

            var btn = MouseButtons.None;
            switch (button)
            {
                case BTN_LEFT: btn = MouseButtons.Left; break;
                case BTN_RIGHT: btn = MouseButtons.Right; break;
                case BTN_MIDDLE: btn = MouseButtons.Middle; break;
                case BTN_SIDE: btn = MouseButtons.X1; break;
                case BTN_EXTRA: btn = MouseButtons.X2; break;
            }

            if (btn != MouseButtons.None)
                SetMouseButton(btn, state == wl_pointer_button_state.Pressed);
        }

        private void PointerAxisCallback(void* data, wl_pointer* proxy, uint time, wl_pointer_axis axis, wl_fixed value)
        {
            // TODO mouse scroll
        }

        private void PointerFrameCallback(void* data, wl_pointer* proxy)
        {
        }

        private void PointerAxisDiscreteCallback(void* data, wl_pointer* proxy, wl_pointer_axis axis, int discrete)
        {
        }

        private void PointerAxisStopCallback(void* data, wl_pointer* proxy, uint time, wl_pointer_axis axis)
        {
        }

        private void PointerAxisSourceCallback(void* data, wl_pointer* proxy, wl_pointer_axis_source axis_source)
        {
        }

        #endregion Pointer

        #endregion Seat

        private static void XdgWmBasePingCallback(void* data, xdg_wm_base* wmBase, uint serial)
            => XdgShellBindings.xdg_wm_base_pong(wmBase, serial);

        private Window GetWindowBySurface(wl_surface* surface)
        {
            if (surface == null)
                return null;

            for (var i = 0; i < _windows.Count; i++)
            {
                if (_windows[i].Surface.Pointer == surface)
                    return _windows[i];
            }

            return null;
        }

        #region EGL

        public bool EGLInitialized => _eglDisplay != null;

        private void EnsureEGLInitialized()
        {
            if (EGLInitialized)
                return;

            Egl.Load();
            _eglDisplay = Egl.GetDisplay(_wlDisplay.Pointer);
            Egl.Initialize(_eglDisplay, out _eglMajor, out _eglMinor);
            if (_eglMajor == 1 && _eglMinor < 4)
                throw new OpenWindowException("OpenWindow requires at least EGL 1.4 to create a GL capable window on Linux.");
            Egl.BindAPI(Egl.OPENGL_API);

            LogDebug($"Loaded EGL version {_eglMajor}.{_eglMinor}");
        }

        public EGLDisplay* GetEGLDisplay()
        {
            EnsureEGLInitialized();
            return _eglDisplay;
        }

        public void GetEGLVersion(out int major, out int minor)
        {
            EnsureEGLInitialized();
            major = _eglMajor;
            minor = _eglMinor;
        }

        #endregion

        public override Window CreateWindow()
        {
            LogDebug("Creating wl surface");
            var wlSurface = _wlCompositor.CreateSurface();
            if (wlSurface.IsNull)
                throw new OpenWindowException("Failed to create compositor surface.");

            LogDebug("Getting xdg surface");
            var xdgSurface = _xdgWmBase.GetXdgSurface(wlSurface);
            LogDebug("Window ctor");
            var window = new WaylandWindow(this, _wlDisplay, _wlCompositor, wlSurface, xdgSurface, _xdgDecorationManager, _wpViewporter);
            // TODO remove windows
            _windows.Add(window);
            return window;
        }

        /// <inheritdoc />
        public override void PumpEvents()
        {
            _wlDisplay.Flush();
            _wlDisplay.Roundtrip();
        }

        /// <inheritdoc />
        public override void WaitEvent()
        {
            _wlDisplay.Dispatch();
        }

        /// <inheritdoc />
        public override KeyMod GetKeyModifiers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsCapsLockOn()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsNumLockOn()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsScrollLockOn()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void SetCursorPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            _wlSeat.FreeListener();
            _wlShm.FreeListener();
            _wlRegistry.FreeListener();
            _wlDisplay.FreeListener();

            _wlSeat.Release();
            _xdgDecorationManager.Destroy();
            _wlShm.Destroy();
            _wpViewporter.Destroy();
            _wlCompositor.Destroy();
            _wlRegistry.Destroy();
            _wlDisplay.Disconnect();
            _wlDisplay.Destroy();

            WaylandBindings.Unload();
            XdgShellBindings.Unload();
            XdgDecorationUnstableV1Bindings.Unload();
            ViewporterBindings.Unload();
        }
    }
}
