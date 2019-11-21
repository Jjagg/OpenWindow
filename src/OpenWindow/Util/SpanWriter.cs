using System;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenWindow
{
    internal ref struct SpanWriter
    {
        public Span<byte> FullSpan;
        public Span<byte> Span;

        public int Offset => FullSpan.Length - Span.Length;

        public SpanWriter(Span<byte> span)
        {
            FullSpan = span;
            Span = span;
        }

        public void Write(byte v)
        {
            Span[0] = v;
            Span = Span.Slice(1);
        }

        public unsafe void Write(byte v, int amount)
        {
            Span.Slice(0, amount).Fill(v);
            Span.Slice(amount);
        }

        public void Write(sbyte v)
        {
            Span[0] = (byte) v;
            Span = Span.Slice(1);
        }

        public void Write(ushort v)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(Span, v);
            Span = Span.Slice(2);
        }

        public void Write(short v)
        {
            BinaryPrimitives.WriteInt16LittleEndian(Span, v);
            Span = Span.Slice(2);
        }

        public void Write(uint v)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(Span, v);
            Span = Span.Slice(4);
        }

        public void Write(int v)
        {
            BinaryPrimitives.WriteInt32LittleEndian(Span, v);
            Span = Span.Slice(4);
        }

        public void Write(float v)
        {
            Utf8Formatter.TryFormat(v, Span, out _);
            Span = Span.Slice(4);
        }

        public void Write<T>(Span<T> src) where T : struct
        {
            var srcBytes = MemoryMarshal.AsBytes(src);
            srcBytes.CopyTo(Span);
            Span = Span.Slice(srcBytes.Length);
        }

        public void Write<T>(ReadOnlySpan<T> src) where T : struct
        {
            var srcBytes = MemoryMarshal.AsBytes(src);
            srcBytes.CopyTo(Span);
            Span = Span.Slice(srcBytes.Length);
        }

        public void Skip(int v) => Span = Span.Slice(v);

        public bool IsFull => Span.IsEmpty;

        public static implicit operator SpanWriter(Span<byte> span)
            => new SpanWriter(span);

    }
}
