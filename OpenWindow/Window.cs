// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Win32Window = OpenWindow.Windows.Win32Window;
using System.Collections.Generic;
using System;
using OpenWindow.Common;
using OpenWindow.EventArgs;

namespace OpenWindow
{
    public abstract class Window
    {

        private static readonly Dictionary<string, Window> _managedWindows = new Dictionary<string, Window>();

        #region Shared Window API

        /// <summary>
        /// Get or set if this window is in fullscreen.
        /// </summary>
        public abstract bool IsFullscreen { get; set; }
        /// <summary>
        /// Get or set if this window is focused.
        /// </summary>
        public abstract bool IsFocused { get; set; }

        /// <summary>
        /// Get or set the position of the top left of this window (including border).
        /// </summary>
        public abstract Point Position { get; set; }
        /// <summary>
        /// Get or set the size of this window (including border).
        /// </summary>
        public abstract Point Size { get; set; }
        /// <summary>
        /// Get or set the bounds of this window (including border).
        /// </summary>
        public abstract Rectangle Bounds { get; set; }
        /// <summary>
        /// Get or set the bounds of this window (excluding border).
        /// </summary>
        public abstract Rectangle ClientBounds { get; set; }

        // TODO see if this fits
        public abstract Message GetMessage();

        /// <summary>
        /// Close this window.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Get a byte array with the status data for each virtual key.
        /// </summary>
        /// <returns>
        /// A byte array representing the state of the keyboard.
        /// Use <see cref="VirtualKey"/> members to index into it.
        /// </returns>
        public abstract byte[] GetKeyboardState();

        /// <summary>
        /// Check if the specified key is down.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>True if the key is down, false if it is up.</returns>
        public abstract bool IsDown(VirtualKey key);

        #endregion

        #region Events

        public event FocusChangedHandler FocusChanged;
        public delegate void FocusChangedHandler(object sender, FocusChangedEventArgs args);

        public event KeyDownHandler KeyDown;
        public delegate void KeyDownHandler(object sender, KeyEventArgs args);

        public event KeyUpHandler KeyUp;
        public delegate void KeyUpHandler(object sender, KeyEventArgs args);

        public event TextInputHandler TextInput;
        public delegate void TextInputHandler(object sender, TextInputEventArgs args);

        #endregion

        #region Create

        public static Window Create(int x, int y, int width, int height)
        {
            if (!OpenWindow.Initialized)
                OpenWindow.Initialize();
            switch (OpenWindow.Service)
            {
                case WindowingService.None:
                    throw new InvalidOperationException("No active windowing service found.");
                case WindowingService.Windows:
                    return CreateWin32(x, y, width, height);
                case WindowingService.X:
                    return CreateX(x, y, width, height);
                case WindowingService.Mir:
                    return CreateMir(x, y, width, height);
                case WindowingService.Wayland:
                    return CreateWayland(x, y, width, height);
                case WindowingService.Cocoa:
                    return CreateCocoa(x, y, width, height);
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Window CreateWin32(int x, int y, int width, int height)
        {
            return Win32Window.Create(x, y, width, height);
        }

        private static Window CreateX(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        private static Window CreateMir(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        private static Window CreateWayland(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        private static Window CreateCocoa(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Functions

        protected void RaiseTextInput(char c)
        {
            TextInput?.Invoke(this, new TextInputEventArgs(c));
        }

        #endregion
    }
}
