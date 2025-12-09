using Capnp;

namespace CapnpGen;

[TypeId(0xb706e295e5860f3dUL)]
public class RpcRequest<TRequest> : ICapnpSerializable
    where TRequest : class
{
    public const ulong typeId = 0xb706e295e5860f3dUL;

    public string Method { get; set; }

    public TRequest Request { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Method = reader.Method;
        Request = CapnpSerializable.Create<TRequest>(reader.Request);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Method = Method;
        writer.Request.SetObject(Request);
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

        public string Method => ctx.ReadText(0, "");
        public DeserializerState Request => ctx.StructReadPointer(1);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(0, 2);
        }

        public string Method
        {
            get => ReadText(0, "");
            set => WriteText(0, value, "");
        }

        public DynamicSerializerState Request
        {
            get => BuildPointer<DynamicSerializerState>(1);
            set => Link(1, value);
        }
    }
}

[TypeId(0xca749dac8d513c9fUL)]
public class ArithmeticOperationRequest : ICapnpSerializable
{
    public const ulong typeId = 0xca749dac8d513c9fUL;

    public int NumA { get; set; }

    public int NumB { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        NumA = reader.NumA;
        NumB = reader.NumB;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.NumA = NumA;
        writer.NumB = NumB;
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

        public int NumA => ctx.ReadDataInt(0UL);
        public int NumB => ctx.ReadDataInt(32UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 0);
        }

        public int NumA
        {
            get => this.ReadDataInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public int NumB
        {
            get => this.ReadDataInt(32UL);
            set => this.WriteData(32UL, value);
        }
    }
}

[TypeId(0xc64f52df07418506UL)]
public class ArithmeticOperationReply : ICapnpSerializable
{
    public const ulong typeId = 0xc64f52df07418506UL;

    public int Result { get; set; }

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

        public int Result => ctx.ReadDataInt(0UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 0);
        }

        public int Result
        {
            get => this.ReadDataInt(0UL);
            set => this.WriteData(0UL, value);
        }
    }
}
