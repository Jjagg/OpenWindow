﻿namespace OpenWindow.Backends.Windows
{
    internal static class KeyMap
    {
        public static Key[] Map;
        public static VirtualKey[] InvMap;

        public static void Create()
        {
            Map = new Key[256];
            Map[(int) VirtualKey.Back] = Key.Backspace;
            Map[(int) VirtualKey.Tab] = Key.Tab;
            //Map[(int) VirtualKey.Clear] = ;
            Map[(int) VirtualKey.Return] = Key.Enter;
            //Map[(int) VirtualKey.Shift] = Key.;
            //Map[(int) VirtualKey.Control] = Key.;
            //Map[(int) VirtualKey.Alt] = Key.;
            Map[(int) VirtualKey.Pause] = Key.Pause;
            Map[(int) VirtualKey.CapsLock] = Key.CapsLock;
            Map[(int) VirtualKey.Escape] = Key.Escape;
            //Map[(int) VirtualKey.Convert] = Key.;
            //Map[(int) VirtualKey.NonConvert] = Key.;
            //Map[(int) VirtualKey.Accept] = Key.;
            //Map[(int) VirtualKey.ModeChange] = Key.;
            Map[(int) VirtualKey.Space] = Key.Space;
            Map[(int) VirtualKey.Prior] = Key.PageUp;
            Map[(int) VirtualKey.Next] = Key.PageDown;
            Map[(int) VirtualKey.End] = Key.End;
            Map[(int) VirtualKey.Home] = Key.Home;
            Map[(int) VirtualKey.Left] = Key.Left;
            Map[(int) VirtualKey.Up] = Key.Up;
            Map[(int) VirtualKey.Right] = Key.Right;
            Map[(int) VirtualKey.Down] = Key.Down;
            //Map[(int) VirtualKey.Select] = Key.;
            //Map[(int) VirtualKey.Print] = Key.;
            //Map[(int) VirtualKey.Execute] = Key.;
            Map[(int) VirtualKey.PrintScr] = Key.PrintScreen;
            Map[(int) VirtualKey.Insert] = Key.Insert;
            Map[(int) VirtualKey.Delete] = Key.Delete;
            //Map[(int) VirtualKey.Help] = Key.;
            Map[(int) VirtualKey.Key0] = Key.D0;
            Map[(int) VirtualKey.Key1] = Key.D1;
            Map[(int) VirtualKey.Key2] = Key.D2;
            Map[(int) VirtualKey.Key3] = Key.D3;
            Map[(int) VirtualKey.Key4] = Key.D4;
            Map[(int) VirtualKey.Key5] = Key.D5;
            Map[(int) VirtualKey.Key6] = Key.D6;
            Map[(int) VirtualKey.Key7] = Key.D7;
            Map[(int) VirtualKey.Key8] = Key.D8;
            Map[(int) VirtualKey.Key9] = Key.D9;
            Map[(int) VirtualKey.A] = Key.A;
            Map[(int) VirtualKey.B] = Key.B;
            Map[(int) VirtualKey.C] = Key.C;
            Map[(int) VirtualKey.D] = Key.D;
            Map[(int) VirtualKey.E] = Key.E;
            Map[(int) VirtualKey.F] = Key.F;
            Map[(int) VirtualKey.G] = Key.G;
            Map[(int) VirtualKey.H] = Key.H;
            Map[(int) VirtualKey.I] = Key.I;
            Map[(int) VirtualKey.J] = Key.J;
            Map[(int) VirtualKey.K] = Key.K;
            Map[(int) VirtualKey.L] = Key.L;
            Map[(int) VirtualKey.M] = Key.M;
            Map[(int) VirtualKey.N] = Key.N;
            Map[(int) VirtualKey.O] = Key.O;
            Map[(int) VirtualKey.P] = Key.P;
            Map[(int) VirtualKey.Q] = Key.Q;
            Map[(int) VirtualKey.R] = Key.R;
            Map[(int) VirtualKey.S] = Key.S;
            Map[(int) VirtualKey.T] = Key.T;
            Map[(int) VirtualKey.U] = Key.U;
            Map[(int) VirtualKey.V] = Key.V;
            Map[(int) VirtualKey.W] = Key.W;
            Map[(int) VirtualKey.X] = Key.X;
            Map[(int) VirtualKey.Y] = Key.Y;
            Map[(int) VirtualKey.Z] = Key.Z;
            Map[(int) VirtualKey.LeftWin] = Key.LeftSuper;
            Map[(int) VirtualKey.RightWin] = Key.RightSuper;
            //Map[(int) VirtualKey.Apps] = Key.;
            //Map[(int) VirtualKey.Sleep] = Key.;
            Map[(int) VirtualKey.Numpad0] = Key.Np0;
            Map[(int) VirtualKey.Numpad1] = Key.Np1;
            Map[(int) VirtualKey.Numpad2] = Key.Np2;
            Map[(int) VirtualKey.Numpad3] = Key.Np3;
            Map[(int) VirtualKey.Numpad4] = Key.Np4;
            Map[(int) VirtualKey.Numpad5] = Key.Np5;
            Map[(int) VirtualKey.Numpad6] = Key.Np6;
            Map[(int) VirtualKey.Numpad7] = Key.Np7;
            Map[(int) VirtualKey.Numpad8] = Key.Np8;
            Map[(int) VirtualKey.Numpad9] = Key.Np9;
            Map[(int) VirtualKey.Multiply] = Key.NpMultiply;
            Map[(int) VirtualKey.Add] = Key.NpPlus;
            //Map[(int) VirtualKey.Separator] = Key.;
            Map[(int) VirtualKey.Subtract] = Key.NpMinus;
            Map[(int) VirtualKey.Decimal] = Key.NpDecimal;
            Map[(int) VirtualKey.Divide] = Key.NpDivide;
            Map[(int) VirtualKey.F1] = Key.F1;
            Map[(int) VirtualKey.F2] = Key.F2;
            Map[(int) VirtualKey.F3] = Key.F3;
            Map[(int) VirtualKey.F4] = Key.F4;
            Map[(int) VirtualKey.F5] = Key.F5;
            Map[(int) VirtualKey.F6] = Key.F6;
            Map[(int) VirtualKey.F7] = Key.F7;
            Map[(int) VirtualKey.F8] = Key.F8;
            Map[(int) VirtualKey.F9] = Key.F9;
            Map[(int) VirtualKey.F10] = Key.F10;
            Map[(int) VirtualKey.F11] = Key.F11;
            Map[(int) VirtualKey.F12] = Key.F12;
            Map[(int) VirtualKey.F13] = Key.F13;
            Map[(int) VirtualKey.F14] = Key.F14;
            Map[(int) VirtualKey.F15] = Key.F15;
            Map[(int) VirtualKey.F16] = Key.F16;
            Map[(int) VirtualKey.F17] = Key.F17;
            Map[(int) VirtualKey.F18] = Key.F18;
            Map[(int) VirtualKey.F19] = Key.F19;
            Map[(int) VirtualKey.F20] = Key.F20;
            Map[(int) VirtualKey.F21] = Key.F21;
            Map[(int) VirtualKey.F22] = Key.F22;
            Map[(int) VirtualKey.F23] = Key.F23;
            Map[(int) VirtualKey.F24] = Key.F24;
            Map[(int) VirtualKey.NumLock] = Key.NumLock;
            Map[(int) VirtualKey.ScrollLock] = Key.ScrollLock;
            Map[(int) VirtualKey.LeftShift] = Key.LeftShift;
            Map[(int) VirtualKey.RightShift] = Key.RightShift;
            Map[(int) VirtualKey.LeftControl] = Key.LeftControl;
            Map[(int) VirtualKey.RightControl] = Key.RightControl;
            Map[(int) VirtualKey.LeftAlt] = Key.LeftAlt;
            Map[(int) VirtualKey.RightAlt] = Key.RightAlt;
            //Map[(int) VirtualKey.BrowserBack] = Key.;
            //Map[(int) VirtualKey.BrowserForward] = Key.;
            //Map[(int) VirtualKey.BrowserRefresh] = Key.;
            //Map[(int) VirtualKey.BrowserStop] = Key.;
            //Map[(int) VirtualKey.BrowserSearch] = Key.;
            //Map[(int) VirtualKey.BrowserFavorites] = Key.;
            //Map[(int) VirtualKey.BrowserHome] = Key.;
            //Map[(int) VirtualKey.VolumeMute] = Key.;
            //Map[(int) VirtualKey.VolumeDown] = Key.;
            //Map[(int) VirtualKey.VolumeUp] = Key.;
            //Map[(int) VirtualKey.MediaNextTrack] = Key.;
            //Map[(int) VirtualKey.MediaPrevTrack] = Key.;
            //Map[(int) VirtualKey.MediaStop] = Key.;
            //Map[(int) VirtualKey.MediaPlayPause] = Key.;
            //Map[(int) VirtualKey.LaunchMail] = Key.;
            //Map[(int) VirtualKey.LaunchMediaSelect] = Key.;
            //Map[(int) VirtualKey.LaunchApp1] = Key.;
            //Map[(int) VirtualKey.LaunchApp2] = Key.;
            Map[(int) VirtualKey.OemPlus] = Key.Plus;
            Map[(int) VirtualKey.OemComma] = Key.Comma;
            Map[(int) VirtualKey.OemMinus] = Key.Minus;
            Map[(int) VirtualKey.OemPeriod] = Key.Period;
            Map[(int) VirtualKey.Oem1] = Key.Semicolon;
            Map[(int) VirtualKey.Oem2] = Key.Slash;
            Map[(int) VirtualKey.Oem3] = Key.Backtick;
            Map[(int) VirtualKey.Oem4] = Key.LeftBracket;
            Map[(int) VirtualKey.Oem5] = Key.Backslash;
            Map[(int) VirtualKey.Oem6] = Key.RightBracket;
            Map[(int) VirtualKey.Oem7] = Key.Quote;
            //Map[(int) VirtualKey.Oem8] = Key.;
            //Map[(int) VirtualKey.Oem102] = Key.;
            //Map[(int) VirtualKey.Processkey] = Key.;
            //Map[(int) VirtualKey.Packet] = Key.;
            //Map[(int) VirtualKey.Attn] = Key.;
            //Map[(int) VirtualKey.CrSel] = Key.;
            //Map[(int) VirtualKey.ExSel] = Key.;
            //Map[(int) VirtualKey.EraseEOF] = Key.;
            //Map[(int) VirtualKey.Play] = Key.;
            //Map[(int) VirtualKey.Zoom] = Key.;
            //Map[(int) VirtualKey.Pa1] = Key.;
            //Map[(int) VirtualKey.OemClear] = Key.;

            InvMap = new VirtualKey[256];
            for (var vk = 0; vk < Map.Length; vk++)
                InvMap[(int) Map[vk]] = (VirtualKey) vk;
        }
    }
}