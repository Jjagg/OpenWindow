using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal sealed class WlMessage : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
        public struct MessageStruct
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string Name;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Signature;
            // array of pointers
            public IntPtr Types;
        }

        public static int StructSize = MarshalHelpers.SizeOf<MessageStruct>();

        public MessageStruct Message;

        public WlMessage(string name, string signature, IntPtr[] types)
        {
            Message.Name = name;
            Message.Signature = signature;

            Message.Types = Marshal.AllocHGlobal(types.Length * IntPtr.Size);
            for (var i = 0; i < types.Length; i++)
                Marshal.WriteIntPtr(Message.Types, i * IntPtr.Size, types[i]);
        }

        public void MarshalTo(IntPtr ptr)
        {
            Marshal.StructureToPtr(Message, ptr, false);
        }

        private void ReleaseUnmanagedResources()
        {
            if (Message.Types != IntPtr.Zero)
            {
                Marshal.Release(Message.Types);
                Message.Types = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WlMessage()
        {
            ReleaseUnmanagedResources();
        }
    }
}
