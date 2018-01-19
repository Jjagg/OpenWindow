// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindowingService : WindowingService
    {
        protected override void Initialize()
        {
        }

        public override Display[] Displays { get; }
        public override Window CreateWindow(bool show = true)
        {
            var window = new WaylandWindow(GlSettings, show);
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
            WlInterfaces.CleanUp();
        }
    }
}