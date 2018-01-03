using System.Runtime.InteropServices;

namespace OpenWindow
{
    internal static class MarshalHelpers
    {
        public static int SizeOf<T>()
        {
#if NETSTANDARD1_1
            return Marshal.SizeOf(typeof(T));
#else
            return Marshal.SizeOf<T>();
#endif
        }
    }
}