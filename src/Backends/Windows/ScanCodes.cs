// Jjagg:
//     These scan codes where copied from SDL,
//     below follows the original documentation.

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

/* Windows scancode to SDL scancode mapping table */
/* derived from Microsoft scan code document, http://download.microsoft.com/download/1/6/1/161ba512-40e2-4cc9-843a-923143f3456c/scancode.doc */

namespace OpenWindow.Backends.Windows
{
    internal static class WindowsScanCodes
    {
        public static ScanCode[] ScanCodes =
        {
        /*  0                        1                        2                       3                        4                   5                        6                        7 */
        /*  8                        9                        A                       B                        C                   D                        E                        F */
            ScanCode.Unknown,        ScanCode.Escape,         ScanCode.D1,            ScanCode.D2,             ScanCode.D3,        ScanCode.D4,             ScanCode.D5,             ScanCode.D6,          /* 0 */
            ScanCode.D7,             ScanCode.D8,             ScanCode.D9,            ScanCode.D0,             ScanCode.Minus,     ScanCode.Equals,         ScanCode.Backspace,      ScanCode.Tab,         /* 0 */

            ScanCode.Q,              ScanCode.W,              ScanCode.E,             ScanCode.R,              ScanCode.T,         ScanCode.Y,              ScanCode.U,              ScanCode.I,           /* 1 */
            ScanCode.O,              ScanCode.P,              ScanCode.Leftbracket,   ScanCode.Rightbracket,   ScanCode.Return,    ScanCode.Lctrl,          ScanCode.A,              ScanCode.S,           /* 1 */

            ScanCode.D,              ScanCode.F,              ScanCode.G,             ScanCode.H,              ScanCode.J,         ScanCode.K,              ScanCode.L,              ScanCode.Semicolon,   /* 2 */
            ScanCode.Apostrophe,     ScanCode.Grave,          ScanCode.Lshift,        ScanCode.Backslash,      ScanCode.Z,         ScanCode.X,              ScanCode.C,              ScanCode.V,           /* 2 */

            ScanCode.B,              ScanCode.N,              ScanCode.M,             ScanCode.Comma,          ScanCode.Period,    ScanCode.Slash,          ScanCode.Rshift,         ScanCode.Printscreen, /* 3 */
            ScanCode.Lalt,           ScanCode.Space,          ScanCode.Capslock,      ScanCode.F1,             ScanCode.F2,        ScanCode.F3,             ScanCode.F4,             ScanCode.F5,          /* 3 */

            ScanCode.F6,             ScanCode.F7,             ScanCode.F8,            ScanCode.F9,             ScanCode.F10,       ScanCode.Numlockclear,   ScanCode.Scrolllock,     ScanCode.Home,        /* 4 */
            ScanCode.Up,             ScanCode.Pageup,         ScanCode.KpMinus,       ScanCode.Left,           ScanCode.Kp_5,      ScanCode.Right,          ScanCode.KpPlus,         ScanCode.End,         /* 4 */

            ScanCode.Down,           ScanCode.Pagedown,       ScanCode.Insert,        ScanCode.Delete,         ScanCode.Unknown,   ScanCode.Unknown,        ScanCode.Nonusbackslash, ScanCode.F11,         /* 5 */
            ScanCode.F12,            ScanCode.Pause,          ScanCode.Unknown,       ScanCode.Lgui,           ScanCode.Rgui,      ScanCode.Application,    ScanCode.Unknown,        ScanCode.Unknown,     /* 5 */

            ScanCode.Unknown,        ScanCode.Unknown,        ScanCode.Unknown,       ScanCode.Unknown,        ScanCode.F13,       ScanCode.F14,            ScanCode.F15,            ScanCode.F16,         /* 6 */
            ScanCode.F17,            ScanCode.F18,            ScanCode.F19,           ScanCode.Unknown,        ScanCode.Unknown,   ScanCode.Unknown,        ScanCode.Unknown,        ScanCode.Unknown,     /* 6 */
            
            ScanCode.International2, ScanCode.Unknown,        ScanCode.Unknown,       ScanCode.International1, ScanCode.Unknown,   ScanCode.Unknown,        ScanCode.Unknown,        ScanCode.Unknown,     /* 7 */
            ScanCode.Unknown,        ScanCode.International4, ScanCode.Unknown,       ScanCode.International5, ScanCode.Unknown,   ScanCode.International3, ScanCode.Unknown ,       ScanCode.Unknown      /* 7 */
        };
    }
}