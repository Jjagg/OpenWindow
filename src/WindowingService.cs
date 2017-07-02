// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        }

        private static WindowingServiceType GetWindowingServiceType()
        {
            if (Win32Available())
                return WindowingServiceType.Win32;

            // LINUX
            throw new NotSupportedException("No Linux back ends are implemented yet.");

            // OSX
            throw new NotSupportedException("No OSX back ends are implemented yet.");
        }

        private static bool Win32Available()
        {
            // life is hack
            try
            {
                Backends.Windows.Native.GetCurrentThreadId();
                return true;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        }

        private static WindowingService CreateService(WindowingServiceType type)
        {
            switch (type)
            {
                case WindowingServiceType.Win32:
                    return new Backends.Windows.WindowsWindowingService();
                case WindowingServiceType.X:
                    throw new NotImplementedException();
                case WindowingServiceType.Mir:
                    throw new NotImplementedException();
                case WindowingServiceType.Wayland:
                    throw new NotImplementedException();
                case WindowingServiceType.Cocoa:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region Public API

        /// <summary>
        /// The number of windows managed by this service.
        /// </summary>
        public int WindowCount => ManagedWindows.Count;

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
