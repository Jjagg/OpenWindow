// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using OpenWindow;

namespace HelloOpenWindow
{
    internal static class Program
    {
        private static WindowingService _service;
        private static Window _window;

        private static readonly Random Rand = new Random();

        private static void Main()
        {
            Console.WriteLine();
            WindowingService.Logger.OutputWriter = Console.Out;

            _service = WindowingService.Get();

            _window = _service.CreateWindow(false);
            _window.ClientBounds = new Rectangle(100, 100, 400, 400);
            _window.Title = "Hello, OpenWindow!";
            _window.Show();

            _window.CloseRequested += (s, e) => Console.WriteLine("Received request to close the window!");
            _window.Closing += (s, e) => Console.WriteLine("Closing the window! Bye :)");
            _window.FocusChanged += (s, e) => Console.WriteLine(e.HasFocus ? "Got focus!" : "Lost focus!");

            _window.KeyPressed += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.B: // border
                        _window.Decorated = !_window.Decorated;
                        break;
                    case Key.R: // resizable
                        _window.Resizable = !_window.Resizable;
                        break;
                    case Key.H: // hide
                        _window.Hide();
                        _service.PumpEvents();
                        Thread.Sleep(2000);
                        _window.Show();
                        break;
                    case Key.M: // move
                        SetRandomBounds();
                        break;
                    case Key.P: // print
                        PrintWindowInfo();
                        break;
                    case Key.J:
                        _window.Minimize();
                        break;
                    case Key.K:
                        _window.Restore();
                        break;
                    case Key.L:
                        _window.Maximize();
                        break;
                    case Key.Escape:
                        _window.Close();
                        break;
                }
            };

            _window.TextInput += (s, e) => Console.WriteLine($"Got text input: {e.Character}");

            while (!_window.ShouldClose)
            {
                _service.PumpEvents();
                Thread.Sleep(10);
            }

            // destroy the window
            _window.Dispose();
            _service.Dispose();
        }

        private static void SetRandomBounds()
        {
            var x = Rand.Next(100, 300);
            var y = Rand.Next(100, 300);
            var width = Rand.Next(100, 800);
            var height = Rand.Next(100, 500);
            _window.Bounds = new Rectangle(x, y, width, height);
        }

        private static void PrintWindowInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Window Info:");
            Console.WriteLine($"Bounds: {_window.Bounds}");
            var modifiers = _window.GetKeyModifiers();
            Console.WriteLine($"Control: {(modifiers & KeyMod.Control) > 0}");
            Console.WriteLine($"Shift: {(modifiers & KeyMod.Shift) > 0}");
            Console.WriteLine($"Alt: {(modifiers & KeyMod.Alt) > 0}");
            Console.WriteLine($"Caps Lock: {_window.IsCapsLockOn()}");
            Console.WriteLine($"Num Lock: {_window.IsNumLockOn()}");
            Console.WriteLine($"Scroll Lock: {_window.IsScrollLockOn()}");
            Console.WriteLine();
            Console.Out.Flush();
        }
    }
}
