// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow
{
    public abstract class Window
    {

        #region Window API: Properties

        /// <summary>
        /// Get a pointer to the native window handle.
        /// </summary>
        public abstract IntPtr Handle { get; }
        
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
        public abstract OwPoint Position { get; set; }
        /// <summary>
        /// Get or set the size of this window (including border).
        /// </summary>
        public abstract OwPoint Size { get; set; }
        /// <summary>
        /// Get or set the bounds of this window (including border).
        /// </summary>
        public abstract OwRectangle Bounds { get; set; }
        /// <summary>
        /// Get or set the bounds of this window (excluding border).
        /// </summary>
        public abstract OwRectangle ClientBounds { get; set; }

        /// <summary>
        /// Get or set the text that is displayed in the title bar of the window.
        /// </summary>
        public abstract string Title { get; set; }

        #endregion

        #region Window API: Functions
        
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

        public abstract IntPtr GetDeviceContext();

        public abstract void ReleaseDeviceContext(IntPtr deviceContext);

        #endregion

        #region Events

        /// <summary>
        /// Invoked right before the window closes.
        /// </summary>
        public event ClosingHandler Closing;
        public delegate void ClosingHandler(object sender, EventArgs args);

        /// <summary>
        /// Invoked after the window focus changed.
        /// </summary>
        public event FocusChangedHandler FocusChanged;
        public delegate void FocusChangedHandler(object sender, FocusChangedEventArgs args);

        // TODO
        public event KeyDownHandler KeyDown;
        public delegate void KeyDownHandler(object sender, KeyEventArgs args);

        // TODO
        public event KeyUpHandler KeyUp;
        public delegate void KeyUpHandler(object sender, KeyEventArgs args);

        /// <summary>
        /// Invoked when a keypress happens.
        /// </summary>
        public event TextInputHandler TextInput;
        public delegate void TextInputHandler(object sender, TextInputEventArgs args);

        #endregion

        #region Internal Functions

        internal virtual void Update()
        {
        }

        internal bool TryGetWindow(IntPtr handle, out Window window)
        {
            var service = WindowingService.Get();
            return service.TryGetWindow(handle, out window);
        }

        internal void RaiseClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseFocusChanged(bool currentFocus)
        {
            FocusChanged?.Invoke(this, new FocusChangedEventArgs(currentFocus));
        }

        internal void RaiseTextInput(char c)
        {
            TextInput?.Invoke(this, new TextInputEventArgs(c));
        }

        #endregion
    }
}
