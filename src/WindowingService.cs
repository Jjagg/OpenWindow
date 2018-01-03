// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collection.ObjectModel;

namespace OpenWindow
{
    /// <summary>
    /// Singleton object used to create and manage <see cref="Window"/> instances.
    /// </summary>
    public abstract class WindowingService : IDisposable
    {
        #region Fields

        /// <summary>
        /// A map from handles to the windows managed by this service.
        /// </summary>
        protected readonly Dictionary<IntPtr, Window> ManagedWindows;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a <see cref="WindowingService"/>.
        /// </summary>
        protected WindowingService()
        {
            ManagedWindows = new Dictionary<IntPtr, Window>();
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
            Backend = GetWindowingBackend();
            LogInfo($"Detected windowing backend '{Backend}'.");
            _instance = CreateService(Backend);
            _instance.Initialize();
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
                if (UnixGetVariable("XDG_SESSION_TYPE").Contains("x11"))
                    return WindowingBackend.X;
                if (!string.IsNullOrEmpty(UnixGetVariable("WAYLAND_DISPLAY")))
                    return WindowingBackend.Wayland;
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


#if NETSTANDARD1_1
        [DllImport("libc", EntryPoint = "getenv")]
        private static extern IntPtr GetEnv([In][MarshalAs(UnmanagedType.LPStr)] string name);

        // NOTE: .NET's GetEnvironmentVariable is in .NET Standard 1.3+
        // we want to support 1.1 so we PInvoke libc
        private static string UnixGetVariable(string name)
        {
            return Marshal.PtrToStringAnsi(GetEnv(name));
        }
#else
        private static string UnixGetVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
#endif

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
        public int WindowCount => ManagedWindows.Count;

        /// <summary>
        /// The settings to use for an OpenGL window. You only need to touch this when using OpenGL for rendering.
        /// Value at the time of calling <see cref="CreateWindow"/> is used for that window.
        /// </summary>
        public OpenGlSurfaceSettings GlSettings { get; }

        /// <summary>
        /// Get the connected displays.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyCollection" /> containing connected displays.</returns>
        public abstract ReadOnlyCollection<Display> Displays { get; }

        /// <summary>
        /// Get the primary display or <code>null</code> if the primary display could not be found.
        /// </summary>
        public abstract Display PrimaryDisplay { get; }

        /// <summary>
        /// Get a window owned by this service by its handle.
        /// </summary>
        /// <param name="handle">The native handle to the window.</param>
        /// <param name="window">The window with the specified handle or null if the lookup failed.</param>
        /// <returns>True if the window was found, false otherwise.</returns>
        public bool TryGetWindow(IntPtr handle, out Window window)
        {
            return ManagedWindows.TryGetValue(handle, out window);
        }

        /// <summary>
        /// Create a new <see cref="Window"/>.
        /// </summary>
        /// <param name="show">
        /// If <code>true</code>, makes the window visible, else keeps it hidden.
        /// The window can be made visible by calling <see cref="Window.Show"/>.
        /// </param>
        /// <returns>A new <see cref="Window"/>.</returns>
        public abstract Window CreateWindow(bool show = true);

        /// <summary>
        /// Create a <see cref="Window"/> given a handle to an existing native window.
        /// </summary>
        /// <param name="handle">Handle to an existing window.</param>
        /// <returns>A new <see cref="Window"/> created from the native <paramref name="handle"/>.</returns>
        public abstract Window WindowFromHandle(IntPtr handle);

        /// <summary>
        /// Process all received events for windows managed by this service.
        /// </summary>
        public abstract void PumpEvents();

        /// <summary>
        /// Wait for the next event and process it before returning control to the caller.
        /// </summary>
        public abstract void WaitEvent();

        #endregion

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
    }
}
