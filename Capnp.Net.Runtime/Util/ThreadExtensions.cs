using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Capnp.Util;

internal static class ThreadExtensions
{
    private static readonly Lazy<ThreadExtensionsLoggingContext> LoggingContext = new(
        () => new ThreadExtensionsLoggingContext(),
        LazyThreadSafetyMode.PublicationOnly
    );

    public static void SafeJoin(this Thread thread, ILogger? logger = null, int timeout = 5000)
    {
        if (!thread.Join(timeout))
        {
            logger ??= LoggingContext.Value.Logger;

            var name = thread.Name ?? thread.ManagedThreadId.ToString();

            try
            {
                logger.LogError(
                    $"Unable to join thread {name}. Thread is in state {thread.ThreadState}."
                );
                thread.Interrupt();
                if (!thread.Join(timeout))
                    logger.LogError(
                        $"Still unable to join thread {name} after Interrupt(). Thread is in state {thread.ThreadState}."
                    );
            }
            catch { }
        }
    }

    private class ThreadExtensionsLoggingContext
    {
        public ILogger Logger { get; } = Logging.CreateLogger<ThreadExtensionsLoggingContext>();
    }
}
