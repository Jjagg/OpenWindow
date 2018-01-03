// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindow : Window
    {
        #region Private Fields

        private WlSurface _wlSurface;
        private XdgSurface _xdgSurface;
        private XdgToplevel _xdgTopLevel;

        #endregion
        
        #region Constructor

        public WaylandWindow(XdgWmBase xdgWmBase, WlSurface wlSurface, OpenGlSurfaceSettings glSettings, bool show)
            : base(false)
        {
            _wlSurface = wlSurface;
            wlSurface.Enter = SurfaceEnter;
            wlSurface.Leave = SurfaceLeave;
            _xdgSurface = xdgWmBase.GetXdgSurface(wlSurface);
            _xdgTopLevel = _xdgSurface.GetToplevel();
            wlSurface.Commit();
        }

        private void SurfaceEnter(IntPtr data, IntPtr iface, IntPtr output)
        {
        }

        private void SurfaceLeave(IntPtr data, IntPtr iface, IntPtr output)
        {
        }

        #endregion
        
        #region Window Properties
        
        public override Point Position { get; set; }
        public override Size Size { get; set; }
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

        public override MouseState GetMouseState()
        {
            throw new NotImplementedException();
        }

        public override void SetCursorPosition(int x, int y)
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

        protected override void InternalSetCursorVisible(bool value)
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
