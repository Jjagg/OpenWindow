namespace OpenWindow.Backends.Windows
{
    public interface IDataObject
    {
        bool TryGetData<T>(string format, out T data);
        void SetData<T>(string format, T data);
        bool HasFormat(string format);
    }

}