// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow.Backends.Windows
{
    internal class WindowsWindowingService : WindowingService
    {

        public WindowsWindowingService()
        {
            WndProc = ProcessWindowMessage;
        }

        public override Window CreateWindow()
        {
            var window = new Win32Window();

            window.Closing += HandleClosing;
            ManagedWindows.Add(window.Handle, window);

            return window;
        }

        private void HandleClosing(object sender, EventArgs args)
        {
            var window = sender as Window;
            ManagedWindows.Remove(window.Handle);
        }

        public override void Update()
        {
            while (Native.PeekMessage(out var nativeMessage, IntPtr.Zero, 0, 0, 1))
            {
                Native.TranslateMessage(ref nativeMessage);
                Native.DispatchMessage(ref nativeMessage);
            }
        }
        
        // we need to keep a reference to the delegate so it is not garbage collected
        public WndProc WndProc;

        private IntPtr ProcessWindowMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            if (TryGetWindow(hWnd, out var window))
            {
                switch (msg)
                {
                    case WindowMessage.Activate:
                        window.IsFocused = (short) wParam != Constants.WaInactive;
                        break;
                    case WindowMessage.KeyDown:
                        // TODO
                        break;
                    case WindowMessage.KeyUp:
                        // TODO
                        break;
                    case WindowMessage.Char:
                        window.RaiseTextInput((char) wParam);
                        break;
                    case WindowMessage.Destroy:
                        window.RaiseClosing();
                        break;
                }
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }
    }
}
