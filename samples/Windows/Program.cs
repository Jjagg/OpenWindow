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
            var rect = new Rectangle();

            var service = WindowingService.Get();
            var window = service.CreateWindow();
            window.ClientBounds = new Rectangle(100, 100, 200, 200);
            
            window.Closing += (sender, arg) => _closing = true;

            var lastf = false;
            var lastr = false;
            var lastb = false;

            while (!_closing)
            {
                var keystate = window.GetKeyboardState();
                
                var f = (keystate[(int) VirtualKey.F] & 0x80) != 0;

                if (f && !lastf)
                {
                    if (!window.Borderless)
                    {
                        rect = window.Bounds;
                        window.SetFullscreen();
                    }
                    else
                    {
                        window.Borderless = false;
                        window.Bounds = rect;
                    }
                }

                var b = (keystate[(int) VirtualKey.B] & 0x80) != 0;

                if (b && !lastb)
                    window.Borderless = !window.Borderless;

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
                lastb = b;
                lastr = r;

                service.Update();
            }
        }
    }
}
