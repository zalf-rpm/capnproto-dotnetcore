using System;
using System.Threading;
using System.Threading.Tasks;
using Capnp;
using Capnp.Rpc;

namespace CapnpGen;

[TypeId(0xb09c1336dc3ea94eUL)]
[Proxy(typeof(Issue25A_Proxy))]
[Skeleton(typeof(Issue25A_Skeleton))]
public interface IIssue25A : IDisposable
{
    Task<long> MethodA(CancellationToken cancellationToken_ = default);
}

public class Issue25A_Proxy : Proxy, IIssue25A
{
    public async Task<long> MethodA(CancellationToken cancellationToken_ = default)
    {
        var in_ = SerializerState.CreateForRpc<Issue25A.Params_methodA.WRITER>();
        var arg_ = new Issue25A.Params_methodA();
        arg_.serialize(in_);
        var d_ = await Call(
            12726067773386434894UL,
            0,
            in_.Rewrap<DynamicSerializerState>(),
            false,
            cancellationToken_
        ).WhenReturned;
        var r_ = CapnpSerializable.Create<Issue25A.Result_methodA>(d_);
        return r_.Result;
    }
}

public class Issue25A_Skeleton : Skeleton<IIssue25A>
{
    public Issue25A_Skeleton()
    {
        SetMethodTable(MethodA);
    }

    public override ulong InterfaceId => 12726067773386434894UL;

    private Task<AnswerOrCounterquestion> MethodA(
        DeserializerState d_,
        CancellationToken cancellationToken_
    )
    {
        return Impatient.MaybeTailCall(
            Impl.MethodA(cancellationToken_),
            result =>
            {
                var s_ = SerializerState.CreateForRpc<Issue25A.Result_methodA.WRITER>();
                var r_ = new Issue25A.Result_methodA { Result = result };
                r_.serialize(s_);
                return s_;
            }
        );
    }
}

public static class Issue25A
{
    [TypeId(0x994fd8d48f77e57cUL)]
    public class Params_methodA : ICapnpSerializable
    {
        public const ulong typeId = 0x994fd8d48f77e57cUL;

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer) { }

        public void applyDefaults() { }

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
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 0);
            }
        }
    }

    [TypeId(0xf504d63e26c4db14UL)]
    public class Result_methodA : ICapnpSerializable
    {
        public const ulong typeId = 0xf504d63e26c4db14UL;

        public long Result { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Result = reader.Result;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Result = Result;
        }

        public void applyDefaults() { }

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

            public long Result => ctx.ReadDataLong(0UL);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(1, 0);
            }

            public long Result
            {
                get => this.ReadDataLong(0UL);
                set => this.WriteData(0UL, value);
            }
        }
    }
}

[TypeId(0x9a5c2572c0400979UL)]
[Proxy(typeof(CapHolder_Proxy<>))]
[Skeleton(typeof(CapHolder_Skeleton<>))]
public interface ICapHolder<TCapType> : IDisposable
    where TCapType : class
{
    Task<TCapType> Cap(CancellationToken cancellationToken_ = default);
}

public class CapHolder_Proxy<TCapType> : Proxy, ICapHolder<TCapType>
    where TCapType : class
{
    public Task<TCapType> Cap(CancellationToken cancellationToken_ = default)
    {
        var in_ = SerializerState.CreateForRpc<CapHolder<TCapType>.Params_cap.WRITER>();
        var arg_ = new CapHolder<TCapType>.Params_cap();
        arg_.serialize(in_);
        return Impatient.MakePipelineAware(
            Call(
                11122806354480204153UL,
                0,
                in_.Rewrap<DynamicSerializerState>(),
                false,
                cancellationToken_
            ),
            d_ =>
            {
                var r_ = CapnpSerializable.Create<CapHolder<TCapType>.Result_cap>(d_);
                return r_.Cap;
            }
        );
    }
}

