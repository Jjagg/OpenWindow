namespace OpenWindow.Backends.Windows
{
    internal static class Constants
    {
        public const uint MonitorDefaultToNull = 0;
        public const uint MonitorDefaultToPrimary = 1;
        public const uint MonitorDefaultToNearest = 2;

        public const int UNICODE_NOCHAR = 0xffff;

        public const uint SWP_NOMOVE        = 0x0002;
        public const uint SWP_NOSIZE        = 0x0001;
        public const uint SWP_NOZORDER      = 0x0004;
        public static uint SWP_FRAMECHANGED = 0x0020;

        public const uint WS_OVERLAPPED   = 0x00000000;
        public const uint WS_MAXIMIZEBOX  = 0x00010000;
        public const uint WS_MINIMIZEBOX  = 0x00020000;
        public const uint WS_TABSTOP      = 0x00010000;
        public const uint WS_GROUP        = 0x00020000;
        public const uint WS_THICKFRAME   = 0x00040000;
        public const uint WS_SYSMENU      = 0x00080000;
        public const uint WS_HSCROLL      = 0x00100000;
        public const uint WS_VSCROLL      = 0x00200000;
        public const uint WS_DLGFRAME     = 0x00400000;
        public const uint WS_BORDER       = 0x00800000;
        public const uint WS_DISABLED     = 0x08000000;
        public const uint WS_VISIBLE      = 0x10000000;
        public const uint WS_MINIMIZE     = 0x20000000;
        public const uint WS_CHILD        = 0x40000000;
        public const uint WS_POPUP        = 0x80000000;
        public const uint WS_CAPTION      = WS_BORDER | WS_DLGFRAME;
        public const uint WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        public const uint WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;

        public const uint CS_OWNDC = 0x0020;

    }
}
