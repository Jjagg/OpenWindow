using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using OpenWindow;

using System.Collections.Generic;
using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace HelloOpenWindow
{
    internal static class Program
    {
        private static WindowingService _service;
        private static Window _window;

        private static readonly Random _random = new Random();

        private const int MinWidth = 200;
        private const int MinHeight = 120;
        private const int MaxWidth = 800;
        private const int MaxHeight = 480;

        private static void Main()
        {
            Console.WriteLine();
            WindowingService.Logger.OutputWriter = Console.Out;

            _service = WindowingService.Create();

            var wci = new WindowCreateInfo(100, 100, 400, 400, "Hello, OpenWindow! 💩", decorated: true, resizable: false);
            _window = _service.CreateWindow(ref wci);

            var iconWidth = 64;
            var iconHeight = 64;
            Span<Color> iconPixelData = stackalloc Color[iconWidth * iconHeight];
            FillIconPixelData(iconWidth, iconHeight, iconPixelData);
            _window.SetIcon<Color>(iconPixelData, iconWidth, iconHeight);

            var cursorWidth = 32;
            var cursorHeight = 32;
            Span<Color> cursorPixelData = stackalloc Color[cursorWidth * cursorHeight];
            FillIconPixelData(cursorWidth, cursorHeight, cursorPixelData);
            _window.SetCursor<Color>(cursorPixelData, cursorWidth, cursorHeight, 15, 15);

            _window.MinSize = new OpenWindow.Size(MinWidth, MinHeight);
            _window.MaxSize = new OpenWindow.Size(MaxWidth, MaxHeight);

            _window.CloseRequested += (s, e) => Console.WriteLine("Received request to close the window!");
            _window.Closing += (s, e) => Console.WriteLine("Closing the window! Bye :)");
            _window.FocusChanged += (s, e) => Console.WriteLine(e.HasFocus ? "Got focus!" : "Lost focus!");
            _window.MouseDown += (s, e) => Console.WriteLine($"Mouse button '{e.Button}' was pressed.");
            _window.MouseUp += (s, e) => Console.WriteLine($"Mouse button '{e.Button}' was released.");
            //_window.MouseMove += (s, e) => Console.WriteLine($"Mouse move ({e.X} : {e.Y}).");
            _window.MouseFocusChanged += (s, e) => Console.WriteLine(e.HasFocus ? $"Got mouse focus." : "Lost mouse focus.");

            _window.KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.A:
                        ClipboardTest();
                        break;
                    case Key.B: // border
                        _window.Decorated = !_window.Decorated;
                        break;
                    case Key.R: // resizable
                        _window.Resizable = !_window.Resizable;
                        break;
                    case Key.M: // move
                        SetRandomBounds();
                        break;
                    case Key.P: // print
                        PrintWindowInfo();
                        break;
                    case Key.J:
                        _window.Minimize();
                        break;
                    case Key.K:
                        _window.Restore();
                        break;
                    case Key.L:
                        _window.Maximize();
                        break;
                    case Key.C:
                        _window.CursorVisible = !_window.CursorVisible;
                        break;
                    case Key.Escape:
                        _window.Close();
                        break;
                }
            };

            var scNameMap = KeyUtil.CreateScanCodeNameMap();
            scNameMap[ScanCode.Left]  = "←";
            scNameMap[ScanCode.Right] = "→";
            scNameMap[ScanCode.Up]    = "↑";
            scNameMap[ScanCode.Down]  = "↓";

            var keyNameMap = KeyUtil.CreateVirtualKeyNameMap();

            _window.KeyDown += (s, e) => Console.WriteLine($"Key Down: {keyNameMap[e.Key]} ({scNameMap[e.ScanCode]})");
            //_window.KeyUp += (s, e) => Console.WriteLine($"Key Up: {keyNameMap[e.Key]} ({scNameMap[e.ScanCode]})");
            _window.TextInput += (s, e) => Console.WriteLine($"Got text input: {CharacterToPrintable(e.Character)}");

            while (!_window.ShouldClose)
            {
                _service.WaitEvent();
            }

            _service.DestroyWindow(_window);
            _service.Dispose();
        }

        private static string[] _predefinedClipboardFormats = 
        {
            null,
            "CF_TEXT",
            "CF_BITMAP",
            "CF_METAFILEPICT",
            "CF_SYLK",
            "CF_DIF",
            "CF_TIFF",
            "CF_OEMTEXT",
            "CF_DIB",
            "CF_PALETTE",
            "CF_PENDATA",
            "CF_RIFF",
            "CF_WAVE",
            "CF_UNICODETEXT",
            "CF_ENHMETAFILE",
            "CF_HDROP",
            "CF_LOCALE",
            "CF_DIBV5"
        };

        private static unsafe void ClipboardTest()
        {
            var hwnd = ((Win32WindowData) _window.GetPlatformData()).Hwnd;
            if (!Native.OpenClipboard(hwnd))
            {
                Console.WriteLine("Failed to open clipboard!");
            }

            try
            {
                Console.WriteLine("Clipboard formats:");

                uint format = Native.EnumClipboardFormats(0);
                while (format != 0)
                {
                    if (format < _predefinedClipboardFormats.Length)
                    {
                        Console.WriteLine($"  - {_predefinedClipboardFormats[format]} (predefined)");
                    }
                    else
                    {
                        var sb = new StringBuilder(Native.GetClipboardFormatNameLength((short) format));
                        if (Native.GetClipboardFormatName((short) format, sb) <= 0)
                        {
                            Console.WriteLine("- ERROR: Failed to get clipboard format name.");
                        }
                        else
                        {
                            Console.WriteLine($"  - {sb}");
                        }
                    }

                    format = Native.EnumClipboardFormats(format);
                }

                Native.EmptyClipboard();

                var bitmap = new Bitmap("img.png");

                SetClipboardPng();
                SetClipboardDIBV5(bitmap);
                SetClibpoardDIB(bitmap);
            }
            finally
            {
                Native.CloseClipboard();
            }
        }

        private unsafe static void SetClipboardDIBV5(Bitmap bitmap)
        {
            const int BI_BITFIELDS = 3;

            var Rgba32AlphaMask = 0xFF << 24;
            var Rgba32RedMask = 0xFF   << 16;
            var Rgba32GreenMask = 0xFF << 8;
            var Rgba32BlueMask = 0xFF  << 0;

            var biSize = Marshal.SizeOf<BitmapV5Header>();

            var header = new BitmapV5Header
            {
                biSize = biSize,
                biWidth = bitmap.Width,
                biHeight = bitmap.Height,
                biPlanes = 1,
                biBitCount = 32,
                biCompression = BI_BITFIELDS,
                biSizeImage = bitmap.Width * bitmap.Height * 4,
                bV5RedMask = Rgba32RedMask,
                bV5GreenMask = Rgba32GreenMask,
                bV5BlueMask = Rgba32BlueMask,
                bV5AlphaMask = Rgba32AlphaMask,
            };

            var bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

            try
            {
                var size = biSize + bmData.Stride * bmData.Height;
                var hmem = Native.GlobalAlloc(size);
                var ptr = Native.GlobalLock(hmem);
                var dst = new Span<byte>(ptr.ToPointer(), size);
                Marshal.StructureToPtr(header, ptr, false);
                dst = dst.Slice(biSize);

                var src = new Span<byte>(bmData.Scan0.ToPointer(), bmData.Stride * bmData.Height);
                src.CopyTo(dst);

                Native.GlobalUnlock(hmem);
                Native.SetClipboardData(ClipboardFormats.CF_DIBV5, hmem);
            }
            finally
            {
                bitmap.UnlockBits(bmData);
            }
        }

        private unsafe static void SetClibpoardDIB(Bitmap bitmap)
        {
            const int BI_BITFIELDS = 3;

            var Rgba32AlphaMask = 0xFF << 24;
            var Rgba32RedMask = 0xFF   << 16;
            var Rgba32GreenMask = 0xFF << 8;
            var Rgba32BlueMask = 0xFF  << 0;

            var biSize = Marshal.SizeOf<BitmapInfoHeader>();
            var header = new BitmapInfoHeader
            {
                biSize = biSize,
                biWidth = bitmap.Width,
                biHeight = bitmap.Height,
                biPlanes = 1,
                biBitCount = 32,
                biCompression = BI_BITFIELDS,
                biSizeImage = bitmap.Width * bitmap.Height * 4,
            };

            var bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

            try
            {
                var size = biSize + bmData.Stride * bmData.Height + 12;
                var hmem = Native.GlobalAlloc(size);
                var ptr = Native.GlobalLock(hmem);
                var dst = new Span<byte>(ptr.ToPointer(), size);
                Marshal.StructureToPtr(header, ptr, false);
                Marshal.WriteInt32(ptr, biSize, Rgba32RedMask);
                Marshal.WriteInt32(ptr, biSize + 4, Rgba32GreenMask);
                Marshal.WriteInt32(ptr, biSize + 8, Rgba32BlueMask);
                dst = dst.Slice(biSize + 12);

                var src = new Span<byte>(bmData.Scan0.ToPointer(), bmData.Stride * bmData.Height);
                src.CopyTo(dst);

                Native.GlobalUnlock(hmem);
                Native.SetClipboardData(ClipboardFormats.CF_DIB, hmem);
            }
            finally
            {
                bitmap.UnlockBits(bmData);
            }
        }

        private unsafe static void SetClipboardPng()
        {
            using (var fs = File.OpenRead("img.png"))
            {
                var fileLength = (int) fs.Length;

                var hmem = Native.GlobalAlloc(fileLength);
                var ptr = Native.GlobalLock(hmem);

                using (var ums = new UnmanagedMemoryStream((byte*) ptr.ToPointer(), fileLength, fileLength, FileAccess.Write))
                {
                    fs.CopyTo(ums);
                }

                //var dst = new Span<char>(ptr.ToPointer(), str.Length);
                //str.CopyTo(dst);

                Native.GlobalUnlock(hmem);

                var pngFormat = Native.RegisterClipboardFormat("PNG");
                var pngFormat2 = Native.RegisterClipboardFormat("image/png");
                Native.SetClipboardData(pngFormat, hmem);
            }
        }

        private static void FillIconPixelData(int w, int h, Span<Color> pixelData)
        {
            var halfWidth = w / 2;
            var halfHeight = h / 2;

            var r = new Random();
            for (var y = 0; y < h; y++)
            {
                var vy = (byte) (((float) y) / h * 255);
                for (var x = 0; x < w; x++)
                {
                    var vx = (byte) (((float) x) / w * 255);
                    var vd = (byte) (vx * vy / 255);

                    // nice circular gradient for the alpha
                    var dx = (x - halfWidth) / halfWidth;
                    var dy = (y - halfHeight) / halfHeight;
                    var dist = Math.Sqrt(dx * dx + dy * dy);
                    var distEased = dist * dist * dist;
                    var vr = (byte) (Math.Max(255 - 255 * distEased, 0));

                    var c = new Color(vr, vx, vy, vd);
                    pixelData[y * w + x] = c;
                }
            }
        }

        private static string CharacterToPrintable(int character)
        {
            return character switch
            {
                '\t' => "\\t",
                '\n' => "\\n",
                '\r' => "\\r",
                ' ' => "Space",
                _ => $"{char.ConvertFromUtf32(character)} (0x{character:X})",
            };
        }

        private static void SetRandomBounds()
        {
            var x = _random.Next(100, 300);
            var y = _random.Next(100, 300);
            var width = _random.Next(MinWidth, MaxWidth);
            var height = _random.Next(MinHeight, MaxHeight);
            _window.ClientBounds = new OpenWindow.Rectangle(x, y, width, height);
        }

        private static void PrintWindowInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Window Info:");
            Console.WriteLine($"Client bounds: {_window.ClientBounds}");
            var modifiers = _service.GetKeyModifiers();
            Console.WriteLine($"Control: {(modifiers & KeyMod.Control) > 0}");
            Console.WriteLine($"Shift: {(modifiers & KeyMod.Shift) > 0}");
            Console.WriteLine($"Alt: {(modifiers & KeyMod.Alt) > 0}");
            Console.WriteLine($"Caps Lock: {_service.IsCapsLockOn()}");
            Console.WriteLine($"Num Lock: {_service.IsNumLockOn()}");
            Console.WriteLine($"Scroll Lock: {_service.IsScrollLockOn()}");
            Console.WriteLine();
            Console.Out.Flush();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Color
        {
            public byte B;
            public byte G;
            public byte R;
            public byte A;

            public Color(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            public Color(uint value)
            {
                A = (byte) ((value >> 24) & 0xff);
                R = (byte) ((value >> 16) & 0xff);
                G = (byte) ((value >> 8) & 0xff);
                B = (byte) (value & 0xff);
            }

            public static implicit operator Color(uint v) => new Color(v);
        }
    }

    internal enum HResult : int
    {
        S_OK = 0,
        S_FALSE = 1,
        E_NOTIMPL = unchecked((int) 0x80004001),
        OLE_E_ADVISENOTSUPPORTED = unchecked((int) 0x80040003),
        E_FAIL = unchecked((int)0x80004005),
        DV_E_FORMATETC = unchecked((int) 0x80040064),
        DV_E_TYMED = unchecked((int) 0x80040069),
        DV_E_DVASPECT = unchecked((int) 0x8004006B),
    }

    public static class Native
    {
        #region Clipboard

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EmptyClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetClipboardData(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int CountClipboardFormats();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint EnumClipboardFormats(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint RegisterClipboardFormat(string lpszFormat);

        public static string GetClipboardFormatName(short format)
        {
            var len = GetClipboardFormatNameLength(format);
            var sb = new StringBuilder(len);
            if (GetClipboardFormatName(format, sb) > 0)
                return sb.ToString();

            return null;
        }

        public static int GetClipboardFormatNameLength(short format)
            => GetClipboardFormatName(format, null, 0);

        public static int GetClipboardFormatName(short format, StringBuilder lpszFormatName)
            => GetClipboardFormatName(format, lpszFormatName, lpszFormatName.Capacity);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetClipboardFormatName(short format, StringBuilder lpszFormatName, int cchMaxCount);

        public static IntPtr GlobalAlloc(int dwBytes) => GlobalAlloc(0x0042, dwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalAlloc(uint uFlags, int dwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalReAlloc(IntPtr hMem, int dwBytes, uint uFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GlobalUnlock(IntPtr hMem);

        // IntPtr.Zero on success
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        #endregion
    }

    public interface IDataObject
    {
        bool TryGetData<T>(string format, out T data);
        void SetData<T>(string format, T data);
        bool HasFormat(string format);
    }

    public static class ClipboardFormats
    {
        // https://www.codeproject.com/Reference/1091137/Windows-Clipboard-Formats

        public const uint CF_TEXT = 1;
        public const uint CF_BITMAP = 2;
        public const uint CF_METAFILEPICT = 3;
        public const uint CF_SYLK = 4;
        public const uint CF_DIF = 5;
        public const uint CF_TIFF = 6;
        public const uint CF_OEMTEXT = 7;
        public const uint CF_DIB = 8;
        public const uint CF_PALETTE = 9;
        public const uint CF_PENDATA = 10;
        public const uint CF_RIFF = 11;
        public const uint CF_WAVE = 12;
        public const uint CF_UNICODETEXT = 13;
        public const uint CF_ENHMETAFILE = 14;
        public const uint CF_HDROP = 15;
        public const uint CF_LOCALE = 16;
        public const uint CF_DIBV5 = 17;

        private static string[] _predefinedClipboardFormats = 
        {
            null,
            "CF_TEXT",
            "CF_BITMAP",
            "CF_METAFILEPICT",
            "CF_SYLK",
            "CF_DIF",
            "CF_TIFF",
            "CF_OEMTEXT",
            "CF_DIB",
            "CF_PALETTE",
            "CF_PENDATA",
            "CF_RIFF",
            "CF_WAVE",
            "CF_UNICODETEXT",
            "CF_ENHMETAFILE",
            "CF_HDROP",
            "CF_LOCALE",
            "CF_DIBV5"
        };

        public static uint GetFormatId(string format)
        {
            for (var i = 1; i < _predefinedClipboardFormats.Length; i++)
            {
                if (_predefinedClipboardFormats[i].Equals(format, StringComparison.OrdinalIgnoreCase))
                    return (uint) i;
            }

            return Native.RegisterClipboardFormat(format);
        }

        public static string GetFormatName(short id)
        {
            if (id >= 0 && id < _predefinedClipboardFormats.Length)
                return _predefinedClipboardFormats[id];

            return Native.GetClipboardFormatName(id);
        }
    }

    internal class OleDataObject : IDataObject, IComDataObject
    {
        public IComDataObject ComObject { get; }

        public OleDataObject(IComDataObject comObject)
        {
            ComObject = comObject;
        }

        public bool TryGetData<T>(string format, out T data)
        {
            data = default;

            var formatetc = CreateFORMATETC(format);
            if (QueryGetData(ref formatetc) != (int) HResult.S_OK)
                return false;

            ComObject.GetData(ref formatetc, out var medium);
            if (medium.unionmember == IntPtr.Zero)
                return false;

            if (format.Equals("CF_TEXT") || format.Equals("CF_OEMTEXT"))
            {
                if (typeof(T) != typeof(string))
                    return false;
                
                if (HGlobalHelper.ReadString(medium.unionmember, ansi: true, out var strData) != HResult.S_OK)
                    return false;
                
                data = (T) (object) strData;
                return true;
            }

            if (format.Equals("CF_UNICODETEXT"))
            {
                if (typeof(T) != typeof(string))
                    return false;
                
                if (HGlobalHelper.ReadString(medium.unionmember, ansi: false, out var strData) != HResult.S_OK)
                    return false;
                
                data = (T) (object) strData;
                return true;
            }

            return false;
        }

        public bool HasFormat(string format)
        {
            var formatetc = CreateFORMATETC(format);
            return QueryGetData(ref formatetc) == (int) HResult.S_OK;
        }

        private FORMATETC CreateFORMATETC(string name)
        {
            var formatId = ClipboardFormats.GetFormatId(name);
            return new FORMATETC
            {
                cfFormat = (short) formatId,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                lindex = -1,
                ptd = IntPtr.Zero,
                tymed = TYMED.TYMED_HGLOBAL
            };
        }


        public void SetData<T>(string format, T data)
        {
            throw new NotImplementedException();
        }

        public int DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection)
            => ComObject.DAdvise(ref pFormatetc, advf, adviseSink, out connection);

        public void DUnadvise(int connection)
            => ComObject.DUnadvise(connection);

        public int EnumDAdvise(out IEnumSTATDATA enumAdvise)
            => ComObject.EnumDAdvise(out enumAdvise);

        public IEnumFORMATETC EnumFormatEtc(DATADIR direction)
            => ComObject.EnumFormatEtc(direction);

        public int GetCanonicalFormatEtc(ref FORMATETC formatIn, out FORMATETC formatOut)
            => ComObject.GetCanonicalFormatEtc(ref formatIn, out formatOut);

        public void GetData(ref FORMATETC format, out STGMEDIUM medium)
            => ComObject.GetData(ref format, out medium);

        public void GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
            => ComObject.GetDataHere(ref format, ref medium);

        public int QueryGetData(ref FORMATETC format)
            => ComObject.QueryGetData(ref format);

        public void SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
            => ComObject.SetData(ref formatIn, ref medium, release);
    }

    internal class ManagedDataObject : IDataObject, IComDataObject
    {
        private readonly Dictionary<string, object> _dict;

        public ManagedDataObject()
        {
            _dict = new Dictionary<string, object>();
        }

        public string[] GetFormats()
        {
            var keys = new string[_dict.Keys.Count];
            _dict.Keys.CopyTo(keys, 0);
            return keys;
        }

        public bool TryGetData<T>(string format, out T data)
        {
            _dict.TryGetValue(format, out var dataTmp);
            if (dataTmp is T)
            {
                data = (T) dataTmp;
                return true;
            }

            data = default;
            return false;
        }

        public bool HasFormat(string format)
            => _dict.ContainsKey(format);

        public void SetData<T>(string format, T data)
        {
            _dict[format] = data;
        }

        int IComDataObject.DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection)
        {
            connection = 0;
            return (int) HResult.E_NOTIMPL;
        }

        void IComDataObject.DUnadvise(int connection)
        {
            Marshal.ThrowExceptionForHR((int) HResult.E_NOTIMPL);
        }

        int IComDataObject.EnumDAdvise(out IEnumSTATDATA enumAdvise)
        {
            enumAdvise = null;
            return (int) HResult.OLE_E_ADVISENOTSUPPORTED;
        }

        IEnumFORMATETC IComDataObject.EnumFormatEtc(DATADIR direction)
        {
            if (direction == DATADIR.DATADIR_SET)
                Marshal.ThrowExceptionForHR((int) HResult.E_NOTIMPL);

            return new FormatEnumerator(this);
        }

        int IComDataObject.GetCanonicalFormatEtc(ref FORMATETC formatIn, out FORMATETC formatOut)
        {
            const int DATA_S_SAMEFORMATETC = 0x00040130;
            formatOut = new FORMATETC();
            return DATA_S_SAMEFORMATETC;
        }

        void IComDataObject.GetData(ref FORMATETC format, out STGMEDIUM medium)
        {
            var hr = QueryGetDataInternal(format, out var formatName);
            if (hr != HResult.S_OK)
                Marshal.ThrowExceptionForHR((int) hr);

            medium = new STGMEDIUM();
            medium.tymed = format.tymed;

            WriteDataToGlobal(formatName, _dict[formatName], ref medium.unionmember);
        }

        void IComDataObject.GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
        {
            var hr = QueryGetDataInternal(format, out var formatName);
            if (hr != HResult.S_OK)
                Marshal.ThrowExceptionForHR((int) hr);

            WriteDataToGlobal(formatName, _dict[formatName], ref medium.unionmember);
        }

        int IComDataObject.QueryGetData(ref FORMATETC format)
            => (int) QueryGetDataInternal(format, out _);

        private HResult QueryGetDataInternal(in FORMATETC format, out string formatName)
        {
            formatName = null;

            if (format.dwAspect != DVASPECT.DVASPECT_CONTENT)
                return HResult.DV_E_DVASPECT;
            if (format.tymed != TYMED.TYMED_HGLOBAL)
                return HResult.DV_E_TYMED;

            formatName = ClipboardFormats.GetFormatName(format.cfFormat);
            if (formatName == null || !HasFormat(formatName))
                return HResult.DV_E_FORMATETC;

            return HResult.S_OK;
        }

        void IComDataObject.SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
        {
            Marshal.ThrowExceptionForHR((int) HResult.E_NOTIMPL);
        }

        private static HResult WriteDataToGlobal(string format, object data, ref IntPtr hMem)
        {
            HResult hr = HResult.E_FAIL;
            if (format.Equals("CF_TEXT", StringComparison.OrdinalIgnoreCase) ||
                format.Equals("CF_OEMTEXT", StringComparison.OrdinalIgnoreCase))
            {
                if (data is string str)
                {
                    hr = HGlobalHelper.WriteString(str, ref hMem, ansi: true);
                }
                else
                {
                    hr = HResult.DV_E_FORMATETC;
                }
            }
            if (format.Equals("CF_UNICODETEXT", StringComparison.OrdinalIgnoreCase))
            {
                if (data is string str)
                {
                    hr = HGlobalHelper.WriteString(str, ref hMem, ansi: false);
                }
                else
                {
                    hr = HResult.DV_E_FORMATETC;
                }
            }

            return hr;
        }

        private class FormatEnumerator : IEnumFORMATETC
        {
            private readonly FORMATETC[] _formats;
            private int _current;

            public FormatEnumerator(ManagedDataObject mdo)
            {
                var strFormats = mdo.GetFormats();

                for (var i = 0; i < strFormats.Length; i++)
                {
                    var formatName = strFormats[i];
                    var formatId = ClipboardFormats.GetFormatId(formatName);

                    var tymed = formatId switch
                    {
                        ClipboardFormats.CF_BITMAP => TYMED.TYMED_GDI,
                        ClipboardFormats.CF_ENHMETAFILE => TYMED.TYMED_ENHMF,
                        _ => TYMED.TYMED_HGLOBAL
                    };

                    _formats[i] = new FORMATETC
                    {
                        cfFormat = (short) formatId,
                        dwAspect = DVASPECT.DVASPECT_CONTENT,
                        lindex = -1,
                        ptd = IntPtr.Zero,
                        tymed = tymed
                    };
                }
            }

            private FormatEnumerator(FORMATETC[] formats)
            {
                _formats = formats;
            }

            public void Clone(out IEnumFORMATETC newEnum)
            {
                newEnum = new FormatEnumerator(_formats);
            }

            public int Next(int celt, FORMATETC[] rgelt, int[] pceltFetched)
            {
                var fetched = 0;
                var i = 0;

                while (celt-- > 0 && _current < _formats.Length)
                {
                    rgelt[i++] = _formats[_current++];
                    fetched++;
                }

                pceltFetched[0] = fetched;
                return (int) (fetched == celt ? HResult.S_OK : HResult.S_FALSE);
            }

            public int Reset()
            {
                _current = 0;
                return (int) HResult.S_OK;
            }

            public int Skip(int celt)
            {
                if ( celt <= 0 || _current + celt >= _formats.Length)
                    return (int) HResult.S_FALSE;

                _current += celt;
                return (int) HResult.S_OK;
            }
        }
    }

    internal static class HGlobalHelper
    {
        public static unsafe HResult ReadString(IntPtr hMem, bool ansi, out string data)
        {
            var ptr = Native.GlobalLock(hMem);
            data = null;
            if (ptr == IntPtr.Zero)
                return HResult.E_FAIL;

            try
            {
                if (ansi)
                {
                    data = new string((sbyte*) ptr);
                }
                else
                {
                    data = new string ((char*) ptr);
                }
            }
            finally
            {
                Native.GlobalUnlock(hMem);
            }

            return HResult.S_OK;
        }

        public static HResult WriteString(string data, ref IntPtr hMem, bool ansi)
        {
            var size = ansi ? data.Length + 1 : data.Length * 2 + 1;

            const int GHND = 0x0042;
            hMem = hMem == IntPtr.Zero ? Native.GlobalAlloc(GHND, size) : hMem = Native.GlobalReAlloc(hMem, size, GHND);

            if (hMem == IntPtr.Zero) return HResult.E_FAIL;

            var ptr = Native.GlobalLock(hMem);
            if (ptr == IntPtr.Zero) return HResult.E_FAIL;

            if (ansi)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    var c = data[i];
                    if (c > byte.MaxValue) return HResult.E_FAIL;
                    Marshal.WriteByte(ptr + i, (byte) c);
                }
            }
            else
            {
                for (var i = 0; i < data.Length; i++)
                {
                    var c = data[i];
                    Marshal.WriteInt16(ptr + i, c);
                }
            }

            return HResult.S_OK;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BitmapInfoHeader
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BitmapV5Header
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
        public int bV5RedMask;
        public int bV5GreenMask;
        public int bV5BlueMask;
        public int bV5AlphaMask;
        public int bV5CSType;

        public int redX;
        public int redY;
        public int redZ;
        public int greenX;
        public int greenY;
        public int greenZ;
        public int blueX;
        public int blueY;
        public int blueZ;

        public int bV5GammaRed;
        public int bV5GammaGreen;
        public int bV5GammaBlue;
        public int bV5Intent;
        public int bV5ProfileData;
        public int bV5ProfileSize;
        public int bV5Reserved;
    }
}
