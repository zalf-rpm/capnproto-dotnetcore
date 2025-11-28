#pragma warning disable CS1591
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Capnp;

namespace CapnpC.CSharp.Generator.Schema;

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xe682ab4cf923a417UL)]
public class Node : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        File = 0,
        Struct = 1,
        Enum = 2,
        Interface = 3,
        Const = 4,
        Annotation = 5,
        undefined = 65535,
    }

    public const ulong typeId = 0xe682ab4cf923a417UL;
    private object _content;

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
                case WHICH.File:
                    break;
                case WHICH.Struct:
                    _content = null;
                    break;
                case WHICH.Enum:
                    _content = null;
                    break;
                case WHICH.Interface:
                    _content = null;
                    break;
                case WHICH.Const:
                    _content = null;
                    break;
                case WHICH.Annotation:
                    _content = null;
                    break;
            }
        }
    }

    public ulong Id { get; set; }

    public string DisplayName { get; set; }

    public uint DisplayNamePrefixLength { get; set; }

    public ulong ScopeId { get; set; }

    public IReadOnlyList<NestedNode> NestedNodes { get; set; }

    public IReadOnlyList<Annotation> Annotations { get; set; }

    public @struct Struct
    {
        get => _which == WHICH.Struct ? (@struct)_content : null;
        set
        {
            _which = WHICH.Struct;
            _content = value;
        }
    }

    public @enum Enum
    {
        get => _which == WHICH.Enum ? (@enum)_content : null;
        set
        {
            _which = WHICH.Enum;
            _content = value;
        }
    }

    public @interface Interface
    {
        get => _which == WHICH.Interface ? (@interface)_content : null;
        set
        {
            _which = WHICH.Interface;
            _content = value;
        }
    }

    public @const Const
    {
        get => _which == WHICH.Const ? (@const)_content : null;
        set
        {
            _which = WHICH.Const;
            _content = value;
        }
    }

    public annotation Annotation
    {
        get => _which == WHICH.Annotation ? (annotation)_content : null;
        set
        {
            _which = WHICH.Annotation;
            _content = value;
        }
    }

    public IReadOnlyList<Parameter> Parameters { get; set; }

    public bool IsGeneric { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.File:
                which = reader.which;
                break;
            case WHICH.Struct:
                Struct = CapnpSerializable.Create<@struct>(reader.Struct);
                break;
            case WHICH.Enum:
                Enum = CapnpSerializable.Create<@enum>(reader.Enum);
                break;
            case WHICH.Interface:
                Interface = CapnpSerializable.Create<@interface>(reader.Interface);
                break;
            case WHICH.Const:
                Const = CapnpSerializable.Create<@const>(reader.Const);
                break;
            case WHICH.Annotation:
                Annotation = CapnpSerializable.Create<annotation>(reader.Annotation);
                break;
        }

        Id = reader.Id;
        DisplayName = reader.DisplayName;
        DisplayNamePrefixLength = reader.DisplayNamePrefixLength;
        ScopeId = reader.ScopeId;
        NestedNodes = reader.NestedNodes?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<NestedNode>(_)
        );
        Annotations = reader.Annotations?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<Annotation>(_)
        );
        Parameters = reader.Parameters?.ToReadOnlyList(_ => CapnpSerializable.Create<Parameter>(_));
        IsGeneric = reader.IsGeneric;
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
            case WHICH.File:
                break;
            case WHICH.Struct:
                Struct?.serialize(writer.Struct);
                break;
            case WHICH.Enum:
                Enum?.serialize(writer.Enum);
                break;
            case WHICH.Interface:
                Interface?.serialize(writer.Interface);
                break;
            case WHICH.Const:
                Const?.serialize(writer.Const);
                break;
            case WHICH.Annotation:
                Annotation?.serialize(writer.Annotation);
                break;
        }

        writer.Id = Id;
        writer.DisplayName = DisplayName;
        writer.DisplayNamePrefixLength = DisplayNamePrefixLength;
        writer.ScopeId = ScopeId;
        writer.NestedNodes.Init(NestedNodes, (_s1, _v1) => _v1?.serialize(_s1));
        writer.Annotations.Init(Annotations, (_s1, _v1) => _v1?.serialize(_s1));
        writer.Parameters.Init(Parameters, (_s1, _v1) => _v1?.serialize(_s1));
        writer.IsGeneric = IsGeneric;
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(96U);
        public ulong Id => ctx.ReadDataULong(0UL);
        public string DisplayName => ctx.ReadText(0);
        public uint DisplayNamePrefixLength => ctx.ReadDataUInt(64UL);
        public ulong ScopeId => ctx.ReadDataULong(128UL);
        public IReadOnlyList<NestedNode.READER> NestedNodes =>
            ctx.ReadList(1).Cast(NestedNode.READER.create);
        public IReadOnlyList<Annotation.READER> Annotations =>
            ctx.ReadList(2).Cast(Schema.Annotation.READER.create);
        public @struct.READER Struct => which == WHICH.Struct ? new @struct.READER(ctx) : default;
        public @enum.READER Enum => which == WHICH.Enum ? new @enum.READER(ctx) : default;
        public @interface.READER Interface =>
            which == WHICH.Interface ? new @interface.READER(ctx) : default;
        public @const.READER Const => which == WHICH.Const ? new @const.READER(ctx) : default;
        public annotation.READER Annotation =>
            which == WHICH.Annotation ? new annotation.READER(ctx) : default;
        public IReadOnlyList<Parameter.READER> Parameters =>
            ctx.ReadList(5).Cast(Parameter.READER.create);
        public bool IsGeneric => ctx.ReadDataBool(288UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(5, 6);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(96U);
            set => this.WriteData(96U, (ushort)value);
        }

        public ulong Id
        {
            get => this.ReadDataULong(0UL);
            set => this.WriteData(0UL, value);
        }

        public string DisplayName
        {
            get => ReadText(0);
            set => WriteText(0, value, null);
        }

        public uint DisplayNamePrefixLength
        {
            get => this.ReadDataUInt(64UL);
            set => this.WriteData(64UL, value);
        }

        public ulong ScopeId
        {
            get => this.ReadDataULong(128UL);
            set => this.WriteData(128UL, value);
        }

        public ListOfStructsSerializer<NestedNode.WRITER> NestedNodes
        {
            get => BuildPointer<ListOfStructsSerializer<NestedNode.WRITER>>(1);
            set => Link(1, value);
        }

        public ListOfStructsSerializer<Annotation.WRITER> Annotations
        {
            get => BuildPointer<ListOfStructsSerializer<Annotation.WRITER>>(2);
            set => Link(2, value);
        }

        public @struct.WRITER Struct => which == WHICH.Struct ? Rewrap<@struct.WRITER>() : default;

        public @enum.WRITER Enum => which == WHICH.Enum ? Rewrap<@enum.WRITER>() : default;

        public @interface.WRITER Interface =>
            which == WHICH.Interface ? Rewrap<@interface.WRITER>() : default;

        public @const.WRITER Const => which == WHICH.Const ? Rewrap<@const.WRITER>() : default;

        public annotation.WRITER Annotation =>
            which == WHICH.Annotation ? Rewrap<annotation.WRITER>() : default;

        public ListOfStructsSerializer<Parameter.WRITER> Parameters
        {
            get => BuildPointer<ListOfStructsSerializer<Parameter.WRITER>>(5);
            set => Link(5, value);
        }

        public bool IsGeneric
        {
            get => this.ReadDataBool(288UL);
            set => this.WriteData(288UL, value);
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0x9ea0b19b37fb4435UL)]
    public class @struct : ICapnpSerializable
    {
        public const ulong typeId = 0x9ea0b19b37fb4435UL;

        public ushort DataWordCount { get; set; }

        public ushort PointerCount { get; set; }

        public ElementSize PreferredListEncoding { get; set; }

        public bool IsGroup { get; set; }

        public ushort DiscriminantCount { get; set; }

        public uint DiscriminantOffset { get; set; }

        public IReadOnlyList<Field> Fields { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            DataWordCount = reader.DataWordCount;
            PointerCount = reader.PointerCount;
            PreferredListEncoding = reader.PreferredListEncoding;
            IsGroup = reader.IsGroup;
            DiscriminantCount = reader.DiscriminantCount;
            DiscriminantOffset = reader.DiscriminantOffset;
            Fields = reader.Fields?.ToReadOnlyList(_ => CapnpSerializable.Create<Field>(_));
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.DataWordCount = DataWordCount;
            writer.PointerCount = PointerCount;
            writer.PreferredListEncoding = PreferredListEncoding;
            writer.IsGroup = IsGroup;
            writer.DiscriminantCount = DiscriminantCount;
            writer.DiscriminantOffset = DiscriminantOffset;
            writer.Fields.Init(Fields, (_s1, _v1) => _v1?.serialize(_s1));
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

            public ushort DataWordCount => ctx.ReadDataUShort(112UL);
            public ushort PointerCount => ctx.ReadDataUShort(192UL);
            public ElementSize PreferredListEncoding => (ElementSize)ctx.ReadDataUShort(208UL);
            public bool IsGroup => ctx.ReadDataBool(224UL);
            public ushort DiscriminantCount => ctx.ReadDataUShort(240UL);
            public uint DiscriminantOffset => ctx.ReadDataUInt(256UL);
            public IReadOnlyList<Field.READER> Fields => ctx.ReadList(3).Cast(Field.READER.create);
        }

        public class WRITER : SerializerState
        {
            public ushort DataWordCount
            {
                get => this.ReadDataUShort(112UL);
                set => this.WriteData(112UL, value);
            }

            public ushort PointerCount
            {
                get => this.ReadDataUShort(192UL);
                set => this.WriteData(192UL, value);
            }

            public ElementSize PreferredListEncoding
            {
                get => (ElementSize)this.ReadDataUShort(208UL);
                set => this.WriteData(208UL, (ushort)value);
            }

            public bool IsGroup
            {
                get => this.ReadDataBool(224UL);
                set => this.WriteData(224UL, value);
            }

            public ushort DiscriminantCount
            {
                get => this.ReadDataUShort(240UL);
                set => this.WriteData(240UL, value);
            }

            public uint DiscriminantOffset
            {
                get => this.ReadDataUInt(256UL);
                set => this.WriteData(256UL, value);
            }

            public ListOfStructsSerializer<Field.WRITER> Fields
            {
                get => BuildPointer<ListOfStructsSerializer<Field.WRITER>>(3);
                set => Link(3, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xb54ab3364333f598UL)]
    public class @enum : ICapnpSerializable
    {
        public const ulong typeId = 0xb54ab3364333f598UL;

        public IReadOnlyList<Enumerant> Enumerants { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Enumerants = reader.Enumerants?.ToReadOnlyList(_ =>
                CapnpSerializable.Create<Enumerant>(_)
            );
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Enumerants.Init(Enumerants, (_s1, _v1) => _v1?.serialize(_s1));
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

            public IReadOnlyList<Enumerant.READER> Enumerants =>
                ctx.ReadList(3).Cast(Enumerant.READER.create);
        }

        public class WRITER : SerializerState
        {
            public ListOfStructsSerializer<Enumerant.WRITER> Enumerants
            {
                get => BuildPointer<ListOfStructsSerializer<Enumerant.WRITER>>(3);
                set => Link(3, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xe82753cff0c2218fUL)]
    public class @interface : ICapnpSerializable
    {
        public const ulong typeId = 0xe82753cff0c2218fUL;

        public IReadOnlyList<Method> Methods { get; set; }

        public IReadOnlyList<Superclass> Superclasses { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Methods = reader.Methods?.ToReadOnlyList(_ => CapnpSerializable.Create<Method>(_));
            Superclasses = reader.Superclasses?.ToReadOnlyList(_ =>
                CapnpSerializable.Create<Superclass>(_)
            );
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Methods.Init(Methods, (_s1, _v1) => _v1?.serialize(_s1));
            writer.Superclasses.Init(Superclasses, (_s1, _v1) => _v1?.serialize(_s1));
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

            public IReadOnlyList<Method.READER> Methods =>
                ctx.ReadList(3).Cast(Method.READER.create);
            public IReadOnlyList<Superclass.READER> Superclasses =>
                ctx.ReadList(4).Cast(Superclass.READER.create);
        }

        public class WRITER : SerializerState
        {
            public ListOfStructsSerializer<Method.WRITER> Methods
            {
                get => BuildPointer<ListOfStructsSerializer<Method.WRITER>>(3);
                set => Link(3, value);
            }

            public ListOfStructsSerializer<Superclass.WRITER> Superclasses
            {
                get => BuildPointer<ListOfStructsSerializer<Superclass.WRITER>>(4);
                set => Link(4, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xb18aa5ac7a0d9420UL)]
    public class @const : ICapnpSerializable
    {
        public const ulong typeId = 0xb18aa5ac7a0d9420UL;

        public Type Type { get; set; }

        public Value Value { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Type = CapnpSerializable.Create<Type>(reader.Type);
            Value = CapnpSerializable.Create<Value>(reader.Value);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            Type?.serialize(writer.Type);
            Value?.serialize(writer.Value);
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

            public Type.READER Type => ctx.ReadStruct(3, Schema.Type.READER.create);
            public Value.READER Value => ctx.ReadStruct(4, Schema.Value.READER.create);
        }

        public class WRITER : SerializerState
        {
            public Type.WRITER Type
            {
                get => BuildPointer<Type.WRITER>(3);
                set => Link(3, value);
            }

            public Value.WRITER Value
            {
                get => BuildPointer<Value.WRITER>(4);
                set => Link(4, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xec1619d4400a0290UL)]
    public class annotation : ICapnpSerializable
    {
        public const ulong typeId = 0xec1619d4400a0290UL;

        public Type Type { get; set; }

        public bool TargetsFile { get; set; }

        public bool TargetsConst { get; set; }

        public bool TargetsEnum { get; set; }

        public bool TargetsEnumerant { get; set; }

        public bool TargetsStruct { get; set; }

        public bool TargetsField { get; set; }

        public bool TargetsUnion { get; set; }

        public bool TargetsGroup { get; set; }

        public bool TargetsInterface { get; set; }

        public bool TargetsMethod { get; set; }

        public bool TargetsParam { get; set; }

        public bool TargetsAnnotation { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Type = CapnpSerializable.Create<Type>(reader.Type);
            TargetsFile = reader.TargetsFile;
            TargetsConst = reader.TargetsConst;
            TargetsEnum = reader.TargetsEnum;
            TargetsEnumerant = reader.TargetsEnumerant;
            TargetsStruct = reader.TargetsStruct;
            TargetsField = reader.TargetsField;
            TargetsUnion = reader.TargetsUnion;
            TargetsGroup = reader.TargetsGroup;
            TargetsInterface = reader.TargetsInterface;
            TargetsMethod = reader.TargetsMethod;
            TargetsParam = reader.TargetsParam;
            TargetsAnnotation = reader.TargetsAnnotation;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            Type?.serialize(writer.Type);
            writer.TargetsFile = TargetsFile;
            writer.TargetsConst = TargetsConst;
            writer.TargetsEnum = TargetsEnum;
            writer.TargetsEnumerant = TargetsEnumerant;
            writer.TargetsStruct = TargetsStruct;
            writer.TargetsField = TargetsField;
            writer.TargetsUnion = TargetsUnion;
            writer.TargetsGroup = TargetsGroup;
            writer.TargetsInterface = TargetsInterface;
            writer.TargetsMethod = TargetsMethod;
            writer.TargetsParam = TargetsParam;
            writer.TargetsAnnotation = TargetsAnnotation;
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

            public Type.READER Type => ctx.ReadStruct(3, Schema.Type.READER.create);
            public bool TargetsFile => ctx.ReadDataBool(112UL);
            public bool TargetsConst => ctx.ReadDataBool(113UL);
            public bool TargetsEnum => ctx.ReadDataBool(114UL);
            public bool TargetsEnumerant => ctx.ReadDataBool(115UL);
            public bool TargetsStruct => ctx.ReadDataBool(116UL);
            public bool TargetsField => ctx.ReadDataBool(117UL);
            public bool TargetsUnion => ctx.ReadDataBool(118UL);
            public bool TargetsGroup => ctx.ReadDataBool(119UL);
            public bool TargetsInterface => ctx.ReadDataBool(120UL);
            public bool TargetsMethod => ctx.ReadDataBool(121UL);
            public bool TargetsParam => ctx.ReadDataBool(122UL);
            public bool TargetsAnnotation => ctx.ReadDataBool(123UL);
        }

        public class WRITER : SerializerState
        {
            public Type.WRITER Type
            {
                get => BuildPointer<Type.WRITER>(3);
                set => Link(3, value);
            }

            public bool TargetsFile
            {
                get => this.ReadDataBool(112UL);
                set => this.WriteData(112UL, value);
            }

            public bool TargetsConst
            {
                get => this.ReadDataBool(113UL);
                set => this.WriteData(113UL, value);
            }

            public bool TargetsEnum
            {
                get => this.ReadDataBool(114UL);
                set => this.WriteData(114UL, value);
            }

            public bool TargetsEnumerant
            {
                get => this.ReadDataBool(115UL);
                set => this.WriteData(115UL, value);
            }

            public bool TargetsStruct
            {
                get => this.ReadDataBool(116UL);
                set => this.WriteData(116UL, value);
            }

            public bool TargetsField
            {
                get => this.ReadDataBool(117UL);
                set => this.WriteData(117UL, value);
            }

            public bool TargetsUnion
            {
                get => this.ReadDataBool(118UL);
                set => this.WriteData(118UL, value);
            }

            public bool TargetsGroup
            {
                get => this.ReadDataBool(119UL);
                set => this.WriteData(119UL, value);
            }

            public bool TargetsInterface
            {
                get => this.ReadDataBool(120UL);
                set => this.WriteData(120UL, value);
            }

            public bool TargetsMethod
            {
                get => this.ReadDataBool(121UL);
                set => this.WriteData(121UL, value);
            }

            public bool TargetsParam
            {
                get => this.ReadDataBool(122UL);
                set => this.WriteData(122UL, value);
            }

            public bool TargetsAnnotation
            {
                get => this.ReadDataBool(123UL);
                set => this.WriteData(123UL, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xb9521bccf10fa3b1UL)]
    public class Parameter : ICapnpSerializable
    {
        public const ulong typeId = 0xb9521bccf10fa3b1UL;

        public string Name { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Name = reader.Name;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Name = Name;
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

            public string Name => ctx.ReadText(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(0, 1);
            }

            public string Name
            {
                get => ReadText(0);
                set => WriteText(0, value, null);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xdebf55bbfa0fc242UL)]
    public class NestedNode : ICapnpSerializable
    {
        public const ulong typeId = 0xdebf55bbfa0fc242UL;

        public string Name { get; set; }

        public ulong Id { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Name = reader.Name;
            Id = reader.Id;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Name = Name;
            writer.Id = Id;
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

            public string Name => ctx.ReadText(0);
            public ulong Id => ctx.ReadDataULong(0UL);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(1, 1);
            }

            public string Name
            {
                get => ReadText(0);
                set => WriteText(0, value, null);
            }

            public ulong Id
            {
                get => this.ReadDataULong(0UL);
                set => this.WriteData(0UL, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xf38e1de3041357aeUL)]
    public class SourceInfo : ICapnpSerializable
    {
        public const ulong typeId = 0xf38e1de3041357aeUL;

        public ulong Id { get; set; }

        public string DocComment { get; set; }

        public IReadOnlyList<Member> Members { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = reader.Id;
            DocComment = reader.DocComment;
            Members = reader.Members?.ToReadOnlyList(_ => CapnpSerializable.Create<Member>(_));
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Id = Id;
            writer.DocComment = DocComment;
            writer.Members.Init(Members, (_s1, _v1) => _v1?.serialize(_s1));
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

            public ulong Id => ctx.ReadDataULong(0UL);
            public string DocComment => ctx.ReadText(0);
            public IReadOnlyList<Member.READER> Members =>
                ctx.ReadList(1).Cast(Member.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(1, 2);
            }

            public ulong Id
            {
                get => this.ReadDataULong(0UL);
                set => this.WriteData(0UL, value);
            }

            public string DocComment
            {
                get => ReadText(0);
                set => WriteText(0, value, null);
            }

            public ListOfStructsSerializer<Member.WRITER> Members
            {
                get => BuildPointer<ListOfStructsSerializer<Member.WRITER>>(1);
                set => Link(1, value);
            }
        }

        [GeneratedCode("capnpc-csharp", "1.3.0.0")]
        [TypeId(0xc2ba9038898e1fa2UL)]
        public class Member : ICapnpSerializable
        {
            public const ulong typeId = 0xc2ba9038898e1fa2UL;

            public string DocComment { get; set; }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                DocComment = reader.DocComment;
                applyDefaults();
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void serialize(WRITER writer)
            {
                writer.DocComment = DocComment;
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

                public string DocComment => ctx.ReadText(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    SetStruct(0, 1);
                }

                public string DocComment
                {
                    get => ReadText(0);
                    set => WriteText(0, value, null);
                }
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x9aad50a41f4af45fUL)]
public class Field : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        Slot = 0,
        Group = 1,
        undefined = 65535,
    }

    public const ulong typeId = 0x9aad50a41f4af45fUL;
    private object _content;

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
                case WHICH.Slot:
                    _content = null;
                    break;
                case WHICH.Group:
                    _content = null;
                    break;
            }
        }
    }

    public string Name { get; set; }

    public ushort CodeOrder { get; set; }

    public IReadOnlyList<Annotation> Annotations { get; set; }

    public ushort DiscriminantValue { get; set; } = 65535;

    public slot Slot
    {
        get => _which == WHICH.Slot ? (slot)_content : null;
        set
        {
            _which = WHICH.Slot;
            _content = value;
        }
    }

    public group Group
    {
        get => _which == WHICH.Group ? (group)_content : null;
        set
        {
            _which = WHICH.Group;
            _content = value;
        }
    }

    public ordinal Ordinal { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.Slot:
                Slot = CapnpSerializable.Create<slot>(reader.Slot);
                break;
            case WHICH.Group:
                Group = CapnpSerializable.Create<group>(reader.Group);
                break;
        }

        Name = reader.Name;
        CodeOrder = reader.CodeOrder;
        Annotations = reader.Annotations?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<Annotation>(_)
        );
        DiscriminantValue = reader.DiscriminantValue;
        Ordinal = CapnpSerializable.Create<ordinal>(reader.Ordinal);
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
            case WHICH.Slot:
                Slot?.serialize(writer.Slot);
                break;
            case WHICH.Group:
                Group?.serialize(writer.Group);
                break;
        }

        writer.Name = Name;
        writer.CodeOrder = CodeOrder;
        writer.Annotations.Init(Annotations, (_s1, _v1) => _v1?.serialize(_s1));
        writer.DiscriminantValue = DiscriminantValue;
        Ordinal?.serialize(writer.Ordinal);
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(64U);
        public string Name => ctx.ReadText(0);
        public ushort CodeOrder => ctx.ReadDataUShort(0UL);
        public IReadOnlyList<Annotation.READER> Annotations =>
            ctx.ReadList(1).Cast(Annotation.READER.create);
        public ushort DiscriminantValue => ctx.ReadDataUShort(16UL, 65535);
        public slot.READER Slot => which == WHICH.Slot ? new slot.READER(ctx) : default;
        public group.READER Group => which == WHICH.Group ? new group.READER(ctx) : default;
        public ordinal.READER Ordinal => new(ctx);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(3, 4);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(64U);
            set => this.WriteData(64U, (ushort)value);
        }

        public string Name
        {
            get => ReadText(0);
            set => WriteText(0, value, null);
        }

        public ushort CodeOrder
        {
            get => this.ReadDataUShort(0UL);
            set => this.WriteData(0UL, value);
        }

        public ListOfStructsSerializer<Annotation.WRITER> Annotations
        {
            get => BuildPointer<ListOfStructsSerializer<Annotation.WRITER>>(1);
            set => Link(1, value);
        }

        public ushort DiscriminantValue
        {
            get => this.ReadDataUShort(16UL, 65535);
            set => this.WriteData(16UL, value, (ushort)65535);
        }

        public slot.WRITER Slot => which == WHICH.Slot ? Rewrap<slot.WRITER>() : default;

        public group.WRITER Group => which == WHICH.Group ? Rewrap<group.WRITER>() : default;

        public ordinal.WRITER Ordinal => Rewrap<ordinal.WRITER>();
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xc42305476bb4746fUL)]
    public class slot : ICapnpSerializable
    {
        public const ulong typeId = 0xc42305476bb4746fUL;

        public uint Offset { get; set; }

        public Type Type { get; set; }

        public Value DefaultValue { get; set; }

        public bool HadExplicitDefault { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Offset = reader.Offset;
            Type = CapnpSerializable.Create<Type>(reader.Type);
            DefaultValue = CapnpSerializable.Create<Value>(reader.DefaultValue);
            HadExplicitDefault = reader.HadExplicitDefault;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Offset = Offset;
            Type?.serialize(writer.Type);
            DefaultValue?.serialize(writer.DefaultValue);
            writer.HadExplicitDefault = HadExplicitDefault;
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

            public uint Offset => ctx.ReadDataUInt(32UL);
            public Type.READER Type => ctx.ReadStruct(2, Schema.Type.READER.create);
            public Value.READER DefaultValue => ctx.ReadStruct(3, Value.READER.create);
            public bool HadExplicitDefault => ctx.ReadDataBool(128UL);
        }

        public class WRITER : SerializerState
        {
            public uint Offset
            {
                get => this.ReadDataUInt(32UL);
                set => this.WriteData(32UL, value);
            }

            public Type.WRITER Type
            {
                get => BuildPointer<Type.WRITER>(2);
                set => Link(2, value);
            }

            public Value.WRITER DefaultValue
            {
                get => BuildPointer<Value.WRITER>(3);
                set => Link(3, value);
            }

            public bool HadExplicitDefault
            {
                get => this.ReadDataBool(128UL);
                set => this.WriteData(128UL, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xcafccddb68db1d11UL)]
    public class group : ICapnpSerializable
    {
        public const ulong typeId = 0xcafccddb68db1d11UL;

        public ulong TypeId { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TypeId = reader.TypeId;
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.TypeId = TypeId;
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

            public ulong TypeId => ctx.ReadDataULong(128UL);
        }

        public class WRITER : SerializerState
        {
            public ulong TypeId
            {
                get => this.ReadDataULong(128UL);
                set => this.WriteData(128UL, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xbb90d5c287870be6UL)]
    public class ordinal : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            Implicit = 0,
            Explicit = 1,
            undefined = 65535,
        }

        public const ulong typeId = 0xbb90d5c287870be6UL;
        private object _content;

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
                    case WHICH.Implicit:
                        break;
                    case WHICH.Explicit:
                        _content = 0;
                        break;
                }
            }
        }

        public ushort? Explicit
        {
            get => _which == WHICH.Explicit ? (ushort?)_content : null;
            set
            {
                _which = WHICH.Explicit;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Implicit:
                    which = reader.which;
                    break;
                case WHICH.Explicit:
                    Explicit = reader.Explicit;
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
                case WHICH.Implicit:
                    break;
                case WHICH.Explicit:
                    writer.Explicit = Explicit.Value;
                    break;
            }
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(80U);
            public ushort Explicit => which == WHICH.Explicit ? ctx.ReadDataUShort(96UL) : default;
        }

        public class WRITER : SerializerState
        {
            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(80U);
                set => this.WriteData(80U, (ushort)value);
            }

            public ushort Explicit
            {
                get => which == WHICH.Explicit ? this.ReadDataUShort(96UL) : default;
                set => this.WriteData(96UL, value);
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x978a7cebdc549a4dUL)]
public class Enumerant : ICapnpSerializable
{
    public const ulong typeId = 0x978a7cebdc549a4dUL;

    public string Name { get; set; }

    public ushort CodeOrder { get; set; }

    public IReadOnlyList<Annotation> Annotations { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Name = reader.Name;
        CodeOrder = reader.CodeOrder;
        Annotations = reader.Annotations?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<Annotation>(_)
        );
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Name = Name;
        writer.CodeOrder = CodeOrder;
        writer.Annotations.Init(Annotations, (_s1, _v1) => _v1?.serialize(_s1));
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

        public string Name => ctx.ReadText(0);
        public ushort CodeOrder => ctx.ReadDataUShort(0UL);
        public IReadOnlyList<Annotation.READER> Annotations =>
            ctx.ReadList(1).Cast(Annotation.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 2);
        }

        public string Name
        {
            get => ReadText(0);
            set => WriteText(0, value, null);
        }

        public ushort CodeOrder
        {
            get => this.ReadDataUShort(0UL);
            set => this.WriteData(0UL, value);
        }

        public ListOfStructsSerializer<Annotation.WRITER> Annotations
        {
            get => BuildPointer<ListOfStructsSerializer<Annotation.WRITER>>(1);
            set => Link(1, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xa9962a9ed0a4d7f8UL)]
public class Superclass : ICapnpSerializable
{
    public const ulong typeId = 0xa9962a9ed0a4d7f8UL;

    public ulong Id { get; set; }

    public Brand Brand { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Id = reader.Id;
        Brand = CapnpSerializable.Create<Brand>(reader.Brand);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Id = Id;
        Brand?.serialize(writer.Brand);
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

        public ulong Id => ctx.ReadDataULong(0UL);
        public Brand.READER Brand => ctx.ReadStruct(0, Schema.Brand.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 1);
        }

        public ulong Id
        {
            get => this.ReadDataULong(0UL);
            set => this.WriteData(0UL, value);
        }

        public Brand.WRITER Brand
        {
            get => BuildPointer<Brand.WRITER>(0);
            set => Link(0, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x9500cce23b334d80UL)]
public class Method : ICapnpSerializable
{
    public const ulong typeId = 0x9500cce23b334d80UL;

    public string Name { get; set; }

    public ushort CodeOrder { get; set; }

    public ulong ParamStructType { get; set; }

    public ulong ResultStructType { get; set; }

    public IReadOnlyList<Annotation> Annotations { get; set; }

    public Brand ParamBrand { get; set; }

    public Brand ResultBrand { get; set; }

    public IReadOnlyList<Node.Parameter> ImplicitParameters { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Name = reader.Name;
        CodeOrder = reader.CodeOrder;
        ParamStructType = reader.ParamStructType;
        ResultStructType = reader.ResultStructType;
        Annotations = reader.Annotations?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<Annotation>(_)
        );
        ParamBrand = CapnpSerializable.Create<Brand>(reader.ParamBrand);
        ResultBrand = CapnpSerializable.Create<Brand>(reader.ResultBrand);
        ImplicitParameters = reader.ImplicitParameters?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<Node.Parameter>(_)
        );
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Name = Name;
        writer.CodeOrder = CodeOrder;
        writer.ParamStructType = ParamStructType;
        writer.ResultStructType = ResultStructType;
        writer.Annotations.Init(Annotations, (_s1, _v1) => _v1?.serialize(_s1));
        ParamBrand?.serialize(writer.ParamBrand);
        ResultBrand?.serialize(writer.ResultBrand);
        writer.ImplicitParameters.Init(ImplicitParameters, (_s1, _v1) => _v1?.serialize(_s1));
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

        public string Name => ctx.ReadText(0);
        public ushort CodeOrder => ctx.ReadDataUShort(0UL);
        public ulong ParamStructType => ctx.ReadDataULong(64UL);
        public ulong ResultStructType => ctx.ReadDataULong(128UL);
        public IReadOnlyList<Annotation.READER> Annotations =>
            ctx.ReadList(1).Cast(Annotation.READER.create);
        public Brand.READER ParamBrand => ctx.ReadStruct(2, Brand.READER.create);
        public Brand.READER ResultBrand => ctx.ReadStruct(3, Brand.READER.create);

        public IReadOnlyList<Node.Parameter.READER> ImplicitParameters =>
            ctx.ReadList(4).Cast(Node.Parameter.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(3, 5);
        }

        public string Name
        {
            get => ReadText(0);
            set => WriteText(0, value, null);
        }

        public ushort CodeOrder
        {
            get => this.ReadDataUShort(0UL);
            set => this.WriteData(0UL, value);
        }

        public ulong ParamStructType
        {
            get => this.ReadDataULong(64UL);
            set => this.WriteData(64UL, value);
        }

        public ulong ResultStructType
        {
            get => this.ReadDataULong(128UL);
            set => this.WriteData(128UL, value);
        }

        public ListOfStructsSerializer<Annotation.WRITER> Annotations
        {
            get => BuildPointer<ListOfStructsSerializer<Annotation.WRITER>>(1);
            set => Link(1, value);
        }

        public Brand.WRITER ParamBrand
        {
            get => BuildPointer<Brand.WRITER>(2);
            set => Link(2, value);
        }

        public Brand.WRITER ResultBrand
        {
            get => BuildPointer<Brand.WRITER>(3);
            set => Link(3, value);
        }

        public ListOfStructsSerializer<Node.Parameter.WRITER> ImplicitParameters
        {
            get => BuildPointer<ListOfStructsSerializer<Node.Parameter.WRITER>>(4);
            set => Link(4, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd07378ede1f9cc60UL)]
public class Type : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        Void = 0,
        Bool = 1,
        Int8 = 2,
        Int16 = 3,
        Int32 = 4,
        Int64 = 5,
        Uint8 = 6,
        Uint16 = 7,
        Uint32 = 8,
        Uint64 = 9,
        Float32 = 10,
        Float64 = 11,
        Text = 12,
        Data = 13,
        List = 14,
        Enum = 15,
        Struct = 16,
        Interface = 17,
        AnyPointer = 18,
        undefined = 65535,
    }

    public const ulong typeId = 0xd07378ede1f9cc60UL;
    private object _content;

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
                case WHICH.Void:
                    break;
                case WHICH.Bool:
                    break;
                case WHICH.Int8:
                    break;
                case WHICH.Int16:
                    break;
                case WHICH.Int32:
                    break;
                case WHICH.Int64:
                    break;
                case WHICH.Uint8:
                    break;
                case WHICH.Uint16:
                    break;
                case WHICH.Uint32:
                    break;
                case WHICH.Uint64:
                    break;
                case WHICH.Float32:
                    break;
                case WHICH.Float64:
                    break;
                case WHICH.Text:
                    break;
                case WHICH.Data:
                    break;
                case WHICH.List:
                    _content = null;
                    break;
                case WHICH.Enum:
                    _content = null;
                    break;
                case WHICH.Struct:
                    _content = null;
                    break;
                case WHICH.Interface:
                    _content = null;
                    break;
                case WHICH.AnyPointer:
                    _content = null;
                    break;
            }
        }
    }

    public list List
    {
        get => _which == WHICH.List ? (list)_content : null;
        set
        {
            _which = WHICH.List;
            _content = value;
        }
    }

    public @enum Enum
    {
        get => _which == WHICH.Enum ? (@enum)_content : null;
        set
        {
            _which = WHICH.Enum;
            _content = value;
        }
    }

    public @struct Struct
    {
        get => _which == WHICH.Struct ? (@struct)_content : null;
        set
        {
            _which = WHICH.Struct;
            _content = value;
        }
    }

    public @interface Interface
    {
        get => _which == WHICH.Interface ? (@interface)_content : null;
        set
        {
            _which = WHICH.Interface;
            _content = value;
        }
    }

    public anyPointer AnyPointer
    {
        get => _which == WHICH.AnyPointer ? (anyPointer)_content : null;
        set
        {
            _which = WHICH.AnyPointer;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.Void:
                which = reader.which;
                break;
            case WHICH.Bool:
                which = reader.which;
                break;
            case WHICH.Int8:
                which = reader.which;
                break;
            case WHICH.Int16:
                which = reader.which;
                break;
            case WHICH.Int32:
                which = reader.which;
                break;
            case WHICH.Int64:
                which = reader.which;
                break;
            case WHICH.Uint8:
                which = reader.which;
                break;
            case WHICH.Uint16:
                which = reader.which;
                break;
            case WHICH.Uint32:
                which = reader.which;
                break;
            case WHICH.Uint64:
                which = reader.which;
                break;
            case WHICH.Float32:
                which = reader.which;
                break;
            case WHICH.Float64:
                which = reader.which;
                break;
            case WHICH.Text:
                which = reader.which;
                break;
            case WHICH.Data:
                which = reader.which;
                break;
            case WHICH.List:
                List = CapnpSerializable.Create<list>(reader.List);
                break;
            case WHICH.Enum:
                Enum = CapnpSerializable.Create<@enum>(reader.Enum);
                break;
            case WHICH.Struct:
                Struct = CapnpSerializable.Create<@struct>(reader.Struct);
                break;
            case WHICH.Interface:
                Interface = CapnpSerializable.Create<@interface>(reader.Interface);
                break;
            case WHICH.AnyPointer:
                AnyPointer = CapnpSerializable.Create<anyPointer>(reader.AnyPointer);
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
            case WHICH.Void:
                break;
            case WHICH.Bool:
                break;
            case WHICH.Int8:
                break;
            case WHICH.Int16:
                break;
            case WHICH.Int32:
                break;
            case WHICH.Int64:
                break;
            case WHICH.Uint8:
                break;
            case WHICH.Uint16:
                break;
            case WHICH.Uint32:
                break;
            case WHICH.Uint64:
                break;
            case WHICH.Float32:
                break;
            case WHICH.Float64:
                break;
            case WHICH.Text:
                break;
            case WHICH.Data:
                break;
            case WHICH.List:
                List?.serialize(writer.List);
                break;
            case WHICH.Enum:
                Enum?.serialize(writer.Enum);
                break;
            case WHICH.Struct:
                Struct?.serialize(writer.Struct);
                break;
            case WHICH.Interface:
                Interface?.serialize(writer.Interface);
                break;
            case WHICH.AnyPointer:
                AnyPointer?.serialize(writer.AnyPointer);
                break;
        }
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(0U);
        public list.READER List => which == WHICH.List ? new list.READER(ctx) : default;
        public @enum.READER Enum => which == WHICH.Enum ? new @enum.READER(ctx) : default;
        public @struct.READER Struct => which == WHICH.Struct ? new @struct.READER(ctx) : default;
        public @interface.READER Interface =>
            which == WHICH.Interface ? new @interface.READER(ctx) : default;
        public anyPointer.READER AnyPointer =>
            which == WHICH.AnyPointer ? new anyPointer.READER(ctx) : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(3, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(0U);
            set => this.WriteData(0U, (ushort)value);
        }

        public list.WRITER List => which == WHICH.List ? Rewrap<list.WRITER>() : default;

        public @enum.WRITER Enum => which == WHICH.Enum ? Rewrap<@enum.WRITER>() : default;

        public @struct.WRITER Struct => which == WHICH.Struct ? Rewrap<@struct.WRITER>() : default;

        public @interface.WRITER Interface =>
            which == WHICH.Interface ? Rewrap<@interface.WRITER>() : default;

        public anyPointer.WRITER AnyPointer =>
            which == WHICH.AnyPointer ? Rewrap<anyPointer.WRITER>() : default;
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0x87e739250a60ea97UL)]
    public class list : ICapnpSerializable
    {
        public const ulong typeId = 0x87e739250a60ea97UL;

        public Type ElementType { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            ElementType = CapnpSerializable.Create<Type>(reader.ElementType);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            ElementType?.serialize(writer.ElementType);
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

            public Type.READER ElementType => ctx.ReadStruct(0, Type.READER.create);
        }

        public class WRITER : SerializerState
        {
            public Type.WRITER ElementType
            {
                get => BuildPointer<Type.WRITER>(0);
                set => Link(0, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0x9e0e78711a7f87a9UL)]
    public class @enum : ICapnpSerializable
    {
        public const ulong typeId = 0x9e0e78711a7f87a9UL;

        public ulong TypeId { get; set; }

        public Brand Brand { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TypeId = reader.TypeId;
            Brand = CapnpSerializable.Create<Brand>(reader.Brand);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.TypeId = TypeId;
            Brand?.serialize(writer.Brand);
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

            public ulong TypeId => ctx.ReadDataULong(64UL);
            public Brand.READER Brand => ctx.ReadStruct(0, Schema.Brand.READER.create);
        }

        public class WRITER : SerializerState
        {
            public ulong TypeId
            {
                get => this.ReadDataULong(64UL);
                set => this.WriteData(64UL, value);
            }

            public Brand.WRITER Brand
            {
                get => BuildPointer<Brand.WRITER>(0);
                set => Link(0, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xac3a6f60ef4cc6d3UL)]
    public class @struct : ICapnpSerializable
    {
        public const ulong typeId = 0xac3a6f60ef4cc6d3UL;

        public ulong TypeId { get; set; }

        public Brand Brand { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TypeId = reader.TypeId;
            Brand = CapnpSerializable.Create<Brand>(reader.Brand);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.TypeId = TypeId;
            Brand?.serialize(writer.Brand);
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

            public ulong TypeId => ctx.ReadDataULong(64UL);
            public Brand.READER Brand => ctx.ReadStruct(0, Schema.Brand.READER.create);
        }

        public class WRITER : SerializerState
        {
            public ulong TypeId
            {
                get => this.ReadDataULong(64UL);
                set => this.WriteData(64UL, value);
            }

            public Brand.WRITER Brand
            {
                get => BuildPointer<Brand.WRITER>(0);
                set => Link(0, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xed8bca69f7fb0cbfUL)]
    public class @interface : ICapnpSerializable
    {
        public const ulong typeId = 0xed8bca69f7fb0cbfUL;

        public ulong TypeId { get; set; }

        public Brand Brand { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TypeId = reader.TypeId;
            Brand = CapnpSerializable.Create<Brand>(reader.Brand);
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.TypeId = TypeId;
            Brand?.serialize(writer.Brand);
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

            public ulong TypeId => ctx.ReadDataULong(64UL);
            public Brand.READER Brand => ctx.ReadStruct(0, Schema.Brand.READER.create);
        }

        public class WRITER : SerializerState
        {
            public ulong TypeId
            {
                get => this.ReadDataULong(64UL);
                set => this.WriteData(64UL, value);
            }

            public Brand.WRITER Brand
            {
                get => BuildPointer<Brand.WRITER>(0);
                set => Link(0, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xc2573fe8a23e49f1UL)]
    public class anyPointer : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            Unconstrained = 0,
            Parameter = 1,
            ImplicitMethodParameter = 2,
            undefined = 65535,
        }

        public const ulong typeId = 0xc2573fe8a23e49f1UL;
        private object _content;

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
                    case WHICH.Unconstrained:
                        _content = null;
                        break;
                    case WHICH.Parameter:
                        _content = null;
                        break;
                    case WHICH.ImplicitMethodParameter:
                        _content = null;
                        break;
                }
            }
        }

        public unconstrained Unconstrained
        {
            get => _which == WHICH.Unconstrained ? (unconstrained)_content : null;
            set
            {
                _which = WHICH.Unconstrained;
                _content = value;
            }
        }

        public parameter Parameter
        {
            get => _which == WHICH.Parameter ? (parameter)_content : null;
            set
            {
                _which = WHICH.Parameter;
                _content = value;
            }
        }

        public implicitMethodParameter ImplicitMethodParameter
        {
            get =>
                _which == WHICH.ImplicitMethodParameter ? (implicitMethodParameter)_content : null;
            set
            {
                _which = WHICH.ImplicitMethodParameter;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Unconstrained:
                    Unconstrained = CapnpSerializable.Create<unconstrained>(reader.Unconstrained);
                    break;
                case WHICH.Parameter:
                    Parameter = CapnpSerializable.Create<parameter>(reader.Parameter);
                    break;
                case WHICH.ImplicitMethodParameter:
                    ImplicitMethodParameter = CapnpSerializable.Create<implicitMethodParameter>(
                        reader.ImplicitMethodParameter
                    );
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
                case WHICH.Unconstrained:
                    Unconstrained?.serialize(writer.Unconstrained);
                    break;
                case WHICH.Parameter:
                    Parameter?.serialize(writer.Parameter);
                    break;
                case WHICH.ImplicitMethodParameter:
                    ImplicitMethodParameter?.serialize(writer.ImplicitMethodParameter);
                    break;
            }
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(64U);

            public unconstrained.READER Unconstrained =>
                which == WHICH.Unconstrained ? new unconstrained.READER(ctx) : default;

            public parameter.READER Parameter =>
                which == WHICH.Parameter ? new parameter.READER(ctx) : default;

            public implicitMethodParameter.READER ImplicitMethodParameter =>
                which == WHICH.ImplicitMethodParameter
                    ? new implicitMethodParameter.READER(ctx)
                    : default;
        }

        public class WRITER : SerializerState
        {
            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(64U);
                set => this.WriteData(64U, (ushort)value);
            }

            public unconstrained.WRITER Unconstrained =>
                which == WHICH.Unconstrained ? Rewrap<unconstrained.WRITER>() : default;

            public parameter.WRITER Parameter =>
                which == WHICH.Parameter ? Rewrap<parameter.WRITER>() : default;

            public implicitMethodParameter.WRITER ImplicitMethodParameter =>
                which == WHICH.ImplicitMethodParameter
                    ? Rewrap<implicitMethodParameter.WRITER>()
                    : default;
        }

        [GeneratedCode("capnpc-csharp", "1.3.0.0")]
        [TypeId(0x8e3b5f79fe593656UL)]
        public class unconstrained : ICapnpSerializable
        {
            public enum WHICH : ushort
            {
                AnyKind = 0,
                Struct = 1,
                List = 2,
                Capability = 3,
                undefined = 65535,
            }

            public const ulong typeId = 0x8e3b5f79fe593656UL;

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
                        case WHICH.AnyKind:
                            break;
                        case WHICH.Struct:
                            break;
                        case WHICH.List:
                            break;
                        case WHICH.Capability:
                            break;
                    }
                }
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.AnyKind:
                        which = reader.which;
                        break;
                    case WHICH.Struct:
                        which = reader.which;
                        break;
                    case WHICH.List:
                        which = reader.which;
                        break;
                    case WHICH.Capability:
                        which = reader.which;
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
                    case WHICH.AnyKind:
                        break;
                    case WHICH.Struct:
                        break;
                    case WHICH.List:
                        break;
                    case WHICH.Capability:
                        break;
                }
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

                public WHICH which => (WHICH)ctx.ReadDataUShort(80U);
            }

            public class WRITER : SerializerState
            {
                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(80U);
                    set => this.WriteData(80U, (ushort)value);
                }
            }
        }

        [GeneratedCode("capnpc-csharp", "1.3.0.0")]
        [TypeId(0x9dd1f724f4614a85UL)]
        public class parameter : ICapnpSerializable
        {
            public const ulong typeId = 0x9dd1f724f4614a85UL;

            public ulong ScopeId { get; set; }

            public ushort ParameterIndex { get; set; }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                ScopeId = reader.ScopeId;
                ParameterIndex = reader.ParameterIndex;
                applyDefaults();
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void serialize(WRITER writer)
            {
                writer.ScopeId = ScopeId;
                writer.ParameterIndex = ParameterIndex;
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

                public ulong ScopeId => ctx.ReadDataULong(128UL);
                public ushort ParameterIndex => ctx.ReadDataUShort(80UL);
            }

            public class WRITER : SerializerState
            {
                public ulong ScopeId
                {
                    get => this.ReadDataULong(128UL);
                    set => this.WriteData(128UL, value);
                }

                public ushort ParameterIndex
                {
                    get => this.ReadDataUShort(80UL);
                    set => this.WriteData(80UL, value);
                }
            }
        }

        [GeneratedCode("capnpc-csharp", "1.3.0.0")]
        [TypeId(0xbaefc9120c56e274UL)]
        public class implicitMethodParameter : ICapnpSerializable
        {
            public const ulong typeId = 0xbaefc9120c56e274UL;

            public ushort ParameterIndex { get; set; }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                ParameterIndex = reader.ParameterIndex;
                applyDefaults();
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void serialize(WRITER writer)
            {
                writer.ParameterIndex = ParameterIndex;
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

                public ushort ParameterIndex => ctx.ReadDataUShort(80UL);
            }

            public class WRITER : SerializerState
            {
                public ushort ParameterIndex
                {
                    get => this.ReadDataUShort(80UL);
                    set => this.WriteData(80UL, value);
                }
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0x903455f06065422bUL)]
public class Brand : ICapnpSerializable
{
    public const ulong typeId = 0x903455f06065422bUL;

