using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EchoServiceGrpc.Services;

public class GrpcEchoService(ILogger<GrpcEchoService> logger) : Echoer.EchoerBase
{
    private readonly ILogger<GrpcEchoService> _logger = logger;

    public override Task<EchoReply> Echo(EchoRequest request, ServerCallContext context)
    {
        return Task.FromResult(new EchoReply { Payload = request.Payload });
    }
}
