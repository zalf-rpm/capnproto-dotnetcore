using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Capnp.Util;

namespace Capnp.Rpc;

internal class PromisedCapability : RemoteResolvingCapability
{
    private readonly object _reentrancyBlocker = new();
    private readonly uint _remoteId;
    private readonly TaskCompletionSource<ConsumedCapability> _resolvedCap = new();
    private readonly StrictlyOrderedAwaitTask<Proxy> _whenResolvedProxy;
    private bool _released;

    public PromisedCapability(IRpcEndpoint ep, uint remoteId) : base(ep)
    {
        _remoteId = remoteId;

        async Task<Proxy> AwaitProxy()
        {
            return new Proxy(await _resolvedCap.Task);
        }

        _whenResolvedProxy = AwaitProxy().EnforceAwaitOrder();
    }

    public override StrictlyOrderedAwaitTask WhenResolved => _whenResolvedProxy;

    protected override ConsumedCapability? ResolvedCap
    {
        get
        {
            try
            {
                return _resolvedCap.Task.IsCompleted ? _resolvedCap.Task.Result : null;
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException!;
            }
        }
    }

    public override T? GetResolvedCapability<T>() where T : class
    {
        return _whenResolvedProxy.WrappedTask.GetResolvedCapability<T>();
    }

    internal override Action? Export(IRpcEndpoint endpoint, CapDescriptor.WRITER writer)
    {
        lock (_reentrancyBlocker)
        {
            if (_resolvedCap.Task.IsCompletedSuccessfully)
            {
                using var proxy = new Proxy(_resolvedCap.Task.Result);
                proxy.Export(endpoint, writer);
            }
            else
            {
                if (_ep == endpoint)
                {
                    writer.which = CapDescriptor.WHICH.ReceiverHosted;
                    writer.ReceiverHosted = _remoteId;

                    Debug.Assert(!_released);
                    ++_pendingCallsOnPromise;

                    return () =>
                    {
                        var release = false;

                        lock (_reentrancyBlocker)
                        {
                            if (--_pendingCallsOnPromise == 0 && _resolvedCap.Task.IsCompleted)
                            {
                                _released = true;
                                release = true;
                            }
                        }

                        if (release) _ep.ReleaseImport(_remoteId);
                    };
                }

                this.ExportAsSenderPromise(endpoint, writer);
            }
        }

        return null;
    }

    private async void TrackCall(StrictlyOrderedAwaitTask call)
    {
        try
        {
            await call;
        }
        catch
        {
        }
        finally
        {
            var release = false;

            lock (_reentrancyBlocker)
            {
                if (--_pendingCallsOnPromise == 0 && _resolvedCap.Task.IsCompleted)
                {
                    release = true;
                    _released = true;
                }
            }

            if (release) _ep.ReleaseImport(_remoteId);
        }
    }

    protected override void GetMessageTarget(MessageTarget.WRITER wr)
    {
        wr.which = MessageTarget.WHICH.ImportedCap;
        wr.ImportedCap = _remoteId;
    }

    internal override IPromisedAnswer DoCall(ulong interfaceId, ushort methodId, DynamicSerializerState args)
    {
        lock (_reentrancyBlocker)
        {
            if (_resolvedCap.Task.IsCompleted) return CallOnResolution(interfaceId, methodId, args);

            Debug.Assert(!_released);
            ++_pendingCallsOnPromise;
        }

        var promisedAnswer = base.DoCall(interfaceId, methodId, args);
        TrackCall(promisedAnswer.WhenReturned);
        return promisedAnswer;
    }

    public void ResolveTo(ConsumedCapability resolvedCap)
    {
        var release = false;

        lock (_reentrancyBlocker)
        {
#if DebugFinalizers
                if (resolvedCap != null)
                    resolvedCap.ResolvingCap = this;
#endif
            _resolvedCap.SetResult(resolvedCap!);

            if (_pendingCallsOnPromise == 0)
            {
                release = true;
                _released = true;
            }
        }

        if (release) _ep.ReleaseImport(_remoteId);
    }

    public void Break(string message)
    {
        var release = false;

        lock (_reentrancyBlocker)
        {
#if false
                _resolvedCap.SetException(new RpcException(message));
#else
            _resolvedCap.SetResult(LazyCapability.CreateBrokenCap(message));
#endif

            if (_pendingCallsOnPromise == 0)
            {
                release = true;
                _released = true;
            }
        }

        if (release) _ep.ReleaseImport(_remoteId);
    }

    protected override async void ReleaseRemotely()
    {
        if (!_released)
        {
            _released = true;
            _ep.ReleaseImport(_remoteId);
        }

        try
        {
            using var _ = await _whenResolvedProxy;
        }
        catch
        {
        }
    }

    protected override Call.WRITER SetupMessage(DynamicSerializerState args, ulong interfaceId, ushort methodId)
    {
        var call = base.SetupMessage(args, interfaceId, methodId);

        call.Target.which = MessageTarget.WHICH.ImportedCap;
        call.Target.ImportedCap = _remoteId;

        return call;
    }
}