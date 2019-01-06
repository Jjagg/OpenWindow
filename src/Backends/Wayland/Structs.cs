﻿using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal struct wl_object { }
    internal struct wl_proxy { }
    internal struct wl_fixed
    {
        // According to spec wl_fixed should be opaque,
        // but conversion methods are not public and
        // we need to know the size of the struct because
        // it's used directly.
        // See: https://gitlab.freedesktop.org/wayland/wayland/issues/69
        //
        // So we implement the conversions ourselves in here

        private int _value;

        public wl_fixed(int value)
        {
            _value = value << 8;
        }

        public wl_fixed(double value)
        {
            _value = (int) (value * 256.0);
        }

        public int ToInt() => _value >> 8;
        public double ToDouble() => _value / 256.0;
    }

    internal unsafe struct wl_interface
    {
        public byte* Name;
        public uint Version;
        public int RequestCount;
        public wl_message* Requests;
        public int EventCount;
        public wl_message* Events;
    }

    internal unsafe struct wl_message
    {
        public byte* Name;
        public byte* Signature;
        // array of pointers
        public wl_interface** Types;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct wl_argument
    {
        [FieldOffset(0)] public int I;
        [FieldOffset(0)] public uint U;
        [FieldOffset(0)] public IntPtr Ptr;

        public static implicit operator wl_argument(int i) => new wl_argument { I = i };
        public static implicit operator wl_argument(uint u) => new wl_argument { U = u };
        public static implicit operator wl_argument(IntPtr ptr) => new wl_argument { Ptr = ptr };
        public static implicit operator wl_argument(void* ptr) => new wl_argument { Ptr = (IntPtr) ptr };
    }

    internal unsafe struct wl_array
    {
        public uint Size;
        public uint Alloc;
        public void* Data;
    }
}
