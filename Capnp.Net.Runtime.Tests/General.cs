using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capnp.Rpc;
using Capnp.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
public class General : TestBase
{
    [TestMethod]
    public void AwaitOrderTest()
    {
        // This test verifies an execution order assumption about the .NET environment:
        // When I register multiple continuations on the same Task, using the await
        // keyword, I expect all continuations be executed in the same order they were
        // registered. Despite I could not find any official statement on this behavior,
        // the Capnp.Net.Runtime implementation relies on that assumption. Should that
        // assumption turn out to be wrong, you might observe RPCs which are executed in
        // a different order than they were requested.

        var returnCounter = 0;

        async Task ExpectCount(Task task, int count)
        {
            await task;
            Assert.AreEqual(count, returnCounter++);
        }

        var tcs = new TaskCompletionSource<int>();

        var tasks = from i in Enumerable.Range(0, 100) select ExpectCount(tcs.Task, i);

        tcs.SetResult(0);

        Task.WhenAll(tasks).Wait();
    }

    [TestMethod]
    public void MakePipelineAwareOnFastPath()
    {
        var mock = new PromisedAnswerMock();
        mock.Return();
        for (var i = 0; i < 100; i++)
        {
            var t = Impatient.MakePipelineAware(mock, _ => (object)null);
            Assert.IsTrue(t.IsCompleted);
        }
        ;
    }

    [TestMethod]
    [TestCategory("Coverage")]
    public void SafeJoinCompletedThread()
    {
        var thread = new Thread(() => { });
        thread.Start();
        thread.SafeJoin(null, 200);
    }

    [TestMethod]
    //[TestCategory("Coverage")]
    public void SafeJoinBusyThread()
    {
        var thread = new Thread(() =>
        {
            try
            {
                while (true)
                    ;
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Interrupted");
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Aborted");
            }
        });
        thread.IsBackground = true;
        thread.Start();
        thread.SafeJoin(null, 5);
    }

    [TestMethod]
    [TestCategory("Coverage")]
    public void SafeJoinSleepingThread()
    {
        var thread = new Thread(() =>
        {
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Interrupted");
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Aborted");
            }
        });

        thread.Start();
        thread.SafeJoin(null, 5);
    }

    [TestMethod]
    [TestCategory("Coverage")]
    public void SafeJoinDeadlockedThread()
    {
        var lk = new object();

        lock (lk)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    lock (lk) { }
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Interrupted");
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Aborted");
                }
            });

            thread.Start();
            thread.SafeJoin(null, 5);
        }
    }

    [TestMethod]
    //[TestCategory("Coverage")]
    public void SafeJoinDefensiveThread()
    {
        var thread = new Thread(() =>
        {
            for (; ; )
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Interrupted");
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Aborted");
                }
        });
        thread.IsBackground = true;
        thread.Start();
        thread.SafeJoin(null, 5);
    }

    private class PromisedAnswerMock : IPromisedAnswer
    {
        private readonly TaskCompletionSource<DeserializerState> _tcs = new();

        public PromisedAnswerMock()
        {
            WhenReturned = _tcs.Task.EnforceAwaitOrder();
        }

        public StrictlyOrderedAwaitTask<DeserializerState> WhenReturned { get; }

        public ConsumedCapability Access(MemberAccessPath access)
        {
            throw new NotImplementedException();
        }

        public ConsumedCapability Access(MemberAccessPath access, Task<IDisposable> proxyTask)
        {
            throw new NotImplementedException();
        }

        public bool IsTailCall => false;

        public void Dispose() { }

        public void Return()
        {
            _tcs.SetResult(default);
        }

        public void Cancel()
        {
            _tcs.SetCanceled();
        }

        public void Fault()
        {
            _tcs.SetException(new InvalidOperationException("test fault"));
        }
    }
}
