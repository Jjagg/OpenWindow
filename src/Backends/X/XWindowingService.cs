// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow.Backends.X
{
    internal class XWindowingService : WindowingService
    {
        private IntPtr _xcbConnection;
        
        public override Display[] Displays { get; }
        
        protected override void Initialize()
        {
            _xcbConnection = Native.Connect(string.Empty, IntPtr.Zero);
            if (_xcbConnection == IntPtr.Zero)
                throw new OpenWindowException("Failed to connect to the X server.");
        }

        public override Window CreateWindow()
        {
            return null;
        }

        public override void Update()
        {
        }

        protected override void Dispose(bool disposing)
        {
            Native.Disconnect(_xcbConnection);
        }
    }
}