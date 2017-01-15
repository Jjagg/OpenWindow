// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using OpenWindow;
using System;

namespace Windows
{
    class Program
    {

        private static bool _closing;

        static void Main(string[] args)
        {
            var rand = new Random();
            var rect = new OwRectangle();

            var service = WindowingService.Get();
            var window = service.CreateWindow();
            window.ClientBounds = new OwRectangle(100, 100, 200, 200);
            
            window.Closing += (sender, arg) => _closing = true;

            var lastf = false;
            var lastr = false;

            while (!_closing)
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
                    window.Bounds = new OwRectangle(x, y, width, height);
                }

                lastf = f;
                lastr = r;

                service.Update();
            }
        }
    }
}
