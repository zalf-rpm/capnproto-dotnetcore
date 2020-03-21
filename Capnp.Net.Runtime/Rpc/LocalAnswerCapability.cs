﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Capnp.Rpc
{

    class LocalAnswerCapability : RefCountingCapability, IResolvingCapability
    {
        readonly Task<Proxy> _whenResolvedProxy;

        public LocalAnswerCapability(Task<Proxy> proxyTask)
        {
            _whenResolvedProxy = proxyTask;

            async Task<ConsumedCapability?> AwaitResolved() => (await _whenResolvedProxy).ConsumedCap;
            WhenResolved = AwaitResolved();
        }

        internal override void Freeze(out IRpcEndpoint? boundEndpoint)
        {
            boundEndpoint = null;
        }

        internal override void Unfreeze()
        {
        }


        public Task<ConsumedCapability?> WhenResolved { get; private set; }

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
            else
            {
                return this.ExportAsSenderPromise(endpoint, writer);
            }
        }

        async Task<DeserializerState> CallImpl(ulong interfaceId, ushort methodId, DynamicSerializerState args, CancellationToken cancellationToken)
        {
            var proxy = await _whenResolvedProxy;

            cancellationToken.ThrowIfCancellationRequested();

            if (proxy.IsNull)
                throw new RpcException("Broken capability");

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

        protected async override void ReleaseRemotely()
        {
            try { using var _ = await _whenResolvedProxy; }
            catch { }
        }
    }
}