    public IReadOnlyList<Scope> Scopes { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Scopes = reader.Scopes?.ToReadOnlyList(_ => CapnpSerializable.Create<Scope>(_));
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Scopes.Init(Scopes, (_s1, _v1) => _v1?.serialize(_s1));
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

        public IReadOnlyList<Scope.READER> Scopes => ctx.ReadList(0).Cast(Scope.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(0, 1);
        }

        public ListOfStructsSerializer<Scope.WRITER> Scopes
        {
            get => BuildPointer<ListOfStructsSerializer<Scope.WRITER>>(0);
            set => Link(0, value);
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xabd73485a9636bc9UL)]
    public class Scope : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            Bind = 0,
            Inherit = 1,
            undefined = 65535,
        }

        public const ulong typeId = 0xabd73485a9636bc9UL;
        private object _content;

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
                    case WHICH.Bind:
                        _content = null;
                        break;
                    case WHICH.Inherit:
                        break;
                }
            }
        }

        public ulong ScopeId { get; set; }

        public IReadOnlyList<Binding> Bind
        {
            get => _which == WHICH.Bind ? (IReadOnlyList<Binding>)_content : null;
            set
            {
                _which = WHICH.Bind;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Bind:
                    Bind = reader.Bind?.ToReadOnlyList(_ => CapnpSerializable.Create<Binding>(_));
                    break;
                case WHICH.Inherit:
                    which = reader.which;
                    break;
            }

            ScopeId = reader.ScopeId;
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
                case WHICH.Bind:
                    writer.Bind.Init(Bind, (_s1, _v1) => _v1?.serialize(_s1));
                    break;
                case WHICH.Inherit:
                    break;
            }

            writer.ScopeId = ScopeId;
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(64U);
            public ulong ScopeId => ctx.ReadDataULong(0UL);

            public IReadOnlyList<Binding.READER> Bind =>
                which == WHICH.Bind ? ctx.ReadList(0).Cast(Binding.READER.create) : default;
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(2, 1);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(64U);
                set => this.WriteData(64U, (ushort)value);
            }

            public ulong ScopeId
            {
                get => this.ReadDataULong(0UL);
                set => this.WriteData(0UL, value);
            }

            public ListOfStructsSerializer<Binding.WRITER> Bind
            {
                get =>
                    which == WHICH.Bind
                        ? BuildPointer<ListOfStructsSerializer<Binding.WRITER>>(0)
                        : default;
                set => Link(0, value);
            }
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xc863cd16969ee7fcUL)]
    public class Binding : ICapnpSerializable
    {
        public enum WHICH : ushort
        {
            Unbound = 0,
            Type = 1,
            undefined = 65535,
        }

        public const ulong typeId = 0xc863cd16969ee7fcUL;
        private object _content;

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
                    case WHICH.Unbound:
                        break;
                    case WHICH.Type:
                        _content = null;
                        break;
                }
            }
        }

        public Type Type
        {
            get => _which == WHICH.Type ? (Type)_content : null;
            set
            {
                _which = WHICH.Type;
                _content = value;
            }
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Unbound:
                    which = reader.which;
                    break;
                case WHICH.Type:
                    Type = CapnpSerializable.Create<Type>(reader.Type);
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
                case WHICH.Unbound:
                    break;
                case WHICH.Type:
                    Type?.serialize(writer.Type);
                    break;
            }
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

            public WHICH which => (WHICH)ctx.ReadDataUShort(0U);
            public Type.READER Type =>
                which == WHICH.Type ? ctx.ReadStruct(0, Schema.Type.READER.create) : default;
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

            public Type.WRITER Type
            {
                get => which == WHICH.Type ? BuildPointer<Type.WRITER>(0) : default;
                set => Link(0, value);
            }
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xce23dcd2d7b00c9bUL)]
public class Value : ICapnpSerializable
{
    public enum WHICH : ushort
    {
        Void = 0,
        Bool = 1,
        Int8 = 2,
        Int16 = 3,
        Int32 = 4,
        Int64 = 5,
        Uint8 = 6,
        Uint16 = 7,
        Uint32 = 8,
        Uint64 = 9,
        Float32 = 10,
        Float64 = 11,
        Text = 12,
        Data = 13,
        List = 14,
        Enum = 15,
        Struct = 16,
        Interface = 17,
        AnyPointer = 18,
        undefined = 65535,
    }

