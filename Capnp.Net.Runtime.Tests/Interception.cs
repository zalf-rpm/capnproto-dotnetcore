﻿using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Capnp.Net.Runtime.Tests.GenImpls;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using Capnproto_test.Capnp.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
[TestCategory("Coverage")]
public class Interception : TestBase
{
    [TestMethod]
    public void InterceptServerSideObserveCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            var counters = new Counters();

            server.Main = policy.Attach<ITestInterface>(new TestInterfaceImpl(counters));
            using (var main = client.GetMain<ITestInterface>())
            {
                var request1 = main.Foo(123, true);
                var fcc = policy.Calls.ReceiveAsync();
                Assert.IsTrue(fcc.Wait(MediumNonDbgTimeout));
                var cc = fcc.Result;

                var pr = new TestInterface.Params_Foo.READER(cc.InArgs);
                Assert.AreEqual(123u, pr.I);

                cc.ForwardToBob();

                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));
                var rr = new TestInterface.Result_Foo.READER(cc.OutArgs);
                Assert.AreEqual("foo", rr.X);

                cc.ReturnToAlice();

                Assert.IsTrue(request1.Wait(MediumNonDbgTimeout));

                Assert.AreEqual("foo", request1.Result);
            }
        }
    }

    [TestMethod]
    public void InterceptServerSideRedirectCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = policy.Attach<ITestInterface>(new TestInterfaceImpl(counters));
            var redirTarget = new TestInterfaceImpl2();
            using (var main = client.GetMain<ITestInterface>())
            using (redirTarget)
            {
                var request1 = main.Foo(123, true);
                var fcc = policy.Calls.ReceiveAsync();
                Assert.IsTrue(fcc.Wait(MediumNonDbgTimeout));
                var cc = fcc.Result;

                Assert.ThrowsException<ArgumentNullException>(() => cc.Bob = null);
                cc.Bob = redirTarget;
                cc.ForwardToBob();

                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));

                cc.ReturnToAlice();

                Assert.IsTrue(request1.Wait(MediumNonDbgTimeout));

                Assert.AreEqual("bar", request1.Result);
            }

            Assert.IsTrue(redirTarget.IsDisposed);
            Assert.AreEqual(0, counters.CallCount);
        }
    }

    [TestMethod]
    public void InterceptClientSideModifyCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var request1 = main.Foo(321, false);
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));

                Assert.AreEqual(InterceptionState.RequestedFromAlice, cc.State);

                var pr = new TestInterface.Params_Foo.READER(cc.InArgs);
                Assert.AreEqual(321u, pr.I);
                Assert.AreEqual(false, pr.J);

                var pw = cc.InArgs.Rewrap<TestInterface.Params_Foo.WRITER>();
                pw.I = 123u;
                pw.J = true;

                cc.ForwardToBob();

                var rx = policy.Returns.ReceiveAsync();

                // Racing against Bob's answer
                Assert.IsTrue(cc.State == InterceptionState.ForwardedToBob ||
                              cc.State == InterceptionState.ReturnedFromBob);

                Assert.IsTrue(rx.Wait(MediumNonDbgTimeout));
                var rr = new TestInterface.Result_Foo.READER(cc.OutArgs);
                Assert.AreEqual("foo", rr.X);

                Assert.IsFalse(request1.IsCompleted);

                var rw = ((DynamicSerializerState)cc.OutArgs).Rewrap<TestInterface.Result_Foo.WRITER>();
                rw.X = "bar";
                cc.OutArgs = rw;

                Assert.AreEqual(InterceptionState.ReturnedFromBob, cc.State);
                cc.ReturnToAlice();
                Assert.AreEqual(InterceptionState.ReturnedToAlice, cc.State);

                Assert.IsTrue(request1.IsCompleted);

                Assert.AreEqual("bar", request1.Result);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideShortCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var request1 = main.Foo(321, false);
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                Assert.IsFalse(request1.IsCompleted);

                var rw = SerializerState.CreateForRpc<TestInterface.Result_Foo.WRITER>();
                rw.X = "bar";
                cc.OutArgs = rw;

                cc.ReturnToAlice();

                Assert.IsTrue(request1.IsCompleted);

                Assert.AreEqual("bar", request1.Result);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideRejectCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var request1 = main.Foo(321, false);
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                Assert.IsFalse(request1.IsCompleted);

                cc.Exception = "rejected";

                cc.ReturnToAlice();

                Assert.IsTrue(request1.IsCompleted);
                Assert.IsTrue(request1.IsFaulted);
                Assert.AreEqual("rejected", request1.Exception.InnerException.Message);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideCancelReturn()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var request1 = main.Foo(321, false);
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                Assert.IsFalse(request1.IsCompleted);
                Assert.IsFalse(cc.CancelFromAlice.IsCancellationRequested);

                cc.ReturnCanceled = true;

                cc.ReturnToAlice();

                Assert.IsTrue(request1.IsCompleted);
                Assert.IsTrue(request1.IsCanceled);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideOverrideCanceledCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestMoreStuffImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestMoreStuff>()))
            {
                var request1 = main.NeverReturn(new TestInterfaceImpl(new Counters()), new CancellationToken(true));
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                Assert.IsFalse(request1.IsCompleted);
                Assert.IsTrue(cc.CancelFromAlice.IsCancellationRequested);

                cc.ForwardToBob();
                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout), "must return");
                Assert.IsTrue(cc.ReturnCanceled, "must be canceled");
                cc.ReturnCanceled = false;
                cc.Exception = "Cancelled";

                cc.ReturnToAlice();

                Assert.IsTrue(request1.IsCompleted);
                Assert.IsTrue(request1.IsFaulted);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideOverrideFaultedCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var request1 = main.Bar();
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                Assert.IsFalse(request1.IsCompleted);

                cc.ForwardToBob();
                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));
                Assert.IsNotNull(cc.Exception);
                cc.ReturnCanceled = false;
                cc.Exception = null;

                cc.ReturnToAlice();
                Assert.ThrowsException<InvalidOperationException>(() => cc.ReturnToAlice());

                Assert.IsTrue(request1.IsCompleted);
                Assert.IsFalse(request1.IsFaulted);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideRedirectCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var request1 = main.Foo(123, true);
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                Assert.IsFalse(request1.IsCompleted);

                var counters2 = new Counters();
                var impl2 = new TestInterfaceImpl(counters2);
                cc.Bob = impl2;
                cc.ForwardToBob();

                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));

                cc.ReturnToAlice();

                Assert.IsTrue(request1.IsCompleted);
                Assert.AreEqual("foo", request1.Result);
                Assert.AreEqual(0, counters.CallCount);
                Assert.AreEqual(1, counters2.CallCount);
            }
        }
    }

    [TestMethod]
    public void InterceptClientSideModifyPipelinedCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair(TcpRpcTestOptions.ClientTracer);

        using (server)
        using (client)
        {
            var counters = new Counters();
            var impl = new TestMoreStuffImpl(counters);
            server.Main = impl;
            using (var main = policy.Attach(client.GetMain<ITestMoreStuff>()))
            {
                var req = main.GetNull().Eager().GetCallSequence(0);
                Assert.IsTrue(policy.Calls.TryReceive(out var ccGetNull));
                Assert.IsTrue(policy.Calls.TryReceive(out var ccGetCallSequence));

                ccGetNull.ForwardToBob();

                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));

                ccGetNull.ReturnToAlice();

                ccGetCallSequence.Bob = Proxy.Share<ITestMoreStuff>(impl);
                ccGetCallSequence.ForwardToBob();

                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));

                ccGetCallSequence.ReturnToAlice();

                Assert.IsTrue(req.IsCompleted && !req.IsFaulted && !req.IsCanceled);
                Assert.AreEqual(0u, req.Result);
            }
        }
    }

    [TestMethod]
    public void InterfaceAndMethodId()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestInterface>()))
            {
                var baz = main.Baz(new TestAllTypes());
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));

                Assert.IsTrue(cc.MethodId == 2);
                Assert.AreEqual(new TestInterface_Skeleton().InterfaceId, cc.InterfaceId);
            }
        }
    }

    [TestMethod]
    public void TailCall()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestTailCallerImpl(counters);
            using (var main = client.GetMain<ITestTailCaller>())
            {
                var calleeCallCount = new Counters();
                var callee = policy.Attach<ITestTailCallee>(new TestTailCalleeImpl(calleeCallCount));

                var promise = main.Foo(456, callee);
                var ccf = policy.Calls.ReceiveAsync();
                Assert.IsTrue(ccf.Wait(MediumNonDbgTimeout));
                var cc = ccf.Result;
                cc.ForwardToBob();
                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));
                cc.ReturnToAlice();
                Assert.IsTrue(promise.Wait(MediumNonDbgTimeout));
                Assert.AreEqual("from TestTailCaller", promise.Result.T);
            }
        }
    }

    [TestMethod]
    public void InterceptInCaps()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestMoreStuffImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestMoreStuff>()))
            {
                var counters2 = new Counters();
                var cap = new TestInterfaceImpl(counters2);
                var promise = main.CallFoo(cap);
                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                cc.InterceptInCaps();
                cc.ForwardToBob();
                var cc2f = policy.Calls.ReceiveAsync();
                Assert.IsTrue(cc2f.Wait(MediumNonDbgTimeout));
                var cc2 = cc2f.Result;
                cc2.ForwardToBob();
                var cc2fr = policy.Returns.ReceiveAsync();
                Assert.IsTrue(cc2fr.Wait(MediumNonDbgTimeout));
                Assert.AreSame(cc2, cc2fr.Result);
                Assert.AreEqual(1, counters2.CallCount);
                cc2.ReturnToAlice();
                var ccfr = policy.Returns.ReceiveAsync();
                Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                Assert.AreSame(cc, ccfr.Result);
                cc.ReturnToAlice();
                Assert.IsTrue(promise.Wait(MediumNonDbgTimeout));
            }
        }
    }

    [TestMethod]
    public void InterceptOutCaps()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = policy.Attach<ITestMoreStuff>(new TestMoreStuffImpl(counters));
            using (var main = client.GetMain<ITestMoreStuff>())
            {
                var counters2 = new Counters();
                var cap = new TestInterfaceImpl(counters2);
                main.Hold(cap);
                {
                    var ccf = policy.Calls.ReceiveAsync();
                    Assert.IsTrue(ccf.Wait(MediumNonDbgTimeout));
                    ccf.Result.ForwardToBob();
                    var ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    ccf.Result.ReturnToAlice();
                }

                var ghf = main.GetHeld();
                {
                    var ccf = policy.Calls.ReceiveAsync();
                    Assert.IsTrue(ccf.Wait(MediumNonDbgTimeout));
                    ccf.Result.ForwardToBob();
                    var ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    ccf.Result.InterceptOutCaps();
                    ccf.Result.ReturnToAlice();
                }

                Assert.IsTrue(ghf.Wait(MediumNonDbgTimeout));
                var held = ghf.Result;

                var foof = held.Foo(123, true);
                {
                    var ccf = policy.Calls.ReceiveAsync();
                    Assert.IsTrue(ccf.Wait(MediumNonDbgTimeout));
                    ccf.Result.ForwardToBob();
                    var ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    ccf.Result.ReturnToAlice();
                }

                Assert.IsTrue(foof.Wait(MediumNonDbgTimeout));
            }
        }
    }

    [TestMethod]
    public void UninterceptOutCaps()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestMoreStuffImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestMoreStuff>()))
            {
                var counters2 = new Counters();
                var cap = new TestInterfaceImpl(counters2);
                main.Hold(cap);
                {
                    Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                    cc.InterceptInCaps();
                    cc.ForwardToBob();
                    var ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    cc.ReturnToAlice();
                }

                main.CallHeld();
                {
                    // CallHeld
                    Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                    cc.ForwardToBob();

                    // actual call on held cap.
                    var ccf = policy.Calls.ReceiveAsync();
                    Assert.IsTrue(ccf.Wait(MediumNonDbgTimeout));
                    ccf.Result.ForwardToBob();

                    var ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    ccf.Result.ReturnToAlice();

                    ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    cc.ReturnToAlice();
                }

                var ghf = main.GetHeld();
                {
                    Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                    cc.InterceptInCaps();
                    cc.ForwardToBob();
                    var ccfr = policy.Returns.ReceiveAsync();
                    Assert.IsTrue(ccfr.Wait(MediumNonDbgTimeout));
                    cc.UninterceptOutCaps();
                    cc.ReturnToAlice();
                }

                Assert.IsTrue(ghf.Wait(MediumNonDbgTimeout));
                var held = ghf.Result;

                var foof = held.Foo(123, true);
                Assert.IsTrue(foof.Wait(MediumNonDbgTimeout));
            }
        }
    }

    [TestMethod]
    public void UninterceptInCaps()
    {
        var policy = new MyPolicy("a");

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestMoreStuffImpl(counters);
            using (var main = policy.Attach(client.GetMain<ITestMoreStuff>()))
            {
                var counters2 = new Counters();
                var cap = policy.Attach<ITestInterface>(new TestInterfaceImpl(counters2));

                var foof = main.CallFoo(cap);

                Assert.IsTrue(policy.Calls.TryReceive(out var cc));
                cc.UninterceptInCaps();
                cc.ForwardToBob();
                Assert.IsTrue(policy.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));
                cc.ReturnToAlice();
                Assert.IsTrue(foof.Wait(MediumNonDbgTimeout));
            }
        }
    }

    [TestMethod]
    public void MultiAttachAndDetach()
    {
        var a = new MyPolicy("a");
        var b = new MyPolicy("b");
        var c = new MyPolicy("c");

        var counters = new Counters();
        var impl = new TestInterfaceImpl(counters);

        var implA = a.Attach<ITestInterface>(impl);
        var implAbc = b.Attach(a.Attach(b.Attach(c.Attach(implA))));
        var implAc = b.Detach(implAbc);

        var (server, client) = SetupClientServerPair();

        using (server)
        using (client)
        {
            //client.WhenConnected.Wait();

            server.Main = implAc;
            using (var main = client.GetMain<ITestInterface>())
            {
                var foof = main.Foo(123, true);

                var ccf1 = c.Calls.ReceiveAsync();
                Assert.IsTrue(ccf1.Wait(MediumNonDbgTimeout));
                var cc1 = ccf1.Result;
                cc1.ForwardToBob();

                var ccf2 = a.Calls.ReceiveAsync();
                Assert.IsTrue(ccf2.Wait(MediumNonDbgTimeout));
                var cc2 = ccf2.Result;
                cc2.ForwardToBob();

                Assert.IsTrue(a.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));
                cc2.ReturnToAlice();

                Assert.IsTrue(c.Returns.ReceiveAsync().Wait(MediumNonDbgTimeout));
                cc1.ReturnToAlice();

                Assert.IsTrue(foof.Wait(MediumNonDbgTimeout));
            }
        }
    }

    [TestMethod]
    public void AttachWrongUse()
    {
        var impl = new TestInterfaceImpl2();
        Assert.ThrowsException<ArgumentNullException>(() => default(IInterceptionPolicy).Attach(impl));
        Assert.ThrowsException<ArgumentNullException>(() => new MyPolicy("x").Attach(default(ITestInterface)));
    }

    [TestMethod]
    public void DetachWrongUse()
    {
        var impl = new TestInterfaceImpl2();
        Assert.ThrowsException<ArgumentNullException>(() => default(IInterceptionPolicy).Detach(impl));
        Assert.ThrowsException<ArgumentNullException>(() => new MyPolicy("x").Detach(default(ITestInterface)));
    }

    [TestMethod]
    public void AttachDetach()
    {
        ConsumedCapability GetCap(object obj)
        {
            return ((Proxy)obj).ConsumedCap;
        }

        var a = new MyPolicy("a");
        var b = new MyPolicy("b");
        var c = new MyPolicy("c");
        var proxy = Proxy.Share<ITestInterface>(new TestInterfaceImpl2());
        var attached = a.Attach(b.Attach(c.Attach(proxy)));
        Assert.AreEqual(GetCap(attached), GetCap(b.Attach(attached)));
        var detached = c.Detach(a.Detach(b.Detach(attached)));
        Assert.AreEqual(GetCap(proxy), GetCap(detached));
    }

    private class MyPolicy : IInterceptionPolicy
    {
        private readonly BufferBlock<CallContext> _callSubject = new();
        private readonly string _id;
        private readonly BufferBlock<CallContext> _returnSubject = new();

        public MyPolicy(string id)
        {
            _id = id;
        }

        public IReceivableSourceBlock<CallContext> Calls => _callSubject;
        public IReceivableSourceBlock<CallContext> Returns => _returnSubject;

        public bool Equals(IInterceptionPolicy other)
        {
            return other is MyPolicy myPolicy && _id.Equals(myPolicy._id);
        }

        public void OnCallFromAlice(CallContext callContext)
        {
            Assert.IsTrue(_callSubject.Post(callContext));
        }

        public void OnReturnFromBob(CallContext callContext)
        {
            Assert.IsTrue(_returnSubject.Post(callContext));
        }

        public override bool Equals(object obj)
        {
            return obj is IInterceptionPolicy other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}