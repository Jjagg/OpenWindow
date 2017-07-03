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
        #region Wiggle

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
        static extern void glBegin(uint mode);

        [DllImport("opengl32.dll")]
        static extern void glColor3f(float red, float green, float blue);

        [DllImport("opengl32.dll")]
        static extern void glVertex2i(int x, int y);

        [DllImport("opengl32.dll")]
        static extern void glEnd();

        [DllImport("opengl32.dll")]
        static extern void glFlush();

        #endregion

        private static Window _window;
        private static IntPtr _hdc;
        private static IntPtr _hrc;

        private static bool _closing;

        static void Main(string[] args)
        {
            var service = WindowingService.Get();

            _window = CreateWindow(service);

            _window.Closing += HandleClosing;

            _hdc = _window.GetDeviceContext();
            _hrc = WglCreateContext(_hdc);
            WglMakeCurrent(_hdc, _hrc);
            _window.ReleaseDeviceContext(_hdc);

            while (true)
            {
                service.Update();

                if (_closing)
                    break;

                DrawTriangle();

                _hdc = _window.GetDeviceContext();

                // because we enabled double buffering we need to swap buffers here.
                SwapBuffers(_hdc);

                _window.ReleaseDeviceContext(_hdc);

                Thread.Sleep(10);
            }
        }

        private static void HandleClosing(object sender, EventArgs args)
        {
            _closing = true;
            
            WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            WglDeleteContext(_hrc);
        }

        static Window CreateWindow(WindowingService service)
        {
            // We need to tell OpenWindow we want to use OpenGL for rendering
            // other settings can stay at default values
            // Note that double buffering is enabled by default
            service.GlSettings.EnableOpenGl = true;

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
            glVertex2i(0, 1);
            glColor3f(0.0f, 1.0f, 0.0f);
            glVertex2i(-1, -1);
            glColor3f(0.0f, 0.0f, 1.0f);
            glVertex2i(1, -1);
            glEnd();
            glFlush();
        }
    }
}
