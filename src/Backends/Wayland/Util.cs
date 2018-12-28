using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenWindow.Backends.Wayland
{
    internal static unsafe class Util
    {       
        public static void CreateInterface(wl_interface* iface, string name, uint version, int requestCount, int eventCount)
        {
            iface->Name = StringToUtf8(name);
            iface->Version = version;
            iface->RequestCount = requestCount;
            iface->EventCount = eventCount;
        }

        public static void CreateMessage(wl_message* msg, string name, string signature, wl_interface** types)
        {
            msg->Name = StringToUtf8(name);
            msg->Signature = StringToUtf8(signature);
            msg->Types = types;
        }

        public static byte* StringToUtf8(string s)
        {
            var byteCount = System.Text.Encoding.UTF8.GetByteCount(s) + 1;
            var bytePtr = (byte*) Marshal.AllocHGlobal(byteCount);
            Marshal.WriteByte((IntPtr) bytePtr, byteCount - 1, (byte) '\0');
            StringToUtf8(s, bytePtr, byteCount); 
            return bytePtr;
        }

        public static void StringToUtf8(string s, byte* bytePtr, int byteCount)
        {
            fixed (char* ptr = s)
                System.Text.Encoding.UTF8.GetBytes(ptr, s.Length, bytePtr, byteCount); 
        }

        public static string Utf8ToString(byte* start)
        {
            var end = start;
            while (*end != 0)
                end++;

            var len = (int) (end - start);
            return Encoding.UTF8.GetString(start, len);
        }
    }
}