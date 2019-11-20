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
        public static extern bool SwapBuffers(IntPtr hdc);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        private static IntPtr _windowsOpenGlLib;
        private static bool _extensionsLoaded;

        private delegate IntPtr WglCreateContextAttribsARBDelegate(IntPtr hdc, IntPtr hshareContext, ref int attribList);
        private static WglCreateContextAttribsARBDelegate WglCreateContextAttribsARB;

        private delegate bool WglSwapIntervalEXTDelegate(int interval);
        private static WglSwapIntervalEXTDelegate WglSwapIntervalEXT;
        private delegate int WglGetSwapIntervalEXTDelegate();
        private static WglGetSwapIntervalEXTDelegate WglGetSwapIntervalEXT;

        public WglInterface(WindowingService ws)
        {
            if (!_extensionsLoaded)
                LoadExtensions(ws);
        }

        private static void LoadExtensions(WindowingService ws)
        {
            var cctx = WglGetCurrentContext();

            if (cctx == IntPtr.Zero)
            {
                // We need a current context to query for extension methods so we create one

                // TODO provide way to specifically create dummy window
                var dummyWci = new WindowCreateInfo(1, 1, string.Empty, false, false);
                var dummyWnd = ws.CreateWindow(ref dummyWci);
                var wdata = (Win32WindowData) dummyWnd.GetPlatformData();
                var hwnd = wdata.Hwnd;
                var hdc = GetDC(hwnd);
                var lastError = Marshal.GetLastWin32Error();
                var ctx = WglCreateContext(hdc);
                if (ctx != IntPtr.Zero)
                {
                    if (WglMakeCurrent(hdc, ctx))
                    {
                        LoadExtensionsWithContext();
                        WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
                    }
                    else
                    {
                        WindowingService.LogWarning("Failed to make dummy context current. WGL extensions not loaded.");
                    }

                    WglDeleteContext(ctx);
                }
                else
                {
                    WindowingService.LogWarning("Failed to create dummy context. WGL extensions not loaded.");
                }

                ReleaseDC(hwnd, hdc);
                ws.DestroyWindow(dummyWnd);
            }
            else
            {
                LoadExtensionsWithContext();
            }

            _extensionsLoaded = true;
        }

        private static void LoadExtensionsWithContext()
        {
            var ccptr = WglGetProcAddress("wglCreateContextAttribsARB");
            if (ccptr != IntPtr.Zero)
                WglCreateContextAttribsARB = Marshal.GetDelegateForFunctionPointer<WglCreateContextAttribsARBDelegate>(ccptr);
            
            Console.WriteLine("WglCreateContextAttribsARB: " + (ccptr != IntPtr.Zero));

            var wsiptr = WglGetProcAddress("wglSwapIntervalEXT");
            Console.WriteLine("WglSwapIntervalEXT: " + (wsiptr != IntPtr.Zero));
            if (wsiptr != IntPtr.Zero)
            {
                WglSwapIntervalEXT = Marshal.GetDelegateForFunctionPointer<WglSwapIntervalEXTDelegate>(wsiptr);
                WglGetSwapIntervalEXT = Marshal.GetDelegateForFunctionPointer<WglGetSwapIntervalEXTDelegate>(WglGetProcAddress("wglGetSwapIntervalEXT"));
            }
        }

        public override IntPtr GetProcAddressImpl(string func)
        {
            if (_windowsOpenGlLib == IntPtr.Zero)
                _windowsOpenGlLib = LoadLibrary("opengl32.dll");

            var ptr = WinGetProcAddress(_windowsOpenGlLib, func);
            if (ptr == null)
                ptr = WglGetProcAddress(func);

            return ptr;
        }

        public override bool SetVSyncImpl(VSyncState state)
        {
            return WglSwapIntervalEXT?.Invoke((int) state) ?? false;
        }

        public override VSyncState GetVSyncImpl()
        {
            var result = WglGetSwapIntervalEXT?.Invoke() ?? 0;
            return (VSyncState) result;
        }

        public override IntPtr CreateContextImpl(WindowData wdata)
        {
            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            var hrc = WglCreateContext(hdc);
            ReleaseDC(hWnd, hdc);
            return hrc;
        }

        private const int WGL_CONTEXT_MAJOR_VERSION_ARB = 0x2091;
        private const int WGL_CONTEXT_MINOR_VERSION_ARB = 0x2092;

        public override IntPtr CreateContextImpl(WindowData wdata, int major, int minor)
        {
            if (WglCreateContextAttribsARB == null)
                return CreateContextImpl(wdata);

            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            Span<int> attribs = stackalloc int[5];
            attribs[0] = WGL_CONTEXT_MAJOR_VERSION_ARB; attribs[1] = major;
            attribs[2] = WGL_CONTEXT_MINOR_VERSION_ARB; attribs[3] = minor;
            attribs[4] = 0;
            var hrc = WglCreateContextAttribsARB(hdc, IntPtr.Zero, ref attribs.GetPinnableReference());
            ReleaseDC(hWnd, hdc);
            return hrc;
        }

        public override bool DestroyContextImpl(IntPtr ctx) => WglDeleteContext(ctx);

        public override IntPtr GetCurrentContextImpl() => WglGetCurrentContext();

        public override bool MakeCurrentImpl(WindowData wdata, IntPtr ctx)
        {
            if (ctx == IntPtr.Zero)
                return WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);

            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            var result = WglMakeCurrent(hdc, ctx);
            ReleaseDC(hWnd, hdc);
            return result;
        }

        public override bool SwapBuffersImpl(WindowData wdata)
        {
            var hWnd = ((Win32WindowData) wdata).Hwnd;
            var hdc = GetDC(hWnd);
            var result = SwapBuffers(hdc);
            ReleaseDC(hWnd, hdc);
            return result;
        }
    }
}
