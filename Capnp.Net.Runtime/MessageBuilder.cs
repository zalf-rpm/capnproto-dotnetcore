using System;
using System.Collections.Generic;
using Capnp.Rpc;

namespace Capnp;

/// <summary>
///     Entry point for building Cap'n Proto messages.
/// </summary>
public class MessageBuilder
{
    private readonly DynamicSerializerState _rootPtrBuilder;

    private MessageBuilder(ISegmentAllocator allocator)
    {
        Allocator = allocator;
        _rootPtrBuilder = new DynamicSerializerState(this);
        _rootPtrBuilder.SetStruct(0, 1);
        _rootPtrBuilder.Allocate();
    }

    /// <summary>
    ///     Gets or sets the root object. The root object must be set exactly once per message.
    ///     Setting it manually is only required (and allowed) when it was created with <see cref="CreateObject{TS}" />.
    /// </summary>
    /// <exception cref="ArgumentNullException">Attempt to set null reference</exception>
    public SerializerState? Root
    {
        get => _rootPtrBuilder.TryGetPointer(0);
        set => _rootPtrBuilder.Link(0, value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    ///     Returns the wire representation of the built message.
    /// </summary>
    public WireFrame Frame => new(Allocator.Segments);

    /// <summary>
    ///     Returns this message builder's segment allocator.
    /// </summary>
    public ISegmentAllocator Allocator { get; }

    internal List<ConsumedCapability>? Caps { get; private set; }

    /// <summary>
    ///     Constructs an instance using a custom segment allocator and reserves space for the root pointer.
    /// </summary>
    /// <typeparam name="T">Segment allocator implementation type</typeparam>
    public static MessageBuilder Create<T>()
        where T : ISegmentAllocator, new()
    {
        return new MessageBuilder(new T());
    }

    /// <summary>
    ///     Constructs an instance using the default segment allocator and reserves space for the root pointer.
    /// </summary>
    /// <param name="defaultSegmentSize">Default segment size, <see cref="SegmentAllocator" /></param>
    public static MessageBuilder Create(int defaultSegmentSize = 128)
    {
        return new MessageBuilder(new SegmentAllocator(defaultSegmentSize));
    }

    /// <summary>
    ///     Creates a new object inside the message.
    /// </summary>
    /// <typeparam name="TS">Serializer state specialization</typeparam>
    /// <returns>Serializer state instance representing the new object</returns>
    public TS CreateObject<TS>()
        where TS : SerializerState, new()
    {
        var ts = new TS();
        ts.Bind(this);
        return ts;
    }

    /// <summary>
    ///     Creates an object and sets it as root object.
    /// </summary>
    /// <typeparam name="TS">Serializer state specialization (must be a struct)</typeparam>
    /// <returns>Serializer state instance representing the new object</returns>
    public TS BuildRoot<TS>()
        where TS : SerializerState, new()
    {
        if (Root != null)
            throw new InvalidOperationException("Root already set");

        var root = CreateObject<TS>();
        if (root.Kind != ObjectKind.Struct)
            throw new InvalidOperationException("Root object must be a struct");

        Root = root;
        return root;
    }

    /// <summary>
    ///     Initializes the capability table for using the message builder in RPC context.
    /// </summary>
    public void InitCapTable()
    {
        if (Caps != null)
            throw new InvalidOperationException("Capability table was already initialized");

        Caps = new List<ConsumedCapability>();
    }
}