public class CapHolder_Skeleton<TCapType> : Skeleton<ICapHolder<TCapType>>
    where TCapType : class
{
    public CapHolder_Skeleton()
    {
        SetMethodTable(Cap);
    }

    public override ulong InterfaceId => 11122806354480204153UL;

    private Task<AnswerOrCounterquestion> Cap(
        DeserializerState d_,
        CancellationToken cancellationToken_
    )
    {
        return Impatient.MaybeTailCall(
            Impl.Cap(cancellationToken_),
            cap =>
            {
                var s_ = SerializerState.CreateForRpc<CapHolder<TCapType>.Result_cap.WRITER>();
                var r_ = new CapHolder<TCapType>.Result_cap { Cap = cap };
                r_.serialize(s_);
                return s_;
            }
        );
    }
}

public static class CapHolder<TCapType>
    where TCapType : class
{
    [TypeId(0x9fb50287386a4515UL)]
    public class Params_cap : ICapnpSerializable
    {
        public const ulong typeId = 0x9fb50287386a4515UL;

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer) { }

        public void applyDefaults() { }

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
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 0);
            }
        }
    }

    [TypeId(0x9b3a7a5298aae998UL)]
    public class Result_cap : ICapnpSerializable
    {
        public const ulong typeId = 0x9b3a7a5298aae998UL;

        public TCapType Cap { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Cap = CapnpSerializable.Create<TCapType>(reader.Cap);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Cap.SetObject(Cap);
        }

        public void applyDefaults() { }

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

            public DeserializerState Cap => ctx.StructReadPointer(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public DynamicSerializerState Cap
            {
                get => BuildPointer<DynamicSerializerState>(0);
                set => Link(0, value);
            }
        }
    }
}

[TypeId(0xc210c6a49d79653cUL)]
[Proxy(typeof(CapHolderA_Proxy))]
[Skeleton(typeof(CapHolderA_Skeleton))]
public interface ICapHolderA : IDisposable
{
    Task<IIssue25A> Cap(CancellationToken cancellationToken_ = default);
}

public class CapHolderA_Proxy : Proxy, ICapHolderA
{
    public Task<IIssue25A> Cap(CancellationToken cancellationToken_ = default)
    {
        var in_ = SerializerState.CreateForRpc<CapHolderA.Params_cap.WRITER>();
        var arg_ = new CapHolderA.Params_cap();
        arg_.serialize(in_);
        return Impatient.MakePipelineAware(
            Call(
                13983895253304304956UL,
                0,
                in_.Rewrap<DynamicSerializerState>(),
                false,
                cancellationToken_
            ),
            d_ =>
            {
                var r_ = CapnpSerializable.Create<CapHolderA.Result_cap>(d_);
                return r_.Cap;
            }
        );
    }
}

public class CapHolderA_Skeleton : Skeleton<ICapHolderA>
{
    public CapHolderA_Skeleton()
    {
        SetMethodTable(Cap);
    }

    public override ulong InterfaceId => 13983895253304304956UL;

    private Task<AnswerOrCounterquestion> Cap(
        DeserializerState d_,
        CancellationToken cancellationToken_
    )
    {
        return Impatient.MaybeTailCall(
            Impl.Cap(cancellationToken_),
            cap =>
            {
                var s_ = SerializerState.CreateForRpc<CapHolderA.Result_cap.WRITER>();
                var r_ = new CapHolderA.Result_cap { Cap = cap };
                r_.serialize(s_);
                return s_;
            }
        );
    }
}

public static class CapHolderA
{
    [TypeId(0xfa15f780f3b1df22UL)]
    public class Params_cap : ICapnpSerializable
    {
        public const ulong typeId = 0xfa15f780f3b1df22UL;

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer) { }

        public void applyDefaults() { }

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
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 0);
            }
        }
    }

    [TypeId(0xc2166ff01687f001UL)]
    public class Result_cap : ICapnpSerializable
    {
        public const ulong typeId = 0xc2166ff01687f001UL;

        public IIssue25A Cap { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Cap = reader.Cap;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Cap = Cap;
        }

        public void applyDefaults() { }

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

            public IIssue25A Cap => ctx.ReadCap<IIssue25A>(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public IIssue25A Cap
            {
                get => ReadCap<IIssue25A>(0);
                set => LinkObject(0, value);
            }
        }
    }
}

