using System;
using System.Runtime.InteropServices;

namespace OpenWindow
{
    /// <summary>
    /// A wrapper for a native window.
    /// Exposes a unified API to interact with native Windows across platforms.
    /// </summary>
    public abstract class Window : IDisposable
    {
        #region Private Fields

        private bool _isCloseRequested;
        private string _title = string.Empty;
        private bool _decorated;
        private bool _resizable;
        private Size _minSize;
        private Size _maxSize;
        private bool _cursorVisible = true;

        private bool _disposed;

        protected WindowingService Service;

        #endregion

        #region Window API: Properties

        /// <summary>
        /// Get or set arbitrary data associated with this window.
        /// OpenWindow does not touch this property. You can safely use it
        /// to attach any data you want.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// <code>false</code> if the underlying native window was created by OpenWindow with the
        /// <see cref="WindowingService.CreateWindow"/> method, <code>true</code>
        /// if it was created from an existing native window handle with the
        /// <see cref="WindowingService.WindowFromHandle"/> method.
        /// </summary>
        public bool UserManaged { get; }

        /// <summary>
        /// If set to <c>true</c>, a request to close this window has been sent either by calling <see cref="Close"/>
        /// or by the window manager. An application usually wants to monitor this flag, but can freely decide what to
        /// do when it is set.
        ///
        /// When the value of this flag changes from <c>false</c> to <c>true</c>, <see cref="CloseRequested"/> is invoked.
        /// </summary>
        public bool IsCloseRequested => _isCloseRequested;

