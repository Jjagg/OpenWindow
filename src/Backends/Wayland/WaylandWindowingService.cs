// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.ObjectModel;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindowingService : WindowingService
    {
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }

        private WlDisplay _wlDisplay;
        private WlRegistry _wlRegistry;
        private XdgWmBase _xdgWmBase;
        private WlCompositor _wlCompositor;
        private WlShm _wlShm;

        protected override void Initialize()
        {
            _wlDisplay = WlDisplay.Connect();
            if (_wlDisplay == null)
                throw new OpenWindowException("Failed to connect to Wayland display.");

            _wlRegistry = _wlDisplay.GetRegistry();

            _wlRegistry.Global = RegistryGlobal;
            _wlRegistry.GlobalRemove = RegistryGlobalRemove;
            _wlRegistry.SetListener();

            _wlDisplay.Roundtrip();
            _wlDisplay.Roundtrip();
        }

        private void RegistryGlobalRemove(IntPtr data, IntPtr iface, uint name)
        {
        }

        private void RegistryGlobal(IntPtr data, IntPtr registry, uint name, string iface, uint version)
        {
            switch (iface)
            {
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
            _wlDisplay.Dispose();
            WaylandInterfaces.CleanUp();
            XdgShellInterfaces.CleanUp();
        }
    }
}
