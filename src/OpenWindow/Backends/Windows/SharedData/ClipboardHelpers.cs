using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal static class ClipboardHelpers
    {
        private const int BI_BITFIELDS = 3;
        private const int Rgba32AlphaMask = 0xFF << 24;
        private const int Rgba32RedMask   = 0xFF << 16;
        private const int Rgba32GreenMask = 0xFF << 8;
        private const int Rgba32BlueMask  = 0xFF << 0;

        public static unsafe bool TrySetClipboardOwner(IntPtr hwnd)
        {
            var owner = Native.GetClipboardOwner();
            if (owner == hwnd)
                return true;

            if (!Native.OpenClipboard(hwnd))
            {
                WindowingService.LogWarning("Failed to open clipboard.");
                return false;
            }

            try
            {
                if (!Native.EmptyClipboard())
                {
                    WindowingService.LogWarning("Failed to empty clipboard.");
                    return false;
                }

                return true;
            }
            finally
            {
                if (!Native.CloseClipboard())
                {
                    WindowingService.LogWarning("Failed to empty clipboard.");
                }
            }
        }

        private static void PrintClipboardFormats(IntPtr hwnd)
        {
            Native.OpenClipboard(hwnd);

            try
            {
                Console.WriteLine("Clipboard formats:");

                short format = Native.EnumClipboardFormats(0);
                while (format != 0)
                {
                    var name = ClipboardFormats.GetFormatName(format);
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("- ERROR: Failed to get clipboard format name.");
                    }
                    else
                    {
                        Console.WriteLine($"  - {name}");
                    }

                    format = Native.EnumClipboardFormats(format);
                }
            }
            finally
            {
                Native.CloseClipboard();
            }
        }

        public unsafe static void SetClipboardDIBV5(Span<byte> rgbaData, int width, int height)
        {
            var biSize = Marshal.SizeOf<BitmapV5Header>();

            var header = new BitmapV5Header
            {
                biSize = biSize,
                biWidth = width,
                biHeight = height,
                biPlanes = 1,
                biBitCount = 32,
                biCompression = BI_BITFIELDS,
                biSizeImage = width * height * 4,
                bV5RedMask = Rgba32RedMask,
                bV5GreenMask = Rgba32GreenMask,
                bV5BlueMask = Rgba32BlueMask,
                bV5AlphaMask = Rgba32AlphaMask,
            };

            var size = biSize + width * height * 4;
            var hmem = Native.GlobalAlloc(Constants.GHND, size);
            var ptr = Native.GlobalLock(hmem);
            var dst = new Span<byte>(ptr.ToPointer(), size);
            Marshal.StructureToPtr(header, ptr, false);
            dst = dst.Slice(biSize);

            rgbaData.CopyTo(dst);

            Native.GlobalUnlock(hmem);
            Native.SetClipboardData(ClipboardFormats.CF_DIBV5, hmem);
        }

        public unsafe static void SetClibpoardDIB(Span<byte> rgbaData, int width, int height)
        {

            var biSize = Marshal.SizeOf<BitmapInfoHeader>();
            var header = new BitmapInfoHeader
            {
                biSize = biSize,
                biWidth = width,
                biHeight = height,
                biPlanes = 1,
                biBitCount = 32,
                biCompression = BI_BITFIELDS,
                biSizeImage = width * height * 4,
            };

            var size = biSize + width * height * 4 + 12;
            var hmem = Native.GlobalAlloc(Constants.GHND, size);
            var ptr = Native.GlobalLock(hmem);
            var dst = new Span<byte>(ptr.ToPointer(), size);
            Marshal.StructureToPtr(header, ptr, false);
            Marshal.WriteInt32(ptr, biSize, Rgba32RedMask);
            Marshal.WriteInt32(ptr, biSize + 4, Rgba32GreenMask);
            Marshal.WriteInt32(ptr, biSize + 8, Rgba32BlueMask);
            dst = dst.Slice(biSize + 12);

            rgbaData.CopyTo(dst);

            Native.GlobalUnlock(hmem);
            Native.SetClipboardData(ClipboardFormats.CF_DIB, hmem);
        }

        public unsafe static void SetClipboardPng(Stream pngStream)
        {
            var size = (int) pngStream.Length;

            var hmem = Native.GlobalAlloc(Constants.GHND, size);
            var ptr = Native.GlobalLock(hmem);

            using (var ums = new UnmanagedMemoryStream((byte*) ptr.ToPointer(), size, size, FileAccess.Write))
                pngStream.CopyTo(ums);

            Native.GlobalUnlock(hmem);

            var pngFormat = Native.RegisterClipboardFormat("PNG");
            Native.SetClipboardData(pngFormat, hmem);
        }

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
                    data = new string((char*) ptr);
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
}