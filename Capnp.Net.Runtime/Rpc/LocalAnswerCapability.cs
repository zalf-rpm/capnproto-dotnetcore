using System;
using System.Threading;
using System.Threading.Tasks;
using Capnp.Util;

namespace Capnp.Rpc;

internal class LocalAnswerCapability : RefCountingCapability, IResolvingCapability
{
    private readonly StrictlyOrderedAwaitTask<Proxy> _whenResolvedProxy;

    public LocalAnswerCapability(Task<Proxy> proxyTask)
    {
        _whenResolvedProxy = proxyTask.EnforceAwaitOrder();
    }

    public LocalAnswerCapability(StrictlyOrderedAwaitTask<DeserializerState> answer, MemberAccessPath access) :
        this(TransferOwnershipToDummyProxy(answer, access))
    {
    }

    public StrictlyOrderedAwaitTask WhenResolved => _whenResolvedProxy;

    public T? GetResolvedCapability<T>() where T : class
    {
        return _whenResolvedProxy.WrappedTask.GetResolvedCapability<T>();
    }

    private static async Task<Proxy> TransferOwnershipToDummyProxy(StrictlyOrderedAwaitTask<DeserializerState> answer,
        MemberAccessPath access)
    {
        var result = await answer;
        var cap = access.Eval(result);
        var proxy = new Proxy(cap);
        cap?.Release();
        return proxy;
    }

    internal override Action? Export(IRpcEndpoint endpoint, CapDescriptor.WRITER writer)
    {
        if (_whenResolvedProxy.IsCompleted)
        {
            try
            {
                _whenResolvedProxy.Result.Export(endpoint, writer);
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException!;
            }

            return null;
        }

        return this.ExportAsSenderPromise(endpoint, writer);
    }

    private async Task<DeserializerState> CallImpl(ulong interfaceId, ushort methodId, DynamicSerializerState args,
        CancellationToken cancellationToken)
    {
        var proxy = await _whenResolvedProxy;

        cancellationToken.ThrowIfCancellationRequested();

        if (proxy.IsNull)
        {
            args.Dispose();
            throw new RpcException("Broken capability");
        }

        var call = proxy.Call(interfaceId, methodId, args, default);
        var whenReturned = call.WhenReturned;

        using var registration = cancellationToken.Register(() => call.Dispose());
        return await whenReturned;
    }

    internal override IPromisedAnswer DoCall(ulong interfaceId, ushort methodId, DynamicSerializerState args)
    {
        var cts = new CancellationTokenSource();
        return new LocalAnswer(cts, CallImpl(interfaceId, methodId, args, cts.Token));
    }

    protected override async void ReleaseRemotely()
    {
        try
        {
            using var _ = await _whenResolvedProxy;
        }
        catch
        {
        }
    }
}