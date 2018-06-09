// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow
{
    /// <summary>
    /// A window that is not tied to a native window.
    /// </summary>
    public class VirtualWindow : Window
    {
        private KeyMod _keyModifiers;
        private bool _capsLock;
        private bool _numLock;
        private bool _scrollLock;
        private MouseButtons _mouseButtons;
        private Point _cursorPosition;

        private readonly bool[] _keyMap;

        /// <summary>
        /// Create a virtual window.
        /// </summary>
        public VirtualWindow() : base(true)
        {
            var keys = Enum.GetValues(typeof(Key));
            _keyMap = new bool[keys.Length];
        }

        /// <summary>
        /// Size of the top border if this window is decorated. <seealso cref="Window.Decorated"/>.
        /// </summary>
        public int BorderTop { get; set; }

        /// <summary>
        /// Size of the bottom border if this window is decorated. <seealso cref="Window.Decorated"/>.
        /// </summary>
        public int BorderBottom { get; set; }

        /// <summary>
        /// Size of the left border if this window is decorated. <seealso cref="Window.Decorated"/>.
        /// </summary>
        public int BorderLeft { get; set; }

        /// <summary>
        /// Size of the right border if this window is decorated. <seealso cref="Window.Decorated"/>.
        /// </summary>
        public int BorderRight { get; set; }

        /// <inheritdoc />
        public override Point Position { get; set; }

        /// <inheritdoc />
        public override Size Size { get; set; }

        /// <inheritdoc />
        public override Size ClientSize {
            get => Decorated
                ? new Size(Size.Width - BorderLeft - BorderRight, Size.Height - BorderTop - BorderBottom)
                : Size;
            set
            {
                if (!Decorated)
                    Size = value;
                else
                    Size = new Size(Size.Width + BorderLeft + BorderRight, Size.Height + BorderTop + BorderBottom);
            }
        }

        /// <inheritdoc />
        public override Rectangle Bounds
        {
            get => new Rectangle(Position, Size);
            set
            {
                Position = value.Position;
                Size = value.Size;
            }
        }

        /// <inheritdoc />
        public override Rectangle ClientBounds
        {
            get
            {
                if (!Decorated)
                    return Bounds;
                return new Rectangle(
                    Position.X + BorderLeft,
                    Position.Y + BorderTop,
                    Size.Width - BorderRight - BorderLeft,
                    Size.Height - BorderBottom - BorderTop);
            }
            set
            {
                if (!Decorated)
                    Bounds = value;
                else
                {
                    Bounds = new Rectangle(
                        value.X - BorderLeft,
                        value.Y - BorderTop,
                        value.Width + BorderLeft + BorderRight,
                        value.Height + BorderTop + BorderBottom);
                }
            }
        }

        public override Display GetContainingDisplay()
        {
            throw new InvalidOperationException("Cannot get the display of a virtual window.");
        }

        /// <summary>
        /// Set the keyboard modifier keys.
        /// </summary>
        /// <param name="modifiers">Modifiers to set.</param>
        public void SetModifiers(KeyMod modifiers)
        {
            _keyModifiers = modifiers;
        }

        /// <summary>
        /// Set the state of a key.
        /// </summary>
        /// <param name="key">The key to set the state of.</param>
        /// <param name="down"><code>true</code> for down or <code>false</code> for up.</param>
        public void SetKeyDown(Key key, bool down)
        {
            _keyMap[(int) key] = down;
        }

        /// <inheritdoc />
        public override bool IsDown(Key key)
        {
            return _keyMap[(int) key];
        }

        /// <inheritdoc />
        public override KeyMod GetKeyModifiers()
        {
            return _keyModifiers;
        }

        /// <summary>
        /// Set caps lock on or off.
        /// </summary>
        /// <param name="value"><code>true</code> for on or <code>false</code> for off.</param>
        public void SetCapsLock(bool value)
        {
            _capsLock = value;
        }

        /// <inheritdoc />
        public override bool IsCapsLockOn()
        {
            return _capsLock;
        }

        /// <summary>
        /// Set num lock on or off.
        /// </summary>
        /// <param name="value"><code>true</code> for on or <code>false</code> for off.</param>
        public void SetNumLock(bool value)
        {
            _numLock = value;
        }

        /// <inheritdoc />
        public override bool IsNumLockOn()
        {
            return _numLock;
        }

        /// <summary>
        /// Set scroll lock on or off.
        /// </summary>
        /// <param name="value"><code>true</code> for on or <code>false</code> for off.</param>
        public void SetScrollLock(bool value)
        {
            _scrollLock = value;
        }

        /// <inheritdoc />
        public override bool IsScrollLockOn()
        {
            return _scrollLock;
        }

        /// <summary>
        /// Set the mouse buttons state.
        /// </summary>
        /// <param name="buttons">The mouse buttons that are down.</param>
        public void SetMouseButtons(MouseButtons buttons)
        {
            _mouseButtons = buttons;
        }

        /// <inheritdoc />
        public override MouseState GetMouseState()
        {
            return new MouseState(_mouseButtons, _cursorPosition);
        }

        /// <inheritdoc />
        public override void SetCursorPosition(int x, int y)
        {
            _cursorPosition = new Point(x, y);
        }

        protected override void InternalSetVisible(bool value)
        {
        }

        protected override void InternalMaximize()
        {
        }

        protected override void InternalMinimize()
        {
        }

        protected override void InternalRestore()
        {
        }

        protected override void InternalSetTitle(string value)
        {
        }

        protected override void InternalSetBorderless(bool value)
        {
        }

        protected override void InternalSetResizable(bool value)
        {
        }

        protected override void InternalSetMinSize(Size value)
        {
            if (value == Size.Empty)
                return;
            Size = new Size(Math.Max(value.Width, Size.Width), Math.Max(value.Height, Size.Height));
        }

        protected override void InternalSetMaxSize(Size value)
        {
            if (value == Size.Empty)
                return;
            Size = new Size(Math.Min(value.Width, Size.Width), Math.Min(value.Height, Size.Height));
        }

        protected override void InternalSetCursorVisible(bool value)
        {
        }
    }
}