using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal sealed class ArgumentList : IDisposable
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct ArgumentStruct
        {
            [FieldOffset(0)] public int I; // int
            [FieldOffset(0)] public uint U; // uint
            [FieldOffset(0)] public int F; // fixed
            [FieldOffset(0)] public IntPtr S; // string
            [FieldOffset(0)] public IntPtr O; // object
            [FieldOffset(0)] public uint N; // new_id
            [FieldOffset(0)] public IntPtr A; // wl_array
            [FieldOffset(0)] public int H; // fd
        }

        private static readonly int StructSize = MarshalHelpers.SizeOf<ArgumentStruct>();

        public IntPtr Pointer { get; }

        public ArgumentList(object arg1)
        {
            Pointer = Marshal.AllocHGlobal(StructSize);
            Set(0, arg1);
        }

        public ArgumentList(object arg1, object arg2)
        {
            Pointer = Marshal.AllocHGlobal(2 * StructSize);
            Set(0, arg1);
            Set(1, arg2);
        }

        public ArgumentList(object arg1, object arg2, object arg3)
        {
            Pointer = Marshal.AllocHGlobal(3 * StructSize);
            Set(0, arg1);
            Set(1, arg2);
            Set(2, arg3);
        }

        public ArgumentList(object arg1, object arg2, object arg3, object arg4)
        {
            Pointer = Marshal.AllocHGlobal(3 * StructSize);
            Set(0, arg1);
            Set(1, arg2);
            Set(2, arg3);
            Set(3, arg4);
        }

        public ArgumentList(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            Pointer = Marshal.AllocHGlobal(3 * StructSize);
            Set(0, arg1);
            Set(1, arg2);
            Set(2, arg3);
            Set(3, arg4);
            Set(4, arg5);
        }

        public ArgumentList(params object[] args)
        {
            Pointer = Marshal.AllocHGlobal(args.Length * StructSize);

            for (var i = 0; i < args.Length; i++)
                Set(i, args[i]);
        }

        private void Set(int i, object o)
        {
            if (o is int)
                Set(i, (int) o);
            else if (o is uint)
                Set(i, (uint) o);
            else if (o is string)
                Set(i, (string) o);
            else if (o is WlObject)
                Set(i, (WlObject) o);
            else if (o is WlArray)
                Set(i, (WlArray) o);
        }

        private void Set(int i, int value)
        {
            Marshal.WriteInt32(Pointer, i * StructSize, value);
        }

        private void Set(int i, uint value)
        {
            Marshal.WriteInt32(Pointer, i * StructSize, (int) value);
        }

        private void Set(int i, string value)
        {
            var ptr = Marshal.StringToHGlobalAnsi(value);
            Marshal.WriteIntPtr(Pointer, i * StructSize, ptr);
        }

        private void Set(int i, WlObject value)
        {
            Marshal.WriteIntPtr(Pointer, i * StructSize, value.Pointer);
        }

        private void Set(int i, WlArray value)
        {
            Marshal.WriteIntPtr(Pointer, i * StructSize, value.Pointer);
        }

        private void Free()
        {
            Marshal.FreeHGlobal(Pointer);
        }

        public void Dispose()
        {
            Free();
            GC.SuppressFinalize(this);
        }

        ~ArgumentList()
        {
            Free();
        }
    }

    internal sealed class WlArray : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct ArrayStruct
        {
            public uint Size;
            public uint Alloc;
            public IntPtr Data;
        }

        internal readonly IntPtr Pointer;

        public WlArray()
        {
            Pointer = Marshal.AllocHGlobal(MarshalHelpers.SizeOf<ArrayStruct>());
            ArrayInit(Pointer);
        }

        private void ReleaseUnmanagedResources()
        {
            ArrayRelease(Pointer);
            Marshal.FreeHGlobal(Pointer);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WlArray()
        {
            ReleaseUnmanagedResources();
        }

        [DllImport("libwayland-client.so")]
        private static extern void ArrayInit(IntPtr array);

        [DllImport("libwayland-client.so")]
        private static extern void ArrayRelease(IntPtr array);

        [DllImport("libwayland-client.so")]
        private static extern IntPtr ArrayAdd(IntPtr array, uint size);
    }

    internal sealed class WlMessage : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MessageStruct
        {
            public string Name;
            public string Signature;
            public IntPtr Types;
        }

        public static int StructSize = MarshalHelpers.SizeOf<MessageStruct>();

        public readonly MessageStruct Message;

        public WlMessage(string name, string signature, IReadOnlyList<WlInterface> types)
        {
            Message.Name = name;
            Message.Signature = signature;

            Message.Types = Marshal.AllocHGlobal(types.Count * IntPtr.Size);
            for (var i = 0; i < types.Count; i++)
                Marshal.WriteIntPtr(Message.Types, i * IntPtr.Size, types[i].Pointer);
        }

        public void MarshalTo(IntPtr ptr)
        {
            Marshal.StructureToPtr(Message, ptr, false);
        }

        private void ReleaseUnmanagedResources()
        {
            if (Message.Types != IntPtr.Zero)
                Marshal.Release(Message.Types);
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

    internal sealed class WlInterface : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct InterfaceStruct
        {
            public string Name;
            public int Version;
            public int MethodCount;
            public IntPtr Methods;
            public int EventCount;
            public IntPtr Events;
        }

        private static readonly int MessageSize = MarshalHelpers.SizeOf<WlMessage.MessageStruct>();

        private InterfaceStruct _managed;
        public IntPtr Pointer { get; }
        private bool _finished;

        public WlInterface(string name, int version, int methodCount, int eventCount)
        {
            _managed.Name = name;
            _managed.Version = version;

            _managed.MethodCount = methodCount;
            _managed.Methods =
                methodCount == 0 ? IntPtr.Zero : Marshal.AllocHGlobal(methodCount * WlMessage.StructSize);
            _managed.EventCount = eventCount;
            _managed.Events = eventCount == 0 ? IntPtr.Zero : Marshal.AllocHGlobal(eventCount * WlMessage.StructSize);

            Pointer = Marshal.AllocHGlobal(MarshalHelpers.SizeOf<InterfaceStruct>());
        }

        public void SetRequests(WlMessage[] requests)
        {
            if (_finished)
                throw new Exception("Requests and events must be set before calling Finish.");

            _managed.Methods = requests.Any()
                ? Marshal.AllocHGlobal(requests.Length * IntPtr.Size)
                : IntPtr.Zero;
            for (var i = 0; i < requests.Length; i++)
            {
                var ptr = Marshal.AllocHGlobal(MessageSize);
                Marshal.StructureToPtr(requests[i].Message, ptr, false);
                Marshal.WriteIntPtr(Pointer, i * IntPtr.Size, ptr);
            }
        }

        public void SetEvents(WlMessage[] events)
        {
            if (_finished)
                throw new Exception("Requests and events must be set before calling Finish.");

            _managed.Methods = events.Any()
                ? Marshal.AllocHGlobal(events.Length * IntPtr.Size)
                : IntPtr.Zero;
            for (var i = 0; i < events.Length; i++)
            {
                var ptr = Marshal.AllocHGlobal(MessageSize);
                Marshal.StructureToPtr(events[i].Message, ptr, false);
                Marshal.WriteIntPtr(Pointer, i * IntPtr.Size, ptr);
            }
        }

        public void Finish()
        {
            Marshal.StructureToPtr(_managed, Pointer, false);
            _finished = true;
        }

        private void ReleaseUnmanagedResources()
        {
            var ptr = _managed.Methods;
            for (var i = 0; i < _managed.MethodCount; i++)
            {
                Marshal.FreeHGlobal(ptr);
                ptr += IntPtr.Size;
            }

            ptr = _managed.Events;
            for (var i = 0; i < _managed.MethodCount; i++)
            {
                Marshal.FreeHGlobal(ptr);
                ptr += IntPtr.Size;
            }

            Marshal.FreeHGlobal(Pointer);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WlInterface()
        {
            ReleaseUnmanagedResources();
        }
    }
}