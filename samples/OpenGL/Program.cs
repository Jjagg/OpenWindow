// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using OpenWindow;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenGL
{
    class Program
    {
        #region PInvoke

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglCreateContext")]
        public static extern IntPtr WglCreateContext(IntPtr hdc);

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglMakeCurrent")]
        public static extern bool WglMakeCurrent(IntPtr hdc, IntPtr hglrc);

        [DllImport("opengl32.dll", SetLastError = true, EntryPoint = "wglDeleteContext")]
        public static extern bool WglDeleteContext(IntPtr hrc);

        [DllImport("gdi32.dll")]
        static extern bool SwapBuffers(IntPtr hdc);

        [DllImport("opengl32.dll")]
        static extern void glClear(uint mask);

        [DllImport("opengl32.dll")]
        static extern void glEnable(int cap);

        [DllImport("opengl32.dll")]
        static extern void glBegin(uint mode);

        [DllImport("opengl32.dll")]
        static extern void glColor3f(float red, float green, float blue);

        [DllImport("opengl32.dll")]
        static extern void glVertex2f(float x, float y);

        [DllImport("opengl32.dll")]
        static extern void glEnd();

        [DllImport("opengl32.dll")]
        static extern void glFlush();

        [DllImport("opengl32.dll")]
        static extern void glClearColor(float r, float g, float b, float a);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        // TODO IDisposable and cleanup
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        #endregion

        private static Window _window;
        private static IntPtr _hdc;
        private static IntPtr _hrc;

        static void Main(string[] args)
        {
            var service = WindowingService.Get();

            _window = CreateWindow(service);

            _hdc = GetDC(_window.Handle);
            _hrc = WglCreateContext(_hdc);
            WglMakeCurrent(_hdc, _hrc);
            ReleaseDC(_window.Handle, _hdc);

            glClearColor(0, 0, 0, 1);
            // enable multisampling
            glEnable(0x809D);

            service.PumpEvents();

            while (!_window.ShouldClose)
            {
                DrawTriangle();

                _hdc = GetDC(_window.Handle);

                // because we enabled double buffering we need to swap buffers here.
                SwapBuffers(_hdc);

                ReleaseDC(_window.Handle, _hdc);

                Thread.Sleep(10);

                service.PumpEvents();
            }

            WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            WglDeleteContext(_hrc);
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
            glClear(16384);
            glBegin(4);
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
