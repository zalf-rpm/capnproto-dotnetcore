using System;
using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Grpc.Net.Client;

namespace Benchmark;

public class GrpcBenchmark
{
    private GrpcChannel _channel;
    private Echoer.EchoerClient _echoer;
    private byte[] _payload;

    [Params(20, 200, 2000, 20000, 200000, 2000000)]
    public int PayloadBytes;

    [GlobalSetup]
    public void Setup()
    {
        _channel = GrpcChannel.ForAddress("http://localhost:5000");
        _echoer = new Echoer.EchoerClient(_channel);
        _payload = new byte[PayloadBytes];
        new Random().NextBytes(_payload);
    }

    [GlobalCleanup]
    public void Teardown()
    {
        _channel.Dispose();
    }

    [Benchmark]
    public void Echo()
    {
        var reply = _echoer.Echo(new EchoRequest { Payload = ByteString.CopyFrom(_payload) });
        if (reply?.Payload?.Length != _payload.Length)
            throw new InvalidOperationException("Echo server malfunction");
    }
}
