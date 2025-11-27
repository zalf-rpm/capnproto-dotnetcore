using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capnp;
using Capnp.Rpc;
using CapnpC.CSharp.Generator.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static CapnpC.CSharp.Generator.CodeGen.SyntaxHelpers;
using Type = CapnpC.CSharp.Generator.Model.Type;

namespace CapnpC.CSharp.Generator.CodeGen;

internal class InterfaceSnippetGen
{
    private readonly HashSet<(string, string)> _existingExtensionMethods = new();
    private readonly GenNames _names;

    public InterfaceSnippetGen(GenNames names)
    {
        _names = names;
    }

    private TypeSyntax TransformReturnType(Method method)
    {
        switch (method.Results.Count)
        {
            case 0:
                return IdentifierName(nameof(Task));

            case 1 when method.Results[0].Type.Tag == TypeTag.Struct:
                return GenericName(nameof(Task)).AddTypeArgumentListArguments(
                    _names.MakeTypeSyntax(method.Results[0].Type, method.DeclaringInterface, TypeUsage.DomainClass,
                        Nullability.NonNullable));

            case 1:
                return GenericName(nameof(Task)).AddTypeArgumentListArguments(
                    _names.MakeTypeSyntax(method.Results[0].Type, method.DeclaringInterface, TypeUsage.DomainClass,
                        Nullability.NullableRef));

            default:
                return GenericName(nameof(Task)).AddTypeArgumentListArguments(
                    TupleType(SeparatedList(
                        method.Results.Select(
                            f => TupleElement(_names.MakeTypeSyntax(f.Type, method.DeclaringInterface,
                                TypeUsage.DomainClass, Nullability.NullableRef))))));
        }
    }

    private ParameterSyntax[] TransformParameters(Method method)
    {
        var list = new List<ParameterSyntax>();

        if (method.Params.Count > 0)
        {
            var arg0 = method.Params[0];

            if (arg0.Name == null)
                list.Add(Parameter(_names.AnonymousParameter.Identifier)
                    .WithType(_names.MakeTypeSyntax(arg0.Type, method.DeclaringInterface, TypeUsage.DomainClass,
                        Nullability.NullableRef)));
            else
                foreach (var arg in method.Params)
                    list.Add(Parameter(Identifier(_names.GetCodeIdentifierLowerCamel(arg)))
                        .WithType(_names.MakeTypeSyntax(arg.Type, method.DeclaringInterface, TypeUsage.DomainClass,
                            Nullability.NullableRef)));
        }

        list.Add(Parameter(_names.CancellationTokenParameter.Identifier)
            .WithType(IdentifierName(nameof(CancellationToken)))
            .WithDefault(EqualsValueClause(LiteralExpression(
                SyntaxKind.DefaultLiteralExpression,
                Token(SyntaxKind.DefaultKeyword)))));

        return list.ToArray();
    }

    private IEnumerable<TypeParameterSyntax> MakeTypeParameters(TypeDefinition def)
    {
        foreach (var name in def.GenericParameters)
            yield return TypeParameter(_names.GetGenericTypeParameter(name).Identifier);
    }

    private IEnumerable<TypeParameterConstraintClauseSyntax> MakeTypeParameterConstraints(TypeDefinition def)
    {
        foreach (var name in def.GenericParameters)
            yield return TypeParameterConstraintClause(
                    _names.GetGenericTypeParameter(name).IdentifierName)
                .AddConstraints(ClassOrStructConstraint(SyntaxKind.ClassConstraint));
    }

