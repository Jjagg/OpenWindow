// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal sealed class WlInterface : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
        internal struct InterfaceStruct
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string Name;
            public int Version;
            public int RequestCount;
            public IntPtr Requests;
            public int EventCount;
            public IntPtr Events;
        }

        private static readonly int MessageSize = MarshalHelpers.SizeOf<WlMessage.MessageStruct>();

        private InterfaceStruct _managed;
        public IntPtr Pointer { get; private set; }
        private bool _finished;

        private WlMessage[] Requests;
        private WlMessage[] Events;

        public string Name => _managed.Name;
        public int Version => _managed.Version;

        public WlInterface(string name, int version, int requestCount, int eventCount)
        {
            _managed.Name = name;
            _managed.Version = version;
            _managed.RequestCount = requestCount;
            _managed.Requests = requestCount > 0
                ? Marshal.AllocHGlobal(requestCount * WlMessage.StructSize)
                : IntPtr.Zero;
            _managed.EventCount = eventCount;
            _managed.Events = eventCount > 0
                ? Marshal.AllocHGlobal(eventCount * WlMessage.StructSize)
                : IntPtr.Zero;

            Pointer = Marshal.AllocHGlobal(MarshalHelpers.SizeOf<InterfaceStruct>());
        }

        public void SetRequests(WlMessage[] requests)
        {
            Requests = requests;
            for (var i = 0; i < requests.Length; i++)
                requests[i].MarshalTo(_managed.Requests + i * WlMessage.StructSize);
        }

        public void SetEvents(WlMessage[] events)
        {
            Events = events;
            for (var i = 0; i < events.Length; i++)
                events[i].MarshalTo(_managed.Events + i * WlMessage.StructSize);
        }

        public void Finish()
        {
            Marshal.StructureToPtr(_managed, Pointer, false);
        }

        private void ReleaseUnmanagedResources()
        {
            if (Pointer == IntPtr.Zero)
                return;

            foreach (var req in Requests)
                req.Dispose();
            if (_managed.Requests != IntPtr.Zero)
                Marshal.FreeHGlobal(_managed.Requests);
            if (_managed.Events != IntPtr.Zero)
                Marshal.FreeHGlobal(_managed.Events);
            Marshal.FreeHGlobal(Pointer);
            Pointer = IntPtr.Zero;
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