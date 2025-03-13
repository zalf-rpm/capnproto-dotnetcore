﻿using System;
using System.Threading.Tasks;
using Capnp.Net.Runtime.Tests.GenImpls;
using Capnp.Net.Runtime.Tests.Util;
using Capnp.Rpc;
using CapnpGen;
using Capnproto_test.Capnp.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
[TestCategory("Coverage")]
public class TcpRpcAdvancedStuff : TestBase
{
    [TestMethod]
    public void MultiConnect()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            var counters = new Counters();
            var tcs = new TaskCompletionSource<int>();
            server.Main = new TestInterfaceImpl(counters, tcs);

            for (var i = 1; i <= 10; i++)
            {
                using (var client = SetupClient(addr, port))
                {
                    //client.WhenConnected.Wait();

                    using (var main = client.GetMain<ITestInterface>())
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
                        Assert.AreEqual(2 * i, counters.CallCount);
                    }
                }

                // Bootstrap capability must not be disposed
                Assert.IsFalse(tcs.Task.IsCompleted);
            }
        }
    }

    [TestMethod]
    public void TwoClients()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            var counters = new Counters();
            server.Main = new TestMoreStuffImpl(counters);

            using (var client1 = SetupClient(addr, port))
            using (var client2 = SetupClient(addr, port))
            {
                //Assert.IsTrue(client1.WhenConnected.Wait(MediumNonDbgTimeout));
                //Assert.IsTrue(client2.WhenConnected.Wait(MediumNonDbgTimeout));

                using (var main = client1.GetMain<ITestMoreStuff>())
                {
                    Assert.IsTrue(main.Hold(new TestInterfaceImpl(counters)).Wait(MediumNonDbgTimeout));
                }

                using (var main = client2.GetMain<ITestMoreStuff>())
                {
                    Assert.IsTrue(main.CallHeld().Wait(MediumNonDbgTimeout));
                    var getHeld = main.GetHeld();
                    Assert.IsTrue(getHeld.Wait(MediumNonDbgTimeout));
                    var foo = getHeld.Result.Foo(123, true);
                    Assert.IsTrue(foo.Wait(MediumNonDbgTimeout));
                    Assert.AreEqual("foo", foo.Result);
                }

                client1.Dispose();

                using (var main = client2.GetMain<ITestMoreStuff>())
                {
                    ExpectPromiseThrows(main.CallHeld());
                }
            }
        }
    }

    [TestMethod]
    public void ClosingServerWhileRequestingBootstrap()
    {
        for (var i = 0; i < 100; i++)
        {
            var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
            var server = SetupServer(addr, port);
            var counters = new Counters();
            var tcs = new TaskCompletionSource<int>();
            server.Main = new TestInterfaceImpl(counters, tcs);

            using (var client = SetupClient(addr, port))
            {
                client.WhenConnected.Wait();

                using (var main = client.GetMain<ITestInterface>())
                {
                    server.Dispose();

                    // Resolution must either succeed or be cancelled. A hanging resolution would be inacceptable.

                    try
                    {
                        Assert.IsTrue(((IResolvingCapability)main).WhenResolved.WrappedTask.Wait(MediumNonDbgTimeout));
                    }
                    catch (AggregateException)
                    {
                    }
                }
            }
        }
    }

    [TestMethod]
    public void InheritFromGenericInterface()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            var counters = new Counters();
            server.Main = new B2Impl();

            using (var client = SetupClient(addr, port))
            {
                //client.WhenConnected.Wait();

                using (var main = client.GetMain<IB2>())
                {
                    Assert.IsTrue(main.MethodA("42").Wait(MediumNonDbgTimeout));
                    var b = main.MethodB(123);
                    Assert.IsTrue(b.Wait(MediumNonDbgTimeout));
                    Assert.AreEqual("42", b.Result);
                }
            }
        }
    }

    [TestMethod]
    public void Issue25()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            server.Main = new Issue25BImpl();

            using (var client = SetupClient(addr, port))
            {
                //client.WhenConnected.Wait();

                using (var main = client.GetMain<IIssue25B>())
                {
                    var capholderAPT = main.GetAinCapHolderAnyPointer();
                    Assert.IsTrue(capholderAPT.Wait(MediumNonDbgTimeout));
                    var capholderAP = capholderAPT.Result;
                    var capAPT = capholderAP.Cap();
                    Assert.IsTrue(capAPT.Wait(MediumNonDbgTimeout));
                    using (var a = ((BareProxy)capAPT.Result).Cast<IIssue25A>(true))
                    {
                        var r = a.MethodA();
                        Assert.IsTrue(r.Wait(MediumNonDbgTimeout));
                        Assert.AreEqual(123L, r.Result);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void ExportCapToThirdParty()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            var counters = new Counters();
            server.Main = new TestMoreStuffImpl3();

            using (var client = SetupClient(addr, port))
            {
                //Assert.IsTrue(client.WhenConnected.Wait(MediumNonDbgTimeout));

                using (var main = client.GetMain<ITestMoreStuff>())
                {
                    var held = main.GetHeld().Eager();

                    (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();

                    using (var server2 = SetupServer(addr, port))
                    {
                        server2.Main = new TestMoreStuffImpl2();

                        using (var client2 = SetupClient(addr, port))
                        {
                            //Assert.IsTrue(client2.WhenConnected.Wait(MediumNonDbgTimeout));

                            using (var main2 = client2.GetMain<ITestMoreStuff>())
                            {
                                var fooTask = main2.CallFoo(held);
                                Assert.IsTrue(
                                    main.Hold(new TestInterfaceImpl(new Counters())).Wait(MediumNonDbgTimeout));
                                Assert.IsTrue(fooTask.Wait(MediumNonDbgTimeout));
                                Assert.AreEqual("bar", fooTask.Result);
                            }
                        }
                    }
                }
            }
        }
    }

    [TestMethod]
    public void ExportTailCallCapToThirdParty()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            server.Main = new TestTailCallerImpl2();

            using (var client = SetupClient(addr, port))
            {
                //Assert.IsTrue(client.WhenConnected.Wait(MediumNonDbgTimeout));

                using (var main = client.GetMain<ITestTailCaller>())
                {
                    var callee = new TestTailCalleeImpl(new Counters());
                    var fooTask = main.Foo(123, callee);
                    Assert.IsTrue(fooTask.Wait(MediumNonDbgTimeout));

                    using (var c = fooTask.Result.C)
                    using (var client2 = SetupClient(addr, port))
                    {
                        //Assert.IsTrue(client2.WhenConnected.Wait(MediumNonDbgTimeout));

                        using (var main2 = client2.GetMain<ITestTailCaller>())
                        {
                            var fooTask2 = main2.Foo(123, null);
                            Assert.IsTrue(fooTask2.Wait(MediumNonDbgTimeout));
                            Assert.IsTrue(fooTask2.C().GetCallSequence(0).Wait(MediumNonDbgTimeout));
                        }
                    }
                }
            }
        }
    }

    [TestMethod]
    public void SalamiTactics()
    {
        var (addr, port) = TcpManager.Instance.GetLocalAddressAndPort();
        using (var server = SetupServer(addr, port))
        {
            server.Main = new TestMoreStuffImpl3();

            using (var client = SetupClient(addr, port))
            {
                //client.WhenConnected.Wait();

                using (var main = client.GetMain<ITestMoreStuff>())
                {
                    var echoTask = main.Echo(Proxy.Share<ITestCallOrder>(main));
                    Assert.IsTrue(echoTask.Wait(MediumNonDbgTimeout));
                    using (var echo = echoTask.Result)
                    {
                        var list = new Task<uint>[200];
                        for (uint i = 0; i < list.Length; i++) list[i] = echo.GetCallSequence(i);
                        Assert.IsTrue(Task.WaitAll(list, MediumNonDbgTimeout));
                        for (uint i = 0; i < list.Length; i++) Assert.AreEqual(i, list[i].Result);
                    }
                }
            }
        }
    }


    [TestMethod]
    public void NoTailCallMt()
    {
        NewLocalhostTcpTestbed().RunTest(Testsuite.NoTailCallMt);
    }

    [TestMethod]
    public void CallAfterFinish1()
    {
        NewLocalhostTcpTestbed().RunTest(Testsuite.CallAfterFinish1);
    }

    [TestMethod]
    public void CallAfterFinish2()
    {
        NewLocalhostTcpTestbed().RunTest(Testsuite.CallAfterFinish2);
    }
}