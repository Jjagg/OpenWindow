using OpenWindow;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenGL
{
    class Program
    {
        #region PInvoke Windows

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

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglDeleteContext")]

        public static extern bool WglDeleteContext(IntPtr hrc);


        [DllImport("gdi32.dll")]
        public static extern bool gdiSwapBuffers(IntPtr hdc);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        private static IntPtr _windowsOpenGlLib;

        public static IntPtr WinGlGetProcAddress(string name)
        {
            if (_windowsOpenGlLib == IntPtr.Zero)
                _windowsOpenGlLib = LoadLibrary("opengl32.dll");

            var ptr = WinGetProcAddress(_windowsOpenGlLib, name);
            if (ptr == null)
                ptr = WglGetProcAddress(name);

            return ptr;
        }

        #endregion

        #region EGL

        public static void LoadEGL()
        {
            EGLCreateContext = LoadFunction<EGLCreateContextDelegate>("eglCreateContext");
            EGLMakeCurrent = LoadFunction<EGLMakeCurrentDelegate>("eglMakeCurrent");
            EGLDestroyContext = LoadFunction<EGLDestroyContextDelegate>("eglDestroyContext");
            EGLSwapBuffers = LoadFunction<EGLSwapBuffersDelegate>("eglSwapBuffers");
        }

        private static T LoadFunction<T>(string str) where T : Delegate
        {
            var ptr = EGLGetProcAddress(str);
            if (ptr == IntPtr.Zero)
                return null;

            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        [DllImport("libEGL.so", EntryPoint="eglGetProcAddress")]
        public static extern IntPtr EGLGetProcAddress(string procName);

        public delegate IntPtr EGLCreateContextDelegate(IntPtr display, IntPtr config, IntPtr shareContext, int[] attribList);
        public static EGLCreateContextDelegate EGLCreateContext;

        public delegate bool EGLMakeCurrentDelegate(IntPtr display, IntPtr draw, IntPtr read, IntPtr context);
        public static EGLMakeCurrentDelegate EGLMakeCurrent;

        public delegate bool EGLDestroyContextDelegate(IntPtr display, IntPtr context);
        public static EGLDestroyContextDelegate EGLDestroyContext;

        public delegate bool EGLSwapBuffersDelegate(IntPtr display, IntPtr surface);
        public static EGLSwapBuffersDelegate EGLSwapBuffers;

        #endregion

        #region OpenGL

        public static T LoadFunc<T>(string func, Func<string, IntPtr> getProcAddress) where T : Delegate
        {
            var ptr = getProcAddress(func);
            if (ptr == IntPtr.Zero)
                return null;
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        public static void LoadOpenGL(Func<string, IntPtr> getProcAddress)
        {
            glClear = LoadFunc<glClearDelegate>("glClear", getProcAddress);
            glEnable = LoadFunc<glEnableDelegate>("glEnable", getProcAddress);
            glBegin = LoadFunc<glBeginDelegate>("glBegin", getProcAddress);
            glColor3f = LoadFunc<glColor3fDelegate>("glColor3f", getProcAddress);
            glVertex2f = LoadFunc<glVertex2fDelegate>("glVertex2f", getProcAddress);
            glEnd = LoadFunc<glEndDelegate>("glEnd", getProcAddress);
            glFlush = LoadFunc<glFlushDelegate>("glFlush", getProcAddress);
            glClearColor = LoadFunc<glClearColorDelegate>("glClearColor", getProcAddress);
        }

        public delegate void glClearDelegate(uint mask);
        public static glClearDelegate glClear;

        public delegate void glEnableDelegate(int cap);
        public static glEnableDelegate glEnable;

        public delegate void glBeginDelegate(uint mode);
        public static glBeginDelegate glBegin;

        public delegate void glColor3fDelegate(float red, float green, float blue);
        public static glColor3fDelegate glColor3f;

        public delegate void glVertex2fDelegate(float x, float y);
        public static glVertex2fDelegate glVertex2f;

        public delegate void glEndDelegate();
        public static glEndDelegate glEnd;

        public delegate void glFlushDelegate();
        public static glFlushDelegate glFlush;

        public delegate void glClearColorDelegate(float r, float g, float b, float a);
        public static glClearColorDelegate glClearColor;

        #endregion

        private static Window _window;

        static void Main(string[] args)
        {
            var service = WindowingService.Get();

            _window = CreateWindow(service);
            _window.Show();

            var wdata = _window.GetPlatformData();

            if (wdata.Backend == WindowingBackend.Win32)
            {
                RunWindows(service, (Win32WindowData) wdata);
            }
            else if (wdata.Backend == WindowingBackend.Wayland)
            {
                RunWayland(service, (WaylandWindowData) wdata);
            }
            else
            {
                throw new PlatformNotSupportedException("Only Win32 is implemented for the OpenGL sample.");
            }
        }

        private static void RunWindows(WindowingService service, Win32WindowData wdata)
        {
            LoadOpenGL(WinGlGetProcAddress);

            var hwnd = wdata.Hwnd;

            var hdc = GetDC(hwnd);
            var hrc = WglCreateContext(hdc);
            WglMakeCurrent(hdc, hrc);
            ReleaseDC(hwnd, hdc);

            glClearColor(0, 0, 0, 1);
            // enable multisampling
            glEnable(0x809D);

            service.PumpEvents();

            while (!_window.ShouldClose)
            {
                DrawTriangle();

                hdc = GetDC(hwnd);

                // because we enabled double buffering we need to swap buffers here.
                gdiSwapBuffers(hdc);

                ReleaseDC(hwnd, hdc);

                Thread.Sleep(10);

                service.PumpEvents();
            }

            WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            WglDeleteContext(hrc);
        }

        private static unsafe void RunWayland(WindowingService service, WaylandWindowData wdata)
        {
            LoadEGL();
            LoadOpenGL(EGLGetProcAddress);

            var eglContext = EGLCreateContext(wdata.EGLDisplay, wdata.EGLConfig, IntPtr.Zero, null);
            if (eglContext == null)
                throw new Exception("EGL context creation failed.");

            if (!EGLMakeCurrent(wdata.EGLDisplay, wdata.EGLSurface, wdata.EGLSurface, eglContext))
                throw new Exception("EGL make current failed.");

            glClearColor(0, 0, 0, 1);
            // enable multisampling
            glEnable(0x809D);

            service.PumpEvents();

            while (!_window.ShouldClose)
            {
                DrawTriangle();

                // because we enabled double buffering we need to swap buffers here.
                var success = EGLSwapBuffers(wdata.EGLDisplay, wdata.EGLSurface);

                if (!success)
                    Console.WriteLine("Swap buffers failed!!!!");

                Thread.Sleep(10);

                service.PumpEvents();
            }

            EGLMakeCurrent(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            EGLDestroyContext(wdata.EGLDisplay, eglContext);
        }

        static Window CreateWindow(WindowingService service)
        {
            // We need to tell OpenWindow we want to use OpenGL for rendering
            // other settings can stay at default values
            // Note that double buffering is enabled by default
            service.GlSettings.EnableOpenGl = true;
            service.GlSettings.MultiSampleCount = 8;

            var window = service.CreateWindow();
            window.ClientBounds = new Rectangle(100, 100, 600, 600);
            window.Title = "I'm rendering with OpenGL!";

            return window;
        }

        private static void DrawTriangle()
        {
            glClear(16384); // clear color buffer
            glBegin(4); // primitive type triangles
            glColor3f(1.0f, 0.0f, 0.0f);
            glVertex2f(0, 1f);
            glColor3f(0.0f, 1.0f, 0.0f);
            glVertex2f(-1, -1);
            glColor3f(0.0f, 0.0f, 1.0f);
            glVertex2f(1, -1);
            glEnd();
            glFlush();
        }
    }
}
