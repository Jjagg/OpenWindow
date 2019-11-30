using System;
using System.Runtime.InteropServices;
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

            var wci = new WindowCreateInfo(100, 100, 400, 400, "Hello, OpenWindow! 💩", decorated: true, resizable: false);
            _window = _service.CreateWindow(ref wci);

            var w = 64;
            var h = 64;
            Span<Color> pixelData = stackalloc Color[w * h];
            var r = new Random();
            for (var y = 0; y < h; y++)
            {
                var vy = (byte) (((float) y) / h * 256);
                for (var x = 0; x < w; x++)
                {
                    var vx = (byte) (((float) x) / w * 256);
                    var vd = (byte) (vx * vy / 255);

                    // nice circular gradient for the alpha
                    var dx = (x - 32) / 32f;
                    var dy = (y - 32) / 32f;
                    var dist = Math.Sqrt(dx * dx + dy * dy);
                    var distEased = dist * dist * dist;
                    var vr = (byte) (Math.Max(255 - 255 * distEased, 0));

                    var c = new Color(vr, vx, vy, vd);
                    pixelData[y * w + x] = c;
                }
            }

            _window.SetIcon<Color>(pixelData, w, h);

            _window.MinSize = new Size(MinWidth, MinHeight);
            _window.MaxSize = new Size(MaxWidth, MaxHeight);

            _window.CloseRequested += (s, e) => Console.WriteLine("Received request to close the window!");
            _window.Closing += (s, e) => Console.WriteLine("Closing the window! Bye :)");
            _window.FocusChanged += (s, e) => Console.WriteLine(e.HasFocus ? "Got focus!" : "Lost focus!");
            _window.MouseDown += (s, e) => Console.WriteLine($"Mouse button '{e.Button}' was pressed.");
            _window.MouseUp += (s, e) => Console.WriteLine($"Mouse button '{e.Button}' was released.");
            //_window.MouseMove += (s, e) => Console.WriteLine($"Mouse move ({e.X} : {e.Y}).");
            _window.MouseFocusChanged += (s, e) => Console.WriteLine(e.HasFocus ? $"Got mouse focus." : "Lost mouse focus.");

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

            var scNameMap = KeyUtil.CreateScanCodeNameMap();
            scNameMap[ScanCode.Left]  = "←";
            scNameMap[ScanCode.Right] = "→";
            scNameMap[ScanCode.Up]    = "↑";
            scNameMap[ScanCode.Down]  = "↓";

            var keyNameMap = KeyUtil.CreateVirtualKeyNameMap();

            _window.KeyDown += (s, e) => Console.WriteLine($"Key Down: {keyNameMap[e.Key]} ({scNameMap[e.ScanCode]})");
            //_window.KeyUp += (s, e) => Console.WriteLine($"Key Up: {keyNameMap[e.Key]} ({scNameMap[e.ScanCode]})");
            _window.TextInput += (s, e) => Console.WriteLine($"Got text input: {CharacterToPrintable(e.Character)}");

            while (!_window.ShouldClose)
            {
                _service.WaitEvent();
            }

            _service.DestroyWindow(_window);
            _service.Dispose();
        }

        private static string CharacterToPrintable(int character)
        {
            return character switch
            {
                '\t' => "\\t",
                '\n' => "\\n",
                '\r' => "\\r",
                ' ' => "Space",
                _ => $"{char.ConvertFromUtf32(character)} (0x{character:X})",
            };
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

        [StructLayout(LayoutKind.Sequential)]
        private struct Color
        {
            public byte B;
            public byte G;
            public byte R;
            public byte A;

            public Color(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            public Color(uint value)
            {
                A = (byte) ((value >> 24) & 0xff);
                R = (byte) ((value >> 16) & 0xff);
                G = (byte) ((value >> 8) & 0xff);
                B = (byte) (value & 0xff);
            }

            public static implicit operator Color(uint v) => new Color(v);
        }
    }
}
