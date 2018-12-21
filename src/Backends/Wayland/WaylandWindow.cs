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
        private readonly IntPtr _eglWindow;
        private readonly IntPtr _eglSurface;

        #endregion

        #region Constructor

        public WaylandWindow(WlSurface wlSurface, XdgSurface xdgSurface, OpenGlSurfaceSettings glSettings)
            : base(false)
        {
            _wlSurface = wlSurface;
            _xdgSurface = xdgSurface;
            _xdgTopLevel = _xdgSurface.GetToplevel();
            _xdgTopLevel.Configure = Configure;
            _xdgTopLevel.SetListener();

            _wlSurface.Enter = SurfaceEnter;
            _wlSurface.Leave = SurfaceLeave;
            _wlSurface.SetListener();

            _eglWindow = Native.EglWindowCreate(_wlSurface.Pointer, 100, 100);
            if (_eglWindow == IntPtr.Zero)
                throw new OpenWindowException("Failed to create EGL window.");

            _eglSurface = CreateEglSurface(glSettings);

            //_wlSurface.Attach(_wlBuffer, 0, 0);
            //_wlSurface.Commit();
        }

        private IntPtr CreateEglSurface(OpenGlSurfaceSettings glSettings)
        {
            return IntPtr.Zero;
        }

        private void Configure(IntPtr data, IntPtr iface, int width, int height, WlArray states)
        {

        }

        private void SurfaceEnter(IntPtr data, IntPtr iface, IntPtr output)
        {
        }

        private void SurfaceLeave(IntPtr data, IntPtr iface, IntPtr output)
        {
        }

        #endregion

        #region Window Properties

        public override Point Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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

        #endregion

        #region Protected Methods

        /// <inheritdoc />
        protected override void InternalSetVisible(bool value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InternalMaximize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InternalMinimize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void InternalRestore()
        {
            throw new NotImplementedException();
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
