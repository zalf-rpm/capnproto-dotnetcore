using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Capnp;
using Capnp.Rpc;

namespace CapnpGen;

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x86e148a317d43104UL)]
[Proxy(typeof(Echoer_Proxy))]
[Skeleton(typeof(Echoer_Skeleton))]
public interface IEchoer : IDisposable
{
    Task<IReadOnlyList<byte>> Echo(IReadOnlyList<byte> input, CancellationToken cancellationToken_ = default);
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x86e148a317d43104UL)]
public class Echoer_Proxy : Proxy, IEchoer
{
    public async Task<IReadOnlyList<byte>> Echo(IReadOnlyList<byte> input,
        CancellationToken cancellationToken_ = default)
    {
        var in_ = SerializerState.CreateForRpc<Echoer.Params_Echo.WRITER>();
        var arg_ = new Echoer.Params_Echo
            { Input = input };
        arg_?.serialize(in_);
        using (var d_ = await Call(9719129336158892292UL, 0, in_.Rewrap<DynamicSerializerState>(), false,
                   cancellationToken_).WhenReturned)
        {
            var r_ = CapnpSerializable.Create<Echoer.Result_Echo>(d_);
            return r_.Output;
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x86e148a317d43104UL)]
public class Echoer_Skeleton : Skeleton<IEchoer>
{
    public Echoer_Skeleton()
    {
        SetMethodTable(Echo);
    }

    public override ulong InterfaceId => 9719129336158892292UL;

    private Task<AnswerOrCounterquestion> Echo(DeserializerState d_, CancellationToken cancellationToken_)
    {
        using (d_)
        {
            var in_ = CapnpSerializable.Create<Echoer.Params_Echo>(d_);
            return Impatient.MaybeTailCall(Impl.Echo(in_.Input, cancellationToken_), output =>
                {
                    var s_ = SerializerState.CreateForRpc<Echoer.Result_Echo.WRITER>();
                    var r_ = new Echoer.Result_Echo { Output = output };
                    r_.serialize(s_);
                    return s_;
                }
            );
        }
    }
}

public static class Echoer
{
    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xef00a5b37fcc27d8UL)]
    public class Params_Echo : ICapnpSerializable
    {
        public const ulong typeId = 0xef00a5b37fcc27d8UL;

        public IReadOnlyList<byte> Input { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Input = reader.Input;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Input.Init(Input);
        }

        public void applyDefaults()
        {
        }

        public struct READER
        {
            private readonly DeserializerState ctx;

            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx)
            {
                return new READER(ctx);
            }

            public static implicit operator DeserializerState(READER reader)
            {
                return reader.ctx;
            }

            public static implicit operator READER(DeserializerState ctx)
            {
                return new READER(ctx);
            }

            public IReadOnlyList<byte> Input => ctx.ReadList(0).CastByte();
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public ListOfPrimitivesSerializer<byte> Input
            {
                get => BuildPointer<ListOfPrimitivesSerializer<byte>>(0);
                set => Link(0, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0x8e43ad8401318809UL)]
    public class Result_Echo : ICapnpSerializable
    {
        public const ulong typeId = 0x8e43ad8401318809UL;

        public IReadOnlyList<byte> Output { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Output = reader.Output;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Output.Init(Output);
        }

        public void applyDefaults()
        {
        }

        public struct READER
        {
            private readonly DeserializerState ctx;

            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx)
            {
                return new READER(ctx);
            }

            public static implicit operator DeserializerState(READER reader)
            {
                return reader.ctx;
            }

            public static implicit operator READER(DeserializerState ctx)
            {
                return new READER(ctx);
            }

            public IReadOnlyList<byte> Output => ctx.ReadList(0).CastByte();
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public ListOfPrimitivesSerializer<byte> Output
            {
                get => BuildPointer<ListOfPrimitivesSerializer<byte>>(0);
                set => Link(0, value);
            }
        }
    }
}