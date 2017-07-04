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
        static extern void glBegin(uint mode);

        [DllImport("opengl32.dll")]
        static extern void glColor3f(float red, float green, float blue);

        [DllImport("opengl32.dll")]
        static extern void glVertex2i(int x, int y);

        [DllImport("opengl32.dll")]
        static extern void glEnd();

        [DllImport("opengl32.dll")]
        static extern void glFlush();


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        // TODO IDisposable and cleanup
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        struct PixelFormatDescriptor
        {
            public short nSize;
            public short nVersion;
            public int dwFlags;
            public byte iPixelType;
            public byte cColorBits;
            public byte cRedBits;
            public byte cRedShift;
            public byte cGreenBits;
            public byte cGreenShift;
            public byte cBlueBits;
            public byte cBlueShift;
            public byte cAlphaBits;
            public byte cAlphaShift;
            public byte cAccumBits;
            public byte cAccumRedBits;
            public byte cAccumGreenBits;
            public byte cAccumBlueBits;
            public byte cAccumAlphaBits;
            public byte cDepthBits;
            public byte cStencilBits;
            public byte cAuxBuffers;
            public byte iLayerType;
            public byte bReserved;
            public int dwLayerMask;
            public int dwVisibleMask;
            public int dwDamageMask;
        }

        private const int PfdDrawToWindow = 4;
        private const int PfdSupportOpenGL = 32;
        private const int PfdTypeRgba = 0;

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

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

            _hdc = GetDC(_window.Handle);
            _hrc = WglCreateContext(_hdc);
            WglMakeCurrent(_hdc, _hrc);
            ReleaseDC(_window.Handle, _hdc);

            service.Update();

            while (true)
            {
                service.Update();

                if (_closing)
                    break;

                DrawTriangle();

                _hdc = GetDC(_window.Handle);

                // because we enabled double buffering we need to swap buffers here.
                SwapBuffers(_hdc);

                ReleaseDC(_window.Handle, _hdc);

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
