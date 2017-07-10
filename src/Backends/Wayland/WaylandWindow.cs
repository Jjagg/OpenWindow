// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

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

            var iface = new WlInterface("wl_registry", 1, 1, 2);

            // THIS WORKS
            //var registry = WlProxy.MarshalConstructor(display.Pointer, 1, iface.Pointer, IntPtr.Zero);

            var registry = display.GetRegistry();
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