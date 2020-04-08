using System;

namespace OpenWindow.Backends.Windows
{
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
}