    public const ulong typeId = 0xce23dcd2d7b00c9bUL;
    private object _content;

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
                case WHICH.Void:
                    break;
                case WHICH.Bool:
                    _content = false;
                    break;
                case WHICH.Int8:
                    _content = 0;
                    break;
                case WHICH.Int16:
                    _content = 0;
                    break;
                case WHICH.Int32:
                    _content = 0;
                    break;
                case WHICH.Int64:
                    _content = 0;
                    break;
                case WHICH.Uint8:
                    _content = 0;
                    break;
                case WHICH.Uint16:
                    _content = 0;
                    break;
                case WHICH.Uint32:
                    _content = 0;
                    break;
                case WHICH.Uint64:
                    _content = 0;
                    break;
                case WHICH.Float32:
                    _content = 0F;
                    break;
                case WHICH.Float64:
                    _content = 0;
                    break;
                case WHICH.Text:
                    _content = null;
                    break;
                case WHICH.Data:
                    _content = null;
                    break;
                case WHICH.List:
                    _content = null;
                    break;
                case WHICH.Enum:
                    _content = 0;
                    break;
                case WHICH.Struct:
                    _content = null;
                    break;
                case WHICH.Interface:
                    break;
                case WHICH.AnyPointer:
                    _content = null;
                    break;
            }
        }
    }

    public bool? Bool
    {
        get => _which == WHICH.Bool ? (bool?)_content : null;
        set
        {
            _which = WHICH.Bool;
            _content = value;
        }
    }

    public sbyte? Int8
    {
        get => _which == WHICH.Int8 ? (sbyte?)_content : null;
        set
        {
            _which = WHICH.Int8;
            _content = value;
        }
    }

    public short? Int16
    {
        get => _which == WHICH.Int16 ? (short?)_content : null;
        set
        {
            _which = WHICH.Int16;
            _content = value;
        }
    }

    public int? Int32
    {
        get => _which == WHICH.Int32 ? (int?)_content : null;
        set
        {
            _which = WHICH.Int32;
            _content = value;
        }
    }

    public long? Int64
    {
        get => _which == WHICH.Int64 ? (long?)_content : null;
        set
        {
            _which = WHICH.Int64;
            _content = value;
        }
    }

    public byte? Uint8
    {
        get => _which == WHICH.Uint8 ? (byte?)_content : null;
        set
        {
            _which = WHICH.Uint8;
            _content = value;
        }
    }

    public ushort? Uint16
    {
        get => _which == WHICH.Uint16 ? (ushort?)_content : null;
        set
        {
            _which = WHICH.Uint16;
            _content = value;
        }
    }

    public uint? Uint32
    {
        get => _which == WHICH.Uint32 ? (uint?)_content : null;
        set
        {
            _which = WHICH.Uint32;
            _content = value;
        }
    }

    public ulong? Uint64
    {
        get => _which == WHICH.Uint64 ? (ulong?)_content : null;
        set
        {
            _which = WHICH.Uint64;
            _content = value;
        }
    }

    public float? Float32
    {
        get => _which == WHICH.Float32 ? (float?)_content : null;
        set
        {
            _which = WHICH.Float32;
            _content = value;
        }
    }

    public double? Float64
    {
        get => _which == WHICH.Float64 ? (double?)_content : null;
        set
        {
            _which = WHICH.Float64;
            _content = value;
        }
    }

    public string Text
    {
        get => _which == WHICH.Text ? (string)_content : null;
        set
        {
            _which = WHICH.Text;
            _content = value;
        }
    }

    public IReadOnlyList<byte> Data
    {
        get => _which == WHICH.Data ? (IReadOnlyList<byte>)_content : null;
        set
        {
            _which = WHICH.Data;
            _content = value;
        }
    }

    public object List
    {
        get => _which == WHICH.List ? _content : null;
        set
        {
            _which = WHICH.List;
            _content = value;
        }
    }

    public ushort? Enum
    {
        get => _which == WHICH.Enum ? (ushort?)_content : null;
        set
        {
            _which = WHICH.Enum;
            _content = value;
        }
    }

    public object Struct
    {
        get => _which == WHICH.Struct ? _content : null;
        set
        {
            _which = WHICH.Struct;
            _content = value;
        }
    }

    public object AnyPointer
    {
        get => _which == WHICH.AnyPointer ? _content : null;
        set
        {
            _which = WHICH.AnyPointer;
            _content = value;
        }
    }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        switch (reader.which)
        {
            case WHICH.Void:
                which = reader.which;
                break;
            case WHICH.Bool:
                Bool = reader.Bool;
                break;
            case WHICH.Int8:
                Int8 = reader.Int8;
                break;
            case WHICH.Int16:
                Int16 = reader.Int16;
                break;
            case WHICH.Int32:
                Int32 = reader.Int32;
                break;
            case WHICH.Int64:
                Int64 = reader.Int64;
                break;
            case WHICH.Uint8:
                Uint8 = reader.Uint8;
                break;
            case WHICH.Uint16:
                Uint16 = reader.Uint16;
                break;
            case WHICH.Uint32:
                Uint32 = reader.Uint32;
                break;
            case WHICH.Uint64:
                Uint64 = reader.Uint64;
                break;
            case WHICH.Float32:
                Float32 = reader.Float32;
                break;
            case WHICH.Float64:
                Float64 = reader.Float64;
                break;
            case WHICH.Text:
                Text = reader.Text;
                break;
            case WHICH.Data:
                Data = reader.Data;
                break;
            case WHICH.List:
                List = CapnpSerializable.Create<object>(reader.List);
                break;
            case WHICH.Enum:
                Enum = reader.Enum;
                break;
            case WHICH.Struct:
                Struct = CapnpSerializable.Create<object>(reader.Struct);
                break;
            case WHICH.Interface:
                which = reader.which;
                break;
            case WHICH.AnyPointer:
                AnyPointer = CapnpSerializable.Create<object>(reader.AnyPointer);
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
            case WHICH.Void:
                break;
            case WHICH.Bool:
                writer.Bool = Bool.Value;
                break;
            case WHICH.Int8:
                writer.Int8 = Int8.Value;
                break;
            case WHICH.Int16:
                writer.Int16 = Int16.Value;
                break;
            case WHICH.Int32:
                writer.Int32 = Int32.Value;
                break;
            case WHICH.Int64:
                writer.Int64 = Int64.Value;
                break;
            case WHICH.Uint8:
                writer.Uint8 = Uint8.Value;
                break;
            case WHICH.Uint16:
                writer.Uint16 = Uint16.Value;
                break;
            case WHICH.Uint32:
                writer.Uint32 = Uint32.Value;
                break;
            case WHICH.Uint64:
                writer.Uint64 = Uint64.Value;
                break;
            case WHICH.Float32:
                writer.Float32 = Float32.Value;
                break;
            case WHICH.Float64:
                writer.Float64 = Float64.Value;
                break;
            case WHICH.Text:
                writer.Text = Text;
                break;
            case WHICH.Data:
                writer.Data.Init(Data);
                break;
            case WHICH.List:
                writer.List.SetObject(List);
                break;
            case WHICH.Enum:
                writer.Enum = Enum.Value;
                break;
            case WHICH.Struct:
                writer.Struct.SetObject(Struct);
                break;
            case WHICH.Interface:
                break;
            case WHICH.AnyPointer:
                writer.AnyPointer.SetObject(AnyPointer);
                break;
        }
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

        public WHICH which => (WHICH)ctx.ReadDataUShort(0U);
        public bool Bool => which == WHICH.Bool ? ctx.ReadDataBool(16UL) : default;
        public sbyte Int8 => which == WHICH.Int8 ? ctx.ReadDataSByte(16UL) : default;
        public short Int16 => which == WHICH.Int16 ? ctx.ReadDataShort(16UL) : default;
        public int Int32 => which == WHICH.Int32 ? ctx.ReadDataInt(32UL) : default;
        public long Int64 => which == WHICH.Int64 ? ctx.ReadDataLong(64UL) : default;
        public byte Uint8 => which == WHICH.Uint8 ? ctx.ReadDataByte(16UL) : default;
        public ushort Uint16 => which == WHICH.Uint16 ? ctx.ReadDataUShort(16UL) : default;
        public uint Uint32 => which == WHICH.Uint32 ? ctx.ReadDataUInt(32UL) : default;
        public ulong Uint64 => which == WHICH.Uint64 ? ctx.ReadDataULong(64UL) : default;
        public float Float32 => which == WHICH.Float32 ? ctx.ReadDataFloat(32UL) : default;
        public double Float64 => which == WHICH.Float64 ? ctx.ReadDataDouble(64UL) : default;
        public string Text => which == WHICH.Text ? ctx.ReadText(0) : default;
        public IReadOnlyList<byte> Data =>
            which == WHICH.Data ? ctx.ReadList(0).CastByte() : default;
        public DeserializerState List => which == WHICH.List ? ctx.StructReadPointer(0) : default;
        public ushort Enum => which == WHICH.Enum ? ctx.ReadDataUShort(16UL) : default;
        public DeserializerState Struct =>
            which == WHICH.Struct ? ctx.StructReadPointer(0) : default;
        public DeserializerState AnyPointer =>
            which == WHICH.AnyPointer ? ctx.StructReadPointer(0) : default;
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(2, 1);
        }

        public WHICH which
        {
            get => (WHICH)this.ReadDataUShort(0U);
            set => this.WriteData(0U, (ushort)value);
        }

        public bool Bool
        {
            get => which == WHICH.Bool ? this.ReadDataBool(16UL) : default;
            set => this.WriteData(16UL, value);
        }

        public sbyte Int8
        {
            get => which == WHICH.Int8 ? this.ReadDataSByte(16UL) : default;
            set => this.WriteData(16UL, value);
        }

        public short Int16
        {
            get => which == WHICH.Int16 ? this.ReadDataShort(16UL) : default;
            set => this.WriteData(16UL, value);
        }

        public int Int32
        {
            get => which == WHICH.Int32 ? this.ReadDataInt(32UL) : default;
            set => this.WriteData(32UL, value);
        }

        public long Int64
        {
            get => which == WHICH.Int64 ? this.ReadDataLong(64UL) : default;
            set => this.WriteData(64UL, value);
        }

        public byte Uint8
        {
            get => which == WHICH.Uint8 ? this.ReadDataByte(16UL) : default;
            set => this.WriteData(16UL, value);
        }

        public ushort Uint16
        {
            get => which == WHICH.Uint16 ? this.ReadDataUShort(16UL) : default;
            set => this.WriteData(16UL, value);
        }

        public uint Uint32
        {
            get => which == WHICH.Uint32 ? this.ReadDataUInt(32UL) : default;
            set => this.WriteData(32UL, value);
        }

        public ulong Uint64
        {
            get => which == WHICH.Uint64 ? this.ReadDataULong(64UL) : default;
            set => this.WriteData(64UL, value);
        }

        public float Float32
        {
            get => which == WHICH.Float32 ? this.ReadDataFloat(32UL) : default;
            set => this.WriteData(32UL, value);
        }

        public double Float64
        {
            get => which == WHICH.Float64 ? this.ReadDataDouble(64UL) : default;
            set => this.WriteData(64UL, value);
        }

        public string Text
        {
            get => which == WHICH.Text ? ReadText(0) : default;
            set => WriteText(0, value, null);
        }

        public ListOfPrimitivesSerializer<byte> Data
        {
            get =>
                which == WHICH.Data ? BuildPointer<ListOfPrimitivesSerializer<byte>>(0) : default;
            set => Link(0, value);
        }

        public DynamicSerializerState List
        {
            get => which == WHICH.List ? BuildPointer<DynamicSerializerState>(0) : default;
            set => Link(0, value);
        }

        public ushort Enum
        {
            get => which == WHICH.Enum ? this.ReadDataUShort(16UL) : default;
            set => this.WriteData(16UL, value);
        }

        public DynamicSerializerState Struct
        {
            get => which == WHICH.Struct ? BuildPointer<DynamicSerializerState>(0) : default;
            set => Link(0, value);
        }

        public DynamicSerializerState AnyPointer
        {
            get => which == WHICH.AnyPointer ? BuildPointer<DynamicSerializerState>(0) : default;
            set => Link(0, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xf1c8950dab257542UL)]
