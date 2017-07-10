using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal partial class WlDisplay : IDisposable
    {
        public static WlDisplay Connect()
        {
            return new WlDisplay(Connect(null));
        }

        public void Disconnect()
        {
            Disconnect(Pointer);
        }

        public void Roundtrip()
        {
            Roundtrip(Pointer);
        }

        public void Flush()
        {
            Flush(Pointer);
        }

        public void Dispatch()
        {
            Dispatch(Pointer);
        }
        
        public void DispatchPending()
        {
            Dispatch(Pointer);
        }
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_display_connect")]
        private static extern IntPtr Connect(string strConn);

        [DllImport("libwayland-client.so", EntryPoint = "wl_display_disconnect")]
        private static extern void Disconnect(IntPtr dpy);

        [DllImport("libwayland-client.so", EntryPoint = "wl_display_roundtrip")]
        private static extern int Roundtrip(IntPtr dpy);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_display_flush")]
        private static extern int Flush(IntPtr dpy);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_display_dispatch")]
        private static extern int Dispatch(IntPtr dpy);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_display_dispatch_pending")]
        private static extern int DispatchPending(IntPtr dpy);

        private void ReleaseUnmanagedResources()
        {
            Disconnect(Pointer);
            Destroy();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WlDisplay()
        {
            ReleaseUnmanagedResources();
        }
    }
}