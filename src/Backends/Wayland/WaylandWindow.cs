// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindow : Window
    {
        #region Private Fields
        
        #endregion
        
        #region Constructor

        public WaylandWindow(OpenGLWindowSettings glSettings)
        {
            var display = WlDisplay.Connect();
            if (display == null)
                throw CreateException("Failed to create Wayland display.");

            var registry = display.GetRegistry();

            registry.Global = RegistryGlobal;
            registry.GlobalRemove = RegistryGlobalRemove;
            registry.SetListener();

            display.Roundtrip();
            display.Roundtrip();
            display.Flush();
        }

        private void RegistryGlobalRemove(IntPtr data, IntPtr iface, uint name)
        {
        }

        private static void RegistryGlobal(IntPtr data, IntPtr registry, uint name, string iface, uint version)
        {
            switch (iface)
            {
                case WlCompositor.InterfaceName:
                    break;
                case WlShm.InterfaceName:
                    break;
                case WlShell.InterfaceName:
                    break;
                case WlSeat.InterfaceName:
                    break;
            }
        }

        #endregion
        
        #region Window Properties
        
        public override IntPtr Handle { get; }
        public override bool Borderless { get; set; }
        public override bool Resizable { get; set; }
        public override bool IsFocused { get; set; }
        public override Point Position { get; set; }
        public override Point Size { get; set; } public override Rectangle Bounds { get; set; }
        public override Rectangle ClientBounds { get; set; }
        public override string Title { get; set; }
        
        #endregion
        
        #region Window Functions
        
        public override OpenWindow.Display GetContainingDisplay()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override byte[] GetKeyboardState()
        {
            throw new NotImplementedException();
        }

        public override bool IsDown(VirtualKey key)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #region Private Methods

        private void GlobalAddEvent(object data, IntPtr registry, uint id, string iface, uint version)
        {
            
        }

        private void GlobalRemoveEvent(object data, IntPtr registry, uint id)
        {
            
        }
 
        private Exception CreateException(string message)
        {
            return new OpenWindowException(message);
        }       
        
        #endregion
    }
}