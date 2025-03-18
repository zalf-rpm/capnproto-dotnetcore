using System;
using System.Net;
using BenchmarkDotNet.Attributes;
using Capnp.Rpc;
using CapnpGen;

namespace Benchmark;

public class CapnpBenchmark
{
    private TcpRpcClient _client;
    private IEchoer _echoer;
    private byte[] _payload;

    [Params(0, 256, 1024, 4096)] public int BufferSize;

    [Params(20, 200, 2000, 20000, 200000, 2000000)]
    public int PayloadBytes;

    [GlobalSetup]
    public void Setup()
    {
        _client = new TcpRpcClient(IPAddress.Loopback.ToString(), 5002);
        if (BufferSize > 0)
            _client.AddBuffering(BufferSize);
        _client.WhenConnected.Wait();
        _echoer = _client.GetMain<IEchoer>();
        _payload = new byte[PayloadBytes];
        new Random().NextBytes(_payload);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _echoer.Dispose();
        _client.Dispose();
    }

    [Benchmark]
    public void Echo()
    {
        var t = _echoer.Echo(_payload);
        t.Wait();
        if (t.Result?.Count != _payload.Length)
            throw new InvalidOperationException("Echo server malfunction");
    }
}