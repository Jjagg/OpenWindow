using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace OpenWindow.Backends.Windows
{
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
                    hr = ClipboardHelpers.WriteString(str, ref hMem, ansi: true);
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
                    hr = ClipboardHelpers.WriteString(str, ref hMem, ansi: false);
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

}