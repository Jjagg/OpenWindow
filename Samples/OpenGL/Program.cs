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
        static extern int ChoosePixelFormat(IntPtr hdc, ref PixelFormatDescriptor ppfd);

        [DllImport("gdi32.dll")]
        static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PixelFormatDescriptor ppfd);

        [DllImport("gdi32.dll")]
        static extern int DescribePixelFormat(IntPtr hdc, int iPixelFormat, uint nBytes, ref PixelFormatDescriptor ppfd);

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

        struct PaintStruct
        {
            IntPtr hdc;
            bool fErase;
            Rect rcPaint;
            bool fRestore;
            bool fIncUpdate;
            byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion

        static void Main(string[] args)
        {
            var window = CreateWindow();

            var hdc = window.GetDeviceContext();
            var hrc = WglCreateContext(hdc);
            WglMakeCurrent(hdc, hrc);
            window.ReleaseDeviceContext(hdc);

            while (true)
            {
                var message = window.GetMessage();

                if (message.Type == MessageType.Closing)
                    break;

                hdc = window.GetDeviceContext();

                DrawTriangle();

                window.ReleaseDeviceContext(hdc);

                Thread.Sleep(10);
            }

            WglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            window.ReleaseDeviceContext(hdc);
            WglDeleteContext(hrc);
        }

        static Window CreateWindow()
        {
            var window = Window.Create(100, 100, 600, 600);

            var pfdSize = Marshal.SizeOf<PixelFormatDescriptor>();

            var pfd = new PixelFormatDescriptor();
            pfd.nSize = (short) pfdSize;
            pfd.nVersion = 1;
            pfd.dwFlags = PfdDrawToWindow | PfdSupportOpenGL;
            pfd.iPixelType = PfdTypeRgba;
            pfd.cColorBits = 32;

            var hdc = window.GetDeviceContext();

            var pf = ChoosePixelFormat(hdc, ref pfd);

            if (pf == 0)
                throw new Exception();

            if (!SetPixelFormat(hdc, pf, ref pfd))
                throw new Exception();

            DescribePixelFormat(hdc, pf, (uint) pfdSize, ref pfd);

            window.ReleaseDeviceContext(hdc);

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
