﻿// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using OpenWindow;

namespace HelloOpenWindow
{
    internal class Program
    {
        private static WindowingService _service;
        private static Window _window;

        private static readonly Random Rand = new Random();
        private static bool _closing;

        private static bool _lastr;
        private static bool _lastb;

        private static void Main()
        {
            Initialize();

            while (!_closing)
            {
                Update();
                _service.Update();
            }
        }

        private static void Initialize()
        {
            _service = WindowingService.Get();
            _service.SetLogWriter(Console.Out);

            _window = _service.CreateWindow();
            _window.ClientBounds = new Rectangle(100, 100, 400, 400);
            _window.Title = "Hello, OpenWindow!";

            _window.Closing += (sender, arg) => _closing = true;
        }

        private static void Update()
        {
            var keystate = _window.GetKeyboardState();

            var b = (keystate[(int) VirtualKey.B] & 0x80) != 0;

            if (b && !_lastb)
                _window.Borderless = !_window.Borderless;

            if ((keystate[(int) VirtualKey.Escape] & 0x80) != 0)
                _window.Close();

            var r = (keystate[(int) VirtualKey.R] & 0x80) != 0;
            if (r && !_lastr)
            {
                var x = Rand.Next(100, 300);
                var y = Rand.Next(100, 300);
                var width = Rand.Next(100, 800);
                var height = Rand.Next(100, 500);
                _window.Bounds = new Rectangle(x, y, width, height);
            }

            _lastb = b;
            _lastr = r;
        }
    }
}
