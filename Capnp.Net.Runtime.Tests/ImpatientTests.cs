using System;
using System.Threading.Tasks;
using Capnp.Net.Runtime.Tests.GenImpls;
using Capnp.Rpc;
using Capnp.Util;
using Capnproto_test.Capnp.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
[TestCategory("Coverage")]
public class ImpatientTests
{
    [TestMethod]
    public async Task Unwrap()
    {
        var impl = new TestInterfaceImpl2();
        Assert.AreEqual(impl, await impl.Unwrap<ITestInterface>());
        using (var proxy = Proxy.Share<ITestInterface>(impl))
        using (var reso = ((Proxy)proxy).GetResolvedCapability<ITestInterface>())
        {
            Assert.AreEqual(((Proxy)proxy).ConsumedCap, ((Proxy)reso).ConsumedCap);
        }

        Assert.IsNull(await default(ITestInterface).Unwrap());
        var tcs = new TaskCompletionSource<ITestInterface>();
        tcs.SetResult(null);
        Assert.IsNull(await tcs.Task.Eager(true).Unwrap());
        var excepted = Task.FromException<ITestInterface>(
            new InvalidTimeZoneException("So annoying")
        );
        await Assert.ThrowsAsync<RpcException>(async () => await excepted.Eager(true).Unwrap());
    }

    [TestMethod]
    public async Task MaybeTailCall3()
    {
        var flag = false;

        SerializerState Fn(int a, int b, int c)
        {
            Assert.AreEqual(0, a);
            Assert.AreEqual(1, b);
            Assert.AreEqual(2, c);
            flag = true;
            return null;
        }

        var t = Task.FromResult((0, 1, 2));
        await Impatient.MaybeTailCall(t, Fn);
        Assert.IsTrue(flag);
    }

    [TestMethod]
    public async Task MaybeTailCall4()
    {
        var flag = false;

        SerializerState Fn(int a, int b, int c, int d)
        {
            Assert.AreEqual(0, a);
            Assert.AreEqual(1, b);
            Assert.AreEqual(2, c);
            Assert.AreEqual(3, d);
            flag = true;
            return null;
        }

        var t = Task.FromResult((0, 1, 2, 3));
        await Impatient.MaybeTailCall(t, Fn);
        Assert.IsTrue(flag);
    }

    [TestMethod]
    public async Task MaybeTailCall5()
    {
        var flag = false;

        SerializerState Fn(int a, int b, int c, int d, int e)
        {
            Assert.AreEqual(0, a);
            Assert.AreEqual(1, b);
            Assert.AreEqual(2, c);
            Assert.AreEqual(3, d);
            Assert.AreEqual(4, e);
            flag = true;
            return null;
        }

        var t = Task.FromResult((0, 1, 2, 3, 4));
        await Impatient.MaybeTailCall(t, Fn);
        Assert.IsTrue(flag);
    }

    [TestMethod]
    public async Task MaybeTailCall6()
    {
        var flag = false;

        SerializerState Fn(int a, int b, int c, int d, int e, int f)
        {
            Assert.AreEqual(0, a);
            Assert.AreEqual(1, b);
            Assert.AreEqual(2, c);
            Assert.AreEqual(3, d);
            Assert.AreEqual(4, e);
            Assert.AreEqual(5, f);
            flag = true;
            return null;
        }

        var t = Task.FromResult((0, 1, 2, 3, 4, 5));
        await Impatient.MaybeTailCall(t, Fn);
        Assert.IsTrue(flag);
    }

    [TestMethod]
    public async Task MaybeTailCall7()
    {
        var flag = false;

        SerializerState Fn(int a, int b, int c, int d, int e, int f, int g)
        {
            Assert.AreEqual(0, a);
            Assert.AreEqual(1, b);
            Assert.AreEqual(2, c);
            Assert.AreEqual(3, d);
            Assert.AreEqual(4, e);
            Assert.AreEqual(5, f);
            Assert.AreEqual(6, g);
            flag = true;
            return null;
        }

        var t = Task.FromResult((0, 1, 2, 3, 4, 5, 6));
        await Impatient.MaybeTailCall(t, Fn);
        Assert.IsTrue(flag);
    }

    [TestMethod]
    public void ObsoleteGetAnswer()
    {
#pragma warning disable CS0618
        var answer = new PromisedAnswerMock();
        Assert.Throws<ArgumentException>(() => Impatient.GetAnswer(Task.FromResult(new object())));
        var t = Impatient.MakePipelineAware(answer, _ => _);
        Assert.AreEqual(answer, Impatient.GetAnswer(t));
#pragma warning restore CS0618
    }

    [TestMethod]
    public async Task Access()
    {
        var answer = new PromisedAnswerMock();

        async Task AwaitReturn()
        {
            await answer.WhenReturned;
        }

        var cap = Impatient.Access(
            AwaitReturn(),
            new MemberAccessPath(),
            Task.FromResult<IDisposable>(new TestInterfaceImpl2())
        );
        using (var proxy = new BareProxy(cap))
        {
            await proxy.WhenResolved;
        }
    }

    [TestMethod]
    public void ObsoletePseudoEager()
    {
#pragma warning disable CS0618
        var task = Task.FromResult<ITestInterface>(new TestInterfaceImpl2());
        Assert.IsTrue(task.PseudoEager() is Proxy proxy && proxy.WhenResolved.IsCompleted);
#pragma warning restore CS0618
    }

    [TestMethod]
    public void Eager()
    {
        var task = Task.FromResult<ITestInterface>(new TestInterfaceImpl2());
        Assert.Throws<ArgumentException>(() => task.Eager());
        Assert.Throws<ArgumentException>(() => task.Eager());
    }

    private class PromisedAnswerMock : IPromisedAnswer
    {
        private readonly TaskCompletionSource<DeserializerState> _tcs = new();

        public PromisedAnswerMock()
        {
            WhenReturned = _tcs.Task.EnforceAwaitOrder();
        }

        public StrictlyOrderedAwaitTask<DeserializerState> WhenReturned { get; }

        public bool IsTailCall => false;

        public ConsumedCapability Access(MemberAccessPath access)
        {
            throw new NotImplementedException();
        }

        public ConsumedCapability Access(MemberAccessPath access, Task<IDisposable> proxyTask)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }
    }
}
