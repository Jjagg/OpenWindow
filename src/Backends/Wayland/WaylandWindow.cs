using System;
using System.Runtime.InteropServices;
using OpenWindow.Backends.Wayland.Managed;

namespace OpenWindow.Backends.Wayland
{
    internal unsafe class WaylandWindow : Window
    {
        #region Private Fields

        private readonly WlCompositor _wlCompositor;
        private readonly WlSurface _wlSurface;
        private readonly XdgSurface _xdgSurface;
        private readonly XdgToplevel _xdgTopLevel;
        private readonly ZxdgToplevelDecorationV1 _xdgDecoration;

        #endregion

        #region Constructor

        public WaylandWindow(WlCompositor wlCompositor, WlSurface wlSurface, XdgSurface xdgSurface, ZxdgDecorationManagerV1 xdgDecorationManager, OpenGlSurfaceSettings glSettings)
            : base(false)
        {
            _wlCompositor = wlCompositor;
            _wlSurface = wlSurface;
            _wlSurface.SetListener(SurfaceEnter, SurfaceLeave);
            _xdgSurface = xdgSurface;
            _xdgSurface.SetListener(ConfigureSurfaceCallback);
            _xdgTopLevel = _xdgSurface.GetToplevel();
            _xdgTopLevel.SetListener(ConfigureTopLevelCallback, null);
            if (!xdgDecorationManager.IsNull)
                _xdgDecoration = xdgDecorationManager.GetToplevelDecoration(_xdgTopLevel);
        }

        private void ConfigureSurfaceCallback(void* data, xdg_surface* proxy, uint serial)
        {
            _xdgSurface.AckConfigure(serial);
        }

        private void ConfigureTopLevelCallback(void* data,  xdg_toplevel* toplevel, int width, int height, wl_array* states)
        {
            RaiseResize();
        }

        private void SurfaceEnter(void* data, wl_surface* surface, wl_output* output)
        {
        }

        private void SurfaceLeave(void* data, wl_surface* surface, wl_output* output)
        {
        }

        #endregion

        #region Window Properties

        // setting global position is not supported in Wayland
        // TODO need to expose popup interface to position window relative to parent
        public override Point Position { get => Point.Zero; set { } }
        public override Size Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Size ClientSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Rectangle Bounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Rectangle ClientBounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        #region Window Functions

        /// <inheritdoc />
        public override Display GetContainingDisplay()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsDown(Key key)
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
        public override MouseState GetMouseState()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void SetCursorPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override WindowData GetPlatformData()
        {
            var ws = (WaylandWindowingService) WindowingService.Get();
            return new WaylandWindowData(ws.GetDisplayProxy(), ws.GetRegistryProxy(), (IntPtr) _wlSurface.Pointer, ws.GetGlobals());
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc />
        protected override void InternalSetVisible(bool value)
        {
            if (!value)
                WaylandWindowingService.LogWarning(
                    "Showing/hiding the window is not supported on Wayland. " +
                    "Wayland windows are hidden when they don't have a mapped surface. " +
                    "We do not destroy the surface because it requires users to recreate the EGL or vk surface.");
        }

        /// <inheritdoc />
        protected override void InternalMaximize()
        {
            _xdgTopLevel.SetMaximized();
        }

        /// <inheritdoc />
        protected override void InternalMinimize()
        {
            _xdgTopLevel.SetMinimized();
        }

        /// <inheritdoc />
        protected override void InternalRestore()
        {
            _xdgTopLevel.UnsetMaximized();
        }

        /// <inheritdoc />
        protected override void InternalSetTitle(string value)
        {
            _xdgTopLevel.SetTitle(value);
        }

        /// <inheritdoc />
        protected override void InternalSetBorderless(bool value)
        {
            // TODO client side border fallback?
            if (!_xdgDecoration.IsNull)
                _xdgDecoration.SetMode(value ? zxdg_toplevel_decoration_v1_mode.ClientSide : zxdg_toplevel_decoration_v1_mode.ServerSide);
            else if (!value)
                WindowingService.LogWarning("Border enabled but Wayland compositor does not support server side decoration.");
        }

        /// <inheritdoc />
        protected override void InternalSetResizable(bool value)
        {
            // nothing to do, use _resizable to check if resizing is allowed
            // TODO client side border resizing
        }

        /// <inheritdoc />
        protected override void InternalSetMinSize(Size value)
        {
            _xdgTopLevel.SetMinSize(value.Width, value.Height);
        }

        /// <inheritdoc />
        protected override void InternalSetMaxSize(Size value)
        {
            _xdgTopLevel.SetMaxSize(value.Width, value.Height);
        }

        /// <inheritdoc />
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

        #region IDisposable

        protected override void ReleaseUnmanagedResources()
        {
            _xdgDecoration.Destroy();
            _xdgTopLevel.Destroy();
            _xdgSurface.Destroy();
            _wlSurface.Destroy();
        }

        #endregion
    }
}
