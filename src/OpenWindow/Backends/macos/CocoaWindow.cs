using System;
using System.Buffers;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Windows
{
    internal sealed class CocoaWindow : Window
    {
        public override Point Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Size ClientSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Rectangle ClientBounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Display GetContainingDisplay()
        {
            throw new NotImplementedException();
        }

        public override WindowData GetPlatformData()
        {
            throw new NotImplementedException();
        }

        protected override void InternalMaximize()
        {
            throw new NotImplementedException();
        }

        protected override void InternalMinimize()
        {
            throw new NotImplementedException();
        }

        protected override void InternalRestore()
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetCursor(ReadOnlySpan<byte> pixelData, int width, int height, int hotspotX, int hotspotY)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetCursorVisible(bool value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetDecorated(bool value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetIcon(ReadOnlySpan<byte> pixelData, int width, int height)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetMaxSize(Size value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetMinSize(Size value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetResizable(bool value)
        {
            throw new NotImplementedException();
        }

        protected override void InternalSetTitle(string value)
        {
            throw new NotImplementedException();
        }
    }
}