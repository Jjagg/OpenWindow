using System;
using System.Runtime.InteropServices;

namespace OpenWindow.GL
{
    internal sealed class WglInterface : WindowingGlInterface
    {
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "LoadLibrary")]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "GetProcAddress")]
        public static extern IntPtr WinGetProcAddress(IntPtr hModule, string lpProcName);


        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglGetProcAddress")]
        public static extern IntPtr WglGetProcAddress(string procName);

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglCreateContext")]
        public static extern IntPtr WglCreateContext(IntPtr hdc);

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglMakeCurrent")]
        public static extern bool WglMakeCurrent(IntPtr hdc, IntPtr hglrc);

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglGetCurrentContext")]
        public static extern IntPtr WglGetCurrentContext();
        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglGetCurrentDC")]
        public static extern IntPtr WglGetCurrentDC();

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglDeleteContext")]

        public static extern bool WglDeleteContext(IntPtr hrc);

        [DllImport("gdi32.dll")]
        public static extern bool gdiSwapBuffers(IntPtr hdc);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        private static IntPtr _windowsOpenGlLib;

        private static bool _initialized; 
        private delegate bool WglSwapIntervalEXTDelegate(int interval);
        private static WglSwapIntervalEXTDelegate WglSwapIntervalEXT;
        private delegate int WglGetSwapIntervalEXTDelegate();
        private static WglGetSwapIntervalEXTDelegate WglGetSwapIntervalEXT;

        internal override void Initialize()
        {
            if (!_initialized)
                LoadExtensions();
        }

        private static void LoadExtensions()
        {
            var chdc = WglGetCurrentDC();
            var cctx = WglGetCurrentContext();

            if (cctx == null)
            {
                // We need a current context to query for extension methods so we create one

                // TODO provide way to specifically create dummy window
                var dummyWnd = WindowingService.Get().CreateWindow();
                var wdata = (Win32WindowData) dummyWnd.GetPlatformData();
                var hwnd = wdata.Hwnd;
                var hdc = GetDC(hwnd);
                var ctx = WglCreateContext(hdc);
                if (ctx != IntPtr.Zero || !WglMakeCurrent(hdc, ctx))
                {
                    LoadExtensionsWithContext();
                    WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
                }

                ReleaseDC(hwnd, hdc);
                dummyWnd.Dispose();
            }
            else
            {
                LoadExtensionsWithContext();
            }

            _initialized = true;
        }

        private static void LoadExtensionsWithContext()
        {
            var wsiptr = WglGetProcAddress("wglSwapintervalEXT");
            if (wsiptr != IntPtr.Zero)
            {
                WglSwapIntervalEXT = Marshal.GetDelegateForFunctionPointer<WglSwapIntervalEXTDelegate>(wsiptr);
                WglGetSwapIntervalEXT = Marshal.GetDelegateForFunctionPointer<WglGetSwapIntervalEXTDelegate>(WglGetProcAddress("wglGetSwapintervalEXT"));
            }
        }

        internal override IntPtr GetProcAddressImpl(string func)
        {
            if (_windowsOpenGlLib == IntPtr.Zero)
                _windowsOpenGlLib = LoadLibrary("opengl32.dll");

            var ptr = WinGetProcAddress(_windowsOpenGlLib, func);
            if (ptr == null)
                ptr = WglGetProcAddress(func);

            return ptr;
        }

        internal override bool SetVSyncImpl(VSyncState state)
        {
            return WglSwapIntervalEXT?.Invoke((int) state) ?? false;
        }

        internal override VSyncState GetVSyncImpl()
        {
            var result = WglGetSwapIntervalEXT?.Invoke() ?? 0;
            return (VSyncState) result;
        }

        internal override IntPtr CreateContextImpl(WindowData wdata)
        {
            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            var hrc = WglCreateContext(hdc);
            WglMakeCurrent(hdc, hrc);
            ReleaseDC(hWnd, hdc);
            return hrc;
        }

        internal override bool DestroyContextImpl(IntPtr ctx) => WglDeleteContext(ctx);

        internal override IntPtr GetCurrentContextImpl() => WglGetCurrentContext();

        internal override bool MakeCurrentImpl(WindowData wdata, IntPtr ctx)
        {
            if (ctx == IntPtr.Zero)
                return WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);

            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            var result = WglMakeCurrent(hdc, ctx);
            ReleaseDC(hWnd, hdc);
            return result;
        }

        internal override bool SwapBuffersImpl(WindowData wdata)
        {
            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            var result = gdiSwapBuffers(hdc);
            ReleaseDC(hWnd, hdc);
            return result;
        }
    }
}
