// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow.Backends.Wayland
{
    internal class WaylandWindow : Window
    {
        #region Private Fields

        private readonly WlSurface _wlSurface;
        private readonly XdgSurface _xdgSurface;
        private readonly XdgToplevel _xdgTopLevel;

        #endregion
        
        #region Constructor

        public WaylandWindow(WlSurface wlSurface, XdgSurface xdgSurface, OpenGlSurfaceSettings glSettings, bool show)
            : base(false)
        {
            _wlSurface = wlSurface;
            _xdgSurface = xdgSurface;
            _xdgTopLevel = _xdgSurface.GetToplevel();

            _wlSurface.Enter = SurfaceEnter;
            _wlSurface.Leave = SurfaceLeave;
            _wlSurface.Commit();
        }

        private void SurfaceEnter(IntPtr data, IntPtr iface, IntPtr output)
        {
        }

        private void SurfaceLeave(IntPtr data, IntPtr iface, IntPtr output)
        {
        }

        #endregion
        
        #region Window Properties
        
        // TODO what to expose here? Is just 1 pointer enough?
        public override IntPtr Handle { get; }
        public override bool Borderless { get; set; }
        public override bool Resizable { get; set; }
        public override bool IsFocused { get; set; }
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
            _xdgTopLevel.Destroy();
            _xdgSurface.Destroy();
            _wlSurface.Destroy();
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
