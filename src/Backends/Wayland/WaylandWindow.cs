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

        public WaylandWindow(OpenGLWindowSettings glSettings, bool show)
            : base(false)
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
        
        public override Point Position { get; set; }
        public override Point Size { get; set; }
        public override Rectangle Bounds { get; set; }
        public override Rectangle ClientBounds { get; set; }

        #endregion
        
        #region Window Functions
        
        public override OpenWindow.Display GetContainingDisplay()
        {
            throw new NotImplementedException();
        }

        public override byte[] GetKeyboardState()
        {
            throw new NotImplementedException();
        }

        public override bool IsDown(Key key)
        {
            throw new NotImplementedException();
        }

        public override KeyMod GetKeyModifiers()
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

        #endregion

        #region Protected Methods

        protected override void InternalSetVisible(bool value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalMaximize()
        {
            throw new NotImplementedException();
        }

        protected override void InternalMinimize()
        {
            throw new NotImplementedException();
        }

        protected override void InternalRestore()
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetTitle(string value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetBorderless(bool value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetResizable(bool value)
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