public class Annotation : ICapnpSerializable
{
    public const ulong typeId = 0xf1c8950dab257542UL;

    public ulong Id { get; set; }

    public Value Value { get; set; }

    public Brand Brand { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Id = reader.Id;
        Value = CapnpSerializable.Create<Value>(reader.Value);
        Brand = CapnpSerializable.Create<Brand>(reader.Brand);
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Id = Id;
        Value?.serialize(writer.Value);
        Brand?.serialize(writer.Brand);
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

        public ulong Id => ctx.ReadDataULong(0UL);
        public Value.READER Value => ctx.ReadStruct(0, Schema.Value.READER.create);
        public Brand.READER Brand => ctx.ReadStruct(1, Schema.Brand.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 2);
        }

        public ulong Id
        {
            get => this.ReadDataULong(0UL);
            set => this.WriteData(0UL, value);
        }

        public Value.WRITER Value
        {
            get => BuildPointer<Value.WRITER>(0);
            set => Link(0, value);
        }

        public Brand.WRITER Brand
        {
            get => BuildPointer<Brand.WRITER>(1);
            set => Link(1, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd1958f7dba521926UL)]
public enum ElementSize : ushort
{
    empty,
    bit,
    @byte,
    twoBytes,
    fourBytes,
    eightBytes,
    pointer,
    inlineComposite,
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xd85d305b7d839963UL)]
public class CapnpVersion : ICapnpSerializable
{
    public const ulong typeId = 0xd85d305b7d839963UL;

