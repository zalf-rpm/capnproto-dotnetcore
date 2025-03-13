using System;
using System.Collections.Generic;
using System.Linq;
using Capnp;
using CapnpC.CSharp.Generator.Model;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static CapnpC.CSharp.Generator.CodeGen.SyntaxHelpers;
using Type = CapnpC.CSharp.Generator.Model.Type;

namespace CapnpC.CSharp.Generator.CodeGen;

internal class ReaderSnippetGen
{
    private readonly GenNames _names;

    public ReaderSnippetGen(GenNames names)
    {
        _names = names;
    }

    private MemberDeclarationSyntax MakeReaderImplicitConversionOperator1()
    {
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ImplicitKeyword),
                IdentifierName(nameof(DeserializerState)))
            .WithModifiers(
                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(_names.ReaderParameter.Identifier)
                            .WithType(_names.ReaderStruct.IdentifierName))))
            .WithExpressionBody(
                ArrowExpressionClause(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        _names.ReaderParameter.IdentifierName,
                        _names.ReaderContextField.IdentifierName)))
            .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken));
    }

    private MemberDeclarationSyntax MakeReaderImplicitConversionOperator2()
    {
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ImplicitKeyword),
                _names.ReaderStruct.IdentifierName)
            .WithModifiers(
                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(_names.ReaderContextField.Identifier)
                            .WithType(_names.Type<DeserializerState>(Nullability.NonNullable)))))
            .WithExpressionBody(
                ArrowExpressionClause(
                    ObjectCreationExpression(_names.ReaderStruct.IdentifierName)
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(_names.ReaderContextField.IdentifierName))))))
            .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken));
    }

    private MemberDeclarationSyntax MakeReaderCreateMethod()
    {
        return MethodDeclaration(_names.ReaderStruct.IdentifierName, _names.ReaderCreateMethod.Identifier)
            .AddModifiers(Public, Static)
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(_names.ContextParameter.Identifier)
                            .WithType(
                                _names.Type<DeserializerState>(Nullability.NonNullable)))))
            .WithExpressionBody(
                ArrowExpressionClause(
                    ObjectCreationExpression(_names.ReaderStruct.IdentifierName)
                        .AddArgumentListArguments(Argument(_names.ContextParameter.IdentifierName))))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private IEnumerable<MemberDeclarationSyntax> MakeReaderStructMembers()
    {
        yield return FieldDeclaration(
                VariableDeclaration(
                        _names.Type<DeserializerState>(Nullability.NonNullable))
                    .AddVariables(_names.ReaderContextField.VariableDeclarator))
            .AddModifiers(Readonly);

        yield return ConstructorDeclaration(_names.ReaderStruct.Identifier)
            .AddModifiers(Public)
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(_names.ContextParameter.Identifier)
                            .WithType(_names.Type<DeserializerState>(Nullability.NonNullable)))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    _names.ReaderContextField.IdentifierName),
                                _names.ContextParameter.IdentifierName)))));

        yield return MakeReaderCreateMethod();
        yield return MakeReaderImplicitConversionOperator1();
        yield return MakeReaderImplicitConversionOperator2();
    }

    private IEnumerable<MemberDeclarationSyntax> MakeGroupReaderStructMembers()
    {
        yield return FieldDeclaration(
                VariableDeclaration(
                        _names.Type<DeserializerState>(Nullability.NonNullable))
                    .AddVariables(_names.ReaderContextField.VariableDeclarator))
            .AddModifiers(Readonly);

        yield return ConstructorDeclaration(_names.ReaderStruct.Identifier)
            .AddModifiers(Public)
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(Parameter(_names.GroupReaderContextArg.Identifier)
                        .WithType(_names.Type<DeserializerState>(Nullability.NonNullable)))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    _names.ReaderContextField.IdentifierName),
                                _names.GroupReaderContextArg.IdentifierName)))));

        yield return MakeReaderCreateMethod();
        yield return MakeReaderImplicitConversionOperator1();
        yield return MakeReaderImplicitConversionOperator2();
    }

    private PropertyDeclarationSyntax MakeReaderProperty(TypeSyntax type, string name, ExpressionSyntax right,
        bool cond)
    {
        if (cond)
            right = ConditionalExpression(
                BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    _names.UnionDiscriminatorProp.IdentifierName,
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        _names.UnionDiscriminatorEnum.IdentifierName,
                        IdentifierName(name))),
                right,
                LiteralExpression(
                    SyntaxKind.DefaultLiteralExpression,
                    Token(SyntaxKind.DefaultKeyword)));

        return PropertyDeclaration(type, name)
            .AddModifiers(Public)
            .WithExpressionBody(ArrowExpressionClause(right))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static Func<ExpressionSyntax, ExpressionSyntax> MakeCastFunc(TypeSyntax type)
    {
        return x => CastExpression(type, x);
    }

    private PropertyDeclarationSyntax MakeReadProperty(TypeSyntax type, string name, SimpleNameSyntax readName,
        object indexOrBitOffset, ExpressionSyntax secondArg,
        Func<ExpressionSyntax, ExpressionSyntax> cast, bool cond)
    {
        var right = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    _names.ReaderContextField.IdentifierName,
                    readName))
            .AddArgumentListArguments(
                Argument(ValueOf(indexOrBitOffset)));

        if (secondArg != null) right = right.AddArgumentListArguments(Argument(secondArg));

        ExpressionSyntax expr = right;

        if (cast != null) expr = cast(expr);

        return MakeReaderProperty(type, name, expr, cond);
    }

    private PropertyDeclarationSyntax MakeReadProperty(TypeSyntax type, string name, string readName,
        object indexOrBitOffset, ExpressionSyntax secondArg,
        Func<ExpressionSyntax, ExpressionSyntax> cast, bool cond)
    {
        return MakeReadProperty(type, name, IdentifierName(readName), indexOrBitOffset, secondArg, cast, cond);
    }

    private PropertyDeclarationSyntax MakeReadPrimitiveProperty<T>(Field field, string readName)
    {
        return MakeReadProperty(
            _names.Type<T>(Nullability.NonNullable),
            _names.GetCodeIdentifier(field).ToString(),
            readName,
            field.BitOffset.Value,
            ValueOf(field.DefaultValue.ScalarValue),
            null,
            field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadEnumProperty(Field field)
    {
        var typeSyntax =
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader, Nullability.NonNullable);
        return MakeReadProperty(typeSyntax,
            _names.GetCodeIdentifier(field).ToString(),
            nameof(SerializerExtensions.ReadDataUShort), field.BitOffset.Value,
            ValueOf(field.DefaultValue.ScalarValue),
            x => CastExpression(typeSyntax, x),
            field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadTextProperty(Field field)
    {
        var cantBeNull = !field.DiscValue.HasValue && field.DefaultValue.ScalarValue != null;

        return MakeReadProperty(
            _names.Type<string>(cantBeNull ? Nullability.NonNullable : Nullability.NullableRef),
            _names.GetCodeIdentifier(field).ToString(),
            nameof(DeserializerState.ReadText),
            (int)field.Offset,
            ValueOf(field.DefaultValue.ScalarValue),
            null,
            field.DiscValue.HasValue);
    }

    private MemberAccessExpressionSyntax MakeReaderCreator(TypeSyntax qtype)
    {
        return MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            qtype,
            _names.ReaderCreateMethod.IdentifierName);
    }

    private PropertyDeclarationSyntax MakeReadStructProperty(Field field)
    {
        var qtype = _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader, Nullability.NonNullable);
        var creator = MakeReaderCreator(qtype);

        return MakeReadProperty(qtype, _names.GetCodeIdentifier(field).ToString(),
            nameof(DeserializerState.ReadStruct), (int)field.Offset,
            creator, null, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeHasStructProperty(Field field)
    {
        var propertyType = _names.Type<bool>(Nullability.NonNullable);
        var propertyName = "Has" + _names.GetCodeIdentifier(field);

        return MakeReadProperty(
            propertyType,
            propertyName,
            nameof(DeserializerState.IsStructFieldNonNull),
            (int)field.Offset,
            null,
            null,
            false);
    }

    private PropertyDeclarationSyntax MakeReadGroupProperty(Field field)
    {
        var type = QualifiedName(
            _names.MakeTypeName(field.Type.Definition).IdentifierName,
            _names.ReaderStruct.IdentifierName);

        var right = ObjectCreationExpression(type)
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(_names.ReaderContextField.IdentifierName))));

        return MakeReaderProperty(type, _names.GetCodeIdentifier(field).ToString(), right, field.DiscValue.HasValue);
    }

    private ExpressionSyntax MakeReadListPropertyImpl(Type elementType, TypeDefinition scope, ExpressionSyntax context,
        int depth)
    {
        var elementTypeSyntax = _names.MakeTypeSyntax(elementType, scope, TypeUsage.Reader, Nullability.NonNullable);

        if (elementType.Tag == TypeTag.Interface ||
            elementType.Tag == TypeTag.CapabilityPointer)
        {
            if (depth == 0)
                return InvocationExpression(MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    _names.ReaderContextField.IdentifierName,
                    GenericName(nameof(DeserializerState.ReadCapList))
                        .AddTypeArgumentListArguments(elementTypeSyntax)
                )).AddArgumentListArguments(Argument(context));

            return InvocationExpression(
                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    context,
                    GenericName(nameof(DeserializerState.RequireCapList))
                        .AddTypeArgumentListArguments(elementTypeSyntax)
                ));
        }

        if (depth == 0)
            context = InvocationExpression(MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    _names.ReaderContextField.IdentifierName,
                    IdentifierName(nameof(DeserializerState.ReadList))))
                .AddArgumentListArguments(Argument(context));
        else
            context = InvocationExpression(
                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    context,
                    IdentifierName(nameof(DeserializerState.RequireList))
                ));

        var lambdaParamName = "_" + depth;
        var lambdaParam = Parameter(Identifier(lambdaParamName));
        var lambdaArg = IdentifierName(lambdaParamName);
        string castFuncName;

        switch (elementType.Tag)
        {
            case TypeTag.List:
            {
                var innerImpl = MakeReadListPropertyImpl(
                    elementType.ElementType,
                    scope,
                    lambdaArg,
                    depth + 1);

                return InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            context,
                            IdentifierName(nameof(ListDeserializer.Cast))))
                    .AddArgumentListArguments(
                        Argument(SimpleLambdaExpression(lambdaParam, innerImpl)));
            }

            case TypeTag.ListPointer:
            {
                context = InvocationExpression(MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    context,
                    IdentifierName(nameof(DeserializerState.RequireList))
                )).AddArgumentListArguments(Argument(lambdaArg));

                return InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            context,
                            IdentifierName(nameof(ReadOnlyListExtensions.LazyListSelect))))
                    .AddArgumentListArguments(
                        Argument(SimpleLambdaExpression(lambdaParam, context)));
            }

            case TypeTag.Struct:
            {
                return InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            context,
                            IdentifierName(nameof(ListDeserializer.Cast))))
                    .AddArgumentListArguments(
                        Argument(MakeReaderCreator(elementTypeSyntax)));
            }

            case TypeTag.Enum:
            {
                var cons = SimpleLambdaExpression(
                    lambdaParam,
                    CastExpression(elementTypeSyntax, lambdaArg));

                return InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            context,
                            IdentifierName(nameof(ListDeserializer.CastEnums))))
                    .AddArgumentListArguments(
                        Argument(cons));
            }

            case TypeTag.AnyPointer:
            case TypeTag.StructPointer:
            {
                return context;
            }

            case TypeTag.Void:
            {
                return MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    context,
                    IdentifierName(nameof(ListDeserializer.Count)));
            }

            case TypeTag.Data:
                castFuncName = nameof(ListDeserializer.CastData);
                break;

            case TypeTag.Text:
                castFuncName = nameof(ListDeserializer.CastText2);
                break;

            case TypeTag.Bool:
                castFuncName = nameof(ListDeserializer.CastBool);
                break;

            case TypeTag.F32:
                castFuncName = nameof(ListDeserializer.CastFloat);
                break;

            case TypeTag.F64:
                castFuncName = nameof(ListDeserializer.CastDouble);
                break;

            case TypeTag.S8:
                castFuncName = nameof(ListDeserializer.CastSByte);
                break;

            case TypeTag.U8:
                castFuncName = nameof(ListDeserializer.CastByte);
                break;

            case TypeTag.S16:
                castFuncName = nameof(ListDeserializer.CastShort);
                break;

            case TypeTag.U16:
            case TypeTag.AnyEnum:
                castFuncName = nameof(ListDeserializer.CastUShort);
                break;

            case TypeTag.S32:
                castFuncName = nameof(ListDeserializer.CastInt);
                break;

            case TypeTag.U32:
                castFuncName = nameof(ListDeserializer.CastUInt);
                break;

            case TypeTag.S64:
                castFuncName = nameof(ListDeserializer.CastLong);
                break;

            case TypeTag.U64:
                castFuncName = nameof(ListDeserializer.CastULong);
                break;

            default:
                throw new NotImplementedException("Unexpected type tag, don't know how to deal with this");
        }

        return InvocationExpression(MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            context,
            IdentifierName(castFuncName)));
    }

    private PropertyDeclarationSyntax MakeReadListProperty(Field field)
    {
        var elementType = field.Type.ElementType;
        var context = ValueOf((int)field.Offset);
        var impl = MakeReadListPropertyImpl(elementType, field.DeclaringType, context, 0);
        var listType = _names.MakeTypeSyntax(
            field.Type,
            field.DeclaringType,
            TypeUsage.Reader,
            field.DiscValue.HasValue ? Nullability.NullableRef : Nullability.NonNullable);
        return MakeReaderProperty(listType, _names.GetCodeIdentifier(field).ToString(), impl, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadAnyListProperty(Field field)
    {
        var type = _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader, Nullability.NonNullable);

        return MakeReadProperty(type, _names.GetCodeIdentifier(field).ToString(),
            nameof(DeserializerState.ReadList),
            (int)field.Offset, null, x => CastExpression(type, x), field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadCapProperty(Field field)
    {
        var nullableType =
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader, Nullability.NullableRef);
        var nonNullableType =
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader, Nullability.NonNullable);
        var readName = GenericName(nameof(DeserializerState.ReadCap))
            .AddTypeArgumentListArguments(nonNullableType);

        return MakeReadProperty(nullableType, _names.GetCodeIdentifier(field).ToString(),
            readName,
            (int)field.Offset, null, null, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadAnyCapProperty(Field field)
    {
        var type = _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader, Nullability.NonNullable);

        return MakeReadProperty(type, _names.GetCodeIdentifier(field).ToString(),
            nameof(DeserializerState.ReadCap),
            (int)field.Offset, null, null, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadDataProperty(Field field)
    {
        var context = InvocationExpression(MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                _names.ReaderContextField.IdentifierName,
                IdentifierName(nameof(DeserializerState.ReadList))))
            .AddArgumentListArguments(Argument(ValueOf((int)field.Offset)));
        var impl = InvocationExpression(MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            context,
            IdentifierName(nameof(ListDeserializer.CastByte))));

        return MakeReaderProperty(
            _names.MakeTypeSyntax(field.Type, field.DeclaringType, TypeUsage.Reader,
                field.DiscValue.HasValue ? Nullability.NullableRefAndValue : Nullability.NonNullable),
            _names.GetCodeIdentifier(field).ToString(), impl, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReadAnyPointerProperty(Field field)
    {
        var type = IdentifierName(nameof(DeserializerState));

        return MakeReadProperty(type, _names.GetCodeIdentifier(field).ToString(),
            nameof(DeserializerState.StructReadPointer),
            (int)field.Offset, null, null, field.DiscValue.HasValue);
    }

    private PropertyDeclarationSyntax MakeReaderUnionSelector(TypeDefinition def)
    {
        var type = _names.UnionDiscriminatorEnum.IdentifierName;
        return MakeReadProperty(
            type,
            _names.UnionDiscriminatorProp.ToString(),
            nameof(SerializerExtensions.ReadDataUShort),
            def.UnionInfo.TagOffset,
            ValueOf(default(ushort)),
            MakeCastFunc(type), false);
    }

    private PropertyDeclarationSyntax MakeReaderFieldProperty(Field field)
    {
        switch (field.Type.Tag)
        {
            case TypeTag.Bool:
                return MakeReadPrimitiveProperty<bool>(field,
                    nameof(SerializerExtensions.ReadDataBool));

            case TypeTag.S8:
                return MakeReadPrimitiveProperty<sbyte>(field,
                    nameof(SerializerExtensions.ReadDataSByte));

            case TypeTag.U8:
                return MakeReadPrimitiveProperty<byte>(field,
                    nameof(SerializerExtensions.ReadDataByte));

            case TypeTag.S16:
                return MakeReadPrimitiveProperty<short>(field,
                    nameof(SerializerExtensions.ReadDataShort));

            case TypeTag.U16:
                return MakeReadPrimitiveProperty<ushort>(field,
                    nameof(SerializerExtensions.ReadDataUShort));

            case TypeTag.S32:
                return MakeReadPrimitiveProperty<int>(field,
                    nameof(SerializerExtensions.ReadDataInt));

            case TypeTag.U32:
                return MakeReadPrimitiveProperty<uint>(field,
                    nameof(SerializerExtensions.ReadDataUInt));

            case TypeTag.S64:
                return MakeReadPrimitiveProperty<long>(field,
                    nameof(SerializerExtensions.ReadDataLong));

            case TypeTag.U64:
                return MakeReadPrimitiveProperty<ulong>(field,
                    nameof(SerializerExtensions.ReadDataULong));

            case TypeTag.F32:
                return MakeReadPrimitiveProperty<float>(field,
                    nameof(SerializerExtensions.ReadDataFloat));

            case TypeTag.F64:
                return MakeReadPrimitiveProperty<double>(field,
                    nameof(SerializerExtensions.ReadDataDouble));

            case TypeTag.Enum:
                return MakeReadEnumProperty(field);

            case TypeTag.Text:
                return MakeReadTextProperty(field);

            case TypeTag.Struct:
                return MakeReadStructProperty(field);

            case TypeTag.Group:
                return MakeReadGroupProperty(field);

            case TypeTag.List:
                return MakeReadListProperty(field);

            case TypeTag.Interface:
                return MakeReadCapProperty(field);

            case TypeTag.CapabilityPointer:
                return MakeReadAnyCapProperty(field);

            case TypeTag.ListPointer:
                return MakeReadAnyListProperty(field);

            case TypeTag.AnyPointer:
            case TypeTag.StructPointer:
                return MakeReadAnyPointerProperty(field);

            case TypeTag.Data:
                return MakeReadDataProperty(field);

            default:
                return null;
        }
    }

    private PropertyDeclarationSyntax MakeHasValueFieldProperty(Field field)
    {
        switch (field.Type.Tag)
        {
            case TypeTag.Struct:
            case TypeTag.List:
                return MakeHasStructProperty(field);
            default:
                return null;
        }
    }

    public StructDeclarationSyntax MakeReaderStruct(TypeDefinition def)
    {
        var readerDecl = StructDeclaration(_names.ReaderStruct.ToString()).AddModifiers(Public);

        var members = def.Tag == TypeTag.Group ? MakeGroupReaderStructMembers() : MakeReaderStructMembers();

        readerDecl = readerDecl.AddMembers(members.ToArray());

        if (def.UnionInfo != null) readerDecl = readerDecl.AddMembers(MakeReaderUnionSelector(def));

        foreach (var field in def.Fields)
        {
            var propDecl = MakeReaderFieldProperty(field);

            if (propDecl != null) readerDecl = readerDecl.AddMembers(propDecl);

            var hasValueDecl = MakeHasValueFieldProperty(field);
            if (hasValueDecl != null) readerDecl = readerDecl.AddMembers(hasValueDecl);
        }

        return readerDecl;
    }
}