[TypeId(0xf567e74a1a5cbd2dUL)]
[Proxy(typeof(Issue25B_Proxy))]
[Skeleton(typeof(Issue25B_Skeleton))]
public interface IIssue25B : IDisposable
{
    Task<ICapHolder<object>> GetAinCapHolderAnyPointer(
        CancellationToken cancellationToken_ = default
    );
    Task<ICapHolder<IIssue25A>> GetAinCapHolderGenericA(
        CancellationToken cancellationToken_ = default
    );
    Task<ICapHolderA> GetAinCapHolderNonGenericA(CancellationToken cancellationToken_ = default);
}

public class Issue25B_Proxy : Proxy, IIssue25B
{
    public Task<ICapHolder<object>> GetAinCapHolderAnyPointer(
        CancellationToken cancellationToken_ = default
    )
    {
        var in_ = SerializerState.CreateForRpc<Issue25B.Params_getAinCapHolderAnyPointer.WRITER>();
        var arg_ = new Issue25B.Params_getAinCapHolderAnyPointer();
        arg_.serialize(in_);
        return Impatient.MakePipelineAware(
            Call(
                17683356767349423405UL,
                0,
                in_.Rewrap<DynamicSerializerState>(),
                false,
                cancellationToken_
            ),
            d_ =>
            {
                var r_ = CapnpSerializable.Create<Issue25B.Result_getAinCapHolderAnyPointer>(d_);
                return r_.AInCapHolder;
            }
        );
    }

    public Task<ICapHolder<IIssue25A>> GetAinCapHolderGenericA(
        CancellationToken cancellationToken_ = default
    )
    {
        var in_ = SerializerState.CreateForRpc<Issue25B.Params_getAinCapHolderGenericA.WRITER>();
        var arg_ = new Issue25B.Params_getAinCapHolderGenericA();
        arg_.serialize(in_);
        return Impatient.MakePipelineAware(
            Call(
                17683356767349423405UL,
                1,
                in_.Rewrap<DynamicSerializerState>(),
                false,
                cancellationToken_
            ),
            d_ =>
            {
                var r_ = CapnpSerializable.Create<Issue25B.Result_getAinCapHolderGenericA>(d_);
                return r_.AInCapHolder;
            }
        );
    }

    public Task<ICapHolderA> GetAinCapHolderNonGenericA(
        CancellationToken cancellationToken_ = default
    )
    {
        var in_ = SerializerState.CreateForRpc<Issue25B.Params_getAinCapHolderNonGenericA.WRITER>();
        var arg_ = new Issue25B.Params_getAinCapHolderNonGenericA();
        arg_.serialize(in_);
        return Impatient.MakePipelineAware(
            Call(
                17683356767349423405UL,
                2,
                in_.Rewrap<DynamicSerializerState>(),
                false,
                cancellationToken_
            ),
            d_ =>
            {
                var r_ = CapnpSerializable.Create<Issue25B.Result_getAinCapHolderNonGenericA>(d_);
                return r_.AInCapHolder;
            }
        );
    }
}

public class Issue25B_Skeleton : Skeleton<IIssue25B>
{
    public Issue25B_Skeleton()
    {
        SetMethodTable(
            GetAinCapHolderAnyPointer,
            GetAinCapHolderGenericA,
            GetAinCapHolderNonGenericA
        );
    }

    public override ulong InterfaceId => 17683356767349423405UL;

    private Task<AnswerOrCounterquestion> GetAinCapHolderAnyPointer(
        DeserializerState d_,
        CancellationToken cancellationToken_
    )
    {
        return Impatient.MaybeTailCall(
            Impl.GetAinCapHolderAnyPointer(cancellationToken_),
            aInCapHolder =>
            {
                var s_ =
                    SerializerState.CreateForRpc<Issue25B.Result_getAinCapHolderAnyPointer.WRITER>();
                var r_ = new Issue25B.Result_getAinCapHolderAnyPointer
                {
                    AInCapHolder = aInCapHolder,
                };
                r_.serialize(s_);
                return s_;
            }
        );
    }