    public ushort Major { get; set; }

    public byte Minor { get; set; }

    public byte Micro { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Major = reader.Major;
        Minor = reader.Minor;
        Micro = reader.Micro;
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Major = Major;
        writer.Minor = Minor;
        writer.Micro = Micro;
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

        public ushort Major => ctx.ReadDataUShort(0UL);
        public byte Minor => ctx.ReadDataByte(16UL);
        public byte Micro => ctx.ReadDataByte(24UL);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(1, 0);
        }

        public ushort Major
        {
            get => this.ReadDataUShort(0UL);
            set => this.WriteData(0UL, value);
        }

        public byte Minor
        {
            get => this.ReadDataByte(16UL);
            set => this.WriteData(16UL, value);
        }

        public byte Micro
        {
            get => this.ReadDataByte(24UL);
            set => this.WriteData(24UL, value);
        }
    }
}

[GeneratedCode("capnpc-csharp", "1.3.0.0")]
[TypeId(0xbfc546f6210ad7ceUL)]
public class CodeGeneratorRequest : ICapnpSerializable
{
    public const ulong typeId = 0xbfc546f6210ad7ceUL;

    public IReadOnlyList<Node> Nodes { get; set; }

    public IReadOnlyList<RequestedFile> RequestedFiles { get; set; }

