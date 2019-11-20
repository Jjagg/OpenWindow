using System;

namespace OpenWindow
{
    /// <summary>
    /// State of keys (up or down) indexed by scancode or key code.
    /// </summary>
    public class KeyboardState
    {
        /// <summary>
        /// The window that currently has keyboard focus.
        /// This can be <c>null</c> if the focused window is not managed by OpenWindow.
        /// </summary>
        public Window FocusedWindow { get; internal set; }

        /// <summary>
        /// Keyboard state with a boolean indicating if keys are down.
        /// Indexed with a key's <see cref="ScanCode"/>.
        /// </summary>
        internal bool[] ScanState { get; }

        /// <summary>
        /// Map from scan codes to key codes.
        /// This should be filled in by subclasses on initialization
        /// and updated when the keyboard layout changes.
        /// </summary>
        private Key[] _scanToKey { get; }

        /// <summary>
        /// Map from key codes to scan codes.
        /// This should be filled in by subclasses on initialization
        /// and updated when the keyboard layout changes.
        /// </summary>
        private ScanCode[] _keyToScan { get; }

        internal KeyboardState()
        {
            ScanState = new bool[(int) ScanCode.Count];
            _scanToKey = new Key[(int) ScanCode.Count];
            _keyToScan = new ScanCode[(int) Key.Count];
        }

        /// <summary>
        /// Check if the key with the given scan code is down.
        /// </summary>
        public bool IsDown(ScanCode sc) => ScanState[(int) sc];

        /// <summary>
        /// Check if the key with the given scan code is up.
        /// </summary>
        public bool IsUp(ScanCode sc) => !ScanState[(int) sc];

        /// <summary>
        /// Check if the given virtual key is down.
        /// </summary>
        public bool IsDown(Key key) => ScanState[(int) Map(key)];

        /// <summary>
        /// Check if the given virtual key is up.
        /// </summary>
        public bool IsUp(Key key) => !ScanState[(int) Map(key)];


        /// <summary>
        /// Map the given scan code to the matching virtual key for the current keyboard layout.
        /// </summary>
        public Key Map(ScanCode sc) => _scanToKey[(int) sc];

        /// <summary>
        /// Map the given scan code to the matching virtual key for the current keyboard layout.
        /// </summary>
        public ScanCode Map(Key key) => _keyToScan[(int) key];

        internal void Clear()
        {
            for (var i = 0; i < _scanToKey.Length; i++)
                _scanToKey[i] = Key.Unknown;

            for (var i = 0; i < _keyToScan.Length; i++)
                _keyToScan[i] = ScanCode.Unknown;

            for (var i = 0; i < _scanToKey.Length; i++)
                ScanState[i] = false;
        }

        internal void Set(ScanCode sc, Key key)
        {
            _scanToKey[(int) sc] = key;
            _keyToScan[(int) key] = sc;
        }
    }
}
