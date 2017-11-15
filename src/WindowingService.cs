// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace OpenWindow
{
    public abstract class WindowingService
    {
        #region Fields

        protected readonly Dictionary<IntPtr, Window> ManagedWindows;

        #endregion

        #region Constructor

        protected WindowingService()
        {
            ManagedWindows = new Dictionary<IntPtr, Window>();
            GlSettings = new OpenGLWindowSettings();

            Logger = new Logger();
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
            _instance = CreateService(type);
            _instance.Initialize();
        }

        protected abstract void Initialize();

        private static WindowingServiceType GetWindowingServiceType()
        {
            if (Win32Available())
                return WindowingServiceType.Win32;

            // LINUX
            if (WaylandAvailable())
                return WindowingServiceType.Wayland;

            // OSX
            throw new NotSupportedException("No OSX back ends are implemented yet.");
        }

        private static bool Win32Available()
        {
            // life is hack
            try
            {
                // just try P/Invoking a simple Win32 function
                Backends.Windows.Native.GetCurrentThreadId();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool WaylandAvailable()
        {
            return true;
        }

        private static WindowingService CreateService(WindowingServiceType type)
        {
            switch (type)
            {
                case WindowingServiceType.Win32:
                    return new Backends.Windows.Win32WindowingService();
                case WindowingServiceType.X:
                    throw new NotImplementedException();
                case WindowingServiceType.Mir:
                    throw new NotImplementedException();
                case WindowingServiceType.Wayland:
                    return new Backends.Wayland.WaylandWindowingService();
                case WindowingServiceType.Quartz:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region Internal helpers

        internal static void Log(MessageType t, string content)
        {
            Get().Logger.LogMessage(t, content);
        }

        #endregion

        #region Public API

        /// <summary>
        /// Provides logged messages.
        /// </summary>
        public Logger Logger { get; set; }

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
    }
}
