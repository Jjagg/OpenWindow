using System;

namespace OpenWindow.GL
{
    internal abstract class WindowingGlInterface
    {
        public abstract IntPtr GetProcAddressImpl(string func);
        public abstract bool SetVSyncImpl(VSyncState state);
        public abstract VSyncState GetVSyncImpl();
        public abstract IntPtr CreateContextImpl(WindowData wdata);
        public abstract IntPtr CreateContextImpl(WindowData wdata, int major, int minor);
        public abstract bool MakeCurrentImpl(WindowData wdata, IntPtr ctx);
        public abstract IntPtr GetCurrentContextImpl();
        public abstract bool DestroyContextImpl(IntPtr ctx);
        public abstract bool SwapBuffersImpl(WindowData wdata);
    }
}
