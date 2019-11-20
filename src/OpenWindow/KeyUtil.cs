using System;

namespace OpenWindow
{
    public class KeyNameMap
    {
        private readonly string[] _values;

        internal KeyNameMap(string[] values)
        {
            _values = values;
        }

        public string this[Key key]
        {
            get => _values[(int) key];
            set => _values[(int) key] = value;
        }
    }

    public class ScanCodeNameMap
    {
        private readonly string[] _values;

        internal ScanCodeNameMap(string[] values)
        {
            _values = values;
        }

        public string this[ScanCode sc]
        {
            get => _values[(int) sc];
            set => _values[(int) sc] = value;
        }
    }

    public static class KeyUtil
    {
        public static ScanCodeNameMap CreateScanCodeNameMap()
        {
            var map = new string[(int) ScanCode.Count];
            for (var i = 0; i < map.Length; i++)
            {
                var sc = (ScanCode) i;
                map[i] = sc.GetName();
            }

            return new ScanCodeNameMap(map);
        }

        public static KeyNameMap CreateVirtualKeyNameMap()
        {
            var map = new string[(int) Key.Count];
            for (var i = 0; i < map.Length; i++)
            {
                var key = (Key) i;
                map[i] = key.GetName();
            }

            return new KeyNameMap(map);
        }
    }
}
