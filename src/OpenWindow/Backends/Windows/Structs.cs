using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Msg
    {
        public IntPtr hwnd;
        public WindowMessage message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point pt;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WndClass
    {
        public uint style;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WndProc lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WndClassEx {
        uint cbSize;
        uint style;
        WndProc lpfnWndProc;
        int cbClsExtra;
        int cbWndExtra;
        IntPtr hInstance;
        IntPtr hIcon;
        IntPtr hCursor;
        IntPtr hbrBackground;
        string lpszMenuName;
        string lpszClassName;
        IntPtr hIconSm;
    }

    internal delegate IntPtr WndProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    internal struct MonitorInfo
    {
        public int cbSize;
        public Rect monitorRect;
        public Rect workAreaRect;
        public uint flags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        public bool IsPrimary => flags == 1;

        public static MonitorInfo Create()
        {
            return new MonitorInfo
            {
                cbSize = 72
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowInfo
    {
        public uint cbSize;
        public Rect rcWindow;
        public Rect rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public int atomWindowType;
        public int wCreatorVersion;

        public static WindowInfo Create()
        {
            return new WindowInfo()
            {
                cbSize = 64
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Point Position => new Point(Left, Top);
        public Size Size => new Size(Right - Left, Bottom - Top);

        public int Width => Right - Left;
        public int Height => Bottom - Top;

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static implicit operator Rectangle(Rect rect)
        {
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        public static implicit operator Rect(Rectangle rect)
        {
            return new Rect
            {
                Left = rect.Left,
                Top = rect.Top,
                Right = rect.Right,
                Bottom = rect.Bottom
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PixelFormatDescriptor
    {
        public short nSize;
        public short nVersion;
        public PfdFlags dwFlags;
        public PfdType iPixelType;
        public byte cColorBits;
        public byte cRedBits;
        public byte cRedShift;
        public byte cGreenBits;
        public byte cGreenShift;
        public byte cBlueBits;
        public byte cBlueShift;
        public byte cAlphaBits;
        public byte cAlphaShift;
        public byte cAccumBits;
        public byte cAccumRedBits;
        public byte cAccumGreenBits;
        public byte cAccumBlueBits;
        public byte cAccumAlphaBits;
        public byte cDepthBits;
        public byte cStencilBits;
        public byte cAuxBuffers;
        public byte iLayerType;
        public byte bReserved;
        public int dwLayerMask;
        public int dwVisibleMask;
        public int dwDamageMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TrackMouseEvent
    {
        public int cbSize;
        public uint dwFlags;
        public IntPtr hwndTrack;
        public uint dwHoverTime;

        public static TrackMouseEvent CreateLeave(IntPtr handle)
        {
            var tme = new TrackMouseEvent();
            tme.cbSize = Marshal.SizeOf<TrackMouseEvent>();
            tme.dwFlags = 0x00000002;
            tme.hwndTrack = handle;
            return tme;
        }
    }
}
