namespace OpenWindow.Backends.Wayland
{
    internal static class WaylandKeyMaps
    {
        public static ScanCode[] LinuxToOwScanCode =
        {
            ScanCode.Unknown, // #define KEY_RESERVED		0
            ScanCode.Escape,  // #define KEY_ESC			1
            ScanCode.D1,      // #define KEY_1			2
            ScanCode.D2, // #define KEY_2			3
            ScanCode.D3, // #define KEY_3			4
            ScanCode.D4, // #define KEY_4			5
            ScanCode.D5, // #define KEY_5			6
            ScanCode.D6, // #define KEY_6			7
            ScanCode.D7, // #define KEY_7			8
            ScanCode.D8, // #define KEY_8			9
            ScanCode.D9, // #define KEY_9			10
            ScanCode.D0, // #define KEY_0			11
            ScanCode.Minus, // #define KEY_MINUS		12
            ScanCode.Equals, // #define KEY_EQUAL		13
            ScanCode.Backspace, // #define KEY_BACKSPACE		14
            ScanCode.Tab, // #define KEY_TAB			15
            ScanCode.Q, // #define KEY_Q			16
            ScanCode.W, // #define KEY_W			17
            ScanCode.E, // #define KEY_E			18
            ScanCode.R, // #define KEY_R			19
            ScanCode.T, // #define KEY_T			20
            ScanCode.Y, // #define KEY_Y			21
            ScanCode.U, // #define KEY_U			22
            ScanCode.I, // #define KEY_I			23
            ScanCode.O, // #define KEY_O			24
            ScanCode.P, // #define KEY_P			25
            ScanCode.LeftSquareBracket, // #define KEY_LEFTBRACE		26
            ScanCode.RightSquareBracket, // #define KEY_RIGHTBRACE		27
            ScanCode.Enter, // #define KEY_ENTER		28
            ScanCode.LeftControl, // #define KEY_LEFTCTRL		29
            ScanCode.A, // #define KEY_A			30
            ScanCode.S, // #define KEY_S			31
            ScanCode.D, // #define KEY_D			32
            ScanCode.F, // #define KEY_F			33
            ScanCode.G, // #define KEY_G			34
            ScanCode.H, // #define KEY_H			35
            ScanCode.J, // #define KEY_J			36
            ScanCode.K, // #define KEY_K			37
            ScanCode.L, // #define KEY_L			38
            ScanCode.Semicolon, // #define KEY_SEMICOLON		39
            ScanCode.Apostrophe, // #define KEY_APOSTROPHE		40
            ScanCode.Backquote, // #define KEY_GRAVE		41
            ScanCode.LeftShift, // #define KEY_LEFTSHIFT		42
            ScanCode.Backslash, // #define KEY_BACKSLASH		43
            ScanCode.Z, // #define KEY_Z			44
            ScanCode.X, // #define KEY_X			45
            ScanCode.C, // #define KEY_C			46
            ScanCode.V, // #define KEY_V			47
            ScanCode.B, // #define KEY_B			48
            ScanCode.N, // #define KEY_N			49
            ScanCode.M, // #define KEY_M			50
            ScanCode.Comma, // #define KEY_COMMA		51
            ScanCode.Period, // #define KEY_DOT			52
            ScanCode.Slash, // #define KEY_SLASH		53
            ScanCode.RightShift, // #define KEY_RIGHTSHIFT		54
            ScanCode.KpMultiply, // #define KEY_KPASTERISK		55
            ScanCode.LeftAlt, // #define KEY_LEFTALT		56
            ScanCode.Space, // #define KEY_SPACE		57
            ScanCode.CapsLock, // #define KEY_CAPSLOCK		58
            ScanCode.F1, // #define KEY_F1			59
            ScanCode.F2, // #define KEY_F2			60
            ScanCode.F3, // #define KEY_F3			61
            ScanCode.F4, // #define KEY_F4			62
            ScanCode.F5, // #define KEY_F5			63
            ScanCode.F6, // #define KEY_F6			64
            ScanCode.F7, // #define KEY_F7			65
            ScanCode.F8, // #define KEY_F8			66
            ScanCode.F9, // #define KEY_F9			67
            ScanCode.F10, // #define KEY_F10			68
            ScanCode.NumLock, // #define KEY_NUMLOCK		69
            ScanCode.ScrollLock, // #define KEY_SCROLLLOCK		70
            ScanCode.Kp7, // #define KEY_KP7			71
            ScanCode.Kp8, // #define KEY_KP8			72
            ScanCode.Kp9, // #define KEY_KP9			73
            ScanCode.KpMinus, // #define KEY_KPMINUS		74
            ScanCode.Kp4, // #define KEY_KP4			75
            ScanCode.Kp5, // #define KEY_KP5			76
            ScanCode.Kp6, // #define KEY_KP6			77
            ScanCode.KpPlus, // #define KEY_KPPLUS		78
            ScanCode.Kp1, // #define KEY_KP1			79
            ScanCode.Kp2, // #define KEY_KP2			80
            ScanCode.Kp3, // #define KEY_KP3			81
            ScanCode.Kp0, // #define KEY_KP0			82
            ScanCode.KpPeriod, // #define KEY_KPDOT		83
            ScanCode.Unknown, // 
            ScanCode.Unknown, // #define KEY_ZENKAKUHANKAKU	85
            ScanCode.Unknown, // #define KEY_102ND		86
            ScanCode.F11, // #define KEY_F11			87
            ScanCode.F12, // #define KEY_F12			88
            ScanCode.Unknown, // #define KEY_RO			89
            ScanCode.Unknown, // #define KEY_KATAKANA		90
            ScanCode.Unknown, // #define KEY_HIRAGANA		91
            ScanCode.Unknown, // #define KEY_HENKAN		92
            ScanCode.Unknown, // #define KEY_KATAKANAHIRAGANA	93
            ScanCode.Unknown, // #define KEY_MUHENKAN		94
            ScanCode.Unknown, // #define KEY_KPJPCOMMA		95
            ScanCode.KpEnter, // #define KEY_KPENTER		96
            ScanCode.RightControl, // #define KEY_RIGHTCTRL		97
            ScanCode.KpDivide, // #define KEY_KPSLASH		98
            ScanCode.SysRq, // #define KEY_SYSRQ		99
            ScanCode.RightAlt, // #define KEY_RIGHTALT		100
            ScanCode.Unknown, // #define KEY_LINEFEED		101
            ScanCode.Home, // #define KEY_HOME		102
            ScanCode.Up, // #define KEY_UP			103
            ScanCode.PageUp, // #define KEY_PAGEUP		104
            ScanCode.Left, // #define KEY_LEFT		105
            ScanCode.Right, // #define KEY_RIGHT		106
            ScanCode.End, // #define KEY_END			107
            ScanCode.Down, // #define KEY_DOWN		108
            ScanCode.PageDown, // #define KEY_PAGEDOWN		109
            ScanCode.Insert, // #define KEY_INSERT		110
            ScanCode.Delete, // #define KEY_DELETE		111
            ScanCode.Unknown, // #define KEY_MACRO		112
            ScanCode.Unknown, // #define KEY_MUTE		113
            ScanCode.Unknown, // #define KEY_VOLUMEDOWN		114
            ScanCode.Unknown, // #define KEY_VOLUMEUP		115
            ScanCode.Unknown, // #define KEY_POWER		116	/* SC System Power Down */
            ScanCode.Unknown, // #define KEY_KPEQUAL		117
            ScanCode.Unknown, // #define KEY_KPPLUSMINUS		118
            ScanCode.Pause, // #define KEY_PAUSE		119
            ScanCode.Unknown, // #define KEY_SCALE		120	/* AL Compiz Scale (Expose) */

            ScanCode.Unknown, // #define KEY_KPCOMMA		121
            ScanCode.Unknown, // #define KEY_HANGEUL		122
            // #define KEY_HANGUEL		KEY_HANGEUL
            ScanCode.Unknown, // #define KEY_HANJA		123
            ScanCode.Unknown, // #define KEY_YEN			124
            ScanCode.LeftMeta, // #define KEY_LEFTMETA		125
            ScanCode.RightMeta, // #define KEY_RIGHTMETA		126
            ScanCode.Menu, // #define KEY_COMPOSE		127

            ScanCode.Unknown, // #define KEY_STOP		128	/* AC Stop */
            ScanCode.Unknown, // #define KEY_AGAIN		129
            ScanCode.Unknown, // #define KEY_PROPS		130	/* AC Properties */
            ScanCode.Unknown, // #define KEY_UNDO		131	/* AC Undo */
            ScanCode.Unknown, // #define KEY_FRONT		132
            ScanCode.Unknown, // #define KEY_COPY		133	/* AC Copy */
            ScanCode.Unknown, // #define KEY_OPEN		134	/* AC Open */
            ScanCode.Unknown, // #define KEY_PASTE		135	/* AC Paste */
            ScanCode.Unknown, // #define KEY_FIND		136	/* AC Search */
            ScanCode.Unknown, // #define KEY_CUT			137	/* AC Cut */
            ScanCode.Unknown, // #define KEY_HELP		138	/* AL Integrated Help Center */
            ScanCode.Menu, // #define KEY_MENU		139	/* Menu (show menu) */
            ScanCode.Unknown, // #define KEY_CALC		140	/* AL Calculator */
            ScanCode.Unknown, // #define KEY_SETUP		141
            ScanCode.Unknown, // #define KEY_SLEEP		142	/* SC System Sleep */
            ScanCode.Unknown, // #define KEY_WAKEUP		143	/* System Wake Up */
            ScanCode.Unknown, // #define KEY_FILE		144	/* AL Local Machine Browser */
            ScanCode.Unknown, // #define KEY_SENDFILE		145
            ScanCode.Unknown, // #define KEY_DELETEFILE		146
            ScanCode.Unknown, // #define KEY_XFER		147
            ScanCode.Unknown, // #define KEY_PROG1		148
            ScanCode.Unknown, // #define KEY_PROG2		149
            ScanCode.Unknown, // #define KEY_WWW			150	/* AL Internet Browser */
            ScanCode.Unknown, // #define KEY_MSDOS		151
            ScanCode.Unknown, // #define KEY_COFFEE		152	/* AL Terminal Lock/Screensaver */
            ScanCode.Unknown, // #define KEY_SCREENLOCK		KEY_COFFEE
            ScanCode.Unknown, // #define KEY_ROTATE_DISPLAY	153	/* Display orientation for e.g. tablets */
            ScanCode.Unknown, // #define KEY_DIRECTION		KEY_ROTATE_DISPLAY
            ScanCode.Unknown, // #define KEY_CYCLEWINDOWS	154
            ScanCode.Unknown, // #define KEY_MAIL		155
            ScanCode.Unknown, // #define KEY_BOOKMARKS		156	/* AC Bookmarks */
            ScanCode.Unknown, // #define KEY_COMPUTER		157
            ScanCode.Unknown, // #define KEY_BACK		158	/* AC Back */
            ScanCode.Unknown, // #define KEY_FORWARD		159	/* AC Forward */
            ScanCode.Unknown, // #define KEY_CLOSECD		160
            ScanCode.Unknown, // #define KEY_EJECTCD		161
            ScanCode.Unknown, // #define KEY_EJECTCLOSECD	162
            ScanCode.Unknown, // #define KEY_NEXTSONG		163
            ScanCode.Unknown, // #define KEY_PLAYPAUSE		164
            ScanCode.Unknown, // #define KEY_PREVIOUSSONG	165
            ScanCode.Unknown, // #define KEY_STOPCD		166
            ScanCode.Unknown, // #define KEY_RECORD		167
            ScanCode.Unknown, // #define KEY_REWIND		168
            ScanCode.Unknown, // #define KEY_PHONE		169	/* Media Select Telephone */
            ScanCode.Unknown, // #define KEY_ISO			170
            ScanCode.Unknown, // #define KEY_CONFIG		171	/* AL Consumer Control Configuration */
            ScanCode.Unknown, // #define KEY_HOMEPAGE		172	/* AC Home */
            ScanCode.Unknown, // #define KEY_REFRESH		173	/* AC Refresh */
            ScanCode.Unknown, // #define KEY_EXIT		174	/* AC Exit */
            ScanCode.Unknown, // #define KEY_MOVE		175
            ScanCode.Unknown, // #define KEY_EDIT		176
            ScanCode.Unknown, // #define KEY_SCROLLUP		177
            ScanCode.Unknown, // #define KEY_SCROLLDOWN		178
            ScanCode.Unknown, // #define KEY_KPLEFTPAREN		179
            ScanCode.Unknown, // #define KEY_KPRIGHTPAREN	180
            ScanCode.Unknown, // #define KEY_NEW			181	/* AC New */
            ScanCode.Unknown, // #define KEY_REDO		182	/* AC Redo/Repeat */

            ScanCode.F13, // #define KEY_F13			183
            ScanCode.F14, // #define KEY_F14			184
            ScanCode.F15, // #define KEY_F15			185
            ScanCode.F16, // #define KEY_F16			186
            ScanCode.F17, // #define KEY_F17			187
            ScanCode.F18, // #define KEY_F18			188
            ScanCode.F19, // #define KEY_F19			189
            ScanCode.F20, // #define KEY_F20			190
            ScanCode.F21, // #define KEY_F21			191
            ScanCode.F22, // #define KEY_F22			192
            ScanCode.F23, // #define KEY_F23			193
            ScanCode.F24, // #define KEY_F24			194
        };

