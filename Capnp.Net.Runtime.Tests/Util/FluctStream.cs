using System;
using System.IO;
using System.Threading;

namespace Capnp.Net.Runtime.Tests;

internal class FluctStream : Stream
{
    private readonly Stream _baseStream;
    private readonly Random _rng = new();

    public FluctStream(Stream baseStream)
    {
        _baseStream = baseStream;
    }

    public override bool CanRead => _baseStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => _baseStream.CanWrite;

    public override long Length => _baseStream.Length;

    public override long Position
    {
        get => _baseStream.Position;
        set => throw new NotImplementedException();
    }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var n = _rng.Next(0, 8);
        if (n >= 7)
            Thread.Sleep(n - 7);
        return _baseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);
    }
}
