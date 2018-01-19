namespace OpenWindow
{
    /// <summary>
    /// Virtual keys used to report keyboard input.
    /// </summary>
    public enum Key
    {
        /// <summary>
        /// Reported key when a scancode is not mapped by OpenWindow.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 0 key.
        /// </summary>
        D0,
        /// <summary>
        /// 1 key.
        /// </summary>
        D1,
        /// <summary>
        /// 2 key.
        /// </summary>
        D2,
        /// <summary>
        /// 3 key.
        /// </summary>
        D3,
        /// <summary>
        /// 4 key.
        /// </summary>
        D4,
        /// <summary>
        /// 5 key.
        /// </summary>
        D5,
        /// <summary>
        /// 6 key.
        /// </summary>
        D6,
        /// <summary>
        /// 7 key.
        /// </summary>
        D7,
        /// <summary>
        /// 8 key.
        /// </summary>
        D8,
        /// <summary>
        /// 9 key.
        /// </summary>
        D9,

        /// <summary>
        /// A key.
        /// </summary>
        A,
        /// <summary>
        /// B key.
        /// </summary>
        B,
        /// <summary>
        /// C key.
        /// </summary>
        C,
        /// <summary>
        /// D key.
        /// </summary>
        D,
        /// <summary>
        /// E key.
        /// </summary>
        E,
        /// <summary>
        /// F key.
        /// </summary>
        F,
        /// <summary>
        /// G key.
        /// </summary>
        G,
        /// <summary>
        /// H key.
        /// </summary>
        H,
        /// <summary>
        /// I key.
        /// </summary>
        I,
        /// <summary>
        /// J key.
        /// </summary>
        J,
        /// <summary>
        /// K key.
        /// </summary>
        K,
        /// <summary>
        /// L key.
        /// </summary>
        L,
        /// <summary>
        /// M key.
        /// </summary>
        M,
        /// <summary>
        /// N key.
        /// </summary>
        N,
        /// <summary>
        /// O key.
        /// </summary>
        O,
        /// <summary>
        /// P key.
        /// </summary>
        P,
        /// <summary>
        /// Q key.
        /// </summary>
        Q,
        /// <summary>
        /// R key.
        /// </summary>
        R,
        /// <summary>
        /// S key.
        /// </summary>
        S,
        /// <summary>
        /// T key.
        /// </summary>
        T,
        /// <summary>
        /// U key.
        /// </summary>
        U,
        /// <summary>
        /// V key.
        /// </summary>
        V,
        /// <summary>
        /// W key.
        /// </summary>
        W,
        /// <summary>
        /// X key.
        /// </summary>
        X,
        /// <summary>
        /// Y key.
        /// </summary>
        Y,
        /// <summary>
        /// Z key.
        /// </summary>
        Z,

        /// <summary>
        /// F1 key.
        /// </summary>
        F1,
        /// <summary>
        /// F2 key.
        /// </summary>
        F2,
        /// <summary>
        /// F3 key.
        /// </summary>
        F3,
        /// <summary>
        /// F4 key.
        /// </summary>
        F4,
        /// <summary>
        /// F5 key.
        /// </summary>
        F5,
        /// <summary>
        /// F6 key.
        /// </summary>
        F6,
        /// <summary>
        /// F7 key.
        /// </summary>
        F7,
        /// <summary>
        /// F8 key.
        /// </summary>
        F8,
        /// <summary>
        /// F9 key.
        /// </summary>
        F9,
        /// <summary>
        /// F10 key.
        /// </summary>
        F10,
        /// <summary>
        /// F11 key.
        /// </summary>
        F11,
        /// <summary>
        /// F12 key.
        /// </summary>
        F12,
        /// <summary>
        /// F13 key.
        /// </summary>
        F13,
        /// <summary>
        /// F14 key.
        /// </summary>
        F14,
        /// <summary>
        /// F15 key.
        /// </summary>
        F15,
        /// <summary>
        /// F16 key.
        /// </summary>
        F16,
        /// <summary>
        /// F17 key.
        /// </summary>
        F17,
        /// <summary>
        /// F18 key.
        /// </summary>
        F18,
        /// <summary>
        /// F19 key.
        /// </summary>
        F19,
        /// <summary>
        /// F20 key.
        /// </summary>
        F20,
        /// <summary>
        /// F21 key.
        /// </summary>
        F21,
        /// <summary>
        /// F22 key.
        /// </summary>
        F22,
        /// <summary>
        /// F23 key.
        /// </summary>
        F23,
        /// <summary>
        /// F24 key.
        /// </summary>
        F24,

        /// <summary>
        /// Escape key.
        /// </summary>
        Escape,
        /// <summary>
        /// Backspace key.
        /// </summary>
        Backspace,
        /// <summary>
        /// Tab key.
        /// </summary>
        Tab,
        /// <summary>
        /// Space key.
        /// </summary>
        Space,
        /// <summary>
        /// Return key.
        /// </summary>
        Enter,

        /// <summary>
        /// Print screen key.
        /// </summary>
        PrintScreen,
        /// <summary>
        /// Pause key.
        /// </summary>
        Pause,
        /// <summary>
        /// Insert key.
        /// </summary>
        Insert,
        /// <summary>
        /// Delete key.
        /// </summary>
        Delete,
        /// <summary>
        /// Home key.
        /// </summary>
        Home,
        /// <summary>
        /// End key.
        /// </summary>
        End,
        /// <summary>
        /// Page up key.
        /// </summary>
        PageUp,
        /// <summary>
        /// Page down key.
        /// </summary>
        PageDown,

        /// <summary>
        /// Backtick/tilde key (`~).
        /// </summary>
        Backtick,
        /// <summary>
        /// Dash/underscore key (-_).
        /// </summary>
        Minus,
        /// <summary>
        /// Plus/equals key (+=)
        /// </summary>
        Plus,
        /// <summary>
        /// Left bracket/left curly brace key ([{)
        /// </summary>
        LeftBracket,
        /// <summary>
        /// Right bracket/right curly brace key ([{)
        /// </summary>
        RightBracket,
        /// <summary>
        /// Backslash/bar key (\|).
        /// </summary>
        Backslash,
        /// <summary>
        /// Semicolon/colon key (;:).
        /// </summary>
        Semicolon,
        /// <summary>
        /// Single-quote/double-quote key ('")
        /// </summary>
        Quote,
        /// <summary>
        /// Comma/left angled-bracket key (,&lt;)
        /// </summary>
        Comma,
        /// <summary>
        /// Period/right angled-bracket key (.>)
        /// </summary>
        Period,
        /// <summary>
        /// Forward slash/question mark key (/?)
        /// </summary>
        Slash,

        /// <summary>
        /// Left control key.
        /// </summary>
        LeftControl,
        /// <summary>
        /// Right control key.
        /// </summary>
        RightControl,
        /// <summary>
        /// Left shift key.
        /// </summary>
        LeftShift,
        /// <summary>
        /// Right shift key.
        /// </summary>
        RightShift,
        /// <summary>
        /// Left super/Windows key.
        /// </summary>
        LeftSuper,
        /// <summary>
        /// Right super/Windows key.
        /// </summary>
        RightSuper,
        /// <summary>
        /// Left alt key.
        /// </summary>
        LeftAlt,
        /// <summary>
        /// Alt-right key.
        /// </summary>
        RightAlt,
        /// <summary>
        /// Menu key.
        /// </summary>
        Menu,

        /// <summary>
        /// Up arrow key.
        /// </summary>
        Up,
        /// <summary>
        /// Down arrow key.
        /// </summary>
        Down,
        /// <summary>
        /// Left arrow key.
        /// </summary>
        Left,
        /// <summary>
        /// Right arrow key.
        /// </summary>
        Right,

        /// <summary>
        /// Num lock key.
        /// </summary>
        NumLock,
        /// <summary>
        /// Caps lock key.
        /// </summary>
        CapsLock,
        /// <summary>
        /// Scroll lock key.
        /// </summary>
        ScrollLock,

        /// <summary>
        /// Numpad '/' key.
        /// </summary>
        NpDivide,
        /// <summary>
        /// Numpad '*' key.
        /// </summary>
        NpMultiply,
        /// <summary>
        /// Numpad '-' key.
        /// </summary>
        NpMinus,
        /// <summary>
        /// Numpad '+' key.
        /// </summary>
        NpPlus,
        /// <summary>
        /// Numpad Enter key.
        /// </summary>
        // TODO: on windows we can't tell what enter we got directly from the VK, we need the bit flags too
        //       currently both enter keys map to Key.Enter on Windows
        NpEnter,
        /// <summary>
        /// Numpad '0' key.
        /// </summary>
        Np0,
        /// <summary>
        /// Numpad '1' key.
        /// </summary>
        Np1,
        /// <summary>
        /// Numpad '2' key.
        /// </summary>
        Np2,
        /// <summary>
        /// Numpad '3' key.
        /// </summary>
        Np3,
        /// <summary>
        /// Numpad '4' key.
        /// </summary>
        Np4,
        /// <summary>
        /// Numpad '5' key.
        /// </summary>
        Np5,
        /// <summary>
        /// Numpad '6' key.
        /// </summary>
        Np6,
        /// <summary>
        /// Numpad '7' key.
        /// </summary>
        Np7,
        /// <summary>
        /// Numpad '8' key.
        /// </summary>
        Np8,
        /// <summary>
        /// Numpad '9' key.
        /// </summary>
        Np9,
        /// <summary>
        /// Numpad '.' key.
        /// </summary>
        NpDecimal
    }
}