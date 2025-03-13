using System;
using Microsoft.Build.Utilities;

namespace CapnpC.CSharp.MsBuild.Generation;

public static class LogExtensions
{
    public static void LogWithNameTag(
        this TaskLoggingHelper loggingHelper,
        Action<string, object[]> loggingMethod,
        string message,
        params object[] messageArgs)
    {
        var fullMessage = $"[Cap'n Proto] {message}";
        loggingMethod?.Invoke(fullMessage, messageArgs);
    }
}