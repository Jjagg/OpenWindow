using System;
using static OpenWindow.ScanCode;

namespace OpenWindow
{
    public static class KeyExtensions
    {
        public static string GetName(this ScanCode sc) => GetNameInternal(sc);
        public static string GetName(this Key key) => GetNameInternal((ScanCode) key);

        private static string GetNameInternal(ScanCode sc)
        {
            return sc switch
            {
                D0 => "0",
                D1 => "1",
                D2 => "2",
                D3 => "3",
                D4 => "4",
                D5 => "5",
                D6 => "6",
                D7 => "7",
                D8 => "8",
                D9 => "0",

                Backquote => "`",
                Minus => "-",
                ScanCode.Equals => "=",
                LeftSquareBracket => "[",
                RightSquareBracket => "]",
                Backslash => "\\",
                Semicolon => ";",
                Apostrophe => "\"",
                Comma => ",",
                Period => ".",
                Slash => "/",

                LeftShift => "L Shift",
                RightShift => "R Shift",
                LeftControl => "L Ctrl",
                RightControl => "R Ctrl",
                LeftAlt => "L Alt",
                RightAlt => "R Alt",
                LeftMeta => "L Meta",
                RightMeta => "R Meta",

                KpDivide => "Keypad /",
                KpMultiply => "Keypad *",
                KpMinus => "Keypad -",
                KpPlus => "Keypad +",
                KpEnter => "Keypad Enter",
                Kp0 => "Keypad 0",
                Kp1 => "Keypad 1",
                Kp2 => "Keypad 2",
                Kp3 => "Keypad 3",
                Kp4 => "Keypad 4",
                Kp5 => "Keypad 5",
                Kp6 => "Keypad 6",
                Kp7 => "Keypad 7",
                Kp8 => "Keypad 8",
                Kp9 => "Keypad 9",
                KpPeriod => "Keypad .",

                _ => sc.ToString(),
            };
        }
    }
}
