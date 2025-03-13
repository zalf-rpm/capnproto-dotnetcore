using System;
using System.IO;

namespace Capnp.Net.Runtime.Tests;

/// <summary>
///     Imitates the behavior of a TCP connection by real hardware, which splits data transfer into multiple packets.
/// </summary>
internal class ScatteringStream : Stream
{
    private readonly Stream _baseStream;
    private readonly int _mtu;

    public ScatteringStream(Stream baseStream, int mtu)
    {
        _baseStream = baseStream;
        _mtu = mtu;
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
        while (count > 0)
        {
            var amount = Math.Min(count, _mtu);
            _baseStream.Write(buffer, offset, amount);
            _baseStream.Flush();
            offset += amount;
            count -= amount;
        }
    }
}