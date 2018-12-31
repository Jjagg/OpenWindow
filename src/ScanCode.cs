namespace OpenWindow
{
    // Jjagg:
    //    These scancodes where taken directly from SDL.
    //    Here's the original license header:
    /*
      Simple DirectMedia Layer
      Copyright (C) 1997-2018 Sam Lantinga <slouken@libsdl.org>
      This software is provided 'as-is', without any express or implied
      warranty.  In no event will the authors be held liable for any damages
      arising from the use of this software.
      Permission is granted to anyone to use this software for any purpose,
      including commercial applications, and to alter it and redistribute it
      freely, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.
    */

    /// <summary>
    /// Physical keys.
    /// </summary>
    // TODO proper explanation of scan codes vs virtual key codes.
    public enum ScanCode
    {
        Unknown = 0,

        /**
        *  \name Usage page 0x07
        *
        *  These values are from usage page 0x07 (USB keyboard page).
        */
        /* @{ */

        /// <summary>
        /// 
        /// </summary>
        A = 4,
        /// <summary>
        /// 
        /// </summary>
        B = 5,
        /// <summary>
        /// 
        /// </summary>
        C = 6,
        /// <summary>
        /// 
        /// </summary>
        D = 7,
        /// <summary>
        /// 
        /// </summary>
        E = 8,
        /// <summary>
        /// 
        /// </summary>
        F = 9,
        /// <summary>
        /// 
        /// </summary>
        G = 10,
        /// <summary>
        /// 
        /// </summary>
        H = 11,
        /// <summary>
        /// 
        /// </summary>
        I = 12,
        /// <summary>
        /// 
        /// </summary>
        J = 13,
        /// <summary>
        /// 
        /// </summary>
        K = 14,
        /// <summary>
        /// 
        /// </summary>
        L = 15,
        /// <summary>
        /// 
        /// </summary>
        M = 16,
        /// <summary>
        /// 
        /// </summary>
        N = 17,
        /// <summary>
        /// 
        /// </summary>
        O = 18,
        /// <summary>
        /// 
        /// </summary>
        P = 19,
        /// <summary>
        /// 
        /// </summary>
        Q = 20,
        /// <summary>
        /// 
        /// </summary>
        R = 21,
        /// <summary>
        /// 
        /// </summary>
        S = 22,
        /// <summary>
        /// 
        /// </summary>
        T = 23,
        /// <summary>
        /// 
        /// </summary>
        U = 24,
        /// <summary>
        /// 
        /// </summary>
        V = 25,
        /// <summary>
        /// 
        /// </summary>
        W = 26,
        /// <summary>
        /// 
        /// </summary>
        X = 27,
        /// <summary>
        /// 
        /// </summary>
        Y = 28,
        /// <summary>
        /// 
        /// </summary>
        Z = 29,
        /// <summary>
        /// 
        /// </summary>

        D1 = 30,
        /// <summary>
        /// 
        /// </summary>
        D2 = 31,
        /// <summary>
        /// 
        /// </summary>
        D3 = 32,
        /// <summary>
        /// 
        /// </summary>
        D4 = 33,
        /// <summary>
        /// 
        /// </summary>
        D5 = 34,
        /// <summary>
        /// 
        /// </summary>
        D6 = 35,
        /// <summary>
        /// 
        /// </summary>
        D7 = 36,
        /// <summary>
        /// 
        /// </summary>
        D8 = 37,
        /// <summary>
        /// 
        /// </summary>
        D9 = 38,
        /// <summary>
        /// 
        /// </summary>
        D0 = 39,
        /// <summary>
        /// 
        /// </summary>

        Return = 40,
        /// <summary>
        /// 
        /// </summary>
        Escape = 41,
        /// <summary>
        /// 
        /// </summary>
        Backspace = 42,
        /// <summary>
        /// 
        /// </summary>
        Tab = 43,
        /// <summary>
        /// 
        /// </summary>
        Space = 44,

        /// <summary>
        /// 
        /// </summary>
        Minus = 45,
        /// <summary>
        /// 
        /// </summary>
        Equals = 46,
        /// <summary>
        /// 
        /// </summary>
        Leftbracket = 47,
        /// <summary>
        /// 
        /// </summary>
        Rightbracket = 48,
        /// <summary>
        /// 
        /// </summary>
        Backslash = 49, /**< Located at the lower left of the return
                         *   key on Iso keyboards and at the right end
                         *   of the Qwerty row on Ansi keyboards.
                         *   Produces Reverse Solidus (backslash) and
                         *   Vertical Line in a Us layout, Reverse
                         *   Solidus and Vertical Line in a Uk Mac
                         *   layout, Number Sign and Tilde in a Uk
                         *   Windows layout, Dollar Sign and Pound Sign
                         *   in a Swiss German layout, Number Sign and
                         *   Apostrophe in a German layout, Grave
                         *   Accent and Pound Sign in a French Mac
                         *   layout, and Asterisk and Micro Sign in a
                         *   French Windows layout.
                         */
        /// <summary>
        /// 
        /// </summary>
        Nonushash = 50, /**< Iso Usb keyboards actually use this code
                         *   instead of 49 for the same key, but all
                         *   Oses I've seen treat the two codes
                         *   identically. So, as an implementor, unless
                         *   your keyboard generates both of those
                         *   codes and your Os treats them differently,
                         *   you should generate Backslash
                         *   instead of this code. As a user, you
                         *   should not rely on this code because Sdl
                         *   will never generate it with most (all?)
                         *   keyboards.
                         */
        /// <summary>
        /// 
        /// </summary>
        Semicolon = 51,
        /// <summary>
        /// 
        /// </summary>
        Apostrophe = 52,
        /// <summary>
        /// 
        /// </summary>
        Grave = 53, /**< Located in the top left corner (on both Ansi
                     *   and Iso keyboards). Produces Grave Accent and
                     *   Tilde in a Us Windows layout and in Us and Uk
                     *   Mac layouts on Ansi keyboards, Grave Accent
                     *   and Not Sign in a Uk Windows layout, Section
                     *   Sign and Plus-Minus Sign in Us and Uk Mac
                     *   layouts on Iso keyboards, Section Sign and
                     *   Degree Sign in a Swiss German layout (Mac:
                     *   only on Iso keyboards), Circumflex Accent and
                     *   Degree Sign in a German layout (Mac: only on
                     *   Iso keyboards), Superscript Two and Tilde in a
                     *   French Windows layout, Commercial At and
                     *   Number Sign in a French Mac layout on Iso
                     *   keyboards, and Less-Than Sign and Greater-Than
                     *   Sign in a Swiss German, German, or French Mac
                     *   layout on Ansi keyboards.
                     */
        /// <summary>
        /// 
        /// </summary>
        Comma = 54,
        /// <summary>
        /// 
        /// </summary>
        Period = 55,
        /// <summary>
        /// 
        /// </summary>
        Slash = 56,

        /// <summary>
        /// 
        /// </summary>
        Capslock = 57,

        /// <summary>
        /// 
        /// </summary>
        F1 = 58,
        /// <summary>
        /// 
        /// </summary>
        F2 = 59,
        /// <summary>
        /// 
        /// </summary>
        F3 = 60,
        /// <summary>
        /// 
        /// </summary>
        F4 = 61,
        /// <summary>
        /// 
        /// </summary>
        F5 = 62,
        /// <summary>
        /// 
        /// </summary>
        F6 = 63,
        /// <summary>
        /// 
        /// </summary>
        F7 = 64,
        /// <summary>
        /// 
        /// </summary>
        F8 = 65,
        /// <summary>
        /// 
        /// </summary>
        F9 = 66,
        /// <summary>
        /// 
        /// </summary>
        F10 = 67,
        /// <summary>
        /// 
        /// </summary>
        F11 = 68,
        /// <summary>
        /// 
        /// </summary>
        F12 = 69,

        /// <summary>
        /// 
        /// </summary>
        Printscreen = 70,
        /// <summary>
        /// 
        /// </summary>
        Scrolllock = 71,
        /// <summary>
        /// 
        /// </summary>
        Pause = 72,
        /// <summary>
        /// 
        /// </summary>
        Insert = 73, /**< insert on Pc, help on some Mac keyboards (but
                          does send code 73, not 117) */
        Home = 74,
        /// <summary>
        /// 
        /// </summary>
        Pageup = 75,
        /// <summary>
        /// 
        /// </summary>
        Delete = 76,
        /// <summary>
        /// 
        /// </summary>
        End = 77,
        /// <summary>
        /// 
        /// </summary>
        Pagedown = 78,
        /// <summary>
        /// 
        /// </summary>
        Right = 79,
        /// <summary>
        /// 
        /// </summary>
        Left = 80,
        /// <summary>
        /// 
        /// </summary>
        Down = 81,
        /// <summary>
        /// 
        /// </summary>
        Up = 82,

        /// <summary>
        /// 
        /// </summary>
        Numlockclear = 83, /**< num lock on Pc, clear on Mac keyboards */
        /// <summary>
        /// 
        /// </summary>
        KpDivide = 84,
        /// <summary>
        /// 
        /// </summary>
        KpMultiply = 85,
        /// <summary>
        /// 
        /// </summary>
        KpMinus = 86,
        /// <summary>
        /// 
        /// </summary>
        KpPlus = 87,
        /// <summary>
        /// 
        /// </summary>
        KpEnter = 88,
        /// <summary>
        /// 
        /// </summary>
        Kp_1 = 89,
        /// <summary>
        /// 
        /// </summary>
        Kp_2 = 90,
        /// <summary>
        /// 
        /// </summary>
        Kp_3 = 91,
        /// <summary>
        /// 
        /// </summary>
        Kp_4 = 92,
        /// <summary>
        /// 
        /// </summary>
        Kp_5 = 93,
        /// <summary>
        /// 
        /// </summary>
        Kp_6 = 94,
        /// <summary>
        /// 
        /// </summary>
        Kp_7 = 95,
        /// <summary>
        /// 
        /// </summary>
        Kp_8 = 96,
        /// <summary>
        /// 
        /// </summary>
        Kp_9 = 97,
        /// <summary>
        /// 
        /// </summary>
        Kp_0 = 98,
        /// <summary>
        /// 
        /// </summary>
        KpPeriod = 99,

        /// <summary>
        /// 
        /// </summary>
        Nonusbackslash = 100, /**< This is the additional key that Iso
                               *   keyboards have over Ansi ones, 
                               *   located between left shift and Y.
                               *   Produces Grave Accent and Tilde in a
                               *   Us or Uk Mac layout, Reverse Solidus
                               *   (backslash) and Vertical Line in a
                               *   Us or Uk Windows layout, and
                               *   Less-Than Sign and Greater-Than Sign
                               *   in a Swiss German, German, or French
                               *   layout. */
        /// <summary>
        /// 
        /// </summary>
        Application = 101, /**< windows contextual menu, compose */
        /// <summary>
        /// 
        /// </summary>
        Power = 102, /**< The Usb document says this is a status flag, 
                      *   not a physical key - but some Mac keyboards
                      *   do have a power key. */
        /// <summary>
        /// 
        /// </summary>
        KpEquals = 103,
        /// <summary>
        /// 
        /// </summary>
        F13 = 104,
        /// <summary>
        /// 
        /// </summary>
        F14 = 105,
        /// <summary>
        /// 
        /// </summary>
        F15 = 106,
        /// <summary>
        /// 
        /// </summary>
        F16 = 107,
        /// <summary>
        /// 
        /// </summary>
        F17 = 108,
        /// <summary>
        /// 
        /// </summary>
        F18 = 109,
        /// <summary>
        /// 
        /// </summary>
        F19 = 110,
        /// <summary>
        /// 
        /// </summary>
        F20 = 111,
        /// <summary>
        /// 
        /// </summary>
        F21 = 112,
        /// <summary>
        /// 
        /// </summary>
        F22 = 113,
        /// <summary>
        /// 
        /// </summary>
        F23 = 114,
        /// <summary>
        /// 
        /// </summary>
        F24 = 115,
        /// <summary>
        /// 
        /// </summary>
        Execute = 116,
        /// <summary>
        /// 
        /// </summary>
        Help = 117,
        /// <summary>
        /// 
        /// </summary>
        Menu = 118,
        /// <summary>
        /// 
        /// </summary>
        Select = 119,
        /// <summary>
        /// 
        /// </summary>
        Stop = 120,
        /// <summary>
        /// 
        /// </summary>
        Again = 121, /**< redo */
        /// <summary>
        /// 
        /// </summary>
        Undo = 122,
        /// <summary>
        /// 
        /// </summary>
        Cut = 123,
        /// <summary>
        /// 
        /// </summary>
        Copy = 124,
        /// <summary>
        /// 
        /// </summary>
        Paste = 125,
        /// <summary>
        /// 
        /// </summary>
        Find = 126,
        /// <summary>
        /// 
        /// </summary>
        Mute = 127,
        /// <summary>
        /// 
        /// </summary>
        Volumeup = 128,
        /// <summary>
        /// 
        /// </summary>
        Volumedown = 129,
    /* not sure whether there's a reason to enable these */
    /*     Lockingcapslock = 130, */
    /*     Lockingnumlock = 131, */
    /*     Lockingscrolllock = 132, */
        /// <summary>
        /// 
        /// </summary>
        KpComma = 133,
        /// <summary>
        /// 
        /// </summary>
        KpEqualsas400 = 134,

        /// <summary>
        /// 
        /// </summary>
        International1 = 135, /**< used on Asian keyboards, see footnotes in Usb doc */
        /// <summary>
        /// 
        /// </summary>
        International2 = 136,
        /// <summary>
        /// 
        /// </summary>
        International3 = 137,
        /// <summary>
        /// 
        /// </summary> /**< Yen */
        International4 = 138,
        /// <summary>
        /// 
        /// </summary>
        International5 = 139,
        /// <summary>
        /// 
        /// </summary>
        International6 = 140,
        /// <summary>
        /// 
        /// </summary>
        International7 = 141,
        /// <summary>
        /// 
        /// </summary>
        International8 = 142,
        /// <summary>
        /// 
        /// </summary>
        International9 = 143,
        /// <summary>
        /// 
        /// </summary>
        Lang1 = 144, /**< Hangul/English toggle */
        /// <summary>
        /// 
        /// </summary>
        Lang2 = 145, /**< Hanja conversion */
        /// <summary>
        /// 
        /// </summary>
        Lang3 = 146, /**< Katakana */
        /// <summary>
        /// 
        /// </summary>
        Lang4 = 147, /**< Hiragana */
        /// <summary>
        /// 
        /// </summary>
        Lang5 = 148, /**< Zenkaku/Hankaku */
        /// <summary>
        /// 
        /// </summary>
        Lang6 = 149, /**< reserved */
        /// <summary>
        /// 
        /// </summary>
        Lang7 = 150, /**< reserved */
        /// <summary>
        /// 
        /// </summary>
        Lang8 = 151, /**< reserved */
        /// <summary>
        /// 
        /// </summary>
        Lang9 = 152, /**< reserved */

        /// <summary>
        /// 
        /// </summary>
        Alterase = 153, /**< Erase-Eaze */
        /// <summary>
        /// 
        /// </summary>
        Sysreq = 154,
        /// <summary>
        /// 
        /// </summary>
        Cancel = 155,
        /// <summary>
        /// 
        /// </summary>
        Clear = 156,
        /// <summary>
        /// 
        /// </summary>
        Prior = 157,
        /// <summary>
        /// 
        /// </summary>
        Return2 = 158,
        /// <summary>
        /// 
        /// </summary>
        Separator = 159,
        /// <summary>
        /// 
        /// </summary>
        Out = 160,
        /// <summary>
        /// 
        /// </summary>
        Oper = 161,
        /// <summary>
        /// 
        /// </summary>
        Clearagain = 162,
        /// <summary>
        /// 
        /// </summary>
        Crsel = 163,
        /// <summary>
        /// 
        /// </summary>
        Exsel = 164,

        /// <summary>
        /// 
        /// </summary>
        Kp_00 = 176,
        /// <summary>
        /// 
        /// </summary>
        Kp_000 = 177,
        /// <summary>
        /// 
        /// </summary>
        Thousandsseparator = 178,
        /// <summary>
        /// 
        /// </summary>
        Decimalseparator = 179,
        /// <summary>
        /// 
        /// </summary>
        Currencyunit = 180,
        /// <summary>
        /// 
        /// </summary>
        Currencysubunit = 181,
        /// <summary>
        /// 
        /// </summary>
        KpLeftparen = 182,
        /// <summary>
        /// 
        /// </summary>
        KpRightparen = 183,
        /// <summary>
        /// 
        /// </summary>
        KpLeftbrace = 184,
        /// <summary>
        /// 
        /// </summary>
        KpRightbrace = 185,
        /// <summary>
        /// 
        /// </summary>
        KpTab = 186,
        /// <summary>
        /// 
        /// </summary>
        KpBackspace = 187,
        /// <summary>
        /// 
        /// </summary>
        KpA = 188,
        /// <summary>
        /// 
        /// </summary>
        KpB = 189,
        /// <summary>
        /// 
        /// </summary>
        KpC = 190,
        /// <summary>
        /// 
        /// </summary>
        KpD = 191,
        /// <summary>
        /// 
        /// </summary>
        KpE = 192,
        /// <summary>
        /// 
        /// </summary>
        KpF = 193,
        /// <summary>
        /// 
        /// </summary>
        KpXor = 194,
        /// <summary>
        /// 
        /// </summary>
        KpPower = 195,
        /// <summary>
        /// 
        /// </summary>
        KpPercent = 196,
        /// <summary>
        /// 
        /// </summary>
        KpLess = 197,
        /// <summary>
        /// 
        /// </summary>
        KpGreater = 198,
        /// <summary>
        /// 
        /// </summary>
        KpAmpersand = 199,
        /// <summary>
        /// 
        /// </summary>
        KpDblampersand = 200,
        /// <summary>
        /// 
        /// </summary>
        KpVerticalbar = 201,
        /// <summary>
        /// 
        /// </summary>
        KpDblverticalbar = 202,
        /// <summary>
        /// 
        /// </summary>
        KpColon = 203,
        /// <summary>
        /// 
        /// </summary>
        KpHash = 204,
        /// <summary>
        /// 
        /// </summary>
        KpSpace = 205,
        /// <summary>
        /// 
        /// </summary>
        KpAt = 206,
        /// <summary>
        /// 
        /// </summary>
        KpExclam = 207,
        /// <summary>
        /// 
        /// </summary>
        KpMemstore = 208,
        /// <summary>
        /// 
        /// </summary>
        KpMemrecall = 209,
        /// <summary>
        /// 
        /// </summary>
        KpMemclear = 210,
        /// <summary>
        /// 
        /// </summary>
        KpMemadd = 211,
        /// <summary>
        /// 
        /// </summary>
        KpMemsubtract = 212,
        /// <summary>
        /// 
        /// </summary>
        KpMemmultiply = 213,
        /// <summary>
        /// 
        /// </summary>
        KpMemdivide = 214,
        /// <summary>
        /// 
        /// </summary>
        KpPlusminus = 215,
        /// <summary>
        /// 
        /// </summary>
        KpClear = 216,
        /// <summary>
        /// 
        /// </summary>
        KpClearentry = 217,
        /// <summary>
        /// 
        /// </summary>
        KpBinary = 218,
        /// <summary>
        /// 
        /// </summary>
        KpOctal = 219,
        /// <summary>
        /// 
        /// </summary>
        KpDecimal = 220,
        /// <summary>
        /// 
        /// </summary>
        KpHexadecimal = 221,

        /// <summary>
        /// 
        /// </summary>
        Lctrl = 224,
        /// <summary>
        /// 
        /// </summary>
        Lshift = 225,
        /// <summary>
        /// 
        /// </summary>
        Lalt = 226, /**< alt, option */
        /// <summary>
        /// 
        /// </summary>
        Lgui = 227, /**< windows, command (apple), meta */
        /// <summary>
        /// 
        /// </summary>
        Rctrl = 228,
        /// <summary>
        /// 
        /// </summary>
        Rshift = 229,
        /// <summary>
        /// 
        /// </summary>
        Ralt = 230, /**< alt gr, option */
        /// <summary>
        /// 
        /// </summary>
        Rgui = 231, /**< windows, command (apple), meta */

        /// <summary>
        /// 
        /// </summary>
        Mode = 257, /**< I'm not sure if this is really not covered
                     *   by any of the above, but since there's a
                     *   special KmodMode for it I'm adding it here
                     */

        /* @} *//* Usage page 0x07 */

        /**
        *  \name Usage page 0x0C
        *
        *  These values are mapped from usage page 0x0C (Usb consumer page).
        */
        /* @{ */

        /// <summary>
        /// 
        /// </summary>
        Audionext = 258,
        /// <summary>
        /// 
        /// </summary>
        Audioprev = 259,
        /// <summary>
        /// 
        /// </summary>
        Audiostop = 260,
        /// <summary>
        /// 
        /// </summary>
        Audioplay = 261,
        /// <summary>
        /// 
        /// </summary>
        Audiomute = 262,
        /// <summary>
        /// 
        /// </summary>
        Mediaselect = 263,
        /// <summary>
        /// 
        /// </summary>
        Www = 264,
        /// <summary>
        /// 
        /// </summary>
        Mail = 265,
        /// <summary>
        /// 
        /// </summary>
        Calculator = 266,
        /// <summary>
        /// 
        /// </summary>
        Computer = 267,
        /// <summary>
        /// 
        /// </summary>
        AcSearch = 268,
        /// <summary>
        /// 
        /// </summary>
        AcHome = 269,
        /// <summary>
        /// 
        /// </summary>
        AcBack = 270,
        /// <summary>
        /// 
        /// </summary>
        AcForward = 271,
        /// <summary>
        /// 
        /// </summary>
        AcStop = 272,
        /// <summary>
        /// 
        /// </summary>
        AcRefresh = 273,
        /// <summary>
        /// 
        /// </summary>
        AcBookmarks = 274,

        /* @} *//* Usage page 0x0C */

        /**
        *  \name Walther keys
        *
        *  These are values that Christian Walther added (for mac keyboard?).
        */
        /* @{ */

        /// <summary>
        /// 
        /// </summary>
        Brightnessdown = 275,
        /// <summary>
        /// 
        /// </summary>
        Brightnessup = 276,
        /// <summary>
        /// 
        /// </summary>
        Displayswitch = 277, /**< display mirroring/dual display switch, video mode switch */
        /// <summary>
        /// 
        /// </summary>
        Kbdillumtoggle = 278,
        /// <summary>
        /// 
        /// </summary>
        Kbdillumdown = 279,
        /// <summary>
        /// 
        /// </summary>
        Kbdillumup = 280,
        /// <summary>
        /// 
        /// </summary>
        Eject = 281,
        /// <summary>
        /// 
        /// </summary>
        Sleep = 282,

        /// <summary>
        /// 
        /// </summary>
        App1 = 283,
        /// <summary>
        /// 
        /// </summary>
        App2 = 284,

        /* @} *//* Walther keys */

        /**
        *  \name Usage page 0x0C (additional media keys)
        *
        *  These values are mapped from usage page 0x0C (Usb consumer page).
        */
        /* @{ */

        /// <summary>
        /// 
        /// </summary>
        Audiorewind = 285,
        /// <summary>
        /// 
        /// </summary>
        Audiofastforward = 286,
        /// <summary>
        /// Not an actual scan code. Has a value one larger than the largest value of all scan codes.
        /// Used to create an array that can be indexed with scan codes.
        /// </summary>
        Count
    }
}