        private static Key[] XkbToOwKey00 =
        {
            // 0x000x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,
            // 0x001x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,

            // 0x002x
            Key.Unknown,     //                                      0x0020
            Key.Unknown,     //                                      0x0021
            Key.Unknown,     //                                      0x0022
            Key.Unknown,     //                                      0x0023
            Key.Unknown,     //                                      0x0024
            Key.Unknown,     //                                      0x0025
            Key.Unknown,     //                                      0x0026
            Key.Apostrophe,  //                                      0x0027
            Key.Unknown,     //                                      0x0028
            Key.Unknown,     //                                      0x0029
            Key.Unknown,     //                                      0x002a
            Key.Unknown,     //                                      0x002b
            Key.Comma,       //                                      0x002c
            Key.Minus,       //                                      0x002d
            Key.Period,      //                                      0x002e
            Key.Slash,       //                                      0x002f

            Key.D0,          // #define XKB_KEY_0                    0x0030  /* U+0030 DIGIT ZERO */
            Key.D1,          // #define XKB_KEY_1                    0x0031  /* U+0031 DIGIT ONE */
            Key.D2,          // #define XKB_KEY_2                    0x0032  /* U+0032 DIGIT TWO */
            Key.D3,          // #define XKB_KEY_3                    0x0033  /* U+0033 DIGIT THREE */
            Key.D4,          // #define XKB_KEY_4                    0x0034  /* U+0034 DIGIT FOUR */
            Key.D5,          // #define XKB_KEY_5                    0x0035  /* U+0035 DIGIT FIVE */
            Key.D6,          // #define XKB_KEY_6                    0x0036  /* U+0036 DIGIT SIX */
            Key.D7,          // #define XKB_KEY_7                    0x0037  /* U+0037 DIGIT SEVEN */
            Key.D8,          // #define XKB_KEY_8                    0x0038  /* U+0038 DIGIT EIGHT */
            Key.D9,          // #define XKB_KEY_9                    0x0039  /* U+0039 DIGIT NINE */

            Key.Unknown,     // #define                              0x003a
            Key.Semicolon,   // #define                              0x003b
            Key.Unknown,     // #define                              0x003c
            Key.Equals,      // #define                              0x003d
            Key.Unknown,     // #define                              0x003e
            Key.Unknown,     // #define                              0x003f

            // 0x004x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,

            Key.Unknown,     //                                      0x0050
            Key.Unknown,     //                                      0x0051
            Key.Unknown,     //                                      0x0052
            Key.Unknown,     //                                      0x0053
            Key.Unknown,     //                                      0x0054
            Key.Unknown,     //                                      0x0055
            Key.Unknown,     //                                      0x0056
            Key.Unknown,     //                                      0x0057
            Key.Unknown,     //                                      0x0058
            Key.Unknown,     //                                      0x0059
            Key.Unknown,     //                                      0x005a
            Key.LeftSquareBracket, //                                0x005b
            Key.Backslash,   //                                      0x005c
            Key.RightSquareBracket, //                               0x005d
            Key.Unknown,     //                                      0x005e
            Key.Unknown,     //                                      0x005f

            // 0x006x
            Key.Tilde,
            Key.A,           //                                      0x0061
            Key.B,           //                                      0x0062
            Key.C,           //                                      0x0063
            Key.D,           //                                      0x0064
            Key.E,           //                                      0x0065
            Key.F,           //                                      0x0066
            Key.G,           //                                      0x0067
            Key.H,           //                                      0x0068
            Key.I,           //                                      0x0069
            Key.J,           //                                      0x006a
            Key.K,           //                                      0x006b
            Key.L,           //                                      0x006c
            Key.M,           //                                      0x006d
            Key.N,           //                                      0x006e
            Key.O,           //                                      0x006f

            Key.P,           //                                      0x0070
            Key.Q,           //                                      0x0071
            Key.R,           //                                      0x0072
            Key.S,           //                                      0x0073
            Key.T,           //                                      0x0074
            Key.U,           //                                      0x0075
            Key.V,           //                                      0x0076
            Key.W,           //                                      0x0077
            Key.X,           //                                      0x0078
            Key.Y,           //                                      0x0079
            Key.Z,           //                                      0x007a
            Key.Unknown,     //                                      0x007b
            Key.Unknown,     //                                      0x007c
            Key.Unknown,     //                                      0x007d
            Key.Unknown,     //                                      0x007e
            Key.Unknown,     //                                      0x007f
        };

