using System;
using System.Collections.Generic;
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

        public delegate int EGLGetErrorDelegate();
        public static EGLGetErrorDelegate EGLGetError;

        public static int EGL_NONE = 0x3038;
        // >= 1.3
        public static int EGL_CONTEXT_MAJOR_VERSION = 0x3098;
        // >= 1.4 + EGL_KHR_create_context
        // >= 1.5
        public static int EGL_CONTEXT_MINOR_VERSION = 0x30FB;


        private VSyncState _vsyncState;
        private WaylandWindowingServiceData _wwsd;

        public EglInterface(WindowingService ws)
        {
            // vsync is enabled by default in EGL
            _vsyncState = VSyncState.On;
            ws.InitializeOpenGl();
            _wwsd = (WaylandWindowingServiceData) ws.GetPlatformData();
            LoadMethods();
        }

        private static void LoadMethods()
        {
            EGLGetError = LoadFunc<EGLGetErrorDelegate>("eglGetError");
            EGLGetDisplay = LoadFunc<EGLGetDisplayDelegate>("eglGetDisplay");
            EGLInitialize = LoadFunc<EGLInitializeDelegate>("eglInitialize");
            EGLSwapInterval = LoadFunc<EGLSwapIntervalDelegate>("eglSwapInterval");
            EGLCreateContext = LoadFunc<EGLCreateContextDelegate>("eglCreateContext");
            EGLMakeCurrent = LoadFunc<EGLMakeCurrentDelegate>("eglMakeCurrent");
            EGLGetCurrentContext = LoadFunc<EGLGetCurrentContextDelegate>("eglGetCurrentContext");
            EGLDestroyContext = LoadFunc<EGLDestroyContextDelegate>("eglDestroyContext");
            EGLSwapBuffers = LoadFunc<EGLSwapBuffersDelegate>("eglSwapBuffers");
        }

        private static T LoadFunc<T>(string func) where T : Delegate
        {
            var ptr = EGLGetProcAddress(func);
            if (ptr == IntPtr.Zero)
                return null;
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        public override IntPtr GetProcAddressImpl(string func) => EGLGetProcAddress(func);

        public override bool SetVSyncImpl(VSyncState state)
        {
            if (state == VSyncState.Adaptive)
            {
                WindowingService.LogError("Adaptive VSync is not supported on EGL.");
                return false;
            }

            var result = EGLSwapInterval(_wwsd.EGLDisplay, (int) state);
            if (result)
            {
                _vsyncState = state;
            }
            else
            {
                LogError("eglSwapInterval");
            }
            
            return result;
        }

        public override VSyncState GetVSyncImpl() => _vsyncState;

        public override IntPtr CreateContextImpl(WindowData wdata)
        {
            var wwdata = (WaylandWindowData) wdata;
            var ctx = EGLCreateContext(_wwsd.EGLDisplay, wwdata.EGLConfig, IntPtr.Zero, ref EGL_NONE);

            if (ctx == IntPtr.Zero)
                LogError("eglCreateContext");
            return ctx;
        }

        public override IntPtr CreateContextImpl(WindowData wdata, int major, int minor)
        {
            var attribCount = 2;

            Span<int> attribs = stackalloc int[attribCount * 2 + 1];

            // TODO verify EGL_CONTEXT_MINOR_VERSION is supported
            attribs[0] = EGL_CONTEXT_MAJOR_VERSION;
            attribs[1] = major;
            attribs[2] = EGL_CONTEXT_MINOR_VERSION;
            attribs[3] = minor;

            attribs[attribs.Length - 1] = EGL_NONE;

            var wwdata = (WaylandWindowData) wdata;
            var ctx = EGLCreateContext(_wwsd.EGLDisplay, wwdata.EGLConfig, IntPtr.Zero, ref attribs.GetPinnableReference());

            if (ctx == IntPtr.Zero)
                LogError("eglCreateContext");
            return ctx;
        }

        public override bool MakeCurrentImpl(WindowData wdata, IntPtr ctx)
        {
            var wwdata = (WaylandWindowData) wdata;
            var surface = ctx == IntPtr.Zero ? IntPtr.Zero : wwdata.EGLSurface;
            var result = EGLMakeCurrent(_wwsd.EGLDisplay, surface, surface, ctx);

            if (!result)
                LogError("eglMakeCurrent");
            return result;
        }

        public override IntPtr GetCurrentContextImpl() => EGLGetCurrentContext();

        public override bool DestroyContextImpl(IntPtr ctx)
        {
            var result = EGLDestroyContext(_wwsd.EGLDisplay, ctx);

            if (!result)
                LogError("eglDestroyContext");
            return result;
        }

        public override bool SwapBuffersImpl(WindowData wdata)
        {
            var wwdata = (WaylandWindowData) wdata;
            var result = EGLSwapBuffers(_wwsd.EGLDisplay, wwdata.EGLSurface);

            if (!result)
                LogError("eglSwapBuffers");
            return result;
        }

        private string[] _errorCodes =
        {
            "EGL_SUCCESS",
            "EGL_NOT_INITIALIZED",
            "EGL_BAD_ACCESS",
            "EGL_BAD_ALLOC",
            "EGL_BAD_ATTRIBUTE",
            "EGL_BAD_CONFIG",
            "EGL_BAD_CONTEXT",
            "EGL_BAD_CURRENT_SURFACE",
            "EGL_BAD_DISPLAY",
            "EGL_BAD_MATCH",
            "EGL_BAD_NATIVE_PIXMAP",
            "EGL_BAD_NATIVE_WINDOW",
            "EGL_BAD_PARAMETER",
            "EGL_BAD_SURFACE",
            "EGL_CONTEXT_LOST",
        };

        private void LogError(string func)
        {
            var errorCode = EGLGetError();
            var errorIndex = errorCode - 0x3000;
            var error = errorIndex < _errorCodes.Length ? _errorCodes[errorIndex] : "unknown error code";
            WindowingService.LogError($"{error} on call to {func}.");
        }

    }
}
