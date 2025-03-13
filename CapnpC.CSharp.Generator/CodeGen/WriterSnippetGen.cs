using System.Collections.Generic;
using System.Linq;
using Capnp;
using CapnpC.CSharp.Generator.Model;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static CapnpC.CSharp.Generator.CodeGen.SyntaxHelpers;

namespace CapnpC.CSharp.Generator.CodeGen;

internal class WriterSnippetGen
{
    private readonly GenNames _names;

    public WriterSnippetGen(GenNames names)
    {
        _names = names;
    }

    private IEnumerable<MemberDeclarationSyntax> MakeWriterStructMembers(TypeDefinition structType)
    {
        yield return ConstructorDeclaration(_names.WriterStruct.Identifier)
            .AddModifiers(Public)
            .WithBody(
                Block(
                    ExpressionStatement(
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName(SerializerStateWorder.SetStructName)))
                            .AddArgumentListArguments(
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(structType.StructDataWordCount))),
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(structType.StructPointerCount)))))));
    }

    private IEnumerable<MemberDeclarationSyntax> MakeGroupWriterStructMembers()
    {
        yield return ConstructorDeclaration(_names.WriterStruct.Identifier)
            .AddModifiers(Public)
            .WithBody(Block());
    }

    private PropertyDeclarationSyntax MakeWriterProperty(
        TypeSyntax type,
        string name,
        ExpressionSyntax getter,
        ExpressionSyntax setter,
        bool cast,
        bool cond)
    {
        if (cast) getter = CastExpression(type, getter);

        if (cond)
            getter = ConditionalExpression(
                BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    _names.UnionDiscriminatorProp.IdentifierName,
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        _names.UnionDiscriminatorEnum.IdentifierName,
                        IdentifierName(name))),
                getter,
                LiteralExpression(
                    SyntaxKind.DefaultLiteralExpression,
                    Token(SyntaxKind.DefaultKeyword)));

        var accessors = new AccessorDeclarationSyntax[setter != null ? 2 : 1];

        accessors[0] = AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
            .WithExpressionBody(ArrowExpressionClause(getter))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        if (setter != null)
            accessors[1] = AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithExpressionBody(ArrowExpressionClause(setter))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        return PropertyDeclaration(type, name)
            .AddModifiers(Public)
            .AddAccessorListAccessors(accessors);
    }

    private ExpressionSyntax MakePointerSyntax(TypeSyntax type, object index)
    {
        return InvocationExpression(
                GenericName(nameof(SerializerState.BuildPointer))
                    .AddTypeArgumentListArguments(type))
            .AddArgumentListArguments(
                Argument(ValueOf(index)));
    }

    private ExpressionSyntax MakeReadCapSyntax(TypeSyntax type, object index)
    {
        return InvocationExpression(
                GenericName(nameof(SerializerState.ReadCap))
                    .AddTypeArgumentListArguments(type))
            .AddArgumentListArguments(
                Argument(ValueOf(index)));
    }

    private ExpressionSyntax MakeTypedPointerSyntax(object index, TypeSyntax type)
    {
        return InvocationExpression(
                GenericName(nameof(SerializerState.BuildPointer))
                    .AddTypeArgumentListArguments(type))
            .AddArgumentListArguments(
                Argument(ValueOf(index)));
    }

    private ExpressionSyntax MakeLinkSyntax(object index, bool suppressNullableWarning)
    {
        ExpressionSyntax value = IdentifierName("value");

        if (suppressNullableWarning) value = _names.SuppressNullableWarning(value);

        return InvocationExpression(
                IdentifierName(SerializerStateWorder.LinkName))
            .AddArgumentListArguments(
                Argument(ValueOf(index)),
                Argument(value));
    }

    private ExpressionSyntax MakeLinkObjectSyntax(object index)
    {
        return InvocationExpression(
                IdentifierName(nameof(SerializerState.LinkObject)))
            .AddArgumentListArguments(
                Argument(ValueOf(index)),
                Argument(IdentifierName("value")));
    }

    private PropertyDeclarationSyntax MakeWriterRefTypeProperty(
        TypeSyntax type,
        string name,
        ExpressionSyntax getter,
        ExpressionSyntax setter,
        bool cast,
        bool cond)
    {
        if (cond) type = _names.MakeNullableRefType(type);

        var prop = MakeWriterProperty(type, name, getter, setter, cast, cond);

        if (cond && _names.NullableEnable)
            prop = prop.AddAttributeLists(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(
                            IdentifierName("DisallowNull")))));
        return prop;
    }

    private PropertyDeclarationSyntax MakePointerProperty(TypeSyntax type, string name, object index, bool cast,
        bool cond)
    {
        var getter = MakePointerSyntax(type, index);
        var setter = MakeLinkSyntax(index, cond);

        return MakeWriterRefTypeProperty(type, name, getter, setter, cast, cond);
    }

    private PropertyDeclarationSyntax MakePointerAsStructProperty(TypeSyntax type, string name, object index, bool cast,
        bool cond)
    {
        var getter = MakeTypedPointerSyntax(index, type);
        var setter = MakeLinkSyntax(index, cond);

        return MakeWriterRefTypeProperty(type, name, getter, setter, cast, cond);
    }

    private PropertyDeclarationSyntax MakeProperty(
        TypeSyntax outerType,
        TypeSyntax innerType,
        string name,
        string readName,
        string writeName,
        object indexOrBitOffset,
        ExpressionSyntax secondArg,
        bool cast,
        bool cond,
        bool pasd)
    {
        var getter = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    ThisExpression(),
                    IdentifierName(readName)))
            .AddArgumentListArguments(
                Argument(ValueOf(indexOrBitOffset)),
                Argument(secondArg));

        ExpressionSyntax value = IdentifierName("value");

        if (cast) value = CastExpression(innerType, value);

        var setter = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    ThisExpression(),
                    IdentifierName(writeName)))
            .AddArgumentListArguments(
                Argument(ValueOf(indexOrBitOffset)),
                Argument(value),
                Argument(secondArg));

        if (pasd) setter.AddArgumentListArguments(Argument(secondArg));

        return MakeWriterProperty(outerType, name, getter, setter, cast, cond);
    }

    private PropertyDeclarationSyntax MakePrimitiveProperty<T>(Field field, string readName)
    {
        return MakeProperty(
            _names.Type<T>(Nullability.NonNullable),
            null,
            _names.GetCodeIdentifier(field).ToString(),
            readName,
            nameof(SerializerExtensions.WriteData),
            field.BitOffset.Value,
            ValueOf(field.DefaultValue.ScalarValue),
            false,
            field.DiscValue.HasValue,
            true);
    }

    private PropertyDeclarationSyntax MakeEnumProperty(Field field, string readName)
    {
        return MakeProperty(
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.NotRelevant, Nullability.NonNullable),
            _names.Type<ushort>(Nullability.NonNullable),
            _names.GetCodeIdentifier(field).ToString(),
            readName,
            nameof(SerializerExtensions.WriteData),
            field.BitOffset.Value,
            ValueOf(field.DefaultValue.ScalarValue),
            true,
            field.DiscValue.HasValue,
            true);
    }

    private PropertyDeclarationSyntax MakeTextProperty(Field field)
    {
        return MakeProperty(
            _names.Type<string>(Nullability.NullableRef),
            null,
            _names.GetCodeIdentifier(field).ToString(),
            nameof(SerializerState.ReadText),
            nameof(SerializerState.WriteText),
            (int)field.Offset,
            ValueOf(field.DefaultValue.ScalarValue),
            false,
            field.DiscValue.HasValue,
            false);
    }

    private PropertyDeclarationSyntax MakeStructProperty(Field field)
    {
        var qtype = _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Writer, Nullability.NonNullable);

        return MakePointerAsStructProperty(qtype, _names.GetCodeIdentifier(field).ToString(),
            (int)field.Offset, false, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeGroupProperty(Field field)
    {
        TypeSyntax type = QualifiedName(
            _names.MakeTypeName(field.Type.Definition).IdentifierName,
            _names.WriterStruct.IdentifierName);

        var getter = InvocationExpression(
            GenericName(nameof(SerializerState.Rewrap))
                .AddTypeArgumentListArguments(type));

        if (field.DiscValue.HasValue) type = _names.MakeNullableRefType(type);

        return MakeWriterProperty(type, _names.GetCodeIdentifier(field).ToString(), getter, null, false,
            field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeListProperty(Field field)
    {
        var qtype = _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Writer, Nullability.NonNullable);

        return MakePointerProperty(qtype, _names.GetCodeIdentifier(field).ToString(),
            (int)field.Offset, false, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakePointerProperty(Field field)
    {
        var type = IdentifierName(nameof(DynamicSerializerState));

        return MakePointerProperty(type, _names.GetCodeIdentifier(field).ToString(), (int)field.Offset, false,
            field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeCapProperty(Field field)
    {
        var nonNullableType =
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Writer, Nullability.NonNullable);
        var nullableType =
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Writer, Nullability.NullableRef);
        var index = (int)field.Offset;
        var name = _names.GetCodeIdentifier(field).ToString();
        var getter = MakeReadCapSyntax(nonNullableType, index);
        var setter = MakeLinkObjectSyntax(index);

        return MakeWriterProperty(nullableType, name, getter, setter, false, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeWriterUnionSelector(TypeDefinition def)
    {
        return MakeProperty(
            _names.UnionDiscriminatorEnum.IdentifierName,
            _names.Type<ushort>(Nullability.NonNullable),
            _names.UnionDiscriminatorProp.ToString(),
            nameof(SerializerExtensions.ReadDataUShort),
            nameof(SerializerExtensions.WriteData),
            def.UnionInfo.TagOffset,
            ValueOf(default(ushort)),
            true, false, true);
    }

    private PropertyDeclarationSyntax MakeWriterFieldProperty(Field field)
    {
        switch (field.Type.Tag)
        {
            case TypeTag.Bool:
                return MakePrimitiveProperty<bool>(field,
                    nameof(SerializerExtensions.ReadDataBool));

            case TypeTag.S8:
                return MakePrimitiveProperty<sbyte>(field,
                    nameof(SerializerExtensions.ReadDataSByte));

            case TypeTag.U8:
                return MakePrimitiveProperty<byte>(field,
                    nameof(SerializerExtensions.ReadDataByte));

            case TypeTag.S16:
                return MakePrimitiveProperty<short>(field,
                    nameof(SerializerExtensions.ReadDataShort));

            case TypeTag.U16:
                return MakePrimitiveProperty<ushort>(field,
                    nameof(SerializerExtensions.ReadDataUShort));

            case TypeTag.S32:
                return MakePrimitiveProperty<int>(field,
                    nameof(SerializerExtensions.ReadDataInt));

            case TypeTag.U32:
                return MakePrimitiveProperty<uint>(field,
                    nameof(SerializerExtensions.ReadDataUInt));

            case TypeTag.S64:
                return MakePrimitiveProperty<long>(field,
                    nameof(SerializerExtensions.ReadDataLong));

            case TypeTag.U64:
                return MakePrimitiveProperty<ulong>(field,
                    nameof(SerializerExtensions.ReadDataULong));

            case TypeTag.F32:
                return MakePrimitiveProperty<float>(field,
                    nameof(SerializerExtensions.ReadDataFloat));

            case TypeTag.F64:
                return MakePrimitiveProperty<double>(field,
                    nameof(SerializerExtensions.ReadDataDouble));

            case TypeTag.Enum:
                return MakeEnumProperty(field, nameof(SerializerExtensions.ReadDataUShort));

            case TypeTag.Text:
                return MakeTextProperty(field);

            case TypeTag.Struct:
                return MakeStructProperty(field);

            case TypeTag.Group:
                return MakeGroupProperty(field);

            case TypeTag.List:
            case TypeTag.Data:
                return MakeListProperty(field);

            case TypeTag.AnyPointer:
            case TypeTag.StructPointer:
            case TypeTag.ListPointer:
                return MakePointerProperty(field);

            case TypeTag.CapabilityPointer:
            case TypeTag.Interface:
                return MakeCapProperty(field);

            default:
                return null;
        }
    }

    public ClassDeclarationSyntax MakeWriterStruct(TypeDefinition def)
    {
        var WriterDecl = ClassDeclaration(_names.WriterStruct.ToString())
            .AddModifiers(Public)
            .AddBaseListTypes(
                SimpleBaseType(IdentifierName(nameof(SerializerState))));

        var members = def.Tag == TypeTag.Group ? MakeGroupWriterStructMembers() : MakeWriterStructMembers(def);

        WriterDecl = WriterDecl.AddMembers(members.ToArray());

        if (def.UnionInfo != null) WriterDecl = WriterDecl.AddMembers(MakeWriterUnionSelector(def));

        foreach (var field in def.Fields)
        {
            var propDecl = MakeWriterFieldProperty(field);

            if (propDecl != null) WriterDecl = WriterDecl.AddMembers(propDecl);
        }

        return WriterDecl;
    }
}