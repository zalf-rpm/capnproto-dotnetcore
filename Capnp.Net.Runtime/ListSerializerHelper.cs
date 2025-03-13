using System;

namespace Capnp;

internal static class ListSerializerHelper
{
    public static void EnsureAllocated(SerializerState serializer)
    {
        if (!serializer.IsAllocated)
            throw new InvalidOperationException("Call Init() first");
    }
}