        /// <summary>
        /// The text that is displayed in the title bar of the window (if it has a title bar).
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                CheckDisposed();
                if (_title != value)
                {
                    _title = value;
                    InternalSetTitle(value);
                }
            }
        }

        /// <summary>
        /// Indicates if this window is decorated, i.e. has a border.
        /// Defaults to true (default has a border).
        /// </summary>
        public bool Decorated
        {
            get => _decorated;
            set
            {
                CheckDisposed();
                if (_decorated != value)
                {
                    _decorated = value;
                    InternalSetDecorated(value);
                }
            }
        }

        /// <summary>
        /// Indicates if users can resize the window.
        /// Defaults to false.
        /// </summary>
        public bool Resizable
        {
            get => _resizable;
            set
            {
                CheckDisposed();
                if (_resizable != value)
                {
                    _resizable = value;
                    InternalSetResizable(value);
                }
            }
        }

        /// <summary>
        /// The minimum allowed size of the window (including border). Set to (0, 0) to not use a minimum size.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   If size is negative or larger than <see cref="MaxSize"/>.
        /// </exception>
        public Size MinSize
        {
            get => _minSize;
            set
            {
                CheckDisposed();
                if (value.Width < 0 || value.Height < 0 || (_maxSize.Width != 0 && value.Width > _maxSize.Width) || (_maxSize.Height != 0 && value.Height > _maxSize.Height))
                    throw new ArgumentOutOfRangeException(nameof(value), "MinSize must be non-negative and smaller than MaxSize!");
                _minSize = value;
                InternalSetMinSize(value);
            }
        }

        /// <summary>
        /// The maximum allowed size of the window (including border). Set to (0, 0) to not use a maximum size.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   If size is negative or smaller than <see cref="MinSize"/>.
        /// </exception>
        public Size MaxSize
        {
            get => _maxSize;
            set
            {
                CheckDisposed();
                if (value.Width < 0 || value.Height < 0 || (value.Width != 0 && value.Width < _minSize.Width) || (value.Height != 0 && value.Height < _minSize.Height))
                    throw new ArgumentOutOfRangeException(nameof(value), "MaxSize must be non-negative and larger than MaxSize!");
                _maxSize = value;
                InternalSetMaxSize(value);
            }
        }

        /// <summary>
        /// Indicates if this window has keyboard focus.
        /// </summary>
        public bool Focused => Service.KeyboardState.FocusedWindow == this;

        /// <summary>
        /// The position of the top left of this window (including border).
        /// </summary>
        public abstract Point Position { get; set; }

        /// <summary>
        /// The size of this window (excluding border).
        /// </summary>
        public abstract Size ClientSize { get; set; }

        /// <summary>
        /// The bounds of this window (excluding border).
        /// </summary>
        public abstract Rectangle ClientBounds { get; set; }

        /// <summary>
        /// Indicates if the mouse cursor is visible.
        /// </summary>
        public bool CursorVisible
        {
            get => _cursorVisible;
            set
            {
                CheckDisposed();
                if (_cursorVisible != value)
                {
                    _cursorVisible = value;
                    InternalSetCursorVisible(value);
                }
            }
        }

        /// <summary>
        /// Surface settings of this window. Not relevant when you do not use OpenGL to render to this window.
        /// <seealso cref="OpenGlSurfaceSettings"/>
        /// <seealso cref="WindowingService.GlSettings"/>
        /// </summary>
        public OpenGlSurfaceSettings GlSettings { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Use this to create a dummy Window.
        /// </summary>
        protected Window()
        {
        }

        /// <summary>
        /// Create a Window.
        /// </summary>
        /// <param name="service">The WindowingService that owns this window.</param>
        /// <param name="userManaged">
        ///   Indicates if this window is created by OpenWindow or if it was created from a handle.
        /// </param>
        /// <param name="wci">Information about the window creation.</param>
        protected Window(WindowingService service, bool userManaged, in WindowCreateInfo wci)
        {
            Service = service;
            UserManaged = userManaged;
            _decorated = wci.Decorated;
            _resizable = wci.Resizable;
            _title = wci.Title;
        }

        #endregion

        #region Window API: Functions

        /// <summary>
        /// Make the window fill the entire display it's on.
        /// </summary>
        public void Maximize()
        {
            CheckDisposed();
            InternalMaximize();
        }

        /// <summary>
        /// Minimize or iconify the window.
        /// </summary>
        public void Minimize()
        {
            CheckDisposed();
            InternalMinimize();
        }

        /// <summary>
        /// Restore the window. If it was minimized or maximized, the original bounds will be restored.
        /// </summary>
        public void Restore()
        {
            CheckDisposed();
            InternalRestore();
        }

        /// <summary>
        /// Get the display that the window is on.
        /// If there are multiple displays containing the window any of them may be returned.
        /// If the window is not visible yet this function may return <c>null</c>.
        /// </summary>
        /// <returns>The display the window is on.</returns>
        public abstract Display GetContainingDisplay();

        /// <summary>
        /// Makes the window borderless and sets the <see cref="ClientBounds"/> to 
        /// the size of the display it is on.
        /// </summary>
        /// <seealso cref="GetContainingDisplay">Used to get the display the window is on.</seealso>
        public void SetFullscreen()
        {
            CheckDisposed();
            Decorated = false;
            ClientBounds = GetContainingDisplay().Bounds;
        }

        /// <summary>
        /// Set the application icon to raw pixel data.
        /// <paramref name="pixelData"/> should be in ARGB8888 format.
        ///
        /// On Windows this sets both the large icon (Alt + Tab icon) and the small icon (window caption icon).
        /// On Wayland this does nothing. Set the icon on Wayland in the .desktop file for your application.
        /// </summary>
        /// <param name="pixelData">Raw pixel data in ARGB8888.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        public void SetIcon<T>(ReadOnlySpan<T> pixelData, int width, int height) where T : struct
        {
            CheckDisposed();
            var byteData = PrepareImageData(pixelData, width, height);
            InternalSetIcon(byteData, width, height);
        }

        /// <summary>
        /// Set the mouse cursor to raw pixel data.
        /// <paramref name="pixelData"/> should be in ARGB8888 format.
        /// </summary>
        /// <param name="pixelData">Raw pixel data in ARGB8888.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="hotspotX">X position relative to the top left of the image that's used for mouse position.</param>
        /// <param name="hotspotY">Y position relative to the top left of the image that's used for mouse position.</param>
        public void SetCursor<T>(ReadOnlySpan<T> pixelData, int width, int height, int hotspotX, int hotspotY) where T : struct
        {
            CheckDisposed();
            var byteData = PrepareImageData(pixelData, width, height);
            InternalSetCursor(byteData, width, height, hotspotX, hotspotY);
        }

        /// <summary>
        /// Sets the <see cref="IsCloseRequested"/> flag to <c>true</c>.
        ///
        /// Note that this in itself does not destroy the window, but typically applications
        /// monitor this flag in their event loop and dispose the window if the application
        /// should really close.
        /// </summary>
        public void Close()
        {
            CheckDisposed();
            _isCloseRequested = true;
            RaiseCloseRequested();
        }

        /// <summary>
        /// Sets the <see cref="IsCloseRequested"/> flag to <c>false</c>.
        ///
        /// Typical usage is in response to <see cref="CloseRequested"/> to cancel a user
        /// closing the window, for example because they have unsaved progress that would be lost.
        /// </summary>
        public void ResetCloseRequested()
        {
            CheckDisposed();
            _isCloseRequested = false;
        }

        /// <summary>
        /// Get an object containing platform-specific information on this window.
        /// </summary>
        public abstract WindowData GetPlatformData();

        #endregion

        #region Window API: Events

        /// <summary>
        /// Invoked when the <see cref="IsCloseRequested"/> flag is set to <c>true</c>.
        /// </summary>
        public event EventHandler<EventArgs> CloseRequested;

        /// <summary>
        /// Invoked right before the window closes.
        /// </summary>
        public event EventHandler<EventArgs> Closing;

        /// <summary>
        /// Invoked when the window is resized.
        /// </summary>
        public event EventHandler<EventArgs> Resize;

        /// <summary>
        /// Invoked when the user starts resizing the window.
        /// </summary>
        public event EventHandler<EventArgs> ResizeStart;

        /// <summary>
        /// Invoked when the user stops resizing the window.
        /// </summary>
        public event EventHandler<EventArgs> ResizeEnd;

        /// <summary>
        /// Invoked when the window is minimized.
        /// </summary>
        public event EventHandler<EventArgs> Minimized;

        /// <summary>
        /// Invoked when the window is minimized.
        /// </summary>
        public event EventHandler<EventArgs> Maximized;

        /// <summary>
        /// Invoked after the window keyboard focus changed.
        /// </summary>
        public event EventHandler<FocusChangedEventArgs> FocusChanged;

        /// <summary>
        /// Invoked after the window mouse focus changed. This means the mouse entered or left the window.
        /// </summary>
        public event EventHandler<FocusChangedEventArgs> MouseFocusChanged;

        /// <summary>
        /// Invoked when a key is pressed down.
        /// <seealso cref="KeyUp"/>
        /// </summary>
        /// <remarks>
        /// For handling text input, use the <see cref="TextInput"/> event.
        /// </remarks>
        public event EventHandler<KeyDownEventArgs> KeyDown;

        /// <summary>
        /// Invoked when a key is released.
        /// <seealso cref="KeyDown"/>
        /// <seealso cref="KeyUp"/>
        /// </summary>
        public event EventHandler<KeyUpEventArgs> KeyUp;

        /// <summary>
        /// Invoked when a keypress happens. Contains the correct character for text input.
        /// </summary>
        public event EventHandler<TextInputEventArgs> TextInput;

        /// <summary>
        /// Invoked when the mouse moves in the client area of the window.
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseMove;

        /// <summary>
        /// Invoked when the mouse wheel is scrolled horizontally or vertically.
        /// </summary>
        public event EventHandler<MouseScrollEventArgs> MouseScroll;

        /// <summary>
        /// Invoked when a mouse button is pressed down.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDown;

        /// <summary>
        /// Invoked when a mouse button is released.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseUp;

        #endregion

        #region Internal Functions

        internal void RaiseCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseResize()
        {
            Resize?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseResizeStart()
        {
            ResizeStart?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseResizeEnd()
        {
            ResizeEnd?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseMinimized()
        {
            Minimized?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseMaximized()
        {
            Maximized?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseFocusChanged(bool newFocus)
        {
            FocusChanged?.Invoke(this, new FocusChangedEventArgs(newFocus));
        }

        internal void RaiseMouseFocusChanged(bool newFocus)
        {
            MouseFocusChanged?.Invoke(this, new FocusChangedEventArgs(newFocus));
        }

        internal void RaiseKeyDown(Key key, ScanCode sc)
        {
            KeyDown?.Invoke(this, new KeyDownEventArgs(key, sc));
        }

        internal void RaiseKeyUp(Key key, ScanCode sc)
        {
            KeyUp?.Invoke(this, new KeyUpEventArgs(key, sc));
        }

        internal void RaiseTextInput(int c)
        {
            TextInput?.Invoke(this, new TextInputEventArgs(c));
        }

        internal void RaiseMouseMoved(int x, int y)
        {
            MouseMove?.Invoke(this, new MouseMoveEventArgs(x, y));
        }

        internal void RaiseMouseScroll(float x, float y)
        {
            MouseScroll?.Invoke(this, new MouseScrollEventArgs(x, y));
        }
        internal void RaiseMouseDown(MouseButtons button, int x, int y)
        {
            MouseDown?.Invoke(this, new MouseEventArgs(button, x, y));
        }

        internal void RaiseMouseUp(MouseButtons button, int x, int y)
        {
            MouseUp?.Invoke(this, new MouseEventArgs(button, x, y));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Maximize the native window.
        /// </summary>
        protected abstract void InternalMaximize();

        /// <summary>
        /// Minimize the native window.
        /// </summary>
        protected abstract void InternalMinimize();

        /// <summary>
        /// Restore the native window.
        /// </summary>
        protected abstract void InternalRestore();

        /// <summary>
        /// Set the title of the native window.
        /// </summary>
        protected abstract void InternalSetTitle(string value);

        /// <summary>
        /// Show or hide the border of the native window.
        /// </summary>
        protected abstract void InternalSetDecorated(bool value);

        /// <summary>
        /// Allow or disallow resizing the native window.
        /// </summary>
        protected abstract void InternalSetResizable(bool value);

        /// <summary>
        /// Set the minimum size of the window.
        /// </summary>
        protected abstract void InternalSetMinSize(Size value);

        /// <summary>
        /// Set the maximum size of the window.
        /// </summary>
        protected abstract void InternalSetMaxSize(Size value);

        /// <summary>
        /// Show or hide the mouse cursor when inside the native windows client bounds.
        /// </summary>
        protected abstract void InternalSetCursorVisible(bool value);

        /// <summary>
        /// Set the application icon.
        /// </summary>
        protected abstract void InternalSetIcon(ReadOnlySpan<byte> pixelData, int width, int height);

        /// <summary>
        /// Set the mouse cursor.
        /// </summary>
        protected abstract void InternalSetCursor(ReadOnlySpan<byte> pixelData, int width, int height, int hotspotX, int hotspotY);

        #endregion

        #region Disposable pattern

        void IDisposable.Dispose()
        {
            Service.DestroyWindow(this);
        }

        /// <summary>
        /// Make sure this window is not disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If this window is disposed.</exception>
        protected void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("Cannot use a destroyed window.");
        }

        /// <summary>
        /// Destroy the window and release unmanaged resources.
        /// </summary>
        internal void Dispose()
        {
            if (_disposed)
                return;

            ReleaseUnmanagedResources();

            _disposed = true;
        }

        /// <summary>
        /// Destroy any unmanaged resources held by this window.
        /// </summary>
        protected virtual void ReleaseUnmanagedResources()
        {
        }

        #endregion

        #region Helper Methods

        private ReadOnlySpan<byte> PrepareImageData<T>(ReadOnlySpan<T> pixelData, int width, int height) where T : struct
        {
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width), "Width must be larger than 0.");
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be larger than 0.");

            var byteLen = width * height * 4;
            var byteData = MemoryMarshal.AsBytes(pixelData);

            if (byteData.Length < byteLen)
            {
                throw new ArgumentException(
                    $"Not enough data to create an image of the specified size. Expected at least {byteLen} bytes of data, but got {byteData.Length}.",
                    nameof(pixelData));
            }

            return byteData;
        }

        #endregion
    }
}
