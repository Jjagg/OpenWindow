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
