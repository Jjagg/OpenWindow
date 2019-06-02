using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenWindow.Backends.Windows;

namespace OpenWindow
{
    /// <summary>
    /// Singleton object used to create and manage <see cref="Window"/> instances.
    /// </summary>
    public abstract class WindowingService : IDisposable
    {
        #region Fields

        protected readonly KeyboardState _keyboardState;
        protected readonly MouseState _mouseState;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a <see cref="WindowingService"/>.
        /// </summary>
        protected WindowingService()
        {
            _keyboardState = new KeyboardState();
            _mouseState = new MouseState();

            GlSettings = new OpenGlSurfaceSettings();
        }

        #endregion

        #region Singleton

        private static WindowingService _instance;

        /// <summary>
        /// Get the <see cref="WindowingService"/> instance.
        /// </summary>
        /// <returns>The <see cref="WindowingService"/> instance.</returns>
        public static WindowingService Get()
        {
            if (_instance == null)
                InitializeInstance();

            return _instance;
        }

        private static void InitializeInstance()
        {
            if (_instance != null)
                return;

            Backend = GetWindowingBackend();
            LogInfo($"Detected windowing backend '{Backend}'.");
            _instance = CreateService(Backend);
            LogDebug("Created WindowingService.");
            _instance.Initialize();
            LogDebug("Initialized WindowingService.");
        }

        /// <summary>
        /// Initialize this <see cref="WindowingService"/>.
        /// </summary>
        protected abstract void Initialize();

        private static WindowingBackend GetWindowingBackend()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WindowingBackend.Win32;

            // LINUX
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")))
                    return WindowingBackend.Wayland;
                if (Environment.GetEnvironmentVariable("XDG_SESSION_TYPE").Contains("x11") || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISPLAY")))
                    return WindowingBackend.X;
                // TODO check if above is reliable enough, we can always just try both
                throw new OpenWindowException("Failed to detect if x11 or Wayland is used." +
                                              "Please open an issue on the OpenWindow repo for this.");
            }

            // OSX
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                throw new NotImplementedException("No OSX back end is implemented yet.");

            throw new NotSupportedException("OS was not reported to be Windows, Linux or OSX by .NET host.");
        }