        private static Key[] XkbToOwKeyFf =
        {
            Key.Unknown,     //                                      0xff00
            Key.Unknown,     //                                      0xff01
            Key.Unknown,     //                                      0xff02
            Key.Unknown,     //                                      0xff03
            Key.Unknown,     //                                      0xff04
            Key.Unknown,     //                                      0xff05
            Key.Unknown,     //                                      0xff06
            Key.Unknown,     //                                      0xff07
            Key.Backspace,   // #define XKB_KEY_BackSpace            0xff08  /* Back space, back char */
            Key.Tab,         // #define XKB_KEY_Tab                  0xff09
            Key.Unknown,     // #define XKB_KEY_Linefeed             0xff0a  /* Linefeed, LF */
            Key.Unknown,     // #define XKB_KEY_Clear                0xff0b
            Key.Unknown,     //                                      0xff0c
            Key.Enter,       // #define XKB_KEY_Return               0xff0d  /* Return, enter */
            Key.Unknown,     //                                      0xff0e
            Key.Unknown,     //                                      0xff0f
            Key.Unknown,     //                                      0xff10
            Key.Unknown,     //                                      0xff11
            Key.Unknown,     //                                      0xff12
            Key.Pause,       // #define XKB_KEY_Pause                0xff13  /* Pause, hold */
            Key.ScrollLock,  // #define XKB_KEY_Scroll_Lock          0xff14
            Key.PrintScreen, // #define XKB_KEY_Sys_Req              0xff15
            Key.Unknown,     //                                      0xff16
            Key.Unknown,     //                                      0xff17
            Key.Unknown,     //                                      0xff18
            Key.Unknown,     //                                      0xff19
            Key.Unknown,     //                                      0xff1a
            Key.Escape,      // #define XKB_KEY_Escape               0xff1b
            Key.Unknown,     //                                      0xff1c
            Key.Unknown,     //                                      0xff1d
            Key.Unknown,     //                                      0xff1e
            Key.Unknown,     //                                      0xff1f

            // 0xff2x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,
            // 0xff3x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,
            // 0xff4x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,
            // 0xff5x
            Key.Home,        //                                      0xff50
            Key.Left,        //                                      0xff51
            Key.Up,          //                                      0xff52
            Key.Right,       //                                      0xff53
            Key.Down,        //                                      0xff54
            Key.PageUp,      //                                      0xff55
            Key.PageDown,    //                                      0xff56
            Key.End,         //                                      0xff57
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,

            // 0xff6x
            Key.Unknown, Key.Unknown, Key.Unknown,
            Key.Insert,      //                                      0xff63
            Key.Unknown, Key.Unknown, Key.Unknown,
            Key.Menu,        //                                      0xff67
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,

            // 0xff7x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,
            Key.NumLock,     //                                      0xff7f

            // 0xff8x
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,
            Key.KpEnter,     //                                      0xff8d
            Key.Unknown,     //                                      0xff8e
            Key.Unknown,     //                                      0xff8f

            Key.Unknown,     //                                      0xff90
            Key.Unknown,     //                                      0xff91
            Key.Unknown,     //                                      0xff92
            Key.Unknown,     //                                      0xff93
            Key.Unknown,     //                                      0xff94
            Key.Kp7,         //                                      0xff95
            Key.Kp4,         //                                      0xff96
            Key.Kp8,         //                                      0xff97
            Key.Kp6,         //                                      0xff98
            Key.Kp2,         //                                      0xff99
            Key.Kp9,         //                                      0xff9a
            Key.Kp3,         //                                      0xff9b
            Key.Kp1,         //                                      0xff9c
            Key.Kp5,         //                                      0xff9d
            Key.Kp0,         //                                      0xff9e
            Key.Delete,      //                                      0xff9f

            // 0xffax
            Key.Unknown,     //                                      0xffa0
            Key.Unknown,     //                                      0xffa1
            Key.Unknown,     //                                      0xffa2
            Key.Unknown,     //                                      0xffa3
            Key.Unknown,     //                                      0xffa4
            Key.Unknown,     //                                      0xffa5
            Key.Unknown,     //                                      0xffa6
            Key.Unknown,     //                                      0xffa7
            Key.Unknown,     //                                      0xffa8
            Key.Unknown,     //                                      0xffa9
            Key.KpMultiply,  //                                      0xffaa
            Key.KpPlus,      //                                      0xffab
            Key.Unknown,     //                                      0xffac
            Key.KpMinus,     //                                      0xffad
            Key.Unknown,     //                                      0xffae
            Key.KpDivide,    //                                      0xffaf

            // 0xffbx
            Key.Kp0,         //                                      0xffb0
            Key.Kp1,         //                                      0xffb1
            Key.Kp2,         //                                      0xffb2
            Key.Kp3,         //                                      0xffb3
            Key.Kp4,         //                                      0xffb4
            Key.Kp5,         //                                      0xffb5
            Key.Kp6,         //                                      0xffb6
            Key.Kp7,         //                                      0xffb7
            Key.Kp8,         //                                      0xffb8
            Key.Kp9,         //                                      0xffb9
            Key.Unknown,     //                                      0xffba
            Key.Unknown,     //                                      0xffbb
            Key.Unknown,     //                                      0xffbc
            Key.Unknown,     //                                      0xffbd
            Key.F1,          //                                      0xffbe
            Key.F2,          //                                      0xffbf

            // 0xffcx
            Key.F3,          //                                      0xffc0
            Key.F4,          //                                      0xffc1
            Key.F5,          //                                      0xffc2
            Key.F6,          //                                      0xffc3
            Key.F7,          //                                      0xffc4
            Key.F8,          //                                      0xffc5
            Key.F9,          //                                      0xffc6
            Key.F10,         //                                      0xffc7
            Key.F11,         //                                      0xffc8
            Key.F12,         //                                      0xffc9
            Key.F13,         //                                      0xffca
            Key.F14,         //                                      0xffcb
            Key.F15,         //                                      0xffcc
            Key.F16,         //                                      0xffcd
            Key.F17,         //                                      0xffce
            Key.F18,         //                                      0xffcf

            // 0xffdx
            Key.F19,         //                                      0xffd0
            Key.F20,         //                                      0xffd1
            Key.F21,         //                                      0xffd2
            Key.F22,         //                                      0xffd3
            Key.F23,         //                                      0xffd4
            Key.F24,         //                                      0xffd5
            Key.Unknown,     //                                      0xffd6
            Key.Unknown,     //                                      0xffd7
            Key.Unknown,     //                                      0xffd8
            Key.Unknown,     //                                      0xffd9
            Key.Unknown,     //                                      0xffda
            Key.Unknown,     //                                      0xffdb
            Key.Unknown,     //                                      0xffdc
            Key.Unknown,     //                                      0xffdd
            Key.Unknown,     //                                      0xffde
            Key.Unknown,     //                                      0xffdf

            Key.Unknown,     //                                      0xffe0
            Key.LeftShift,   //                                      0xffe1
            Key.RightShift,  //                                      0xffe2
            Key.LeftControl, //                                      0xffe3
            Key.RightControl,//                                      0xffe4
            Key.CapsLock,    //                                      0xffe5
            Key.Unknown,     //                                      0xffe6
            Key.LeftMeta,    //                                      0xffe7
            Key.RightMeta,   //                                      0xffe8
            Key.LeftAlt,     //                                      0xffe9
            Key.RightAlt,    //                                      0xffea
            Key.Unknown,     //                                      0xffeb
            Key.Unknown,     //                                      0xffec
            Key.Unknown,     //                                      0xffed
            Key.Unknown,     //                                      0xffee
            Key.Unknown,     //                                      0xffef

            // 0xffex
            Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown, Key.Unknown,

            Key.Delete       // #define XKB_KEY_Delete               0xffff
        };

        public static Key XkbToOwKey(uint key)
        {
            if (key <= 0x7e)
                return XkbToOwKey00[key];
            
            if (key >= 0xff00 && key <= 0xffff)
                return XkbToOwKeyFf[key & 0x00ff];
            
            return Key.Unknown;
        }
    }
}
