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
        private readonly IntPtr _eglWindow;
        private readonly IntPtr _eglSurface;

        #endregion

        #region Constructor

        public WaylandWindow(WlCompositor wlCompositor, WlSurface wlSurface, XdgSurface xdgSurface, OpenGlSurfaceSettings glSettings)
            : base(false)
        {
            _wlCompositor = wlCompositor;
            _wlSurface = wlSurface;
            _xdgSurface = xdgSurface;
            _xdgTopLevel = _xdgSurface.GetToplevel();
            _xdgTopLevel.SetListener(Configure, null);

            _wlSurface.SetListener(SurfaceEnter, SurfaceLeave);

            /*_eglWindow = WlEgl.WindowCreate(_wlSurface.Pointer, 100, 100);
            if (_eglWindow == IntPtr.Zero)
                throw new OpenWindowException("Failed to create EGL window.");*/

            //_eglSurface = CreateEglSurface(glSettings);

            //_wlSurface.Attach(_wlBuffer, 0, 0);
            //_wlSurface.Commit();
        }

        private IntPtr CreateEglSurface(OpenGlSurfaceSettings glSettings)
        {
            var eglDisplay = Egl.GetDisplay(0);
            if (eglDisplay == IntPtr.Zero)
                throw new OpenWindowException("Failed to get EGL display.");

            if (!Egl.Initialize(eglDisplay, out var major, out var minor))
                throw new OpenWindowException("Failed to initialize EGL.");

            WindowingService.LogDebug($"EGL intialized with version {major}.{minor}");
            Egl.GetConfigs(eglDisplay, IntPtr.Zero, 0, out var numConfig);

            var configs = new IntPtr[numConfig];
            Egl.GetConfigs(eglDisplay, ref configs[0], numConfig, out numConfig);

            return IntPtr.Zero;
        }

        private void Configure(void* data,  xdg_toplevel* toplevel, int width, int height, wl_array* states)
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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InternalSetResizable(bool value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InternalSetMinSize(Size value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InternalSetMaxSize(Size value)
        {
            throw new NotImplementedException();
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
            _xdgTopLevel.Destroy();
            _xdgSurface.Destroy();
            _wlSurface.Destroy();
        }

        #endregion
    }
}
