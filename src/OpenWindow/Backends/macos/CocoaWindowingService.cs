using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal class CocoaWindowingService : WindowingService
    {
        public override int WindowCount => throw new NotImplementedException();

        public override ReadOnlyCollection<Display> Displays => throw new NotImplementedException();

        public override Display PrimaryDisplay => throw new NotImplementedException();

        public override Window CreateWindow(ref WindowCreateInfo wci)
        {
            throw new NotImplementedException();
        }

        public override void DestroyWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public override KeyMod GetKeyModifiers()
        {
            throw new NotImplementedException();
        }

        public override WindowingServiceData GetPlatformData()
        {
            throw new NotImplementedException();
        }

        public override bool IsCapsLockOn()
        {
            throw new NotImplementedException();
        }

        public override bool IsNumLockOn()
        {
            throw new NotImplementedException();
        }

        public override bool IsScrollLockOn()
        {
            throw new NotImplementedException();
        }

        public override void PumpEvents()
        {
            throw new NotImplementedException();
        }

        public override void SetCursorPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void WaitEvent()
        {
            throw new NotImplementedException();
        }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}