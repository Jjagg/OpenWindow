namespace OpenWindow
{
    public struct WindowCreateInfo
    {
        public const int DefaultWidth = 400;
        public const int DefaultHeight = 400;

        public static WindowCreateInfo Default = new WindowCreateInfo(0, 0, DefaultWidth, DefaultHeight, string.Empty);

        public int X;
        public int Y;
        public int Width;
        public int Height;
        public string Title;
        public bool Decorated;
        public bool Resizable;

        public WindowCreateInfo(int width, int height, string title, bool decorated = true, bool resizable = true)
            : this(0, 0, width, height, title, decorated, resizable)
        {
        }

        public WindowCreateInfo(int x, int y, int width, int height, string title, bool decorated = true, bool resizable = true)
        {
            X = x;
            Y = y;
            Width = width <= 0 ? DefaultWidth : width;
            Height = height <= 0 ? DefaultHeight : height;
            Title = title ?? string.Empty;
            Decorated = decorated;
            Resizable = resizable;
        }
    }
}
