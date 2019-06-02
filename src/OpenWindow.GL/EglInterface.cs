using System;
using System.Runtime.InteropServices;

namespace OpenWindow.GL
{
    internal sealed class EglInterface : WindowingGlInterface
    {
        [DllImport("libwayland-client.so")]
        public static extern IntPtr wl_display_connect(IntPtr strConn);

        [DllImport("libEGL.so", EntryPoint="eglGetProcAddress")]
        public static extern IntPtr EGLGetProcAddress(string procName);

        public delegate IntPtr EGLGetDisplayDelegate(IntPtr dpy);
        public static EGLGetDisplayDelegate EGLGetDisplay;

        public delegate bool EGLInitializeDelegate(IntPtr display, out int major, out int minor);
        public static EGLInitializeDelegate EGLInitialize;

        public delegate bool EGLSwapIntervalDelegate(IntPtr display, int interval);
        public static EGLSwapIntervalDelegate EGLSwapInterval;

        public delegate IntPtr EGLCreateContextDelegate(IntPtr display, IntPtr config, IntPtr shareContext, ref int attribList);
        public static EGLCreateContextDelegate EGLCreateContext;

        public delegate bool EGLMakeCurrentDelegate(IntPtr display, IntPtr draw, IntPtr read, IntPtr context);
        public static EGLMakeCurrentDelegate EGLMakeCurrent;

        public delegate IntPtr EGLGetCurrentContextDelegate();
        public static EGLGetCurrentContextDelegate EGLGetCurrentContext;

        public delegate bool EGLDestroyContextDelegate(IntPtr display, IntPtr context);
        public static EGLDestroyContextDelegate EGLDestroyContext;

        public delegate bool EGLSwapBuffersDelegate(IntPtr display, IntPtr surface);
        public static EGLSwapBuffersDelegate EGLSwapBuffers;

        public static int EGL_NONE = 0x3038;
        // >= 1.3
        public static int EGL_CONTEXT_MAJOR_VERSION = 0x3098;
        // >= 1.4 + EGL_KHR_create_context
        // >= 1.5
        public static int EGL_CONTEXT_MINOR_VERSION = 0x30FB;


        private static IntPtr _display;
        private static bool _initialized;

        private VSyncState _vsyncState;

        public EglInterface()
        {
            // vsync is enabled by default in EGL
            _vsyncState = VSyncState.On;
        }

        internal override void Initialize()
        {
            LoadMethods();
            LoadDisplayConnection();
        }

        private void LoadDisplayConnection()
        {
            if (WindowingService.Backend == WindowingBackend.Wayland)
            {
                var wlDisplay = wl_display_connect(IntPtr.Zero);
                if (wlDisplay != IntPtr.Zero)
                    _display = EGLGetDisplay(wlDisplay);
                if (_display != IntPtr.Zero)
                {
                    EGLInitialize(_display, out var major, out var minor);
                }
                else
                {
                    WindowingService.LogError("Failed to load EGL display connection. All EGL calls will fail.");
                }
            }
            else
            {
                throw new Exception("EGL implementation currently only supports Wayland");
            }
        }

        private static void LoadMethods()
        {
            EGLGetDisplay = LoadFunc<EGLGetDisplayDelegate>("eglGetDisplay");
            EGLInitialize = LoadFunc<EGLInitializeDelegate>("eglInitialize");
            EGLSwapInterval = LoadFunc<EGLSwapIntervalDelegate>("eglSwapInterval");
            EGLCreateContext = LoadFunc<EGLCreateContextDelegate>("eglCreateContext");
            EGLMakeCurrent = LoadFunc<EGLMakeCurrentDelegate>("eglMakeCurrent");
            EGLGetCurrentContext = LoadFunc<EGLGetCurrentContextDelegate>("eglGetCurrentContext");
            EGLDestroyContext = LoadFunc<EGLDestroyContextDelegate>("eglDestroyContext");
            EGLSwapBuffers = LoadFunc<EGLSwapBuffersDelegate>("eglSwapBuffers");

            _initialized = true;
        }

        private static T LoadFunc<T>(string func) where T : Delegate
        {
            var ptr = EGLGetProcAddress(func);
            if (ptr == IntPtr.Zero)
                return null;
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        internal override IntPtr GetProcAddressImpl(string func) => EGLGetProcAddress(func);

        internal override bool SetVSyncImpl(VSyncState state)
        {
            if (state == VSyncState.Adaptive)
                return false;

            var result = EGLSwapInterval(_display, (int) state);
            if (result)
                _vsyncState = state;
            
            return result;
        }

        internal override VSyncState GetVSyncImpl() => _vsyncState;

        internal override IntPtr CreateContextImpl(WindowData wdata)
        {
            var config = GetConfig(wdata);
            var attribCount = 2;

            Span<int> attribs = stackalloc int[attribCount * 2 + 1];

            // TODO expose way to set EGL_CONTEXT_*_VERSION
            attribs[0] = EGL_CONTEXT_MAJOR_VERSION;
            attribs[1] = 3;
            attribs[2] = EGL_CONTEXT_MINOR_VERSION;
            attribs[3] = 1;

            attribs[attribCount - 1] = EGL_NONE;

            return EGLCreateContext(_display, config, IntPtr.Zero, ref attribs.GetPinnableReference());
        }

        internal override bool MakeCurrentImpl(WindowData wdata, IntPtr ctx)
        {
            var surface = GetSurface(wdata);
            return EGLMakeCurrent(_display, surface, surface, ctx);
        }

        internal override IntPtr GetCurrentContextImpl() => EGLGetCurrentContext();

        internal override bool DestroyContextImpl(IntPtr ctx) => EGLDestroyContext(_display, ctx);

        internal override bool SwapBuffersImpl(WindowData wdata)
        {
            var surface = GetSurface(wdata);
            return EGLSwapBuffers(_display, surface);
        }

        private IntPtr GetConfig(WindowData wdata)
        {
            IntPtr config;
            if (wdata.Backend == WindowingBackend.Wayland)
            {
                var wldata = ((WaylandWindowData) wdata);
                config = wldata.EGLConfig;
            }
            else
            {
                throw new Exception("EGL implementation currently only supports Wayland");
            }

            return config;
        }
 
        private IntPtr GetSurface(WindowData wdata)
        {
            IntPtr surface;
            if (wdata.Backend == WindowingBackend.Wayland)
            {
                var wldata = ((WaylandWindowData) wdata);
                surface = wldata.EGLSurface;
            }
            else
            {
                throw new Exception("EGL implementation currently only supports Wayland");
            }

            return surface;
        }
    }
}
