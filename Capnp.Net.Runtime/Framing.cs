using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Capnp;

/// <summary>
///     Supports the deserialization of Cap'n Proto messages from a stream (see
///     https://capnproto.org/encoding.html#serialization-over-a-stream).
///     Packing and compression cannot be handled yet.
/// </summary>
public static class Framing
{
    /// <summary>
    ///     Deserializes a message from given stream.
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    /// <returns>The deserialized message</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream" /> is null.</exception>
    /// <exception cref="ArgumentException">The stream does not support reading, is null, or is already closed.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="InvalidDataException">Encountered invalid framing data, too many or too large segments</exception>
    /// <exception cref="OutOfMemoryException">Too many or too large segments, probably due to invalid framing data.</exception>
    public static WireFrame ReadSegments(Stream stream)
    {
        using (var reader = new BinaryReader(stream, Encoding.Default, true))
        {
            return reader.ReadWireFrame();
        }
    }

    /// <summary>
    ///     Deserializes the next Cap'n Proto message from given stream.
    /// </summary>
    /// <param name="reader">The stream to read from</param>
    /// <returns>The message</returns>
    public static WireFrame ReadWireFrame(this BinaryReader reader)
    {
        var scount = reader.ReadUInt32();
        if (scount++ == uint.MaxValue) throw new InvalidDataException("Encountered invalid framing data");

        // Cannot have more segments than the traversal limit
        if (scount >= SecurityOptions.TraversalLimit)
            throw new InvalidDataException(
                "Too many segments. Probably invalid data. Try increasing the traversal limit.");

        var buffers = new Memory<ulong>[scount];

        for (uint i = 0; i < scount; i++)
        {
            var size = reader.ReadUInt32();

            if (size == 0) throw new EndOfStreamException("Stream closed");

            if (size >= SecurityOptions.TraversalLimit)
                throw new InvalidDataException(
                    "Too large segment. Probably invalid data. Try increasing the traversal limit.");

            buffers[i] = new Memory<ulong>(new ulong[size]);
        }

        if ((scount & 1) == 0)
            // Padding
            reader.ReadUInt32();

        FillBuffersFromFrames(buffers, scount, reader);

        return new WireFrame(buffers);
    }

    private static InvalidDataException StreamClosed()
    {
        return new InvalidDataException(
            "Prematurely reached end of stream. Expected more bytes according to framing header.");
    }

    private static void FillBuffersFromFrames(Memory<ulong>[] buffers, uint segmentCount, BinaryReader reader)
    {
        for (uint i = 0; i < segmentCount; i++)
        {
            var buffer = MemoryMarshal.Cast<ulong, byte>(buffers[i].Span);

            do
            {
                var obtained = reader.Read(buffer);
                if (obtained == 0)
                    throw StreamClosed();
                buffer = buffer.Slice(obtained);
            } while (buffer.Length > 0);
        }
    }
}