    public MemberDeclarationSyntax MakeInterface(TypeDefinition type)
    {
        var ifaceDecl = InterfaceDeclaration(_names.MakeTypeName(type, NameUsage.Interface).Identifier)
            .AddModifiers(Public)
            .AddAttributeLists(
                _names.MakeTypeDecorationAttributes(type.Id)
                    .AddAttributes(
                        Attribute(IdentifierName("Proxy"))
                            .AddArgumentListArguments(
                                AttributeArgument(
                                    TypeOfExpression(_names.MakeGenericTypeNameForAttribute(type, NameUsage.Proxy)))),
                        Attribute(IdentifierName("Skeleton"))
                            .AddArgumentListArguments(
                                AttributeArgument(
                                    TypeOfExpression(
                                        _names.MakeGenericTypeNameForAttribute(type, NameUsage.Skeleton))))));

        if (type.GenericParameters.Count > 0)
            ifaceDecl = ifaceDecl
                .AddTypeParameterListParameters(MakeTypeParameters(type).ToArray())
                .AddConstraintClauses(MakeTypeParameterConstraints(type).ToArray());

        if (type.Superclasses.Count == 0)
            ifaceDecl = ifaceDecl.AddBaseListTypes(SimpleBaseType(IdentifierName(nameof(IDisposable))));
        else
            foreach (var superClass in type.Superclasses)
                ifaceDecl = ifaceDecl.AddBaseListTypes(
                    SimpleBaseType(_names.MakeTypeSyntax(
                        superClass, type,
                        TypeUsage.DomainClass,
                        Nullability.NonNullable)));

        foreach (var method in type.Methods)
        {
            var methodDecl = MethodDeclaration(
                    TransformReturnType(method),
                    _names.GetCodeIdentifier(method).Identifier)
                .AddParameterListParameters(TransformParameters(method))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

            if (method.GenericParameters.Count > 0)
                methodDecl = methodDecl
                    .AddTypeParameterListParameters(MakeTypeParameters(method).ToArray())
                    .AddConstraintClauses(MakeTypeParameterConstraints(method).ToArray());

            ifaceDecl = ifaceDecl.AddMembers(methodDecl);
        }

        return ifaceDecl;
    }

    private bool IsSubjectToPipelining(Type type, HashSet<Type> visited)
    {
        if (!visited.Add(type))
            return false;

        switch (type.Tag)
        {
            case TypeTag.AnyPointer:
            case TypeTag.CapabilityPointer:
            case TypeTag.Interface:
            case TypeTag.ListPointer:
            case TypeTag.StructPointer:
                return true;

            case TypeTag.List:
                return IsSubjectToPipelining(type.ElementType, visited);

            case TypeTag.Struct:
                return type.Fields.Any(f => IsSubjectToPipelining(f.Type, visited));

            default:
                return false;
        }
    }

    private bool IsSubjectToPipelining(Method method)
    {
        return method.Results.Any(r => IsSubjectToPipelining(r.Type, new HashSet<Type>()));
    }