    public CapnpVersion CapnpVersion { get; set; }

    public IReadOnlyList<Node.SourceInfo> SourceInfo { get; set; }

    void ICapnpSerializable.Deserialize(DeserializerState arg_)
    {
        var reader = READER.create(arg_);
        Nodes = reader.Nodes?.ToReadOnlyList(_ => CapnpSerializable.Create<Node>(_));
        RequestedFiles = reader.RequestedFiles?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<RequestedFile>(_)
        );
        CapnpVersion = CapnpSerializable.Create<CapnpVersion>(reader.CapnpVersion);
        SourceInfo = reader.SourceInfo?.ToReadOnlyList(_ =>
            CapnpSerializable.Create<Node.SourceInfo>(_)
        );
        applyDefaults();
    }

    void ICapnpSerializable.Serialize(SerializerState arg_)
    {
        serialize(arg_.Rewrap<WRITER>());
    }

    public void serialize(WRITER writer)
    {
        writer.Nodes.Init(Nodes, (_s1, _v1) => _v1?.serialize(_s1));
        writer.RequestedFiles.Init(RequestedFiles, (_s1, _v1) => _v1?.serialize(_s1));
        CapnpVersion?.serialize(writer.CapnpVersion);
        writer.SourceInfo.Init(SourceInfo, (_s1, _v1) => _v1?.serialize(_s1));
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

        public IReadOnlyList<Node.READER> Nodes => ctx.ReadList(0).Cast(Node.READER.create);
        public IReadOnlyList<RequestedFile.READER> RequestedFiles =>
            ctx.ReadList(1).Cast(RequestedFile.READER.create);
        public CapnpVersion.READER CapnpVersion =>
            ctx.ReadStruct(2, Schema.CapnpVersion.READER.create);
        public IReadOnlyList<Node.SourceInfo.READER> SourceInfo =>
            ctx.ReadList(3).Cast(Node.SourceInfo.READER.create);
    }

    public class WRITER : SerializerState
    {
        public WRITER()
        {
            SetStruct(0, 4);
        }

        public ListOfStructsSerializer<Node.WRITER> Nodes
        {
            get => BuildPointer<ListOfStructsSerializer<Node.WRITER>>(0);
            set => Link(0, value);
        }

        public ListOfStructsSerializer<RequestedFile.WRITER> RequestedFiles
        {
            get => BuildPointer<ListOfStructsSerializer<RequestedFile.WRITER>>(1);
            set => Link(1, value);
        }

        public CapnpVersion.WRITER CapnpVersion
        {
            get => BuildPointer<CapnpVersion.WRITER>(2);
            set => Link(2, value);
        }

        public ListOfStructsSerializer<Node.SourceInfo.WRITER> SourceInfo
        {
            get => BuildPointer<ListOfStructsSerializer<Node.SourceInfo.WRITER>>(3);
            set => Link(3, value);
        }
    }

    [GeneratedCode("capnpc-csharp", "1.3.0.0")]
    [TypeId(0xcfea0eb02e810062UL)]
    public class RequestedFile : ICapnpSerializable
    {
        public const ulong typeId = 0xcfea0eb02e810062UL;

        public ulong Id { get; set; }

        public string Filename { get; set; }

        public IReadOnlyList<Import> Imports { get; set; }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = reader.Id;
            Filename = reader.Filename;
            Imports = reader.Imports?.ToReadOnlyList(_ => CapnpSerializable.Create<Import>(_));
            applyDefaults();
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void serialize(WRITER writer)
        {
            writer.Id = Id;
            writer.Filename = Filename;
            writer.Imports.Init(Imports, (_s1, _v1) => _v1?.serialize(_s1));
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

            public ulong Id => ctx.ReadDataULong(0UL);
            public string Filename => ctx.ReadText(0);
            public IReadOnlyList<Import.READER> Imports =>
                ctx.ReadList(1).Cast(Import.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                SetStruct(1, 2);
            }

            public ulong Id
            {
                get => this.ReadDataULong(0UL);
                set => this.WriteData(0UL, value);
            }

            public string Filename
            {
                get => ReadText(0);
                set => WriteText(0, value, null);
            }

            public ListOfStructsSerializer<Import.WRITER> Imports
            {
                get => BuildPointer<ListOfStructsSerializer<Import.WRITER>>(1);
                set => Link(1, value);
            }
        }

        [GeneratedCode("capnpc-csharp", "1.3.0.0")]
        [TypeId(0xae504193122357e5UL)]
        public class Import : ICapnpSerializable
        {
            public const ulong typeId = 0xae504193122357e5UL;

            public ulong Id { get; set; }

            public string Name { get; set; }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Id = reader.Id;
                Name = reader.Name;
                applyDefaults();
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void serialize(WRITER writer)
            {
                writer.Id = Id;
                writer.Name = Name;
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

                public ulong Id => ctx.ReadDataULong(0UL);
                public string Name => ctx.ReadText(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    SetStruct(1, 1);
                }

                public ulong Id
                {
                    get => this.ReadDataULong(0UL);
                    set => this.WriteData(0UL, value);
                }

                public string Name
                {
                    get => ReadText(0);
                    set => WriteText(0, value, null);
                }
            }
        }
    }
}
