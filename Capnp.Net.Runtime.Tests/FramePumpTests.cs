using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
[TestCategory("Coverage")]
public class FramePumpTests
{
    [TestMethod]
    public void PipedFramePump()
    {
        int UnpackFrame(WireFrame frame)
        {
            var count = frame.Segments.Count;

            for (var i = 0; i < count; i++) Assert.AreEqual(i + 1, frame.Segments[i].Length);

            return count;
        }

        WireFrame PackFrame(int value)
        {
            var segments = new Memory<ulong>[value];

            for (var i = 0; i < value; i++)
            {
                var a = new ulong[i + 1];
                segments[i] = new Memory<ulong>(a);
            }

            return new WireFrame(segments);
        }

        Thread rxRunner = null;

        using (var listener = new TcpListener(IPAddress.Loopback, 0))
        {
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            var acceptTask = listener.AcceptTcpClientAsync();
            using (var client = new TcpClient("127.0.0.1", port))
            using (var server = acceptTask.Result)
            using (var serverStream = server.GetStream())
            using (var clientStream = client.GetStream())
            using (var bc = new BlockingCollection<int>(16))
            {
                using (var txPump = new FramePump(serverStream))
                using (var rxPump = new FramePump(clientStream))
                {
                    rxRunner = new Thread(() => { rxPump.Run(); });
                    rxRunner.IsBackground = true;

                    rxPump.FrameReceived += f => bc.Add(UnpackFrame(f));

                    rxRunner.Start();

                    for (var i = 0; i < 100; i++)
                    {
                        txPump.Send(PackFrame(1));
                        txPump.Send(PackFrame(8));
                        txPump.Send(PackFrame(2));
                        txPump.Send(PackFrame(7));
                        txPump.Send(PackFrame(3));
                        txPump.Send(PackFrame(6));
                        txPump.Send(PackFrame(4));
                        txPump.Send(PackFrame(5));

                        Assert.IsTrue(SpinWait.SpinUntil(() => bc.Count == 8, 5000));

                        Assert.AreEqual(1, bc.Take());
                        Assert.AreEqual(8, bc.Take());
                        Assert.AreEqual(2, bc.Take());
                        Assert.AreEqual(7, bc.Take());
                        Assert.AreEqual(3, bc.Take());
                        Assert.AreEqual(6, bc.Take());
                        Assert.AreEqual(4, bc.Take());
                        Assert.AreEqual(5, bc.Take());
                    }
                }
            }
        }

        Assert.IsTrue(rxRunner.Join(5000));
    }

    [TestMethod]
    public void FramePumpDeferredProcessing()
    {
        int UnpackAndVerifyFrame(WireFrame frame, int expectedCount)
        {
            var count = frame.Segments.Count;
            Assert.AreEqual(expectedCount, count);

            for (var i = 0; i < count; i++)
            {
                var length = frame.Segments[i].Length;
                Assert.AreEqual(expectedCount - i, length);
                for (var j = 0; j < length; j++)
                {
                    var expected = (ulong)(length - j);
                    var actual = frame.Segments[i].Span[j];
                    Assert.AreEqual(expected, actual);
                }
            }

            return count;
        }

        WireFrame PackFrame(int value)
        {
            var segments = new Memory<ulong>[value];

            for (var i = 0; i < value; i++)
            {
                var a = new ulong[value - i];
                segments[i] = new Memory<ulong>(a);
                for (var j = 0; j < a.Length; j++) a[j] = (ulong)(a.Length - j);
            }

            return new WireFrame(segments);
        }

        Thread rxRunner = null;

        using (var listener = new TcpListener(IPAddress.Loopback, 0))
        {
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            var acceptTask = listener.AcceptTcpClientAsync();
            using (var client = new TcpClient("127.0.0.1", port))
            using (var server = acceptTask.Result)
            using (var serverStream = server.GetStream())
            using (var clientStream = client.GetStream())
            using (var bc = new BlockingCollection<WireFrame>(16))
            {
                using (var txPump = new FramePump(serverStream))
                using (var rxPump = new FramePump(clientStream))
                {
                    rxRunner = new Thread(() => { rxPump.Run(); });
                    rxRunner.IsBackground = true;

                    rxPump.FrameReceived += bc.Add;

                    rxRunner.Start();

                    txPump.Send(PackFrame(1));
                    txPump.Send(PackFrame(8));
                    txPump.Send(PackFrame(2));
                    txPump.Send(PackFrame(7));
                    txPump.Send(PackFrame(3));
                    txPump.Send(PackFrame(6));
                    txPump.Send(PackFrame(4));
                    txPump.Send(PackFrame(5));

                    Assert.IsTrue(SpinWait.SpinUntil(() => bc.Count == 8, 50000));

                    UnpackAndVerifyFrame(bc.Take(), 1);
                    UnpackAndVerifyFrame(bc.Take(), 8);
                    UnpackAndVerifyFrame(bc.Take(), 2);
                    UnpackAndVerifyFrame(bc.Take(), 7);
                    UnpackAndVerifyFrame(bc.Take(), 3);
                    UnpackAndVerifyFrame(bc.Take(), 6);
                    UnpackAndVerifyFrame(bc.Take(), 4);
                    UnpackAndVerifyFrame(bc.Take(), 5);
                }
            }
        }

        Assert.IsTrue(rxRunner.Join(5000));
    }

    private class MyStruct : SerializerState
    {
        public MyStruct()
        {
            SetStruct(0, 1);
        }
    }
}
