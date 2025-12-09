using BenchmarkDotNet.Running;

namespace Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "grpc")
            BenchmarkRunner.Run<GrpcBenchmark>();

        if (args.Length == 0 || args[0] == "capnp")
            BenchmarkRunner.Run<CapnpBenchmark>();
    }
}
