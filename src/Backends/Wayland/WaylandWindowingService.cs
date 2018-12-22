using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindowingService : WindowingService
    {
        private List<Display> _displays;
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }

        private bool _wlShellAvailable;
        private WlDisplay _wlDisplay;
        private WlRegistry _wlRegistry;
        private XdgWmBase _xdgWmBase;
        private WlCompositor _wlCompositor;
        private WlShm _wlShm;
        private readonly List<WlShm.FormatEnum> _formats;

        internal List<WaylandWindowData.GlobalObject> Globals;

        internal WaylandWindowingService()
        {
            _displays = new List<Display>();
            _formats = new List<WlShm.FormatEnum>();
            Globals = new List<WaylandWindowData.GlobalObject>();
        }

        internal IntPtr GetDisplayProxy() => _wlDisplay.Pointer;
        internal IntPtr GetRegistryProxy() => _wlRegistry.Pointer;
        internal WaylandWindowData.GlobalObject[] GetGlobals() => Globals.ToArray();

        protected override void Initialize()
        {
            WaylandBindings.Initialize();
            XdgShellBindings.Initialize();

            LogDebug("Connecting to display...");
            _wlDisplay = WlDisplay.Connect();
            if (_wlDisplay.IsNullPtr)
                throw new OpenWindowException("Failed to connect to Wayland display.");
            _wlDisplay.Error = DisplayErrorHandler;
            _wlDisplay.SetListener();

            LogDebug("Connected to display.");

            _wlRegistry = _wlDisplay.GetRegistry();

            if (_wlRegistry.IsNullPtr)
                throw new OpenWindowException("Failed to connect to get Wayland registry.");

            LogDebug("Got registry.");
            
            _wlRegistry.Global = RegistryGlobal;
            _wlRegistry.GlobalRemove = RegistryGlobalRemove;

            _wlRegistry.SetListener();

            LogDebug("Initiating first display roundtrip.");
            _wlDisplay.Roundtrip();

            LogDebug("Initiating second display roundtrip.");
            _wlDisplay.Roundtrip();

            LogDebug("Skipping Sync.");

            if (_wlCompositor == null)
                throw new OpenWindowException("Server did not advertise a compositor.");
            if (_xdgWmBase == null)
            {
                if (_wlShellAvailable)
                    LogError("Server did not advertise xdg_wm_base, but it advertised a wl_shell. wl_shell is deprecated and not supported by OpenWindow.");
                throw new OpenWindowException("Server did not advertise xdg_wm_base.");
            }
        }

        private void DisplayErrorHandler(IntPtr data, IntPtr iface, IntPtr objectId, uint code, string message)
        {
            LogError($"Irrecoverable error reported by Wayland server: ({message})");
            // todo get error from enum in iface type
            throw new OpenWindowException($"Irrecoverable error reported by Wayland server: {message}");
        }

        private void RegistryGlobal(IntPtr data, IntPtr registry, uint name, string iface, uint version)
        {
            LogDebug($"Registry global announce for type '{iface}' v{version}.");

            var global = new WaylandWindowData.GlobalObject(iface, name, version);
            Globals.Add(global);

            switch (iface)
            {
                case WlShell.InterfaceName:
                    _wlShellAvailable = true;
                    break;
                case WlOutput.InterfaceName:
                    LogDebug($"Binding WlOutput.");
                    var output = new WlOutput(_wlRegistry.Bind(name, WlOutput.Interface, version));
                    AddDisplay(output);
                    LogInfo($"Display connected with id {name}.");
                    break;
                case WlCompositor.InterfaceName:
                    LogDebug($"Binding WlCompositor.");
                    _wlCompositor = new WlCompositor(_wlRegistry.Bind(name, WlCompositor.Interface, version));
                    break;
                case WlShm.InterfaceName:
                    LogDebug($"Binding WlShm.");
                    _wlShm = new WlShm(_wlRegistry.Bind(name, WlShm.Interface, version));
                    _wlShm.Format = ShmFormatHandler;
                    _wlShm.SetListener();
                    break;
                case WlSeat.InterfaceName:
                    // TODO input
                    break;
                case XdgWmBase.InterfaceName:
                    LogDebug($"Binding XdgWmBase.");
                    _xdgWmBase = new XdgWmBase(_wlRegistry.Bind(name, XdgWmBase.Interface, version));
                    _xdgWmBase.Ping = XdgWmBasePingHandler;
                    _xdgWmBase.SetListener();
                    break;
            }
        }

        private void RegistryGlobalRemove(IntPtr data, IntPtr iface, uint name)
        {
            var ifaceStruct = new WlInterface.InterfaceStruct();
            Marshal.PtrToStructure(data, ifaceStruct);
            LogDebug($"Registry global remove for {name} of type '{ifaceStruct.Name}'.");
        }

        private void AddDisplay(WlOutput output)
        {
            // keep track of the output and listen for configuration events
            output.Geometry = OutputGeometryHandler;
            output.Mode = OutputModeHandler;
            output.Scale = OutputScaleHandler;
            output.Done = OutputDoneHandler;
            output.SetListener();
            _displays.Add(new Display(output.Pointer));
        }

        private Display GetDisplay(IntPtr handle)
        {
            Display display = null;
            for (var i = 0; i < _displays.Count; i++)
            {
                if (_displays[i].Handle == handle)
                {
                    display = _displays[i];
                    break;
                }
            }

            return display;
        }


        private void OutputGeometryHandler(IntPtr data, IntPtr iface, int x, int y, int physicalWidth,
            int physicalHeight, WlOutput.SubpixelEnum subpixelEnum, string make, string model, WlOutput.TransformEnum transformEnum)
        {
            var display = GetDisplay(iface);

            if (display == null)
                throw new OpenWindowException("Got an Output Geometry event for unknown output.");

            // TODO check this is in the right coordinate space (unscaled or scaled)
            display.Bounds = display.Bounds.WithPosition(x, y);
            // TODO document how this name is assigned
            display.Name = make + " - " + model;
        }

        private void OutputModeHandler(IntPtr data, IntPtr iface, WlOutput.ModeEnum modeEnum, int width, int height, int refresh)
        {
            var display = GetDisplay(iface);

            if (display == null)
                throw new OpenWindowException("Got an Output Geometry event for unknown output.");

            if (modeEnum.HasFlag(WlOutput.ModeEnum.Current))
                display.Bounds = display.Bounds.WithSize(width, height);
            // TODO expose refresh rate of the output? Probably quite nice to have, but should check other platforms for support.
            // TODO supported display modes of the output - should this exist in a pure windowing lib? Seems like this is graphics territory.
        }

        private void OutputScaleHandler(IntPtr data, IntPtr iface, int factor)
        {
            // TODO high dpi stuff
        }

        private void OutputDoneHandler(IntPtr data, IntPtr iface)
        {
            // we don't really need to handle this explicitly
        }

        private void ShmFormatHandler(IntPtr data, IntPtr iface, WlShm.FormatEnum format)
        {
            LogDebug($"Supported buffer surface format " + format.ToString());
            _formats.Add(format);
        }

        private static void XdgWmBasePingHandler(IntPtr data, IntPtr iface, uint serial)
        {
            XdgWmBase.Pong(iface, serial);
        }

        public override Window CreateWindow()
        {
            LogDebug("Creating wl surface");
            var wlSurface = _wlCompositor.CreateSurface();
            if (wlSurface.IsNullPtr)
                throw new OpenWindowException("Failed to create compositor surface.");

            LogDebug("Getting xdg surface");
            var xdgSurface = _xdgWmBase.GetXdgSurface(wlSurface);
            LogDebug("Window ctor");
            var window = new WaylandWindow(_wlCompositor, wlSurface, xdgSurface, GlSettings);
            return window;
        }

        public override Window WindowFromHandle(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        public override void PumpEvents()
        {
        }

        public override void WaitEvent()
        {
            throw new System.NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            // TODO check what actually needs explicit disposing
            _wlShm?.Destroy();
            _wlCompositor?.Destroy();
            _wlRegistry?.Destroy();
            _wlDisplay.Dispose();
            WaylandBindings.Free();
            XdgShellBindings.Free();
        }
    }
}
