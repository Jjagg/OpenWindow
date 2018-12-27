using System.Runtime.InteropServices;

namespace OpenWindow
{
    internal static class MarshalHelpers
    {
        public static int SizeOf<T>()
        {
            return Marshal.SizeOf<T>();
        }
    }
}