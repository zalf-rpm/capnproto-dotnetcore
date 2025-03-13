using System.Threading;
using System.Threading.Tasks;
using Capnp.Rpc;

namespace Capnp.Net.Runtime.Tests;

internal class ProvidedCapabilityMock : RefCountingSkeleton
{
    private readonly TaskCompletionSource<(ulong, ushort, DeserializerState, CancellationToken)>
        _call = new();

    public Task<(ulong, ushort, DeserializerState, CancellationToken)> WhenCalled =>
        _call.Task;

    public TaskCompletionSource<AnswerOrCounterquestion> Return { get; } = new();

    public override Task<AnswerOrCounterquestion> Invoke(ulong interfaceId, ushort methodId,
        DeserializerState args, CancellationToken cancellationToken = default)
    {
        _call.SetResult((interfaceId, methodId, args, cancellationToken));
        return Return.Task;
    }
}