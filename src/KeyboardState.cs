using System;

namespace OpenWindow
{
    /// <summary>
    /// State of keys (up or down) indexed by scancode and 
    /// </summary>
    public class KeyboardState
    {
        /// <summary>
        /// The window that currently has keyboard focus.
        /// This can be null.
        /// </summary>
        public Window FocusedWindow { get; internal set; }

        /// <summary>
        /// Keyboard state with a boolean indicating if keys are down.
        /// Indexed with a keys <see cref="ScanCode"/>.
        /// </summary>
        internal bool[] KeyState { get; }

        /// <summary>
        /// Map from scan codes to key codes.
        /// This should be filled in by subclasses on initialization
        /// and updated when the keyboard layout changes.
        /// </summary>
        internal Key[] KeyMap { get; }

        internal KeyboardState()
        {
            KeyState = new bool[(int) ScanCode.Count];
        }

        /// <summary>
        /// Check if the key with the given scan code is down.
        /// </summary>
        public bool Down(ScanCode sc) => KeyState[(int) sc];

        /// <summary>
        /// Check if the key with the given scan code is up.
        /// </summary>
        public bool Up(ScanCode sc) => !KeyState[(int) sc];

        /// <summary>
        /// Map the given scan code to the matching virtual key for the current keyboard layout.
        /// </summary>
        public Key Map(ScanCode sc) => KeyMap[(int) sc];

        internal static Key[] GetDefaultKeyMap() => (Key[]) DefaultKeyMap.Clone();

        // Jjagg: The way key mapping works is identical to SDL, so I stole this code from them.
        //        Here's the license:
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
        private static Key[] DefaultKeyMap = 
        {
            0, 0, 0, 0,
            (Key) 'a',
            (Key) 'b',
            (Key) 'c',
            (Key) 'd',
            (Key) 'e',
            (Key) 'f',
            (Key) 'g',
            (Key) 'h',
            (Key) 'i',
            (Key) 'j',
            (Key) 'k',
            (Key) 'l',
            (Key) 'm',
            (Key) 'n',
            (Key) 'o',
            (Key) 'p',
            (Key) 'q',
            (Key) 'r',
            (Key) 's',
            (Key) 't',
            (Key) 'u',
            (Key) 'v',
            (Key) 'w',
            (Key) 'x',
            (Key) 'y',
            (Key) 'z',
            (Key) '1',
            (Key) '2',
            (Key) '3',
            (Key) '4',
            (Key) '5',
            (Key) '6',
            (Key) '7',
            (Key) '8',
            (Key) '9',
            (Key) '0',
            Key.Return,
            Key.Escape,
            Key.Backspace,
            Key.Tab,
            Key.Space,
            (Key) '-',
            (Key) '=',
            (Key) '[',
            (Key) ']',
            (Key) '\\',
            (Key) '#',
            (Key) ';',
            (Key) '\'',
            (Key) '`',
            (Key) ',',
            (Key) '.',
            (Key) '/',
            Key.Capslock,
            Key.F1,
            Key.F2,
            Key.F3,
            Key.F4,
            Key.F5,
            Key.F6,
            Key.F7,
            Key.F8,
            Key.F9,
            Key.F10,
            Key.F11,
            Key.F12,
            Key.Printscreen,
            Key.Scrolllock,
            Key.Pause,
            Key.Insert,
            Key.Home,
            Key.Pageup,
            Key.Delete,
            Key.End,
            Key.Pagedown,
            Key.Right,
            Key.Left,
            Key.Down,
            Key.Up,
            Key.Numlockclear,
            Key.KpDivide,
            Key.KpMultiply,
            Key.KpMinus,
            Key.KpPlus,
            Key.KpEnter,
            Key.Kp_1,
            Key.Kp_2,
            Key.Kp_3,
            Key.Kp_4,
            Key.Kp_5,
            Key.Kp_6,
            Key.Kp_7,
            Key.Kp_8,
            Key.Kp_9,
            Key.Kp_0,
            Key.KpPeriod,
            0,
            Key.Application,
            Key.Power,
            Key.KpEquals,
            Key.F13,
            Key.F14,
            Key.F15,
            Key.F16,
            Key.F17,
            Key.F18,
            Key.F19,
            Key.F20,
            Key.F21,
            Key.F22,
            Key.F23,
            Key.F24,
            Key.Execute,
            Key.Help,
            Key.Menu,
            Key.Select,
            Key.Stop,
            Key.Again,
            Key.Undo,
            Key.Cut,
            Key.Copy,
            Key.Paste,
            Key.Find,
            Key.Mute,
            Key.Volumeup,
            Key.Volumedown,
            0, 0, 0,
            Key.KpComma,
            Key.KpEqualsas400,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            Key.Alterase,
            Key.Sysreq,
            Key.Cancel,
            Key.Clear,
            Key.Prior,
            Key.Return2,
            Key.Separator,
            Key.Out,
            Key.Oper,
            Key.Clearagain,
            Key.Crsel,
            Key.Exsel,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            Key.Kp_00,
            Key.Kp_000,
            Key.Thousandsseparator,
            Key.Decimalseparator,
            Key.Currencyunit,
            Key.Currencysubunit,
            Key.KpLeftparen,
            Key.KpRightparen,
            Key.KpLeftbrace,
            Key.KpRightbrace,
            Key.KpTab,
            Key.KpBackspace,
            Key.KpA,
            Key.KpB,
            Key.KpC,
            Key.KpD,
            Key.KpE,
            Key.KpF,
            Key.KpXor,
            Key.KpPower,
            Key.KpPercent,
            Key.KpLess,
            Key.KpGreater,
            Key.KpAmpersand,
            Key.KpDblampersand,
            Key.KpVerticalbar,
            Key.KpDblverticalbar,
            Key.KpColon,
            Key.KpHash,
            Key.KpSpace,
            Key.KpAt,
            Key.KpExclam,
            Key.KpMemstore,
            Key.KpMemrecall,
            Key.KpMemclear,
            Key.KpMemadd,
            Key.KpMemsubtract,
            Key.KpMemmultiply,
            Key.KpMemdivide,
            Key.KpPlusminus,
            Key.KpClear,
            Key.KpClearentry,
            Key.KpBinary,
            Key.KpOctal,
            Key.KpDecimal,
            Key.KpHexadecimal,
            0, 0,
            Key.Lctrl,
            Key.Lshift,
            Key.Lalt,
            Key.Lgui,
            Key.Rctrl,
            Key.Rshift,
            Key.Ralt,
            Key.Rgui,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            Key.Mode,
            Key.Audionext,
            Key.Audioprev,
            Key.Audiostop,
            Key.Audioplay,
            Key.Audiomute,
            Key.Mediaselect,
            Key.Www,
            Key.Mail,
            Key.Calculator,
            Key.Computer,
            Key.AcSearch,
            Key.AcHome,
            Key.AcBack,
            Key.AcForward,
            Key.AcStop,
            Key.AcRefresh,
            Key.AcBookmarks,
            Key.Brightnessdown,
            Key.Brightnessup,
            Key.Displayswitch,
            Key.Kbdillumtoggle,
            Key.Kbdillumdown,
            Key.Kbdillumup,
            Key.Eject,
            Key.Sleep,
            Key.App1,
            Key.App2,
            Key.Audiorewind,
            Key.Audiofastforward,
        };
    }
}
