using System;
using System.Text;
using System.Threading;
using OpenWindow;

namespace HelloOpenWindow
{
    internal static class Program
    {
        private static WindowingService _service;
        private static Window _window;

        private static readonly Random _random = new Random();

        private const int MinWidth = 200;
        private const int MinHeight = 120;
        private const int MaxWidth = 800;
        private const int MaxHeight = 480;

        private static void Main()
        {
            Console.WriteLine();
            WindowingService.Logger.OutputWriter = Console.Out;

            _service = WindowingService.Create();

            _window = _service.CreateWindow();
            _window.ClientBounds = new Rectangle(100, 100, 400, 400);
            _window.Title = "Hello, OpenWindow! 💩";

            _window.MinSize = new Size(MinWidth, MinHeight);
            _window.MaxSize = new Size(MaxWidth, MaxHeight);

            _window.CloseRequested += (s, e) => Console.WriteLine("Received request to close the window!");
            _window.Closing += (s, e) => Console.WriteLine("Closing the window! Bye :)");
            _window.FocusChanged += (s, e) => Console.WriteLine(e.HasFocus ? "Got focus!" : "Lost focus!");
            _window.MouseDown += (s, e) => Console.WriteLine($"Mouse button '{e.Button}' was pressed.");
            _window.MouseUp += (s, e) => Console.WriteLine($"Mouse button '{e.Button}' was released.");

            _window.KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.B: // border
                        _window.Decorated = !_window.Decorated;
                        break;
                    case Key.R: // resizable
                        _window.Resizable = !_window.Resizable;
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
                    case Key.C:
                        _window.CursorVisible = !_window.CursorVisible;
                        break;
                    case Key.Escape:
                        _window.Close();
                        break;
                }
            };

            _window.KeyDown += (s, e) => Console.WriteLine($"Key Down: {e.Key} ({e.ScanCode})");
            _window.KeyUp += (s, e) => Console.WriteLine($"Key Up: {e.Key} ({e.ScanCode})");
            _window.TextInput += (s, e) => Console.WriteLine($"Got text input: {char.ConvertFromUtf32(e.Character)}");

            while (!_window.ShouldClose)
            {
                _service.WaitEvent();
            }

            _service.DestroyWindow(_window);
            _service.Dispose();
        }

        private static void SetRandomBounds()
        {
            var x = _random.Next(100, 300);
            var y = _random.Next(100, 300);
            var width = _random.Next(MinWidth, MaxWidth);
            var height = _random.Next(MinHeight, MaxHeight);
            _window.ClientBounds = new Rectangle(x, y, width, height);
        }

        private static void PrintWindowInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Window Info:");
            Console.WriteLine($"Client bounds: {_window.ClientBounds}");
            var modifiers = _service.GetKeyModifiers();
            Console.WriteLine($"Control: {(modifiers & KeyMod.Control) > 0}");
            Console.WriteLine($"Shift: {(modifiers & KeyMod.Shift) > 0}");
            Console.WriteLine($"Alt: {(modifiers & KeyMod.Alt) > 0}");
            Console.WriteLine($"Caps Lock: {_service.IsCapsLockOn()}");
            Console.WriteLine($"Num Lock: {_service.IsNumLockOn()}");
            Console.WriteLine($"Scroll Lock: {_service.IsScrollLockOn()}");
            Console.WriteLine();
            Console.Out.Flush();
        }
    }
}
