using System;

namespace OpenWindow.GL
{
    internal abstract class WindowingGlInterface
    {
        internal virtual void Initialize() { }
        internal abstract IntPtr GetProcAddressImpl(string func);
        internal abstract bool SetVSyncImpl(VSyncState state);
        internal abstract VSyncState GetVSyncImpl();
        internal abstract IntPtr CreateContextImpl(WindowData wdata);
        internal abstract bool MakeCurrentImpl(WindowData wdata, IntPtr ctx);
        internal abstract IntPtr GetCurrentContextImpl();
        internal abstract bool DestroyContextImpl(IntPtr ctx);
        internal abstract bool SwapBuffersImpl(WindowData wdata);
    }
}
