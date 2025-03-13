﻿using System;
using Capnp.Net.Runtime.Tests.GenImpls;
using Capnp.Net.Runtime.Tests.Util;
using Capnp.Rpc;
using Capnproto_test.Capnp.Test;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
public class TcpRpcStress : TestBase
{
    private void Repeat(int count, Action action)
    {
        for (var i = 0; i < count; i++)
        {
            Logger.LogTrace("Repetition {0}", i);
            action();
        }
    }

    [TestMethod]
    public void ResolveMain()
    {
        Repeat(5000, () =>
        {
            var (server, client) = SetupClientServerPair();

            using (server)
            using (client)
            {
                //client.WhenConnected.Wait();

                var counters = new Counters();
                var impl = new TestMoreStuffImpl(counters);
                server.Main = impl;
                using (var main = client.GetMain<ITestMoreStuff>())
                {
                    var resolving = main as IResolvingCapability;
                    Assert.IsTrue(resolving.WhenResolved.WrappedTask.Wait(MediumNonDbgTimeout));
                }
            }
        });
    }

    [TestMethod]
    public void Cancel()
    {
        var t = new TcpRpcPorted();
        Repeat(1000, t.Cancel);
    }

    [TestMethod]
    public void Embargo()
    {
        var t = new TcpRpcPorted();
        Repeat(100,
            () =>
                NewLocalhostTcpTestbed(TcpRpcTestOptions.ClientTracer | TcpRpcTestOptions.ClientFluctStream)
                    .RunTest(Testsuite.EmbargoOnPromisedAnswer));
    }

    [TestMethod]
    public void EmbargoServer()
    {
        var t2 = new TcpRpcInterop();
        Repeat(20, t2.EmbargoServer);
    }

    [TestMethod]
    public void EmbargoNull()
    {
        // Some code paths are really rare during this test, therefore increased repetition count.

        var t = new TcpRpcPorted();
        Repeat(1000, t.EmbargoNull);

        var t2 = new TcpRpcInterop();
        Repeat(100, t2.EmbargoNullServer);
    }

    [TestMethod]
    public void RetainAndRelease()
    {
        var t = new TcpRpcPorted();
        Repeat(100, t.RetainAndRelease);
    }

    [TestMethod]
    public void PipelineAfterReturn()
    {
        var t = new TcpRpc();
        Repeat(100, t.PipelineAfterReturn);
    }

    [TestMethod]
    public void ScatteredTransfer()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();

        using (var server = new TcpRpcServer(addr, port))
        using (var client = new TcpRpcClient())
        {
            server.InjectMidlayer(s => new ScatteringStream(s, 7));
            client.InjectMidlayer(s => new ScatteringStream(s, 10));
            client.Connect(addr.ToString(), port);
            //client.WhenConnected.Wait();

            var counters = new Counters();
            server.Main = new TestInterfaceImpl(counters);
            using (var main = client.GetMain<ITestInterface>())
            {
                for (var i = 0; i < 100; i++)
                {
                    var request1 = main.Foo(123, true);
                    var request3 = Assert.ThrowsExceptionAsync<RpcException>(() => main.Bar());
                    var s = new TestAllTypes();
                    Common.InitTestMessage(s);
                    var request2 = main.Baz(s);

                    Assert.IsTrue(request1.Wait(MediumNonDbgTimeout));
                    Assert.IsTrue(request2.Wait(MediumNonDbgTimeout));
                    Assert.IsTrue(request3.Wait(MediumNonDbgTimeout));

                    Assert.AreEqual("foo", request1.Result);
                    Assert.AreEqual(2, counters.CallCount);
                    counters.CallCount = 0;
                }
            }
        }
    }
}