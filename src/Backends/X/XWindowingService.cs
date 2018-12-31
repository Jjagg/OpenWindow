using System;
using System.Collections.ObjectModel;

namespace OpenWindow.Backends.X
{
    internal class XWindowingService : WindowingService
    {
        private IntPtr _xcbConnection;
        
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }

        protected override void Initialize()
        {
            _xcbConnection = Native.Connect(string.Empty, IntPtr.Zero);
            if (_xcbConnection == IntPtr.Zero)
                throw new OpenWindowException("Failed to connect to the X server.");
        }

        public override Window CreateWindow()
        {
            throw new NotImplementedException();
        }

        public override void PumpEvents()
        {
            throw new NotImplementedException();
        }

        public override void WaitEvent()
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            Native.Disconnect(_xcbConnection);
        }
    }
}
