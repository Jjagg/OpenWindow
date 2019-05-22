using System;

namespace OpenWindow.Backends.Wayland.Managed
{
    internal unsafe partial struct WlDisplay
    {
        public static WlDisplay Connect() => WaylandClient.wl_display_connect(null);
        public int Roundtrip() => WaylandClient.wl_display_roundtrip(Pointer);
        public void Disconnect() => WaylandClient.wl_display_disconnect(Pointer);
        public int Dispatch() => WaylandClient.wl_display_dispatch(Pointer);
        public void DispatchPending() => WaylandClient.wl_display_dispatch_pending(Pointer);
        public void Flush() => WaylandClient.wl_display_flush(Pointer);
    }
}