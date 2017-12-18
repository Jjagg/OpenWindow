// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenWindow
{
    public abstract class WindowingService : IDisposable
    {
        #region Fields

        protected readonly Dictionary<IntPtr, Window> ManagedWindows;

        #endregion

        #region Constructor

        protected WindowingService()
        {
            ManagedWindows = new Dictionary<IntPtr, Window>();
            GlSettings = new OpenGLWindowSettings();
        }

        #endregion

        #region Singleton

        private static WindowingService _instance;
        public static WindowingService Get()
        {
            if (_instance == null)
                InitializeInstance();

            return _instance;
        }

        private static void InitializeInstance()
        {
            var type = GetWindowingServiceType();
            LogInfo($"Detected windowing backend '{type}'.");
            _instance = CreateService(type);
            _instance.Initialize();
        }

        protected abstract void Initialize();

        private static WindowingServiceType GetWindowingServiceType()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WindowingServiceType.Win32;

            // LINUX
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (UnixGetVariable("XDG_SESSION_TYPE").Contains("x11"))
                    return WindowingServiceType.X;
                if (!string.IsNullOrEmpty(UnixGetVariable("WAYLAND_DISPLAY")))
                    return WindowingServiceType.Wayland;
                // TODO check if above is reliable enough, we can always just try both
                throw new OpenWindowException("Failed to detect if x11 or Wayland is used." +
                                              "Please open an issue on the OpenWindow repo for this.");
            }

            // OSX
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                throw new NotImplementedException("No OSX back end is implemented yet.");

            throw new NotSupportedException("OS was not reported to be Windows, Linux or OSX by .NET host.");
        }

        private static WindowingService CreateService(WindowingServiceType type)
        {
            switch (type)
            {
                case WindowingServiceType.Win32:
                    return new Backends.Windows.Win32WindowingService();
                case WindowingServiceType.X:
                    return new Backends.X.XWindowingService();
                case WindowingServiceType.Wayland:
                    return new Backends.Wayland.WaylandWindowingService();
                case WindowingServiceType.Quartz:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion


        [DllImport("libc", EntryPoint = "getenv")]
        private static extern IntPtr GetEnv([In][MarshalAs(UnmanagedType.LPStr)] string name);

        // NOTE: .NET's GetEnvironmentVariable is in .NET Standard 1.3+
        // we want to stick to 1.1 so we PInvoke libc
        private static string UnixGetVariable(string name)
        {
            return Marshal.PtrToStringAnsi(GetEnv(name));
        }

        #region Logging

        public static void LogDebug(string message)
        {
            Log(Logger.Level.Debug, message);
        }

        public static void LogInfo(string message)
        {
            Log(Logger.Level.Info, message);
        }

        public static void LogWarning(string message)
        {
            Log(Logger.Level.Warning, message);
        }

        public static void LogError(string message)
        {
            Log(Logger.Level.Error, message);
        }

        internal static void Log(Logger.Level l, string content)
        {
            Logger.Log(l, content);
        }

        #endregion

        #region Public API

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
        public OpenGLWindowSettings GlSettings { get; }

        /// <summary>
        /// Get the connected displays.
        /// </summary>
        /// <returns>An array containing connected displays.</returns>
        public abstract Display[] Displays { get; }

        /// <summary>
        /// Set the output writer for log messages.
        /// </summary>
        public void SetLogWriter(TextWriter writer)
        {
            Logger.OutputWriter = writer;
        }

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
        /// <returns>A new <see cref="Window"/></returns>
        public abstract Window CreateWindow();

        /// <summary>
        /// Update the windows owned by this service.
        /// </summary>
        public abstract void Update();

        #endregion

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