    private IEnumerable<ExpressionSyntax> MakeProxyCallInitializerAssignments(Method method)
    {
        for (var i = 0; i < method.Params.Count; i++)
        {
            var methodParam = method.Params[i];
            var field = method.ParamsStruct.Fields[i];

            yield return AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                _names.GetCodeIdentifier(methodParam).IdentifierName,
                IdentifierName(_names.GetCodeIdentifierLowerCamel(field)));
        }
    }

    private IEnumerable<ArgumentSyntax> MakeProxyReturnResultTupleElements(Method method)
    {
        foreach (var item in method.ResultStruct.Fields)
            yield return Argument(MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                _names.ResultLocal.IdentifierName,
                _names.GetCodeIdentifier(item).IdentifierName));
    }

    private StatementSyntax MakeProxyReturnResult(Method method)
    {
        if (method.ResultStruct.Definition.SpecialName == SpecialName.MethodResultStruct)
        {
            if (method.ResultStruct.Fields.Count == 0) return ReturnStatement();

            return ReturnStatement(TupleExpression()
                .AddArguments(MakeProxyReturnResultTupleElements(method).ToArray()));
        }

        return ReturnStatement(_names.ResultLocal.IdentifierName);
    }

    private StatementSyntax MakeProxyCreateResult(Method method)
    {
        var resultType = method.ResultStruct;
        var domainType = _names.MakeTypeSyntax(resultType, method.DeclaringInterface, TypeUsage.DomainClass,
            Nullability.NonNullable);

        ExpressionSyntax createDomain = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(nameof(CapnpSerializable)),
                    GenericName(nameof(CapnpSerializable.Create))
                        .AddTypeArgumentListArguments(MakeNonNullableType(domainType))))
            .AddArgumentListArguments(
                Argument(_names.DeserializerLocal.IdentifierName));

        if (_names.NullableEnable)
            createDomain = PostfixUnaryExpression(
                SyntaxKind.SuppressNullableWarningExpression,
                createDomain);

        return LocalDeclarationStatement(
            VariableDeclaration(
                    IdentifierName("var"))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                                _names.ResultLocal.Identifier)
                            .WithInitializer(
                                EqualsValueClause(createDomain)))));
    }

    private IEnumerable<TypeParameterSyntax> MakeTypeParameters(Method method)
    {
        foreach (var name in method.GenericParameters)
            yield return TypeParameter(_names.GetGenericTypeParameter(name).Identifier);
    }

    private IEnumerable<TypeParameterConstraintClauseSyntax> MakeTypeParameterConstraints(Method method)
    {
        foreach (var name in method.GenericParameters)
            yield return TypeParameterConstraintClause(
                    _names.GetGenericTypeParameter(name).IdentifierName)
                .AddConstraints(ClassOrStructConstraint(SyntaxKind.ClassConstraint));
    }

    public MemberDeclarationSyntax MakeProxy(TypeDefinition type)
    {
        var classDecl = ClassDeclaration(_names.MakeTypeName(type, NameUsage.Proxy).Identifier)
            .AddAttributeLists(_names.MakeTypeDecorationAttributes(type.Id))
            .AddModifiers(Public)
            .AddBaseListTypes(
                SimpleBaseType(_names.Type<Proxy>(Nullability.NonNullable)),
                SimpleBaseType(_names.MakeGenericTypeName(type, NameUsage.Interface)));

        if (type.GenericParameters.Count > 0)
            classDecl = classDecl
                .AddTypeParameterListParameters(MakeTypeParameters(type).ToArray())
                .AddConstraintClauses(MakeTypeParameterConstraints(type).ToArray());

        var allMethods =
            from c in Types.FromDefinition(type).AllImplementedClasses
            from m in c.Methods
            select m;

        foreach (var method in allMethods)
        {
            var bodyStmts = new List<StatementSyntax>();

            bodyStmts.Add(LocalDeclarationStatement(
                VariableDeclaration(
                        IdentifierName("var"))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                    _names.ParamsLocal.Identifier)
                                .WithInitializer(
                                    EqualsValueClause(
                                        InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName(nameof(SerializerState)),
                                                GenericName(
                                                        Identifier(nameof(SerializerState.CreateForRpc)))
                                                    .WithTypeArgumentList(
                                                        TypeArgumentList(
                                                            SingletonSeparatedList(
                                                                _names.MakeTypeSyntax(
                                                                    method.ParamsStruct,
                                                                    method.ParamsStruct.Definition,
                                                                    TypeUsage.Writer,
                                                                    Nullability.NonNullable))))))))))));

            if (method.ParamsStruct.Definition.SpecialName == SpecialName.MethodParamsStruct)
                bodyStmts.Add(LocalDeclarationStatement(
                    VariableDeclaration(
                            IdentifierName("var"))
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                        _names.AnonymousParameter.Identifier)
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ObjectCreationExpression(
                                                    _names.MakeTypeSyntax(
                                                        method.ParamsStruct,
                                                        method.ParamsStruct.Definition,
                                                        TypeUsage.DomainClass,
                                                        Nullability.NonNullable))
                                                .WithArgumentList(
                                                    ArgumentList())
                                                .WithInitializer(
                                                    InitializerExpression(
                                                        SyntaxKind.ObjectInitializerExpression,
                                                        SeparatedList<ExpressionSyntax>(
                                                            CommonSnippetGen.MakeCommaSeparatedList(
                                                                    MakeProxyCallInitializerAssignments(method))
                                                                .ToArray())))))))));

            bodyStmts.Add(ExpressionStatement(
                ConditionalAccessExpression(
                    _names.AnonymousParameter.IdentifierName,
                    InvocationExpression(
                            MemberBindingExpression(_names.SerializeMethod.IdentifierName))
                        .AddArgumentListArguments(
                            Argument(_names.ParamsLocal.IdentifierName)))));

            var call = InvocationExpression(IdentifierName(nameof(BareProxy.Call)))
                .AddArgumentListArguments(
                    Argument(
                        LiteralExpression(SyntaxKind.NumericLiteralExpression,
                            Literal(method.DeclaringInterface.Id))),
                    Argument(
                        LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(method.Id))),
                    Argument(
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    _names.ParamsLocal.IdentifierName,
                                    GenericName(nameof(SerializerState.Rewrap))
                                        .AddTypeArgumentListArguments(
                                            _names.Type<DynamicSerializerState>(Nullability.NonNullable))))
                            .AddArgumentListArguments()),
                    Argument(
                        LiteralExpression(SyntaxKind.FalseLiteralExpression)),
                    Argument(
                        _names.CancellationTokenParameter.IdentifierName));

            MethodDeclarationSyntax methodDecl;

            if (IsSubjectToPipelining(method))
            {
                methodDecl = MethodDeclaration(
                        TransformReturnType(method),
                        _names.GetCodeIdentifier(method).Identifier)
                    .AddParameterListParameters(TransformParameters(method))
                    .AddModifiers(Public);

                var pipelineAwareCall = InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(Impatient)),
                            IdentifierName(nameof(Impatient.MakePipelineAware))))
                    .AddArgumentListArguments(
                        Argument(call),
                        Argument(SimpleLambdaExpression(
                            Parameter(_names.DeserializerLocal.Identifier),
                            Block(
                                UsingStatement(
                                        Block(
                                            MakeProxyCreateResult(method),
                                            MakeProxyReturnResult(method)))
                                    .WithExpression(_names.DeserializerLocal.IdentifierName)))));

                bodyStmts.Add(ReturnStatement(pipelineAwareCall));
            }
            else
            {
                methodDecl = MethodDeclaration(
                        TransformReturnType(method),
                        _names.GetCodeIdentifier(method).Identifier)
                    .AddParameterListParameters(TransformParameters(method))
                    .AddModifiers(Public, Token(SyntaxKind.AsyncKeyword));

                var whenReturned = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    call,
                    IdentifierName(nameof(IPromisedAnswer.WhenReturned)));

                bodyStmts.Add(UsingStatement(
                        Block(
                            MakeProxyCreateResult(method),
                            MakeProxyReturnResult(method)))
                    .WithDeclaration(VariableDeclaration(
                            IdentifierName("var"))
                        .AddVariables(
                            VariableDeclarator(
                                    _names.DeserializerLocal.Identifier)
                                .WithInitializer(
                                    EqualsValueClause(
                                        AwaitExpression(whenReturned))))));
            }

            if (method.GenericParameters.Count > 0)
                methodDecl = methodDecl
                    .AddTypeParameterListParameters(MakeTypeParameters(method).ToArray())
                    .AddConstraintClauses(MakeTypeParameterConstraints(method).ToArray());

            methodDecl = methodDecl.AddBodyStatements(bodyStmts.ToArray());

            classDecl = classDecl.AddMembers(methodDecl);
        }

        return classDecl;
    }

    private IEnumerable<ArgumentSyntax> MakeSkeletonSetMethodTableArguments(TypeDefinition def)
    {
        foreach (var method in def.Methods)
            if (method.GenericParameters.Count > 0)
                yield return Argument(
                    GenericName(_names.GetCodeIdentifier(method).ToString())
                        .AddTypeArgumentListArguments(
                            Enumerable.Repeat(
                                _names.Type<AnyPointer>(Nullability.NonNullable),
                                method.GenericParameters.Count).ToArray()));
            else
                yield return Argument(_names.GetCodeIdentifier(method).IdentifierName);
    }

    private IEnumerable<ExpressionSyntax> MakeSkeletonMethodResultStructInitializer(Method method)
    {
        foreach (var arg in method.Results)
            yield return AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                _names.GetCodeIdentifier(arg).IdentifierName,
                IdentifierName(_names.GetCodeIdentifierLowerCamel(arg)));
    }

    private IEnumerable<ArgumentSyntax> MakeSkeletonMethodCallArgs(Method method)
    {
        foreach (var arg in method.ParamsStruct.Fields)
            yield return Argument(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    _names.ParamsLocal.IdentifierName,
                    _names.GetCodeIdentifier(arg).IdentifierName));
    }

    private StatementSyntax MakeSkeletonMethodSerializerLocalDeclaration(Method method)
    {
        return LocalDeclarationStatement(
            VariableDeclaration(
                    IdentifierName("var"))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                                _names.SerializerLocal.Identifier)
                            .WithInitializer(
                                EqualsValueClause(
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName(nameof(SerializerState)),
                                            GenericName(
                                                    Identifier(nameof(SerializerState.CreateForRpc)))
                                                .WithTypeArgumentList(
                                                    TypeArgumentList(
                                                        SingletonSeparatedList(
                                                            _names.MakeTypeSyntax(
                                                                method.ResultStruct,
                                                                method.ResultStruct.Definition,
                                                                TypeUsage.Writer, Nullability.NonNullable)))))))))));
    }

    private CSharpSyntaxNode MakeMaybeTailCallLambdaBody(Method method)
    {
        var block = Block(
            MakeSkeletonMethodSerializerLocalDeclaration(method));

        if (method.ResultStruct.Definition.SpecialName == SpecialName.MethodResultStruct)
            block = block.AddStatements(
                LocalDeclarationStatement(
                    VariableDeclaration(
                            IdentifierName("var"))
                        .AddVariables(
                            VariableDeclarator(_names.ResultLocal.Identifier)
                                .WithInitializer(EqualsValueClause(ObjectCreationExpression(
                                        _names.MakeTypeSyntax(
                                            method.ResultStruct,
                                            method.ResultStruct.Definition,
                                            TypeUsage.DomainClass,
                                            Nullability.NonNullable))
                                    .WithInitializer(
                                        InitializerExpression(SyntaxKind.ObjectInitializerExpression)
                                            .AddExpressions(
                                                MakeSkeletonMethodResultStructInitializer(method).ToArray())))))));

        if (method.Results.Count > 0)
            block = block.AddStatements(
                ExpressionStatement(
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                _names.ResultLocal.IdentifierName,
                                _names.SerializeMethod.IdentifierName))
                        .AddArgumentListArguments(
                            Argument(_names.SerializerLocal.IdentifierName))));

        block = block.AddStatements(
            ReturnStatement(_names.SerializerLocal.IdentifierName));

        return block;
    }

    private IEnumerable<StatementSyntax> MakeSkeletonMethodBody(Method method)
    {
        SimpleNameSyntax methodName;

        if (method.GenericParameters.Count == 0)
            methodName = _names.GetCodeIdentifier(method).IdentifierName;
        else
            methodName = GenericName(_names.GetCodeIdentifier(method).Identifier)
                .AddTypeArgumentListArguments(
                    method.GenericParameters.Select(
                            p => _names.GetGenericTypeParameter(p).IdentifierName)
                        .ToArray());

        var call = InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(SkeletonWorder.ImplName),
                methodName));

        if (method.Params.Count > 0)
        {
            var paramsType = method.ParamsStruct;
            var domainType = _names.MakeTypeSyntax(paramsType, method.ParamsStruct.Definition, TypeUsage.DomainClass,
                Nullability.NonNullable);

            ExpressionSyntax createDomain = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(CapnpSerializable)),
                        GenericName(nameof(CapnpSerializable.Create))
                            .AddTypeArgumentListArguments(MakeNonNullableType(domainType))))
                .AddArgumentListArguments(
                    Argument(_names.DeserializerLocal.IdentifierName));

            if (_names.NullableEnable)
                createDomain = PostfixUnaryExpression(
                    SyntaxKind.SuppressNullableWarningExpression,
                    createDomain);

            if (method.ParamsStruct.Definition.SpecialName == SpecialName.MethodParamsStruct)
            {
                yield return LocalDeclarationStatement(
                    VariableDeclaration(
                            IdentifierName("var"))
                        .AddVariables(
                            VariableDeclarator(_names.ParamsLocal.Identifier)
                                .WithInitializer(EqualsValueClause(createDomain))));

                call = call.AddArgumentListArguments(
                    MakeSkeletonMethodCallArgs(method).ToArray());
            }
            else
            {
                call = call.AddArgumentListArguments(
                    Argument(createDomain));
            }
        }

        call = call.AddArgumentListArguments(
            Argument(
                _names.CancellationTokenParameter.IdentifierName));

        if (method.Results.Count == 0)
        {
            var awaitCall = AwaitExpression(call);
            yield return ExpressionStatement(awaitCall);
            yield return MakeSkeletonMethodSerializerLocalDeclaration(method);
            yield return ReturnStatement(_names.SerializerLocal.IdentifierName);
        }
        else
        {
            ExpressionSyntax lambdaArg;

            if (method.ResultStruct.Definition.SpecialName == SpecialName.MethodResultStruct)
            {
                if (method.Results.Count == 1)
                {
                    lambdaArg = SimpleLambdaExpression(
                        Parameter(Identifier(_names.GetCodeIdentifierLowerCamel(method.Results.Single()))),
                        MakeMaybeTailCallLambdaBody(method));
                }
                else
                {
                    // CodeAnalysis.CSharp 3.2.1 has a bug which prevents us from using AddParameterListParameters. :-(

                    var paramList = new List<SyntaxNodeOrToken>();
                    foreach (var arg in method.Results)
                    {
                        if (paramList.Count > 0)
                            paramList.Add(Token(SyntaxKind.CommaToken));
                        paramList.Add(Parameter(Identifier(_names.GetCodeIdentifierLowerCamel(arg))));
                    }

                    lambdaArg = ParenthesizedLambdaExpression(
                        ParameterList(
                            SeparatedList<ParameterSyntax>(paramList)),
                        MakeMaybeTailCallLambdaBody(method));
                }
            }
            else
            {
                lambdaArg = SimpleLambdaExpression(
                    Parameter(_names.ResultLocal.Identifier),
                    MakeMaybeTailCallLambdaBody(method));
            }

            var maybeTailCall = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(Impatient)),
                        IdentifierName(nameof(Impatient.MaybeTailCall))))
                .AddArgumentListArguments(
                    Argument(call),
                    Argument(lambdaArg));

            yield return ReturnStatement(maybeTailCall);
        }
    }

    private IEnumerable<MemberDeclarationSyntax> MakeSkeletonMethods(TypeDefinition def)
    {
        foreach (var method in def.Methods)
        {
            var methodDecl = MethodDeclaration(
                    _names.Type<Task<AnswerOrCounterquestion>>(Nullability.NonNullable),
                    _names.GetCodeIdentifier(method).Identifier)
                .AddParameterListParameters(
                    Parameter(_names.DeserializerLocal.Identifier)
                        .WithType(_names.Type<DeserializerState>(Nullability.NonNullable)),
                    Parameter(_names.CancellationTokenParameter.Identifier)
                        .WithType(_names.Type<CancellationToken>(Nullability.NonNullable)))
                .AddBodyStatements(
                    UsingStatement(
                            Block(
                                MakeSkeletonMethodBody(method).ToArray()))
                        .WithExpression(_names.DeserializerLocal.IdentifierName));

            if (method.Results.Count == 0) methodDecl = methodDecl.AddModifiers(Async);

            if (method.GenericParameters.Count > 0)
                methodDecl = methodDecl
                    .AddTypeParameterListParameters(MakeTypeParameters(method).ToArray())
                    .AddConstraintClauses(MakeTypeParameterConstraints(method).ToArray());

            yield return methodDecl;
        }
    }

    public MemberDeclarationSyntax MakeSkeleton(TypeDefinition type)
    {
        var name = _names.MakeTypeName(type, NameUsage.Skeleton).Identifier;
        var classDecl = ClassDeclaration(name)
            .AddAttributeLists(_names.MakeTypeDecorationAttributes(type.Id))
            .AddModifiers(Public)
            .AddBaseListTypes(
                SimpleBaseType(
                    GenericName(nameof(Skeleton))
                        .AddTypeArgumentListArguments(
                            _names.MakeGenericTypeName(type, NameUsage.Interface))))
            .AddMembers(
                // C'tor
                ConstructorDeclaration(name)
                    .AddModifiers(Public)
                    .AddBodyStatements(
                        ExpressionStatement(
                            InvocationExpression(
                                    IdentifierName(SkeletonWorder.SetMethodTableName))
                                .AddArgumentListArguments(
                                    MakeSkeletonSetMethodTableArguments(type).ToArray()))),
                // InterfaceId
                PropertyDeclaration(_names.Type<ulong>(Nullability.NonNullable), nameof(Skeleton<object>.InterfaceId))
                    .AddModifiers(Public, Override)
                    .WithExpressionBody(
                        ArrowExpressionClause(
                            ValueOf(type.Id)))
                    .WithSemicolonToken(
                        Token(SyntaxKind.SemicolonToken)));

        if (type.GenericParameters.Count > 0)
            classDecl = classDecl
                .AddTypeParameterListParameters(MakeTypeParameters(type).ToArray())
                .AddConstraintClauses(MakeTypeParameterConstraints(type).ToArray());

        classDecl = classDecl.AddMembers(MakeSkeletonMethods(type).ToArray());

        return classDecl;
    }

    public bool RequiresPipeliningSupport(TypeDefinition type)
    {
        return type.Methods.Any(m => ExpandPipeliningPaths(m).Any());
    }

    private IEnumerable<IReadOnlyList<Field>> ExpandPipeliningPaths(Method method)
    {
        var stack = new Stack<List<Field>>();
        foreach (var field in method.ResultStruct.Fields) stack.Push(new List<Field> { field });

        while (stack.Count > 0)
        {
            var path = stack.Pop();
            var last = path[path.Count - 1];

            switch (last.Type.Tag)
            {
                case TypeTag.Interface:
                case TypeTag.CapabilityPointer:
                    yield return path;
                    break;

                case TypeTag.Struct:
                    foreach (var field in last.Type.Fields)
                    {
                        if (path.Contains(field))
                            // Recursive structs protection
                            continue;

                        var copy = new List<Field>();
                        copy.AddRange(path);
                        copy.Add(field);
                        stack.Push(copy);
                    }

                    break;
            }
        }
    }

    private LocalFunctionStatementSyntax MakeLocalAwaitProxyFunction(Method method, IReadOnlyList<Field> path)
    {
        var members = new List<Name>();
        IEnumerable<Field> fields = path;

        if (method.Results.Count >= 2)
        {
            var index = Array.IndexOf(method.ResultStruct.Fields.ToArray(), path[0]) + 1;
            members.Add(new Name($"Item{index}"));
            fields = path.Skip(1);
        }

        foreach (var field in fields) members.Add(_names.GetCodeIdentifier(field));

        ExpressionSyntax memberAccess =
            ParenthesizedExpression(
                AwaitExpression(
                    _names.TaskParameter.IdentifierName));

        memberAccess = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            memberAccess,
            members.First().IdentifierName);

        foreach (var member in members.Skip(1))
            memberAccess = ConditionalAccessExpression(
                memberAccess,
                MemberBindingExpression(member.IdentifierName));

        var idisposable = _names.MakeNullableRefType(IdentifierName(nameof(IDisposable)));

        return LocalFunctionStatement(
                GenericName(
                        Identifier(nameof(Task)))
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SingletonSeparatedList(
                                idisposable))),
                _names.AwaitProxy.Identifier)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.AsyncKeyword)))
            .WithExpressionBody(
                ArrowExpressionClause(memberAccess))
            .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken));
    }

    public IEnumerable<MemberDeclarationSyntax> MakePipeliningSupport(TypeDefinition type)
    {
        foreach (var method in type.Methods)
            foreach (var path in ExpandPipeliningPaths(method))
            {
                if (path.Count == 1 && path[0].Offset == 0)
                    // The "trivial path" is already covered by the "Eager" extension method.
                    continue;

                var accessPath = _names.MakeMemberAccessPathFieldName(method, path);
                var methodName = _names.MakePipeliningSupportExtensionMethodName(path);
                var capType = path[path.Count - 1].Type;
                var capTypeSyntax = _names.MakeTypeSyntax(capType, null, TypeUsage.DomainClass, Nullability.NonNullable);

                if (!_existingExtensionMethods.Add((capTypeSyntax.ToString(), methodName.ToString()))) continue;

                var pathDecl = FieldDeclaration(
                        VariableDeclaration(
                                IdentifierName(nameof(MemberAccessPath)))
                            .AddVariables(
                                VariableDeclarator(
                                        accessPath.Identifier)
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ObjectCreationExpression(
                                                    IdentifierName(nameof(MemberAccessPath)))
                                                .AddArgumentListArguments(
                                                    path.Select(
                                                        f => Argument(
                                                            LiteralExpression(SyntaxKind.NumericLiteralExpression,
                                                                Literal(f.Offset)))).ToArray())))))
                    .AddModifiers(Static, Readonly);


                var methodDecl = MethodDeclaration(capTypeSyntax, methodName.Identifier)
                    .AddModifiers(Public, Static)
                    .AddParameterListParameters(
                        Parameter(
                                _names.TaskParameter.Identifier)
                            .AddModifiers(This)
                            .WithType(TransformReturnType(method)))
                    .AddBodyStatements(
                        MakeLocalAwaitProxyFunction(method, path),
                        ReturnStatement(
                            CastExpression(
                                capTypeSyntax,
                                InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName(nameof(CapabilityReflection)),
                                            GenericName(
                                                    Identifier(nameof(CapabilityReflection.CreateProxy)))
                                                .AddTypeArgumentListArguments(
                                                    capTypeSyntax)))
                                    .AddArgumentListArguments(
                                        Argument(
                                            InvocationExpression(
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName(nameof(Impatient)),
                                                        IdentifierName(nameof(Impatient.Access))))
                                                .AddArgumentListArguments(
                                                    Argument(
                                                        _names.TaskParameter.IdentifierName),
                                                    Argument(
                                                        accessPath.IdentifierName),
                                                    Argument(
                                                        InvocationExpression(
                                                            _names.AwaitProxy.IdentifierName))))))));

                if (type.GenericParameters.Count > 0)
                    methodDecl = methodDecl
                        .AddTypeParameterListParameters(MakeTypeParameters(type).ToArray())
                        .AddConstraintClauses(MakeTypeParameterConstraints(type).ToArray());

                yield return pathDecl;
                yield return methodDecl;
            }
    }
}