using System;
using System.Collections.ObjectModel;

namespace OpenWindow.Backends.X
{
    internal class XWindowingService : WindowingService
    {
        private IntPtr _xcbConnection;
        
        public override ReadOnlyCollection<Display> Displays { get; }
        public override Display PrimaryDisplay { get; }

        public override int WindowCount => throw new NotImplementedException();

        public XWindowingService() : base(WindowingBackend.X)
        {
        }

        protected override void Initialize()
        {
            _xcbConnection = Native.Connect(string.Empty, IntPtr.Zero);
            if (_xcbConnection == IntPtr.Zero)
                throw new OpenWindowException("Failed to connect to the X server.");
        }

        public override WindowingServiceData GetPlatformData()
        {
            throw new NotImplementedException();
        }

        public override void DestroyWindow(Window window)
        {
            throw new NotImplementedException();
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
            Native.Disconnect(_xcbConnection);
        }
    }
}
