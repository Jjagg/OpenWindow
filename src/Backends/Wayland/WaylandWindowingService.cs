// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow.Backends.Wayland
{
    public class WaylandWindowingService : WindowingService
    {
        protected override void Initialize()
        {
        }

        public override Display[] Displays { get; }
        public override Window CreateWindow()
        {
            var window = new WaylandWindow(GlSettings);
            return window;
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}