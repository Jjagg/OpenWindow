// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindowingService : WindowingService
    {
        private List<Display> _displays;
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }

        private readonly Dictionary<IntPtr, Display> _pendingDisplays;

        private WlDisplay _wlDisplay;
        private WlRegistry _wlRegistry;
        private XdgWmBase _xdgWmBase;
        private WlCompositor _wlCompositor;
        private WlShm _wlShm;

        internal WaylandWindowingService()
        {
            _pendingDisplays = new Dictionary<IntPtr, Display>();
        }

        protected override void Initialize()
        {
            _wlDisplay = WlDisplay.Connect();
            if (_wlDisplay == null)
                throw new OpenWindowException("Failed to connect to Wayland display.");

            _wlRegistry = _wlDisplay.GetRegistry();
            if (_wlRegistry.IsNullPtr)
                throw new OpenWindowException("Failed to connect to get Wayland registry.");

            _wlRegistry.Global = RegistryGlobal;
            _wlRegistry.GlobalRemove = RegistryGlobalRemove;
            _wlRegistry.SetListener();

            _wlDisplay.Roundtrip();
            _wlDisplay.Roundtrip();

            _wlDisplay.Flush();
        }

        private void RegistryGlobal(IntPtr data, IntPtr registry, uint name, string iface, uint version)
        {
            switch (iface)
            {
                case WlOutput.InterfaceName:
                    var output = _wlRegistry.Bind<WlOutput>(name, WlOutput.Interface);
                    AddDisplay(output);
                    break;
                case WlCompositor.InterfaceName:
                    _wlCompositor = _wlRegistry.Bind<WlCompositor>(name, WlCompositor.Interface);
                    break;
                case WlShm.InterfaceName:
                    _wlShm = _wlRegistry.Bind<WlShm>(name, WlShm.Interface);
                    break;
                case WlSeat.InterfaceName:
                    // TODO
                    break;
                case XdgWmBase.InterfaceName:
                    _xdgWmBase = _wlRegistry.Bind<XdgWmBase>(name, XdgWmBase.Interface);
                    _xdgWmBase.Ping = PingHandler;
                    break;
            }
        }

        private void RegistryGlobalRemove(IntPtr data, IntPtr iface, uint name)
        {
        }

        private void AddDisplay(WlOutput output)
        {
            // keep track of the output and listen for configuration events
            output.Geometry = OutputGeometryHandler;
            output.Mode = OutputModeHandler;
            output.Scale = OutputScaleHandler;
            output.Done = OutputDoneHandler;
            output.SetListener();
            _pendingDisplays.Add(output.Pointer, new Display(output.Pointer));
        }

        private void OutputGeometryHandler(IntPtr data, IntPtr iface, int x, int y, int physicalWidth,
            int physicalHeight, int subpixel, string make, string model, int transform)
        {
            if (_pendingDisplays.TryGetValue(iface, out var display))
            {
                display.Bounds = new Rectangle(x, y, physicalHeight, physicalHeight);
                display.Name = make + " - " + model;
            }
            else
            {
                throw new OpenWindowException("Got an Output Geometry event for unknown output.");
            }
        }

        private void OutputModeHandler(IntPtr data, IntPtr iface, uint flags, int width, int height, int refresh)
        {
            // TODO
        }

        private void OutputScaleHandler(IntPtr data, IntPtr iface, int factor)
        {
            // TODO
        }

        private void OutputDoneHandler(IntPtr data, IntPtr iface)
        {
            if (_pendingDisplays.TryGetValue(iface, out var display))
            {
                _displays.Add(display);
                _pendingDisplays.Remove(iface);
            }
            else
            {
                throw new OpenWindowException("Got an Output Done event for unknown output.");
            }
        }

        private static void PingHandler(IntPtr data, IntPtr iface, uint serial)
        {
            XdgWmBase.Pong(iface, serial);
        }

        public override Display[] Displays { get; }

        public override Window CreateWindow(bool show = true)
        {
            var wlSurface = _wlCompositor.CreateSurface();
            if (wlSurface.IsNullPtr)
                throw new OpenWindowException("Failed to create Wayland surface.");
            var window = new WaylandWindow(_xdgWmBase, wlSurface, GlSettings, show);
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
            _wlDisplay.Dispose();
            WaylandInterfaces.CleanUp();
            XdgShellInterfaces.CleanUp();
        }
    }
}
