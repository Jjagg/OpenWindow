// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        #endregion

        static void Main(string[] args)
        {
        }
    }
}
