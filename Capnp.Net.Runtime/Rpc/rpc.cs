#pragma warning disable CS1591
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Capnp.Rpc;

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x91b79f1f808db032UL)]
public class Message : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        Unimplemented = 0,
        Abort = 1,
        Call = 2,
        Return = 3,
        Finish = 4,
        Resolve = 5,
        Release = 6,
        ObsoleteSave = 7,
        Bootstrap = 8,
        ObsoleteDelete = 9,
        Provide = 10,
        Accept = 11,
        Join = 12,
        Disembargo = 13,
        undefined = 65535
    }

    public const ulong typeId = 0x91b79f1f808db032UL;
    private object? _content;

    private WHICH _which = WHICH.undefined;

    public WHICH which
    {
        get => _which;
        set
        {
            if (value == _which)
                return;
            _which = value;
            switch (value)
            {
                case WHICH.Unimplemented:
                    _content = null;
                    break;
                case WHICH.Abort:
                    _content = null;
                    break;
                case WHICH.Call:
                    _content = null;
                    break;
                case WHICH.Return:
                    _content = null;
                    break;
                case WHICH.Finish:
                    _content = null;
                    break;
                case WHICH.Resolve:
                    _content = null;
                    break;
                case WHICH.Release:
                    _content = null;
                    break;
                case WHICH.ObsoleteSave:
                    _content = null;
                    break;
                case WHICH.Bootstrap:
                    _content = null;
                    break;
                case WHICH.ObsoleteDelete:
                    _content = null;
                    break;
                case WHICH.Provide:
                    _content = null;
                    break;
                case WHICH.Accept:
                    _content = null;
                    break;
                case WHICH.Join:
                    _content = null;
                    break;
                case WHICH.Disembargo:
                    _content = null;
                    break;
            }
        }
    }

    public Message? Unimplemented
    {
        get => _which == WHICH.Unimplemented ? (Message?)_content : null;
        set
        {
            _which = WHICH.Unimplemented;
            _content = value;
        }
    }

    public Exception? Abort
    {
        get => _which == WHICH.Abort ? (Exception?)_content : null;
        set
        {
            _which = WHICH.Abort;
            _content = value;
        }
    }

    public Call? Call
    {
        get => _which == WHICH.Call ? (Call?)_content : null;
        set
        {
            _which = WHICH.Call;
            _content = value;
        }
    }

    public Return? Return
    {
        get => _which == WHICH.Return ? (Return?)_content : null;
        set
        {
            _which = WHICH.Return;
            _content = value;
        }
    }

    public Finish? Finish
    {
        get => _which == WHICH.Finish ? (Finish?)_content : null;
        set
        {
            _which = WHICH.Finish;
            _content = value;
        }
    }

    public Resolve? Resolve
    {
        get => _which == WHICH.Resolve ? (Resolve?)_content : null;
        set
        {
            _which = WHICH.Resolve;
            _content = value;
        }
    }

    public Release? Release
    {
        get => _which == WHICH.Release ? (Release?)_content : null;
        set
        {
            _which = WHICH.Release;
            _content = value;
        }
    }

    public object? ObsoleteSave
    {
        get => _which == WHICH.ObsoleteSave ? (object?)_content : null;
        set
        {
            _which = WHICH.ObsoleteSave;
            _content = value;
        }
    }

    public Bootstrap? Bootstrap
    {
        get => _which == WHICH.Bootstrap ? (Bootstrap?)_content : null;
        set
        {
            _which = WHICH.Bootstrap;
            _content = value;
        }
    }

    public object? ObsoleteDelete
    {
        get => _which == WHICH.ObsoleteDelete ? (object?)_content : null;
        set
        {
            _which = WHICH.ObsoleteDelete;
            _content = value;
        }
    }

    public Provide? Provide
    {
        get => _which == WHICH.Provide ? (Provide?)_content : null;
        set
        {
            _which = WHICH.Provide;
            _content = value;
        }
    }

    public Accept? Accept
    {
        get => _which == WHICH.Accept ? (Accept?)_content : null;
        set
        {
            _which = WHICH.Accept;
            _content = value;
        }
    }

    public Join? Join
    {
        get => _which == WHICH.Join ? (Join?)_content : null;
        set
        {
            _which = WHICH.Join;
            _content = value;
        }
    }

    public Disembargo? Disembargo
    {
        get => _which == WHICH.Disembargo ? (Disembargo?)_content : null;
        set
        {
            _which = WHICH.Disembargo;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.Unimplemented:
                Unimplemented = CapnpSerializable.Create<Message>(reader.Unimplemented);
                break;
            case WHICH.Abort:
                Abort = CapnpSerializable.Create<Exception>(reader.Abort);
                break;
            case WHICH.Call:
                Call = CapnpSerializable.Create<Call>(reader.Call);
                break;
            case WHICH.Return:
                Return = CapnpSerializable.Create<Return>(reader.Return);
                break;
            case WHICH.Finish:
                Finish = CapnpSerializable.Create<Finish>(reader.Finish);
                break;
            case WHICH.Resolve:
                Resolve = CapnpSerializable.Create<Resolve>(reader.Resolve);
                break;
            case WHICH.Release:
                Release = CapnpSerializable.Create<Release>(reader.Release);
                break;
            case WHICH.ObsoleteSave:
                ObsoleteSave = CapnpSerializable.Create<object>(reader.ObsoleteSave);
                break;
            case WHICH.Bootstrap:
                Bootstrap = CapnpSerializable.Create<Bootstrap>(reader.Bootstrap);
                break;
            case WHICH.ObsoleteDelete:
                ObsoleteDelete = CapnpSerializable.Create<object>(reader.ObsoleteDelete);
                break;
            case WHICH.Provide:
                Provide = CapnpSerializable.Create<Provide>(reader.Provide);
                break;
            case WHICH.Accept:
                Accept = CapnpSerializable.Create<Accept>(reader.Accept);
                break;
            case WHICH.Join:
                Join = CapnpSerializable.Create<Join>(reader.Join);
                break;
            case WHICH.Disembargo:
                Disembargo = CapnpSerializable.Create<Disembargo>(reader.Disembargo);
                break;
        }

        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.which = which;
        switch (which)
        {
            case WHICH.Unimplemented:
                Unimplemented?.serialize(writer.Unimplemented!);
                break;
            case WHICH.Abort:
                Abort?.serialize(writer.Abort!);
                break;
            case WHICH.Call:
                Call?.serialize(writer.Call!);
                break;
            case WHICH.Return:
                Return?.serialize(writer.Return!);
                break;
            case WHICH.Finish:
                Finish?.serialize(writer.Finish!);
                break;
            case WHICH.Resolve:
                Resolve?.serialize(writer.Resolve!);
                break;
            case WHICH.Release:
                Release?.serialize(writer.Release!);
                break;
            case WHICH.ObsoleteSave:
                writer.ObsoleteSave!.SetObject(ObsoleteSave);
                break;
            case WHICH.Bootstrap:
                Bootstrap?.serialize(writer.Bootstrap!);
                break;
            case WHICH.ObsoleteDelete:
                writer.ObsoleteDelete!.SetObject(ObsoleteDelete);
                break;
            case WHICH.Provide:
                Provide?.serialize(writer.Provide!);
                break;
            case WHICH.Accept:
                Accept?.serialize(writer.Accept!);
                break;
            case WHICH.Join:
                Join?.serialize(writer.Join!);
                break;
            case WHICH.Disembargo:
                Disembargo?.serialize(writer.Disembargo!);
                break;
        }
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(0U);
        public READER Unimplemented => which == WHICH.Unimplemented ? ctx.ReadStruct(0, create) : default;
        public Exception.READER Abort => which == WHICH.Abort ? ctx.ReadStruct(0, Exception.READER.create) : default;
        public Call.READER Call => which == WHICH.Call ? ctx.ReadStruct(0, Rpc.Call.READER.create) : default;
        public Return.READER Return => which == WHICH.Return ? ctx.ReadStruct(0, Rpc.Return.READER.create) : default;
        public Finish.READER Finish => which == WHICH.Finish ? ctx.ReadStruct(0, Rpc.Finish.READER.create) : default;

        public Resolve.READER Resolve =>
            which == WHICH.Resolve ? ctx.ReadStruct(0, Rpc.Resolve.READER.create) : default;

        public Release.READER Release =>
            which == WHICH.Release ? ctx.ReadStruct(0, Rpc.Release.READER.create) : default;

        public DeserializerState ObsoleteSave => which == WHICH.ObsoleteSave ? ctx.StructReadPointer(0) : default;

        public Bootstrap.READER Bootstrap =>
            which == WHICH.Bootstrap ? ctx.ReadStruct(0, Rpc.Bootstrap.READER.create) : default;

        public DeserializerState ObsoleteDelete => which == WHICH.ObsoleteDelete ? ctx.StructReadPointer(0) : default;

        public Provide.READER Provide =>
            which == WHICH.Provide ? ctx.ReadStruct(0, Rpc.Provide.READER.create) : default;

        public Accept.READER Accept => which == WHICH.Accept ? ctx.ReadStruct(0, Rpc.Accept.READER.create) : default;
        public Join.READER Join => which == WHICH.Join ? ctx.ReadStruct(0, Rpc.Join.READER.create) : default;

        public Disembargo.READER Disembargo =>
            which == WHICH.Disembargo ? ctx.ReadStruct(0, Rpc.Disembargo.READER.create) : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(0U);
            set => this.WriteData(0U, (ushort)value);
        }

        [DisallowNull]
        public WRITER? Unimplemented
        {
            get => which == WHICH.Unimplemented ? BuildPointer<WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Exception.WRITER? Abort
        {
            get => which == WHICH.Abort ? BuildPointer<Exception.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Call.WRITER? Call
        {
            get => which == WHICH.Call ? BuildPointer<Call.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Return.WRITER? Return
        {
            get => which == WHICH.Return ? BuildPointer<Return.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Finish.WRITER? Finish
        {
            get => which == WHICH.Finish ? BuildPointer<Finish.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Resolve.WRITER? Resolve
        {
            get => which == WHICH.Resolve ? BuildPointer<Resolve.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Release.WRITER? Release
        {
            get => which == WHICH.Release ? BuildPointer<Release.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public DynamicSerializerState? ObsoleteSave
        {
            get => which == WHICH.ObsoleteSave ? BuildPointer<DynamicSerializerState>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Bootstrap.WRITER? Bootstrap
        {
            get => which == WHICH.Bootstrap ? BuildPointer<Bootstrap.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public DynamicSerializerState? ObsoleteDelete
        {
            get => which == WHICH.ObsoleteDelete ? BuildPointer<DynamicSerializerState>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Provide.WRITER? Provide
        {
            get => which == WHICH.Provide ? BuildPointer<Provide.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Accept.WRITER? Accept
        {
            get => which == WHICH.Accept ? BuildPointer<Accept.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Join.WRITER? Join
        {
            get => which == WHICH.Join ? BuildPointer<Join.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Disembargo.WRITER? Disembargo
        {
            get => which == WHICH.Disembargo ? BuildPointer<Disembargo.WRITER>(0) : default;
            set => Link(0, value!);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xe94ccf8031176ec4UL)]
public class Bootstrap : ICapnpSerializable
{
    public const ulong typeId = 0xe94ccf8031176ec4UL;

    public uint QuestionId { get; set; }

    public object? DeprecatedObjectId { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        DeprecatedObjectId = CapnpSerializable.Create<object>(reader.DeprecatedObjectId);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        writer.DeprecatedObjectId.SetObject(DeprecatedObjectId);
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public DeserializerState DeprecatedObjectId => ctx.StructReadPointer(0);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public DynamicSerializerState DeprecatedObjectId
        {
            get => BuildPointer<DynamicSerializerState>(0);
            set => Link(0, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x836a53ce789d4cd4UL)]
public class Call : ICapnpSerializable
{
    public const ulong typeId = 0x836a53ce789d4cd4UL;

    public uint QuestionId { get; set; }

    public MessageTarget? Target { get; set; }

    public ulong InterfaceId { get; set; }

    public ushort MethodId { get; set; }

    public Payload? Params { get; set; }

    public sendResultsTo? SendResultsTo { get; set; }

    public bool AllowThirdPartyTailCall { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        Target = CapnpSerializable.Create<MessageTarget>(reader.Target);
        InterfaceId = reader.InterfaceId;
        MethodId = reader.MethodId;
        Params = CapnpSerializable.Create<Payload>(reader.Params);
        SendResultsTo = CapnpSerializable.Create<sendResultsTo>(reader.SendResultsTo);
        AllowThirdPartyTailCall = reader.AllowThirdPartyTailCall;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        Target?.serialize(writer.Target);
        writer.InterfaceId = InterfaceId;
        writer.MethodId = MethodId;
        Params?.serialize(writer.Params);
        SendResultsTo?.serialize(writer.SendResultsTo);
        writer.AllowThirdPartyTailCall = AllowThirdPartyTailCall;
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public MessageTarget.READER Target => ctx.ReadStruct(0, MessageTarget.READER.create);
        public ulong InterfaceId => ctx.ReadDataULong(64UL);
        public ushort MethodId => ctx.ReadDataUShort(32UL);
        public Payload.READER Params => ctx.ReadStruct(1, Payload.READER.create);
        public sendResultsTo.READER SendResultsTo => new(ctx);
        public bool AllowThirdPartyTailCall => ctx.ReadDataBool(128UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(3, 3);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public MessageTarget.WRITER Target
        {
            get => BuildPointer<MessageTarget.WRITER>(0);
            set => Link(0, value);
        }

        public ulong InterfaceId
        {
            get => this.ReadDataULong(64UL);
            set => this.WriteData(64UL, value);
        }

        public ushort MethodId
        {
            get => this.ReadDataUShort(32UL);
            set => this.WriteData(32UL, value);
        }

        public Payload.WRITER Params
        {
            get => BuildPointer<Payload.WRITER>(1);
            set => Link(1, value);
        }

        public sendResultsTo.WRITER SendResultsTo => Rewrap<sendResultsTo.WRITER>();

        public bool AllowThirdPartyTailCall
        {
            get => this.ReadDataBool(128UL);
            set => this.WriteData(128UL, value);
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xdae8b0f61aab5f99UL)]
    public class sendResultsTo : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            Caller = 0,
            Yourself = 1,
            ThirdParty = 2,
            undefined = 65535
        }

        public const ulong typeId = 0xdae8b0f61aab5f99UL;
        private object? _content;

        private WHICH _which = WHICH.undefined;

        public WHICH which
        {
            get => _which;
            set
            {
                if (value == _which)
                    return;
                _which = value;
                switch (value)
                {
                    case WHICH.Caller:
                        break;
                    case WHICH.Yourself:
                        break;
                    case WHICH.ThirdParty:
                        _content = null;
                        break;
                }
            }
        }

        public object? ThirdParty
        {
            get => _which == WHICH.ThirdParty ? (object?)_content : null;
            set
            {
                _which = WHICH.ThirdParty;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Caller:
                    which = reader.which;
                    break;
                case WHICH.Yourself:
                    which = reader.which;
                    break;
                case WHICH.ThirdParty:
                    ThirdParty = CapnpSerializable.Create<object>(reader.ThirdParty);
                    break;
            }

            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.which = which;
            switch (which)
            {
                case WHICH.Caller:
                    break;
                case WHICH.Yourself:
                    break;
                case WHICH.ThirdParty:
                    writer.ThirdParty!.SetObject(ThirdParty);
                    break;
            }
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(48U);
            public DeserializerState ThirdParty => which == WHICH.ThirdParty ? ctx.StructReadPointer(2) : default;
        }

        public class WRITER : SerializerState
        {
            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(48U);
                set => this.WriteData(48U, (ushort)value);
            }

            [DisallowNull]
            public DynamicSerializerState? ThirdParty
            {
                get => which == WHICH.ThirdParty ? BuildPointer<DynamicSerializerState>(2) : default;
                set => Link(2, value!);
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x9e19b28d3db3573aUL)]
public class Return : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        Results = 0,
        Exception = 1,
        Canceled = 2,
        ResultsSentElsewhere = 3,
        TakeFromOtherQuestion = 4,
        AcceptFromThirdParty = 5,
        undefined = 65535
    }

    public const ulong typeId = 0x9e19b28d3db3573aUL;
    private object? _content;

    private WHICH _which = WHICH.undefined;

    public WHICH which
    {
        get => _which;
        set
        {
            if (value == _which)
                return;
            _which = value;
            switch (value)
            {
                case WHICH.Results:
                    _content = null;
                    break;
                case WHICH.Exception:
                    _content = null;
                    break;
                case WHICH.Canceled:
                    break;
                case WHICH.ResultsSentElsewhere:
                    break;
                case WHICH.TakeFromOtherQuestion:
                    _content = 0;
                    break;
                case WHICH.AcceptFromThirdParty:
                    _content = null;
                    break;
            }
        }
    }

    public uint AnswerId { get; set; }

    public bool ReleaseParamCaps { get; set; }

        = true;

    public Payload? Results
    {
        get => _which == WHICH.Results ? (Payload?)_content : null;
        set
        {
            _which = WHICH.Results;
            _content = value;
        }
    }

    public Exception? Exception
    {
        get => _which == WHICH.Exception ? (Exception?)_content : null;
        set
        {
            _which = WHICH.Exception;
            _content = value;
        }
    }

    public uint? TakeFromOtherQuestion
    {
        get => _which == WHICH.TakeFromOtherQuestion ? (uint?)_content : null;
        set
        {
            _which = WHICH.TakeFromOtherQuestion;
            _content = value;
        }
    }

    public object? AcceptFromThirdParty
    {
        get => _which == WHICH.AcceptFromThirdParty ? (object?)_content : null;
        set
        {
            _which = WHICH.AcceptFromThirdParty;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.Results:
                Results = CapnpSerializable.Create<Payload>(reader.Results);
                break;
            case WHICH.Exception:
                Exception = CapnpSerializable.Create<Exception>(reader.Exception);
                break;
            case WHICH.Canceled:
                which = reader.which;
                break;
            case WHICH.ResultsSentElsewhere:
                which = reader.which;
                break;
            case WHICH.TakeFromOtherQuestion:
                TakeFromOtherQuestion = reader.TakeFromOtherQuestion;
                break;
            case WHICH.AcceptFromThirdParty:
                AcceptFromThirdParty = CapnpSerializable.Create<object>(reader.AcceptFromThirdParty);
                break;
        }

        AnswerId = reader.AnswerId;
        ReleaseParamCaps = reader.ReleaseParamCaps;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.which = which;
        switch (which)
        {
            case WHICH.Results:
                Results?.serialize(writer.Results!);
                break;
            case WHICH.Exception:
                Exception?.serialize(writer.Exception!);
                break;
            case WHICH.Canceled:
                break;
            case WHICH.ResultsSentElsewhere:
                break;
            case WHICH.TakeFromOtherQuestion:
                writer.TakeFromOtherQuestion = TakeFromOtherQuestion!.Value;
                break;
            case WHICH.AcceptFromThirdParty:
                writer.AcceptFromThirdParty!.SetObject(AcceptFromThirdParty);
                break;
        }

        writer.AnswerId = AnswerId;
        writer.ReleaseParamCaps = ReleaseParamCaps;
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(48U);
        public uint AnswerId => ctx.ReadDataUInt(0UL);
        public bool ReleaseParamCaps => ctx.ReadDataBool(32UL, true);
        public Payload.READER Results => which == WHICH.Results ? ctx.ReadStruct(0, Payload.READER.create) : default;

        public Exception.READER Exception =>
            which == WHICH.Exception ? ctx.ReadStruct(0, Rpc.Exception.READER.create) : default;

        public uint TakeFromOtherQuestion => which == WHICH.TakeFromOtherQuestion ? ctx.ReadDataUInt(64UL) : default;

        public DeserializerState AcceptFromThirdParty =>
            which == WHICH.AcceptFromThirdParty ? ctx.StructReadPointer(0) : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(2, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(48U);
            set => this.WriteData(48U, (ushort)value);
        }

        public uint AnswerId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public bool ReleaseParamCaps
        {
            get => this.ReadDataBool(32UL, true);
            set => this.WriteData(32UL, value, true);
        }

        [DisallowNull]
        public Payload.WRITER? Results
        {
            get => which == WHICH.Results ? BuildPointer<Payload.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Exception.WRITER? Exception
        {
            get => which == WHICH.Exception ? BuildPointer<Exception.WRITER>(0) : default;
            set => Link(0, value!);
        }

        public uint TakeFromOtherQuestion
        {
            get => which == WHICH.TakeFromOtherQuestion ? this.ReadDataUInt(64UL) : default;
            set => this.WriteData(64UL, value);
        }

        [DisallowNull]
        public DynamicSerializerState? AcceptFromThirdParty
        {
            get => which == WHICH.AcceptFromThirdParty ? BuildPointer<DynamicSerializerState>(0) : default;
            set => Link(0, value!);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd37d2eb2c2f80e63UL)]
public class Finish : ICapnpSerializable
{
    public const ulong typeId = 0xd37d2eb2c2f80e63UL;

    public uint QuestionId { get; set; }

    public bool ReleaseResultCaps { get; set; }

        = true;

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        ReleaseResultCaps = reader.ReleaseResultCaps;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        writer.ReleaseResultCaps = ReleaseResultCaps;
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public bool ReleaseResultCaps => ctx.ReadDataBool(32UL, true);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 0);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public bool ReleaseResultCaps
        {
            get => this.ReadDataBool(32UL, true);
            set => this.WriteData(32UL, value, true);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xbbc29655fa89086eUL)]
public class Resolve : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        Cap = 0,
        Exception = 1,
        undefined = 65535
    }

    public const ulong typeId = 0xbbc29655fa89086eUL;
    private object? _content;

    private WHICH _which = WHICH.undefined;

    public WHICH which
    {
        get => _which;
        set
        {
            if (value == _which)
                return;
            _which = value;
            switch (value)
            {
                case WHICH.Cap:
                    _content = null;
                    break;
                case WHICH.Exception:
                    _content = null;
                    break;
            }
        }
    }

    public uint PromiseId { get; set; }

    public CapDescriptor? Cap
    {
        get => _which == WHICH.Cap ? (CapDescriptor?)_content : null;
        set
        {
            _which = WHICH.Cap;
            _content = value;
        }
    }

    public Exception? Exception
    {
        get => _which == WHICH.Exception ? (Exception?)_content : null;
        set
        {
            _which = WHICH.Exception;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.Cap:
                Cap = CapnpSerializable.Create<CapDescriptor>(reader.Cap);
                break;
            case WHICH.Exception:
                Exception = CapnpSerializable.Create<Exception>(reader.Exception);
                break;
        }

        PromiseId = reader.PromiseId;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.which = which;
        switch (which)
        {
            case WHICH.Cap:
                Cap?.serialize(writer.Cap!);
                break;
            case WHICH.Exception:
                Exception?.serialize(writer.Exception!);
                break;
        }

        writer.PromiseId = PromiseId;
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(32U);
        public uint PromiseId => ctx.ReadDataUInt(0UL);

        public CapDescriptor.READER Cap =>
            which == WHICH.Cap ? ctx.ReadStruct(0, CapDescriptor.READER.create) : default;

        public Exception.READER Exception =>
            which == WHICH.Exception ? ctx.ReadStruct(0, Rpc.Exception.READER.create) : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(32U);
            set => this.WriteData(32U, (ushort)value);
        }

        public uint PromiseId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        [DisallowNull]
        public CapDescriptor.WRITER? Cap
        {
            get => which == WHICH.Cap ? BuildPointer<CapDescriptor.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public Exception.WRITER? Exception
        {
            get => which == WHICH.Exception ? BuildPointer<Exception.WRITER>(0) : default;
            set => Link(0, value!);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xad1a6c0d7dd07497UL)]
public class Release : ICapnpSerializable
{
    public const ulong typeId = 0xad1a6c0d7dd07497UL;

    public uint Id { get; set; }

    public uint ReferenceCount { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Id = reader.Id;
        ReferenceCount = reader.ReferenceCount;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Id = Id;
        writer.ReferenceCount = ReferenceCount;
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

        public uint Id => ctx.ReadDataUInt(0UL);
        public uint ReferenceCount => ctx.ReadDataUInt(32UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 0);
        }

        public uint Id
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public uint ReferenceCount
        {
            get => this.ReadDataUInt(32UL);
            set => this.WriteData(32UL, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xf964368b0fbd3711UL)]
public class Disembargo : ICapnpSerializable
{
    public const ulong typeId = 0xf964368b0fbd3711UL;

    public MessageTarget? Target { get; set; }

    public context? Context { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Target = CapnpSerializable.Create<MessageTarget>(reader.Target);
        Context = CapnpSerializable.Create<context>(reader.Context);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        Target?.serialize(writer.Target);
        Context?.serialize(writer.Context);
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

        public MessageTarget.READER Target => ctx.ReadStruct(0, MessageTarget.READER.create);
        public context.READER Context => new(ctx);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public MessageTarget.WRITER Target
        {
            get => BuildPointer<MessageTarget.WRITER>(0);
            set => Link(0, value);
        }

        public context.WRITER Context => Rewrap<context.WRITER>();
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xd562b4df655bdd4dUL)]
    public class context : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            SenderLoopback = 0,
            ReceiverLoopback = 1,
            Accept = 2,
            Provide = 3,
            undefined = 65535
        }

        public const ulong typeId = 0xd562b4df655bdd4dUL;
        private object? _content;

        private WHICH _which = WHICH.undefined;

        public WHICH which
        {
            get => _which;
            set
            {
                if (value == _which)
                    return;
                _which = value;
                switch (value)
                {
                    case WHICH.SenderLoopback:
                        _content = 0;
                        break;
                    case WHICH.ReceiverLoopback:
                        _content = 0;
                        break;
                    case WHICH.Accept:
                        break;
                    case WHICH.Provide:
                        _content = 0;
                        break;
                }
            }
        }

        public uint? SenderLoopback
        {
            get => _which == WHICH.SenderLoopback ? (uint?)_content : null;
            set
            {
                _which = WHICH.SenderLoopback;
                _content = value;
            }
        }

        public uint? ReceiverLoopback
        {
            get => _which == WHICH.ReceiverLoopback ? (uint?)_content : null;
            set
            {
                _which = WHICH.ReceiverLoopback;
                _content = value;
            }
        }

        public uint? Provide
        {
            get => _which == WHICH.Provide ? (uint?)_content : null;
            set
            {
                _which = WHICH.Provide;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.SenderLoopback:
                    SenderLoopback = reader.SenderLoopback;
                    break;
                case WHICH.ReceiverLoopback:
                    ReceiverLoopback = reader.ReceiverLoopback;
                    break;
                case WHICH.Accept:
                    which = reader.which;
                    break;
                case WHICH.Provide:
                    Provide = reader.Provide;
                    break;
            }

            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.which = which;
            switch (which)
            {
                case WHICH.SenderLoopback:
                    writer.SenderLoopback = SenderLoopback!.Value;
                    break;
                case WHICH.ReceiverLoopback:
                    writer.ReceiverLoopback = ReceiverLoopback!.Value;
                    break;
                case WHICH.Accept:
                    break;
                case WHICH.Provide:
                    writer.Provide = Provide!.Value;
                    break;
            }
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(32U);
            public uint SenderLoopback => which == WHICH.SenderLoopback ? ctx.ReadDataUInt(0UL) : default;
            public uint ReceiverLoopback => which == WHICH.ReceiverLoopback ? ctx.ReadDataUInt(0UL) : default;
            public uint Provide => which == WHICH.Provide ? ctx.ReadDataUInt(0UL) : default;
        }

        public class WRITER : SerializerState
        {
            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(32U);
                set => this.WriteData(32U, (ushort)value);
            }

            public uint SenderLoopback
            {
                get => which == WHICH.SenderLoopback ? this.ReadDataUInt(0UL) : default;
                set => this.WriteData(0UL, value);
            }

            public uint ReceiverLoopback
            {
                get => which == WHICH.ReceiverLoopback ? this.ReadDataUInt(0UL) : default;
                set => this.WriteData(0UL, value);
            }

            public uint Provide
            {
                get => which == WHICH.Provide ? this.ReadDataUInt(0UL) : default;
                set => this.WriteData(0UL, value);
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x9c6a046bfbc1ac5aUL)]
public class Provide : ICapnpSerializable
{
    public const ulong typeId = 0x9c6a046bfbc1ac5aUL;

    public uint QuestionId { get; set; }

    public MessageTarget? Target { get; set; }

    public object? Recipient { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        Target = CapnpSerializable.Create<MessageTarget>(reader.Target);
        Recipient = CapnpSerializable.Create<object>(reader.Recipient);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        Target?.serialize(writer.Target);
        writer.Recipient.SetObject(Recipient);
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public MessageTarget.READER Target => ctx.ReadStruct(0, MessageTarget.READER.create);
        public DeserializerState Recipient => ctx.StructReadPointer(1);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 2);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public MessageTarget.WRITER Target
        {
            get => BuildPointer<MessageTarget.WRITER>(0);
            set => Link(0, value);
        }

        public DynamicSerializerState Recipient
        {
            get => BuildPointer<DynamicSerializerState>(1);
            set => Link(1, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd4c9b56290554016UL)]
public class Accept : ICapnpSerializable
{
    public const ulong typeId = 0xd4c9b56290554016UL;

    public uint QuestionId { get; set; }

    public object? Provision { get; set; }

    public bool Embargo { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        Provision = CapnpSerializable.Create<object>(reader.Provision);
        Embargo = reader.Embargo;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        writer.Provision.SetObject(Provision);
        writer.Embargo = Embargo;
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public DeserializerState Provision => ctx.StructReadPointer(0);
        public bool Embargo => ctx.ReadDataBool(32UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public DynamicSerializerState Provision
        {
            get => BuildPointer<DynamicSerializerState>(0);
            set => Link(0, value);
        }

        public bool Embargo
        {
            get => this.ReadDataBool(32UL);
            set => this.WriteData(32UL, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xfbe1980490e001afUL)]
public class Join : ICapnpSerializable
{
    public const ulong typeId = 0xfbe1980490e001afUL;

    public uint QuestionId { get; set; }

    public MessageTarget? Target { get; set; }

    public object? KeyPart { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        Target = CapnpSerializable.Create<MessageTarget>(reader.Target);
        KeyPart = CapnpSerializable.Create<object>(reader.KeyPart);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        Target?.serialize(writer.Target);
        writer.KeyPart.SetObject(KeyPart);
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public MessageTarget.READER Target => ctx.ReadStruct(0, MessageTarget.READER.create);
        public DeserializerState KeyPart => ctx.StructReadPointer(1);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 2);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public MessageTarget.WRITER Target
        {
            get => BuildPointer<MessageTarget.WRITER>(0);
            set => Link(0, value);
        }

        public DynamicSerializerState KeyPart
        {
            get => BuildPointer<DynamicSerializerState>(1);
            set => Link(1, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x95bc14545813fbc1UL)]
public class MessageTarget : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        ImportedCap = 0,
        PromisedAnswer = 1,
        undefined = 65535
    }

    public const ulong typeId = 0x95bc14545813fbc1UL;
    private object? _content;

    private WHICH _which = WHICH.undefined;

    public WHICH which
    {
        get => _which;
        set
        {
            if (value == _which)
                return;
            _which = value;
            switch (value)
            {
                case WHICH.ImportedCap:
                    _content = 0;
                    break;
                case WHICH.PromisedAnswer:
                    _content = null;
                    break;
            }
        }
    }

    public uint? ImportedCap
    {
        get => _which == WHICH.ImportedCap ? (uint?)_content : null;
        set
        {
            _which = WHICH.ImportedCap;
            _content = value;
        }
    }

    public PromisedAnswer? PromisedAnswer
    {
        get => _which == WHICH.PromisedAnswer ? (PromisedAnswer?)_content : null;
        set
        {
            _which = WHICH.PromisedAnswer;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.ImportedCap:
                ImportedCap = reader.ImportedCap;
                break;
            case WHICH.PromisedAnswer:
                PromisedAnswer = CapnpSerializable.Create<PromisedAnswer>(reader.PromisedAnswer);
                break;
        }

        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.which = which;
        switch (which)
        {
            case WHICH.ImportedCap:
                writer.ImportedCap = ImportedCap!.Value;
                break;
            case WHICH.PromisedAnswer:
                PromisedAnswer?.serialize(writer.PromisedAnswer!);
                break;
        }
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(32U);
        public uint ImportedCap => which == WHICH.ImportedCap ? ctx.ReadDataUInt(0UL) : default;

        public PromisedAnswer.READER PromisedAnswer => which == WHICH.PromisedAnswer
            ? ctx.ReadStruct(0, Rpc.PromisedAnswer.READER.create)
            : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(32U);
            set => this.WriteData(32U, (ushort)value);
        }

        public uint ImportedCap
        {
            get => which == WHICH.ImportedCap ? this.ReadDataUInt(0UL) : default;
            set => this.WriteData(0UL, value);
        }

        [DisallowNull]
        public PromisedAnswer.WRITER? PromisedAnswer
        {
            get => which == WHICH.PromisedAnswer ? BuildPointer<PromisedAnswer.WRITER>(0) : default;
            set => Link(0, value!);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x9a0e61223d96743bUL)]
public class Payload : ICapnpSerializable
{
    public const ulong typeId = 0x9a0e61223d96743bUL;

    public object? Content { get; set; }

    public IReadOnlyList<CapDescriptor>? CapTable { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Content = CapnpSerializable.Create<object>(reader.Content);
        CapTable = reader.CapTable?.ToReadOnlyList(_ => CapnpSerializable.Create<CapDescriptor>(_)!);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Content.SetObject(Content);
        writer.CapTable.Init(CapTable, (_s1, _v1) => _v1?.serialize(_s1));
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

        public DeserializerState Content => ctx.StructReadPointer(0);
        public IReadOnlyList<CapDescriptor.READER> CapTable => ctx.ReadList(1).Cast(CapDescriptor.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(0, 2);
        }

        public DynamicSerializerState Content
        {
            get => BuildPointer<DynamicSerializerState>(0);
            set => Link(0, value);
        }

        public ListOfStructsSerializer<CapDescriptor.WRITER> CapTable
        {
            get => BuildPointer<ListOfStructsSerializer<CapDescriptor.WRITER>>(1);
            set => Link(1, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x8523ddc40b86b8b0UL)]
public class CapDescriptor : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        None = 0,
        SenderHosted = 1,
        SenderPromise = 2,
        ReceiverHosted = 3,
        ReceiverAnswer = 4,
        ThirdPartyHosted = 5,
        undefined = 65535
    }

    public const ulong typeId = 0x8523ddc40b86b8b0UL;
    private object? _content;

    private WHICH _which = WHICH.undefined;

    public WHICH which
    {
        get => _which;
        set
        {
            if (value == _which)
                return;
            _which = value;
            switch (value)
            {
                case WHICH.None:
                    break;
                case WHICH.SenderHosted:
                    _content = 0;
                    break;
                case WHICH.SenderPromise:
                    _content = 0;
                    break;
                case WHICH.ReceiverHosted:
                    _content = 0;
                    break;
                case WHICH.ReceiverAnswer:
                    _content = null;
                    break;
                case WHICH.ThirdPartyHosted:
                    _content = null;
                    break;
            }
        }
    }

    public uint? SenderHosted
    {
        get => _which == WHICH.SenderHosted ? (uint?)_content : null;
        set
        {
            _which = WHICH.SenderHosted;
            _content = value;
        }
    }

    public uint? SenderPromise
    {
        get => _which == WHICH.SenderPromise ? (uint?)_content : null;
        set
        {
            _which = WHICH.SenderPromise;
            _content = value;
        }
    }

    public uint? ReceiverHosted
    {
        get => _which == WHICH.ReceiverHosted ? (uint?)_content : null;
        set
        {
            _which = WHICH.ReceiverHosted;
            _content = value;
        }
    }

    public PromisedAnswer? ReceiverAnswer
    {
        get => _which == WHICH.ReceiverAnswer ? (PromisedAnswer?)_content : null;
        set
        {
            _which = WHICH.ReceiverAnswer;
            _content = value;
        }
    }

    public ThirdPartyCapDescriptor? ThirdPartyHosted
    {
        get => _which == WHICH.ThirdPartyHosted ? (ThirdPartyCapDescriptor?)_content : null;
        set
        {
            _which = WHICH.ThirdPartyHosted;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.None:
                which = reader.which;
                break;
            case WHICH.SenderHosted:
                SenderHosted = reader.SenderHosted;
                break;
            case WHICH.SenderPromise:
                SenderPromise = reader.SenderPromise;
                break;
            case WHICH.ReceiverHosted:
                ReceiverHosted = reader.ReceiverHosted;
                break;
            case WHICH.ReceiverAnswer:
                ReceiverAnswer = CapnpSerializable.Create<PromisedAnswer>(reader.ReceiverAnswer);
                break;
            case WHICH.ThirdPartyHosted:
                ThirdPartyHosted = CapnpSerializable.Create<ThirdPartyCapDescriptor>(reader.ThirdPartyHosted);
                break;
        }

        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.which = which;
        switch (which)
        {
            case WHICH.None:
                break;
            case WHICH.SenderHosted:
                writer.SenderHosted = SenderHosted!.Value;
                break;
            case WHICH.SenderPromise:
                writer.SenderPromise = SenderPromise!.Value;
                break;
            case WHICH.ReceiverHosted:
                writer.ReceiverHosted = ReceiverHosted!.Value;
                break;
            case WHICH.ReceiverAnswer:
                ReceiverAnswer?.serialize(writer.ReceiverAnswer!);
                break;
            case WHICH.ThirdPartyHosted:
                ThirdPartyHosted?.serialize(writer.ThirdPartyHosted!);
                break;
        }
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(0U);
        public uint SenderHosted => which == WHICH.SenderHosted ? ctx.ReadDataUInt(32UL) : default;
        public uint SenderPromise => which == WHICH.SenderPromise ? ctx.ReadDataUInt(32UL) : default;
        public uint ReceiverHosted => which == WHICH.ReceiverHosted ? ctx.ReadDataUInt(32UL) : default;

        public PromisedAnswer.READER ReceiverAnswer => which == WHICH.ReceiverAnswer
            ? ctx.ReadStruct(0, PromisedAnswer.READER.create)
            : default;

        public ThirdPartyCapDescriptor.READER ThirdPartyHosted => which == WHICH.ThirdPartyHosted
            ? ctx.ReadStruct(0, ThirdPartyCapDescriptor.READER.create)
            : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(0U);
            set => this.WriteData(0U, (ushort)value);
        }

        public uint SenderHosted
        {
            get => which == WHICH.SenderHosted ? this.ReadDataUInt(32UL) : default;
            set => this.WriteData(32UL, value);
        }

        public uint SenderPromise
        {
            get => which == WHICH.SenderPromise ? this.ReadDataUInt(32UL) : default;
            set => this.WriteData(32UL, value);
        }

        public uint ReceiverHosted
        {
            get => which == WHICH.ReceiverHosted ? this.ReadDataUInt(32UL) : default;
            set => this.WriteData(32UL, value);
        }

        [DisallowNull]
        public PromisedAnswer.WRITER? ReceiverAnswer
        {
            get => which == WHICH.ReceiverAnswer ? BuildPointer<PromisedAnswer.WRITER>(0) : default;
            set => Link(0, value!);
        }

        [DisallowNull]
        public ThirdPartyCapDescriptor.WRITER? ThirdPartyHosted
        {
            get => which == WHICH.ThirdPartyHosted ? BuildPointer<ThirdPartyCapDescriptor.WRITER>(0) : default;
            set => Link(0, value!);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd800b1d6cd6f1ca0UL)]
public class PromisedAnswer : ICapnpSerializable
{
    public const ulong typeId = 0xd800b1d6cd6f1ca0UL;

    public uint QuestionId { get; set; }

    public IReadOnlyList<Op>? Transform { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        QuestionId = reader.QuestionId;
        Transform = reader.Transform?.ToReadOnlyList(_ => CapnpSerializable.Create<Op>(_)!);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.QuestionId = QuestionId;
        writer.Transform.Init(Transform, (_s1, _v1) => _v1?.serialize(_s1));
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

        public uint QuestionId => ctx.ReadDataUInt(0UL);
        public IReadOnlyList<Op.READER> Transform => ctx.ReadList(0).Cast(Op.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public uint QuestionId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }

        public ListOfStructsSerializer<Op.WRITER> Transform
        {
            get => BuildPointer<ListOfStructsSerializer<Op.WRITER>>(0);
            set => Link(0, value);
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xf316944415569081UL)]
    public class Op : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            Noop = 0,
            GetPointerField = 1,
            undefined = 65535
        }

        public const ulong typeId = 0xf316944415569081UL;
        private object? _content;

        private WHICH _which = WHICH.undefined;

        public WHICH which
        {
            get => _which;
            set
            {
                if (value == _which)
                    return;
                _which = value;
                switch (value)
                {
                    case WHICH.Noop:
                        break;
                    case WHICH.GetPointerField:
                        _content = 0;
                        break;
                }
            }
        }

        public ushort? GetPointerField
        {
            get => _which == WHICH.GetPointerField ? (ushort?)_content : null;
            set
            {
                _which = WHICH.GetPointerField;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Noop:
                    which = reader.which;
                    break;
                case WHICH.GetPointerField:
                    GetPointerField = reader.GetPointerField;
                    break;
            }

            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.which = which;
            switch (which)
            {
                case WHICH.Noop:
                    break;
                case WHICH.GetPointerField:
                    writer.GetPointerField = GetPointerField!.Value;
                    break;
            }
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(0U);
            public ushort GetPointerField => which == WHICH.GetPointerField ? ctx.ReadDataUShort(16UL) : default;
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(1, 0);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(0U);
                set => this.WriteData(0U, (ushort)value);
            }

            public ushort GetPointerField
            {
                get => which == WHICH.GetPointerField ? this.ReadDataUShort(16UL) : default;
                set => this.WriteData(16UL, value);
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd37007fde1f0027dUL)]
public class ThirdPartyCapDescriptor : ICapnpSerializable
{
    public const ulong typeId = 0xd37007fde1f0027dUL;

    public object? Id { get; set; }

    public uint VineId { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Id = CapnpSerializable.Create<object>(reader.Id);
        VineId = reader.VineId;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Id.SetObject(Id);
        writer.VineId = VineId;
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

        public DeserializerState Id => ctx.StructReadPointer(0);
        public uint VineId => ctx.ReadDataUInt(0UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public DynamicSerializerState Id
        {
            get => BuildPointer<DynamicSerializerState>(0);
            set => Link(0, value);
        }

        public uint VineId
        {
            get => this.ReadDataUInt(0UL);
            set => this.WriteData(0UL, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd625b7063acf691aUL)]
public class Exception : ICapnpSerializable
{
    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xb28c96e23f4cbd58UL)]
    public enum Type : ushort
    {
        failed,
        overloaded,
        disconnected,
        unimplemented
    }

    public const ulong typeId = 0xd625b7063acf691aUL;

    public string? Reason { get; set; }

    public bool ObsoleteIsCallersFault { get; set; }

    public ushort ObsoleteDurability { get; set; }

    public Type TheType { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Reason = reader.Reason;
        ObsoleteIsCallersFault = reader.ObsoleteIsCallersFault;
        ObsoleteDurability = reader.ObsoleteDurability;
        TheType = reader.TheType;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Reason = Reason;
        writer.ObsoleteIsCallersFault = ObsoleteIsCallersFault;
        writer.ObsoleteDurability = ObsoleteDurability;
        writer.TheType = TheType;
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

        public string? Reason => ctx.ReadText(0);
        public bool ObsoleteIsCallersFault => ctx.ReadDataBool(0UL);
        public ushort ObsoleteDurability => ctx.ReadDataUShort(16UL);
        public Type TheType => (Type)ctx.ReadDataUShort(32UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public string? Reason
        {
            get => ReadText(0);
            set => WriteText(0, value, null);
        }

        public bool ObsoleteIsCallersFault
        {
            get => this.ReadDataBool(0UL);
            set => this.WriteData(0UL, value);
        }

        public ushort ObsoleteDurability
        {
            get => this.ReadDataUShort(16UL);
            set => this.WriteData(16UL, value);
        }

        public Type TheType
        {
            get => (Type)this.ReadDataUShort(32UL);
            set => this.WriteData(32UL, (ushort)value);
        }
    }
}