        private static WindowingService CreateService(WindowingBackend type)
        {
            switch (type)
            {
                case WindowingBackend.Win32:
                    return new Backends.Windows.Win32WindowingService();
                case WindowingBackend.X:
                    return new Backends.X.XWindowingService();
                case WindowingBackend.Wayland:
                    return new Backends.Wayland.WaylandWindowingService();
                case WindowingBackend.Quartz:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region Logging

        /// <summary>
        /// Log a message with log level <see cref="OpenWindow.Logger.Level.Debug"/>.
        /// Only included when the DEBUG symbol is set.
        /// </summary>
        /// <param name="message">Message to log.</param>
        [Conditional("DEBUG")]
        public static void LogDebug(string message)
        {
            Log(Logger.Level.Debug, message);
        }

        /// <summary>
        /// Log a message with log level <see cref="OpenWindow.Logger.Level.Info"/>.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogInfo(string message)
        {
            Log(Logger.Level.Info, message);
        }

        /// <summary>
        /// Log a message with log level <see cref="OpenWindow.Logger.Level.Warning"/>.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(string message)
        {
            Log(Logger.Level.Warning, message);
        }

        /// <summary>
        /// Log a message with log level <see cref="OpenWindow.Logger.Level.Error"/>.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogError(string message)
        {
            Log(Logger.Level.Error, message);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message.</param>
        private static void Log(Logger.Level level, string message)
        {
            Logger.Log(level, message);
        }

        #endregion

        #region Public API

        /// <summary>
        /// The current state of the mouse for this window.
        /// </summary>
        public KeyboardState KeyboardState => _keyboardState;

        /// <summary>
        /// The current state of the mouse for this window.
        /// </summary>
        public MouseState MouseState => _mouseState;

        /// <summary>
        /// The <see cref="WindowingBackend"/> that this service uses.
        /// </summary>
        public static WindowingBackend Backend { get; private set; }

        /// <summary>
        /// Provides logged messages.
        /// </summary>
        public static Logger Logger { get; } = new Logger();

        /// <summary>
        /// The number of windows managed by this service.
        /// </summary>
        public abstract int WindowCount {get; }

        /// <summary>
        /// The settings to use for an OpenGL window. You only need to touch this when using OpenGL for rendering.
        /// Value at the time of calling <see cref="CreateWindow"/> is used for that window.
        /// </summary>
        public OpenGlSurfaceSettings GlSettings { get; }

        /// <summary>
        /// Get the connected displays.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyCollection{T}" /> containing connected displays.</returns>
        public abstract ReadOnlyCollection<Display> Displays { get; }

        /// <summary>
        /// Get the primary display or <code>null</code> if the primary display could not be found.
        /// </summary>
        public abstract Display PrimaryDisplay { get; }

        /// <summary>
        /// Create a new <see cref="Window"/>.
        /// </summary>
        /// <returns>A new <see cref="Window"/>.</returns>
        // TODO add width and height in here and maybe minimized/maximized/fullscreen options
        public abstract Window CreateWindow();

        /// <summary>
        /// Create a <see cref="Window"/> given the handle from an existing Win32 Window.
        /// </summary>
        /// <param name="handle">Win32 window handle from an existing window.</param>
        /// <returns>A new <see cref="Window"/> created from the Win32 handle.</returns>
        public Window CreateFromWin32(IntPtr hWnd)
        {
            // TODO add a sample for this
            if (Backend != WindowingBackend.Win32)
                throw new OpenWindowException("CreateFromWin32 called with windowing backend " + Backend);

            return new Win32Window(hWnd);
        }

        /// <summary>
        /// Process all received events for windows managed by this service.
        /// </summary>
        public abstract void PumpEvents();

        /// <summary>
        /// Wait for the next event and process it before returning control to the caller.
        /// </summary>
        public abstract void WaitEvent();

        /// <summary>
        /// Get the key modifiers currently enabled.
        /// </summary>
        /// <returns>The enabled key modifiers.</returns>
        public abstract KeyMod GetKeyModifiers();

        /// <summary>
        /// Check if caps lock is turned on.
        /// </summary>
        /// <returns><code>true</code> if caps lock is turned on, <code>false</code> if it is turned off.</returns>
        public abstract bool IsCapsLockOn();

        /// <summary>
        /// Check if num lock is turned on.
        /// </summary>
        /// <returns><code>true</code> if num lock is turned on, <code>false</code> if it is turned off.</returns>
        public abstract bool IsNumLockOn();

        /// <summary>
        /// Check if scroll lock is turned on.
        /// </summary>
        /// <returns><code>true</code> if scroll lock is turned on, <code>false</code> if it is turned off.</returns>
        public abstract bool IsScrollLockOn();

        /// <summary>
        /// Set the position of the mouse cursor.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public abstract void SetCursorPosition(int x, int y);

        #endregion

        #region Input state methods

        internal void SetFocus(Window window, bool newFocus)
        {
            var hasFocus = _keyboardState.FocusedWindow == window;

            if (!hasFocus && newFocus)
            {
                // unfocus should happen before setting the new focus, but we check just in case
                if (_keyboardState.FocusedWindow != null)
                    _keyboardState.FocusedWindow.RaiseFocusChanged(false);
                window.RaiseFocusChanged(true);
                _keyboardState.FocusedWindow = window;
            }
            else if (hasFocus && !newFocus)
            {
                window.RaiseFocusChanged(false);
                _keyboardState.FocusedWindow = null;
            }
        }

        protected void SetKey(ScanCode sc, bool down)
        {
            if (sc == ScanCode.Unknown)
                return;

            // TODO repeated keys
            if (down && _keyboardState.Up(sc))
            {
                var key = _keyboardState.Map(sc);
                _keyboardState.FocusedWindow?.RaiseKeyDown(key, sc);
                _keyboardState.ScanState[(int) sc] = true;
            }
            else if (!down && _keyboardState.Down(sc))
            {
                var key = _keyboardState.Map(sc);
                _keyboardState.FocusedWindow?.RaiseKeyUp(key, sc);
                _keyboardState.ScanState[(int) sc] = false;
            }
        }

        protected void SendCharacter(int c)
        {
            _keyboardState.FocusedWindow?.RaiseTextInput(c);
        }

        internal void SetMouseFocus(Window window, bool newFocus)
        {
            var hasFocus = _mouseState.FocusedWindow == window;

            if (!hasFocus && newFocus)
            {
                window.RaiseMouseFocusChanged(true);
                _mouseState.FocusedWindow = window;
            }
            else if (hasFocus && !newFocus)
            {
                window.RaiseMouseFocusChanged(false);
                _mouseState.FocusedWindow = null;
            }
        }

        protected void SetMousePosition(int x, int y)
        {
            _mouseState.X = x;
            _mouseState.Y = y;
            _mouseState.FocusedWindow?.RaiseMouseMoved(x, y);
        }

        protected void SetMouseButton(MouseButtons button, bool down)
        {
            if (down)
            {
                _mouseState.FocusedWindow?.RaiseMouseDown(button, _mouseState.X, _mouseState.Y);
                _mouseState.ButtonsDown |= button;
            }
            else
            {
                _mouseState.FocusedWindow?.RaiseMouseUp(button, _mouseState.X, _mouseState.Y);
                _mouseState.ButtonsDown &= ~button;
            }
        }

        protected void SetMouseScroll(float x, float y)
        {
            _mouseState.FocusedWindow?.RaiseMouseScroll(x, y);
            _mouseState.ScrollX += x;
            _mouseState.ScrollY += y;
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Destroy the <see cref="WindowingService"/> instance and release unmanaged resources.
        /// Call this before exiting your program.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
