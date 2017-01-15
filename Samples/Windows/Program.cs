// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using OpenWindow;
using OpenWindow.Common;
using System;
using System.Diagnostics;
using System.Threading;

namespace Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            var rect = new Rectangle();

            var window = Window.Create(100, 100, 200, 200);
            var lastf = false;
            var lastr = false;

            var fullscreen = false;

            while (true)
            {
                var keystate = window.GetKeyboardState();
                
                var f = (keystate[(int) VirtualKey.F] & 0x80) != 0;

                if (f && !lastf)
                {
                    window.IsFullscreen = !window.IsFullscreen;
                    if (window.IsFullscreen)
                        rect = window.Bounds;
                    else
                        window.Bounds = rect;
                }

                if ((keystate[(int)VirtualKey.Escape] & 0x80) != 0)
                    window.Close();

                var r = (keystate[(int)VirtualKey.R] & 0x80) != 0;
                if (r && !lastr)
                {
                    var x = rand.Next(100, 300);
                    var y = rand.Next(100, 300);
                    var width = rand.Next(100, 800);
                    var height = rand.Next(100, 500);
                    window.Bounds = new Rectangle(x, y, width, height);
                }

                lastf = f;
                lastr = r;

                var message = window.GetMessage();

                if (message.Type == MessageType.Closing)
                    break;
            }
        }
    }
}