    private Task<AnswerOrCounterquestion> GetAinCapHolderGenericA(
        DeserializerState d_,
        CancellationToken cancellationToken_
    )
    {
        return Impatient.MaybeTailCall(
            Impl.GetAinCapHolderGenericA(cancellationToken_),
            aInCapHolder =>
            {
                var s_ =
                    SerializerState.CreateForRpc<Issue25B.Result_getAinCapHolderGenericA.WRITER>();
                var r_ = new Issue25B.Result_getAinCapHolderGenericA
                {
                    AInCapHolder = aInCapHolder,
                };
                r_.serialize(s_);
                return s_;
            }
        );
    }

    private Task<AnswerOrCounterquestion> GetAinCapHolderNonGenericA(
        DeserializerState d_,
        CancellationToken cancellationToken_
    )
    {
        return Impatient.MaybeTailCall(
            Impl.GetAinCapHolderNonGenericA(cancellationToken_),
            aInCapHolder =>
            {
                var s_ =
                    SerializerState.CreateForRpc<Issue25B.Result_getAinCapHolderNonGenericA.WRITER>();
                var r_ = new Issue25B.Result_getAinCapHolderNonGenericA
                {
                    AInCapHolder = aInCapHolder,
                };
                r_.serialize(s_);
                return s_;
            }
        );
    }
}

public static class Issue25B
{
    [TypeId(0xe8d098fdc4452255UL)]
    public class Params_getAinCapHolderAnyPointer : ICapnpSerializable
    {
        public const ulong typeId = 0xe8d098fdc4452255UL;

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer) { }

        public void applyDefaults() { }

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
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 0);
            }
        }
    }

    [TypeId(0xf3e141366136b72aUL)]
    public class Result_getAinCapHolderAnyPointer : ICapnpSerializable
    {
        public const ulong typeId = 0xf3e141366136b72aUL;

        public ICapHolder<object> AInCapHolder { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            AInCapHolder = reader.AInCapHolder;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.AInCapHolder = AInCapHolder;
        }

        public void applyDefaults() { }

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

            public ICapHolder<object> AInCapHolder => ctx.ReadCap<ICapHolder<object>>(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public ICapHolder<object> AInCapHolder
            {
                get => ReadCap<ICapHolder<object>>(0);
                set => LinkObject(0, value);
            }
        }
    }

    [TypeId(0x97b7893bbafbac9cUL)]
    public class Params_getAinCapHolderGenericA : ICapnpSerializable
    {
        public const ulong typeId = 0x97b7893bbafbac9cUL;

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer) { }

        public void applyDefaults() { }

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
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 0);
            }
        }
    }

    [TypeId(0xca0d4d05609cf941UL)]
    public class Result_getAinCapHolderGenericA : ICapnpSerializable
    {
        public const ulong typeId = 0xca0d4d05609cf941UL;

        public ICapHolder<IIssue25A> AInCapHolder { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            AInCapHolder = reader.AInCapHolder;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.AInCapHolder = AInCapHolder;
        }

        public void applyDefaults() { }

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

            public ICapHolder<IIssue25A> AInCapHolder => ctx.ReadCap<ICapHolder<IIssue25A>>(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public ICapHolder<IIssue25A> AInCapHolder
            {
                get => ReadCap<ICapHolder<IIssue25A>>(0);
                set => LinkObject(0, value);
            }
        }
    }

    [TypeId(0xda5cc8f4689eb99fUL)]
    public class Params_getAinCapHolderNonGenericA : ICapnpSerializable
    {
        public const ulong typeId = 0xda5cc8f4689eb99fUL;

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer) { }

        public void applyDefaults() { }

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
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 0);
            }
        }
    }

    [TypeId(0x81a8e7a60710b811UL)]
    public class Result_getAinCapHolderNonGenericA : ICapnpSerializable
    {
        public const ulong typeId = 0x81a8e7a60710b811UL;

        public ICapHolderA AInCapHolder { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            AInCapHolder = reader.AInCapHolder;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.AInCapHolder = AInCapHolder;
        }

        public void applyDefaults() { }

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

            public ICapHolderA AInCapHolder => ctx.ReadCap<ICapHolderA>(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public ICapHolderA AInCapHolder
            {
                get => ReadCap<ICapHolderA>(0);
                set => LinkObject(0, value);
            }
        }
    }
}
