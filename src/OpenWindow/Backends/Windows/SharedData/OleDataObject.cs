using System;
using System.Runtime.InteropServices.ComTypes;

using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace OpenWindow.Backends.Windows
{
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
                
                if (ClipboardHelpers.ReadString(medium.unionmember, ansi: true, out var strData) != HResult.S_OK)
                    return false;
                
                data = (T) (object) strData;
                return true;
            }

            if (format.Equals("CF_UNICODETEXT"))
            {
                if (typeof(T) != typeof(string))
                    return false;
                
                if (ClipboardHelpers.ReadString(medium.unionmember, ansi: false, out var strData) != HResult.S_OK)
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

}