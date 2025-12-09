using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CapnpGen;

namespace EchoServiceCapnp.Services;

public class CapnpEchoService : IEchoer
{
    public void Dispose() { }

    public Task<IReadOnlyList<byte>> Echo(
        IReadOnlyList<byte> input,
        CancellationToken cancellationToken_ = default
    )
    {
        return Task.FromResult(input);
    }
}
