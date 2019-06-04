using System;
using System.Runtime.InteropServices;
using System.Threading;
using OpenWindow;
using OpenWindow.GL;

namespace OpenGL
{
    class Program
    {
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

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            var service = WindowingService.Create();

            // We need to tell OpenWindow we want to use OpenGL for rendering
            // other settings can stay at default values
            // Note that double buffering is enabled by default
            service.GlSettings.EnableOpenGl = true;
            service.GlSettings.MultiSampleCount = 8;

            var window = service.CreateWindow();
            window.ClientBounds = new Rectangle(100, 100, 600, 600);
            window.Title = "I'm rendering with OpenGL!";

            OpenWindowGl.Initialize(service);
            LoadOpenGL(OpenWindowGl.GetProcAddress);

            var ctx = OpenWindowGl.CreateContext(window, 3, 0);
            if (ctx == IntPtr.Zero)
            {
                Console.WriteLine("ERROR: Context creation failed.");
                return;
            }

            if (!OpenWindowGl.MakeCurrent(window, ctx))
            {
                Console.WriteLine("ERROR: MakeCurrent failed.");
                return;
            }

            glClearColor(0, 0, 0, 1);
            // enable multisampling
            glEnable(0x809D);

            service.PumpEvents();

            while (!window.ShouldClose)
            {
                DrawTriangle();

                // because we enabled double buffering we need to swap buffers here.
                if (!OpenWindowGl.SwapBuffers(window))
                {
                    Console.WriteLine("ERROR: SwapBuffers failed.");
                    return;
                }

                Thread.Sleep(10);

                service.PumpEvents();
            }

            OpenWindowGl.MakeCurrent(window, IntPtr.Zero);
            OpenWindowGl.DestroyContext(ctx);
            window.Dispose();
            service.Dispose();
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
