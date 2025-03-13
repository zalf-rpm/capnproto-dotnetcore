using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Capnp.Rpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

internal class ProvidedCapabilityMultiCallMock : RefCountingSkeleton
{
    private readonly BufferBlock<TestCallContext> _ccs = new();

    public Task<TestCallContext> WhenCalled => _ccs.ReceiveAsync();

    public override Task<AnswerOrCounterquestion> Invoke(ulong interfaceId, ushort methodId,
        DeserializerState args, CancellationToken cancellationToken = default)
    {
        var cc = new TestCallContext(interfaceId, methodId, args, cancellationToken);
        Assert.IsTrue(_ccs.Post(cc));
        return cc.Result.Task;
    }
}