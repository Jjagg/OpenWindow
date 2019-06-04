namespace OpenWindow.Backends.Windows
{
    internal static class Win32KeyMaps
    {
        public static ScanCode[] WinToOwScanCode =
        {
            ScanCode.Unknown,
            ScanCode.Escape,
            ScanCode.D1,
            ScanCode.D2,
            ScanCode.D3,
            ScanCode.D4,
            ScanCode.D5,
            ScanCode.D6,
            ScanCode.D7,
            ScanCode.D8,
            ScanCode.D9,
            ScanCode.D0,
            ScanCode.Minus,
            ScanCode.Equals,
            ScanCode.Backspace,
            ScanCode.Tab,
            ScanCode.Q,
            ScanCode.W,
            ScanCode.E,
            ScanCode.R,
            ScanCode.T,
            ScanCode.Y,
            ScanCode.U,
            ScanCode.I,
            ScanCode.O,
            ScanCode.P,
            ScanCode.LeftSquareBracket,
            ScanCode.RightSquareBracket,
            ScanCode.Enter,
            ScanCode.LeftControl,
            ScanCode.A,
            ScanCode.S,
            ScanCode.D,
            ScanCode.F,
            ScanCode.G,
            ScanCode.H,
            ScanCode.J,
            ScanCode.K,
            ScanCode.L,
            ScanCode.Semicolon,
            ScanCode.Quote,
            ScanCode.Backquote,
            ScanCode.LeftShift,
            ScanCode.Backslash,
            ScanCode.Z,
            ScanCode.X,
            ScanCode.C,
            ScanCode.V,
            ScanCode.B,
            ScanCode.N,
            ScanCode.M,
            ScanCode.Comma,
            ScanCode.Period,
            ScanCode.Slash,
            ScanCode.RightShift,
            ScanCode.KpMultiply,
            ScanCode.LeftAlt,
            ScanCode.Space,
            ScanCode.CapsLock,
            ScanCode.F1,
            ScanCode.F2,
            ScanCode.F3,
            ScanCode.F4,
            ScanCode.F5,
            ScanCode.F6,
            ScanCode.F7,
            ScanCode.F8,
            ScanCode.F9,
            ScanCode.F10,
            ScanCode.NumLock,
            ScanCode.ScrollLock,
            ScanCode.Kp7,
            ScanCode.Kp8,
            ScanCode.Kp9,
            ScanCode.KpMinus,
            ScanCode.Kp4,
            ScanCode.Kp5,
            ScanCode.Kp6,
            ScanCode.KpPlus,
            ScanCode.Kp1,
            ScanCode.Kp2,
            ScanCode.Kp3,
            ScanCode.Kp0,
            ScanCode.KpPeriod,
            ScanCode.Unknown,
            ScanCode.Unknown,
            ScanCode.Unknown,
            ScanCode.F11,
            ScanCode.F12,
            ScanCode.Pause,
            ScanCode.Unknown,
            ScanCode.LeftMeta,
            ScanCode.RightMeta,
            ScanCode.Application
        };

        public static ScanCode[] WinToOwScanCodeEx =
        {
            ScanCode.Unknown,
            ScanCode.Escape,
            ScanCode.D1,
            ScanCode.D2,
            ScanCode.D3,
            ScanCode.D4,
            ScanCode.D5,
            ScanCode.D6,
            ScanCode.D7,
            ScanCode.D8,
            ScanCode.D9,
            ScanCode.D0,
            ScanCode.Minus,
            ScanCode.Equals,
            ScanCode.Backspace,
            ScanCode.Tab,
            ScanCode.Q,
            ScanCode.W,
            ScanCode.E,
            ScanCode.R,
            ScanCode.T,
            ScanCode.Y,
            ScanCode.U,
            ScanCode.I,
            ScanCode.O,
            ScanCode.P,
            ScanCode.LeftSquareBracket,
            ScanCode.RightSquareBracket,
            ScanCode.KpEnter,
            ScanCode.RightControl,
            ScanCode.A,
            ScanCode.S,
            ScanCode.D,
            ScanCode.F,
            ScanCode.G,
            ScanCode.H,
            ScanCode.J,
            ScanCode.K,
            ScanCode.L,
            ScanCode.Semicolon,
            ScanCode.Quote,
            ScanCode.Backquote,
            ScanCode.LeftShift,
            ScanCode.Backslash,
            ScanCode.Z,
            ScanCode.X,
            ScanCode.C,
            ScanCode.V,
            ScanCode.B,
            ScanCode.N,
            ScanCode.M,
            ScanCode.Comma,
            ScanCode.Period,
            ScanCode.KpDivide,
            ScanCode.RightShift,
            ScanCode.PrintScreen,
            ScanCode.RightAlt,
            ScanCode.Space,
            ScanCode.CapsLock,
            ScanCode.F1,
            ScanCode.F2,
            ScanCode.F3,
            ScanCode.F4,
            ScanCode.F5,
            ScanCode.F6,
            ScanCode.F7,
            ScanCode.F8,
            ScanCode.F9,
            ScanCode.F10,
            ScanCode.NumLock,
            ScanCode.ScrollLock,
            ScanCode.Home,
            ScanCode.Up,
            ScanCode.PageUp,
            ScanCode.KpMinus,
            ScanCode.Left,
            ScanCode.Kp5,
            ScanCode.Right,
            ScanCode.KpPlus,
            ScanCode.End,
            ScanCode.Down,
            ScanCode.PageDown,
            ScanCode.Insert,
            ScanCode.Delete,
            ScanCode.Unknown,
            ScanCode.Unknown,
            ScanCode.Unknown,
            ScanCode.F11,
            ScanCode.F12,
            ScanCode.Pause,
            ScanCode.Unknown,
            ScanCode.Unknown,
            ScanCode.Unknown,
            ScanCode.F11,
            ScanCode.F12,
            ScanCode.Pause,
            ScanCode.Unknown,
            ScanCode.LeftMeta,
            ScanCode.RightMeta,
            ScanCode.Application,
        };

        public static Key[] VkToKey =
        {
            Key.Unknown,   // 0x00
            Key.Unknown,   // 0x01
            Key.Unknown,   // 0x02
            Key.Unknown,   // 0x03
            Key.Unknown,   // 0x04
            Key.Unknown,   // 0x05
            Key.Unknown,   // 0x06
            Key.Unknown,   // 0x07
            Key.Backspace, // 0x08
            Key.Tab,       // 0x09
            Key.Unknown,   // 0x0A
            Key.Unknown,   // 0x0B
            Key.Kp5,   // 0x0C
            Key.Enter,     // 0x0D
            Key.Unknown,   // 0x0E
            Key.Unknown,   // 0x0F
            Key.Unknown,   // 0x10 (Shift)
            Key.Unknown,   // 0x11 (Control)
            Key.Unknown,   // 0x12 (Alt)
            Key.Unknown,   // 0x13
            Key.CapsLock,  // 0x14
            Key.Unknown,   // 0x15
            Key.Unknown,   // 0x16
            Key.Unknown,   // 0x17
            Key.Unknown,   // 0x18
            Key.Unknown,   // 0x19
            Key.Unknown,   // 0x1A
            Key.Escape,    // 0x1B
            Key.Unknown,   // 0x1C (IME Convert)
            Key.Unknown,   // 0x1D (IME NonConvert)
            Key.Unknown,   // 0x1E (IME Accept)
            Key.Unknown,   // 0x1F (IME Mode change request)
            Key.Space,     // 0x20
            Key.Kp9,    // 0x21
            Key.Kp3,  // 0x22
            Key.Kp1,       // 0x23
            Key.Kp7,      // 0x24
            Key.Kp4,      // 0x25
            Key.Kp8,        // 0x26
            Key.Kp6,     // 0x27
            Key.Kp2,      // 0x28
            Key.Unknown,   // 0x29
            Key.Unknown,   // 0x2A
            Key.Unknown,   // 0x2B
            Key.PrintScreen, // 0x2C
            Key.Kp0,    // 0x2D
            Key.KpPeriod,    // 0x2E
            Key.Unknown,   // 0x2F
            Key.D0,   // 0x30
            Key.D1,   // 0x31
            Key.D2,   // 0x32
            Key.D3,   // 0x33
            Key.D4,   // 0x34
            Key.D5,   // 0x35
            Key.D6,   // 0x36
            Key.D7,   // 0x37
            Key.D8,   // 0x38
            Key.D9,   // 0x39
            Key.Unknown, // 0x3A
            Key.Unknown, // 0x3B
            Key.Unknown, // 0x3C
            Key.Unknown, // 0x3D
            Key.Unknown, // 0x3E
            Key.Unknown, // 0x3F
            Key.Unknown, // 0x40
            Key.A,    // 0x41
            Key.B,    // 0x42
            Key.C,    // 0x43
            Key.D,    // 0x44
            Key.E,    // 0x45
            Key.F,    // 0x46
            Key.G,    // 0x47
            Key.H,    // 0x48
            Key.I,    // 0x49
            Key.J,    // 0x4A
            Key.K,    // 0x4B
            Key.L,    // 0x4C
            Key.M,    // 0x4D
            Key.N,    // 0x4E
            Key.O,    // 0x4F
            Key.P,    // 0x50
            Key.Q,    // 0x51
            Key.R,    // 0x52
            Key.S,    // 0x53
            Key.T,    // 0x54
            Key.U,    // 0x55
            Key.V,    // 0x56
            Key.W,    // 0x57
            Key.X,    // 0x58
            Key.Y,    // 0x59
            Key.Z,    // 0x5A
            Key.LeftMeta,  // 0x5B
            Key.RightMeta, // 0x5C
            Key.Application, // 0x5D
            Key.Unknown, // 0x5E
            Key.Unknown, // 0x5F
            Key.Kp0,  // 0x60
            Key.Kp1,  // 0x61
            Key.Kp2,  // 0x62
            Key.Kp3,  // 0x63
            Key.Kp4,  // 0x64
            Key.Kp5,  // 0x65
            Key.Kp6,  // 0x66
            Key.Kp7,  // 0x67
            Key.Kp8,  // 0x68
            Key.Kp9,  // 0x69
            Key.KpMultiply, // 0x6A
            Key.KpPlus, // 0x6B
            Key.Unknown, // 0x6C
            Key.KpMinus, // 0x6D
            Key.KpPeriod, // 0x6E
            Key.KpDivide, // 0x6F
            Key.F1, // 0x70
            Key.F2, // 0x71
            Key.F3, // 0x72
            Key.F4, // 0x73
            Key.F5, // 0x74
            Key.F6, // 0x75
            Key.F7, // 0x76
            Key.F8, // 0x77
            Key.F9, // 0x78
            Key.F10, // 0x79
            Key.F11, // 0x7A
            Key.F12, // 0x7B
            Key.F13, // 0x7C
            Key.F14, // 0x7D
            Key.F15, // 0x7E
            Key.F16, // 0x7F
            Key.F17, // 0x80
            Key.F18, // 0x81
            Key.F19, // 0x82
            Key.F20, // 0x83
            Key.F21, // 0x84
            Key.F22, // 0x85
            Key.F23, // 0x86
            Key.F24, // 0x87
            Key.Unknown, // 0x88
            Key.Unknown, // 0x89
            Key.Unknown, // 0x8A
            Key.Unknown, // 0x8B
            Key.Unknown, // 0x8C
            Key.Unknown, // 0x8D
            Key.Unknown, // 0x8E
            Key.Unknown, // 0x8F
            Key.NumLock, // 0x90
            Key.ScrollLock, // 0x91
            Key.Unknown, // 0x92
            Key.Unknown, // 0x93
            Key.Unknown, // 0x94
            Key.Unknown, // 0x95
            Key.Unknown, // 0x96
            Key.Unknown, // 0x97
            Key.Unknown, // 0x98
            Key.Unknown, // 0x99
            Key.Unknown, // 0x9A
            Key.Unknown, // 0x9B
            Key.Unknown, // 0x9C
            Key.Unknown, // 0x9D
            Key.Unknown, // 0x9E
            Key.Unknown, // 0x9F
            Key.LeftShift, // 0xA0
            Key.RightShift, // 0xA1
            Key.LeftControl, // 0xA2
            Key.RightControl, // 0xA3
            Key.LeftAlt, // 0xA4
            Key.RightAlt, // 0xA5
            Key.Unknown, // 0xA6
            Key.Unknown, // 0xA7
            Key.Unknown, // 0xA8
            Key.Unknown, // 0xA9
            Key.Unknown, // 0xAA
            Key.Unknown, // 0xAB
            Key.Unknown, // 0xAC
            Key.Unknown, // 0xAD
            Key.Unknown, // 0xAE
            Key.Unknown, // 0xAF
            Key.Unknown, // 0xB0
            Key.Unknown, // 0xB1
            Key.Unknown, // 0xB2
            Key.Unknown, // 0xB3
            Key.Unknown, // 0xB4
            Key.Unknown, // 0xB5
            Key.Unknown, // 0xB6
            Key.Unknown, // 0xB7
            Key.Unknown, // 0xB8
            Key.Unknown, // 0xB9
            Key.Semicolon, // 0xBA
            Key.Equals, // 0xBB
            Key.Comma,   // 0xBC
            Key.Minus, // 0xBD
            Key.Period, // 0xBE
            Key.Slash, // 0xBF
            Key.Backquote, // 0xC0
            Key.Unknown, // 0xC1
            Key.Unknown, // 0xC2
            Key.Unknown, // 0xC3
            Key.Unknown, // 0xC4
            Key.Unknown, // 0xC5
            Key.Unknown, // 0xC6
            Key.Unknown, // 0xC7
            Key.Unknown, // 0xC8
            Key.Unknown, // 0xC9
            Key.Unknown, // 0xCA
            Key.Unknown, // 0xCB
            Key.Unknown, // 0xCC
            Key.Unknown, // 0xCD
            Key.Unknown, // 0xCE
            Key.Unknown, // 0xCF
            Key.Unknown, // 0xD0
            Key.Unknown, // 0xD1
            Key.Unknown, // 0xD2
            Key.Unknown, // 0xD3
            Key.Unknown, // 0xD4
            Key.Unknown, // 0xD5
            Key.Unknown, // 0xD6
            Key.Unknown, // 0xD7
            Key.Unknown, // 0xD8
            Key.Unknown, // 0xD9
            Key.Unknown, // 0xDA
            Key.LeftSquareBracket, // 0xDB
            Key.Backslash, // 0xDC
            Key.RightSquareBracket, // 0xDD
            Key.Apostrophe, // 0xDE
            Key.Unknown,   // 0xDF
            Key.Unknown,   // 0xE0
            Key.Unknown,   // 0xE1
            Key.Unknown,   // 0xE2
            Key.Unknown,   // 0xE3
            Key.Unknown,   // 0xE4
            Key.Unknown,   // 0xE5
            Key.Unknown,   // 0xE6
            Key.Unknown,   // 0xE7
            Key.Unknown,   // 0xE8
            Key.Unknown,   // 0xE9
            Key.RightMeta, // 0xEA
            Key.Unknown,   // 0xEB
            Key.Unknown,   // 0xEC
            Key.Unknown,   // 0xED
            Key.Unknown,   // 0xEE
            Key.Unknown,   // 0xEF
            Key.Unknown,   // 0xF0
            Key.LeftMeta,  // 0xF1
            Key.Unknown,   // 0xF2
            Key.Unknown,   // 0xF3
            Key.Unknown,   // 0xF4
            Key.Unknown,   // 0xF5
            Key.Unknown,   // 0xF6
            Key.Unknown,   // 0xF7
            Key.Unknown,   // 0xF8
            Key.Application, // 0xF9
        };

        public static int[] ExtendedKeys =
        {
        };
    }
}
