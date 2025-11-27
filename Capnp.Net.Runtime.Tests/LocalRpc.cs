using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Capnp.Net.Runtime.Tests.GenImpls;
using Capnp.Rpc;
using Capnp.Util;
using Capnproto_test.Capnp.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
[TestCategory("Coverage")]
public class LocalRpc : TestBase
{
    [TestMethod]
    public void DeferredLocalAnswer()
    {
        var tcs = new TaskCompletionSource<int>();
        var impl = new TestPipelineImpl2(tcs.Task);
        var bproxy = BareProxy.FromImpl(impl);
        using (var proxy = bproxy.Cast<ITestPipeline>(true))
        using (var cap = proxy.GetCap(0, null).OutBox_Cap())
        {
            var foo = cap.Foo(123, true);
            tcs.SetResult(0);
            Assert.IsTrue(foo.Wait(MediumNonDbgTimeout));
            Assert.AreEqual("bar", foo.Result);
        }
    }

    [TestMethod]
    public void Embargo()
    {
        NewLocalTestbed().RunTest(Testsuite.EmbargoOnPromisedAnswer);
    }

    [TestMethod]
    public void EmbargoError()
    {
        NewLocalTestbed().RunTest(Testsuite.EmbargoError);
    }

    [TestMethod]
    public void EmbargoNull()
    {
        NewLocalTestbed().RunTest(Testsuite.EmbargoNull);
    }

    [TestMethod]
    public void CallBrokenPromise()
    {
        NewLocalTestbed().RunTest(Testsuite.CallBrokenPromise);
    }

    [TestMethod]
    public void TailCall()
    {
        NewLocalTestbed().RunTest(Testsuite.TailCall);
    }

    [TestMethod]
    public void SendTwice()
    {
        NewLocalTestbed().RunTest(Testsuite.SendTwice);
    }

    [TestMethod]
    public void Cancel()
    {
        NewLocalTestbed().RunTest(Testsuite.Cancel);
    }

    [TestMethod]
    public void RetainAndRelease()
    {
        NewLocalTestbed().RunTest(Testsuite.RetainAndRelease);
    }

    [TestMethod]
    public void PromiseResolve()
    {
        NewLocalTestbed().RunTest(Testsuite.PromiseResolve);
    }

    [TestMethod]
    public void Cancelation()
    {
        NewLocalTestbed().RunTest(Testsuite.Cancelation);
    }

    [TestMethod]
    public void ReleaseOnCancel()
    {
        NewLocalTestbed().RunTest(Testsuite.ReleaseOnCancel);
    }

    [TestMethod]
    public void Release()
    {
        NewLocalTestbed().RunTest(Testsuite.Release);
    }

    [TestMethod]
    public void Pipeline()
    {
        NewLocalTestbed().RunTest(Testsuite.Pipeline);
    }

    [TestMethod]
    public void Basic()
    {
        NewLocalTestbed().RunTest(Testsuite.Basic);
    }

    [TestMethod]
    public void Ownership1()
    {
        NewLocalTestbed().RunTest(Testsuite.Ownership1);
    }

    [TestMethod]
    public void Ownership2()
    {
        NewLocalTestbed().RunTest(Testsuite.Ownership2);
    }

    [TestMethod]
    public void Ownership3()
    {
        NewLocalTestbed().RunTest(Testsuite.Ownership3);
    }

    [TestMethod]
    public void ImportReceiverAnswer()
    {
        NewLocalTestbed().RunTest(Testsuite.Ownership3);
    }

    [TestMethod]
    public void LegacyAccess()
    {
        NewLocalTestbed().RunTest(Testsuite.LegacyAccess);
    }

    [TestMethod]
    public void EagerRace()
    {
        var impl = new TestMoreStuffImpl(new Counters());
        var tcs = new TaskCompletionSource<ITestMoreStuff>();
        using (var promise = tcs.Task.Eager(true))
        using (var cts = new CancellationTokenSource())
        {
            var bb = new BufferBlock<Task<uint>>();
            var counter = 0;

            void Generator()
            {
                while (!cts.IsCancellationRequested)
                {
                    bb.Post(promise.GetCallSequence((uint)Volatile.Read(ref counter)));
                    Interlocked.Increment(ref counter);
                }

                bb.Complete();
            }

            async Task Verifier()
            {
                uint i = 0;
                while (true)
                {
                    Task<uint> t;

                    try
                    {
                        t = await bb.ReceiveAsync();
                    }
                    catch (InvalidOperationException)
                    {
                        break;
                    }

                    var j = await t;
                    Assert.AreEqual(i, j);
                    i++;
                }
            }

            var genTask = Task.Run(() => Generator());
            var verTask = Verifier();
            SpinWait.SpinUntil(() => Volatile.Read(ref counter) >= 100);
            Task.Run(() => tcs.SetResult(impl));
            cts.Cancel();
            Assert.IsTrue(genTask.Wait(MediumNonDbgTimeout));
            Assert.IsTrue(verTask.Wait(MediumNonDbgTimeout));
        }
    }

    [TestMethod]
    public void AwaitNoDeadlock()
    {
        for (var i = 0; i < 100; i++)
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();

            var t1 = tcs1.Task.EnforceAwaitOrder();
            var t2 = tcs2.Task.EnforceAwaitOrder();

            async Task Wait1()
            {
                await t1;
                await t2;
            }

            async Task Wait2()
            {
                await t2;
                await t1;
            }

            var w1 = Wait1();
            var w2 = Wait2();

            Task.Run(() => tcs1.SetResult(0));
            Task.Run(() => tcs2.SetResult(0));

            Assert.IsTrue(Task.WaitAll(new[] { w1, w2 }, MediumNonDbgTimeout));
        }
    }

    [TestMethod]
    public void DisposedProxy()
    {
        var b = new BareProxy();
        Assert.Throws<ArgumentNullException>(() => b.Bind(null));
        var impl = new TestInterfaceImpl2();
        var proxy = Proxy.Share<ITestInterface>(impl);
        var p = (Proxy)proxy;
        Assert.Throws<InvalidOperationException>(() => p.Bind(p.ConsumedCap));
        Assert.IsFalse(p.IsDisposed);
        proxy.Dispose();
        Assert.IsTrue(p.IsDisposed);
        Assert.Throws<ObjectDisposedException>(() => p.ConsumedCap);
        var t = proxy.Foo(123, true);
        Assert.IsTrue(Assert.ThrowsAsync<ObjectDisposedException>(() => t).Wait(MediumNonDbgTimeout));
    }
}