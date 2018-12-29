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
        private List<Display> _displays;
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }

        private bool _wlShellAvailable;
        public WlDisplay _wlDisplay;
        public WlRegistry _wlRegistry;
        private XdgWmBase _xdgWmBase;
        private WlCompositor _wlCompositor;
        private WlShm _wlShm;
        private readonly List<wl_shm_format> _formats;


        private ZxdgDecorationManagerV1 _xdgDecorationManager;

        internal List<WaylandWindowData.GlobalObject> Globals;

        internal WaylandWindowingService()
        {
            _displays = new List<Display>();
            _formats = new List<wl_shm_format>();
            Globals = new List<WaylandWindowData.GlobalObject>();
        }

        internal WaylandWindowData.GlobalObject[] GetGlobals() => Globals.ToArray();
        internal IntPtr GetDisplayProxy() => (IntPtr) _wlDisplay.Pointer;
        internal IntPtr GetRegistryProxy() => (IntPtr) _wlRegistry.Pointer;

        protected override void Initialize()
        {
            WaylandBindings.Load();
            XdgShellBindings.Load();
            XdgDecorationUnstableV1Bindings.Load();

            LogDebug("Connecting to display...");
            _wlDisplay = WlDisplay.Connect();
            if (_wlDisplay.IsNull)
                throw new OpenWindowException("Failed to connect to Wayland display.");
            _wlDisplay.SetListener(DisplayErrorHandler, null);

            LogDebug("Connected to display.");

            _wlRegistry = _wlDisplay.GetRegistry();

            if (_wlRegistry.IsNull)
                throw new OpenWindowException("Failed to connect to get Wayland registry.");

            LogDebug("Got registry.");
            
            _wlRegistry.SetListener(RegistryGlobal, RegistryGlobalRemove);

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

        private void DisplayErrorHandler(void* data, wl_display* display, uint objectId, uint code, byte* messagePtr)
        {
            var message = Util.Utf8ToString(messagePtr);
            LogError($"Irrecoverable error reported by Wayland server: ({message})");
            // todo get error from enum in iface type
            throw new OpenWindowException($"Irrecoverable error reported by Wayland server: {message}");
        }

        private void RegistryGlobal(void* data, wl_registry* registry, uint name, byte* ifaceUtf8, uint version)
        {
            // TODO we should only bind with the required version
            // TODO we should expose interface names in Utf8 format so we can do a direct compare without marshalling
            var iface = Util.Utf8ToString(ifaceUtf8);
            LogDebug($"Registry global announce for type '{iface}' v{version}.");

            var global = new WaylandWindowData.GlobalObject(iface, name, version);
            Globals.Add(global);

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
                case WaylandBindings.wl_shm_name:
                    LogDebug($"Binding WlShm.");
                    _wlShm = _wlRegistry.Bind<wl_shm>(name, WlShm.Interface, version);
                    _wlShm.SetListener(ShmFormatHandler);
                    break;
                case WaylandBindings.wl_seat_name:
                    // TODO input
                    break;
                case XdgShellBindings.xdg_wm_base_name:
                    LogDebug($"Binding XdgWmBase.");
                    _xdgWmBase = _wlRegistry.Bind<xdg_wm_base>(name, XdgWmBase.Interface, version);
                    _xdgWmBase.SetListener(XdgWmBasePingHandler);
                    break;
                case XdgDecorationUnstableV1Bindings.zxdg_decoration_manager_v1_name:
                    _xdgDecorationManager = _wlRegistry.Bind<zxdg_decoration_manager_v1>(name, ZxdgDecorationManagerV1.Interface, version);
                    break;
            }
        }

        private void RegistryGlobalRemove(void* data, wl_registry* registry, uint name)
        {
        }

        private void AddDisplay(WlOutput output)
        {
            // keep track of the output and listen for configuration events
            output.SetListener(
                OutputGeometryHandler,
                OutputModeHandler,
                OutputDoneHandler,
                OutputScaleHandler);
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


        private void OutputGeometryHandler(void* data, wl_output* output, int x, int y, int physicalWidth,
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

        private void OutputModeHandler(void* data, wl_output* output, wl_output_mode modeEnum, int width, int height, int refresh)
        {
            var display = GetDisplay(output);

            if (display == null)
                throw new OpenWindowException("Got an Output Geometry event for unknown output.");

            if (modeEnum.HasFlag(wl_output_mode.Current))
                display.Bounds = display.Bounds.WithSize(width, height);
            // TODO expose refresh rate of the output? Probably quite nice to have, but should check other platforms for support.
            // TODO supported display modes of the output - should this exist in a pure windowing lib? Seems like this is graphics territory.
        }

        private void OutputScaleHandler(void* data, wl_output* output, int factor)
        {
            // TODO high dpi stuff
        }

        private void OutputDoneHandler(void* data, wl_output* output)
        {
            // we don't really need to handle this explicitly
        }

        private void ShmFormatHandler(void* data, wl_shm* shm, wl_shm_format format)
        {
            LogDebug($"Supported buffer surface format " + format.ToString());
            _formats.Add(format);
        }

        private static void XdgWmBasePingHandler(void* data, xdg_wm_base* wmBase, uint serial)
        {
            XdgShellBindings.xdg_wm_base_pong(wmBase, serial);
        }

        public override Window CreateWindow()
        {
            LogDebug("Creating wl surface");
            var wlSurface = _wlCompositor.CreateSurface();
            if (wlSurface.IsNull)
                throw new OpenWindowException("Failed to create compositor surface.");

            LogDebug("Getting xdg surface");
            var xdgSurface = _xdgWmBase.GetXdgSurface(wlSurface);
            LogDebug("Window ctor");
            var window = new WaylandWindow(_wlCompositor, wlSurface, xdgSurface, _xdgDecorationManager, GlSettings);
            return window;
        }

        public override Window WindowFromHandle(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        public override void PumpEvents()
        {
            _wlDisplay.Roundtrip();
        }

        public override void WaitEvent()
        {
            _wlDisplay.Dispatch();
        }

        protected override void Dispose(bool disposing)
        {
            _xdgDecorationManager.Destroy();
            _wlShm.Destroy();
            _wlCompositor.Destroy();
            _wlRegistry.Destroy();
            _wlDisplay.Disconnect();
            _wlDisplay.Destroy();
            WaylandBindings.Unload();
            XdgShellBindings.Unload();
            XdgDecorationUnstableV1Bindings.Unload();
        }
    }
}
