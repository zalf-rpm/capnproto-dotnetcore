﻿using System;
using System.Collections.Generic;
using System.Linq;
using Capnp;
using CapnpC.CSharp.Generator.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CapnpC.CSharp.Generator.CodeGen;

using static SyntaxFactory;
using static SyntaxHelpers;

internal class CodeGenerator
{
    private readonly CommonSnippetGen _commonGen;
    private readonly DomainClassSnippetGen _domClassGen;
    private readonly InterfaceSnippetGen _interfaceGen;
    private readonly SchemaModel _model;
    private readonly GenNames _names;
    private readonly GeneratorOptions _options;
    private readonly ReaderSnippetGen _readerGen;
    private readonly WriterSnippetGen _writerGen;

    public CodeGenerator(SchemaModel model, GeneratorOptions options)
    {
        _model = model;
        _options = options;
        _names = new GenNames(options);
        _commonGen = new CommonSnippetGen(_names);
        _domClassGen = new DomainClassSnippetGen(_names);
        _readerGen = new ReaderSnippetGen(_names);
        _writerGen = new WriterSnippetGen(_names);
        _interfaceGen = new InterfaceSnippetGen(_names);
    }

    internal GenNames GetNames()
    {
        return _names;
    }

    private IEnumerable<MemberDeclarationSyntax> TransformEnum(TypeDefinition def)
    {
        yield return _commonGen.MakeEnum(def);
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

    private IEnumerable<MemberDeclarationSyntax> TransformStruct(TypeDefinition def)
    {
        var topDecl = ClassDeclaration(_names.MakeTypeName(def).Identifier)
            .AddModifiers(_names.TypeVisibilityModifier);

        if (_names.EmitDomainClassesAndInterfaces)
            topDecl = topDecl.AddBaseListTypes(
                SimpleBaseType(_names.Type<ICapnpSerializable>(Nullability.NonNullable)));
        else
            topDecl = topDecl.AddModifiers(Static);


        if (def.GenericParameters.Count > 0)
            topDecl = topDecl
                .AddTypeParameterListParameters(MakeTypeParameters(def).ToArray())
                .AddConstraintClauses(MakeTypeParameterConstraints(def).ToArray());

        topDecl = topDecl
            .AddMembers(_names.MakeTypeIdConst(def.Id))
            .AddAttributeLists(_names.MakeTypeDecorationAttributes(def.Id));

        if (def.UnionInfo != null) topDecl = topDecl.AddMembers(_commonGen.MakeUnionSelectorEnum(def));

        if (_names.EmitDomainClassesAndInterfaces)
            topDecl = topDecl.AddMembers(_domClassGen.MakeDomainClassMembers(def));

        topDecl = topDecl.AddMembers(
            _readerGen.MakeReaderStruct(def),
            _writerGen.MakeWriterStruct(def));

        foreach (var nestedGroup in def.NestedGroups) topDecl = topDecl.AddMembers(Transform(nestedGroup).ToArray());

        foreach (var nestedDef in def.NestedTypes) topDecl = topDecl.AddMembers(Transform(nestedDef).ToArray());

        yield return topDecl;
    }

    private IEnumerable<MemberDeclarationSyntax> TransformInterface(TypeDefinition def)
    {
        if (!_names.EmitDomainClassesAndInterfaces)
            yield break;

        yield return _interfaceGen.MakeInterface(def);
        yield return _interfaceGen.MakeProxy(def);
        yield return _interfaceGen.MakeSkeleton(def);

        if (def.NestedTypes.Any())
        {
            var ns = ClassDeclaration(
                    _names.MakeTypeName(def, NameUsage.Namespace).ToString())
                .AddModifiers(Public, Static);

            if (def.GenericParameters.Count > 0)
                ns = ns
                    .AddTypeParameterListParameters(MakeTypeParameters(def).ToArray())
                    .AddConstraintClauses(MakeTypeParameterConstraints(def).ToArray());

            foreach (var nestedDef in def.NestedTypes) ns = ns.AddMembers(Transform(nestedDef).ToArray());

            yield return ns;
        }
    }

    private IEnumerable<MemberDeclarationSyntax> Transform(TypeDefinition def)
    {
        switch (def.Tag)
        {
            case TypeTag.Enum:
                return TransformEnum(def);

            case TypeTag.Group:
            case TypeTag.Struct:
                return TransformStruct(def);

            case TypeTag.Interface:
                return TransformInterface(def);

            default:
                throw new NotSupportedException($"Cannot declare type of kind {def.Tag} here");
        }
    }

    private ClassDeclarationSyntax TransformForPipeliningSupport(GenFile file)
    {
        var classDecl = default(ClassDeclarationSyntax);

        var q = new Queue<TypeDefinition>();

        foreach (var inner in file.NestedTypes) q.Enqueue(inner);

        while (q.Count > 0)
        {
            var cur = q.Dequeue();

            if (cur.Tag == TypeTag.Interface && _interfaceGen.RequiresPipeliningSupport(cur))
            {
                var members = _interfaceGen.MakePipeliningSupport(cur).ToArray();

                if (members.Length > 0)
                {
                    if (classDecl == null)
                        classDecl = ClassDeclaration(_names.MakePipeliningSupportExtensionClassName(file).Identifier)
                            .AddModifiers(_names.TypeVisibilityModifier, Static, Partial);

                    classDecl = classDecl.AddMembers(members);
                }
            }

            foreach (var inner in cur.NestedTypes) q.Enqueue(inner);
        }

        return classDecl;
    }

    internal string Transform(GenFile file)
    {
        _names.NullableEnable = file.NullableEnable ?? _options.NullableEnableDefault;
        _names.EmitDomainClassesAndInterfaces = file.EmitDomainClassesAndInterfaces;
        _names.TypeVisibility = file.TypeVisibility;

        var topNamespace = GenNames.NamespaceName(file.Namespace) ?? _names.TopNamespace;

        var ns = NamespaceDeclaration(topNamespace);

        if (file.EmitNullableDirective)
            ns = ns.WithLeadingTrivia(
                    Trivia(
                        NullableDirectiveTrivia(
                            Token(_names.NullableEnable ? SyntaxKind.EnableKeyword : SyntaxKind.DisableKeyword),
                            true)))
                .WithTrailingTrivia(
                    Trivia(
                        NullableDirectiveTrivia(
                            Token(SyntaxKind.RestoreKeyword),
                            true)));

        foreach (var def in file.NestedTypes) ns = ns.AddMembers(Transform(def).ToArray());

        if (_names.EmitDomainClassesAndInterfaces)
        {
            var psc = TransformForPipeliningSupport(file);

            if (psc != null) ns = ns.AddMembers(psc);
        }

        var cu = CompilationUnit().AddUsings(
            UsingDirective(ParseName("Capnp")),
            UsingDirective(ParseName("Capnp.Rpc")),
            UsingDirective(ParseName("System")),
            UsingDirective(ParseName("System.CodeDom.Compiler")),
            UsingDirective(ParseName("System.Collections.Generic")));

        if (_names.NullableEnable)
            cu = cu.AddUsings(
                UsingDirective(ParseName("System.Diagnostics.CodeAnalysis")));

        cu = cu.AddUsings(
            UsingDirective(ParseName("System.Threading")),
            UsingDirective(ParseName("System.Threading.Tasks")));

        cu = cu.AddMembers(ns);

        var content = cu.NormalizeWhitespace("    ", Environment.NewLine).ToFullString();

        if (!string.IsNullOrWhiteSpace(file.HeaderText)) content = file.HeaderText + content;

        return content;
    }

    public IReadOnlyList<FileGenerationResult> Generate()
    {
        var result = new List<FileGenerationResult>();

        foreach (var file in _model.FilesToGenerate)
            try
            {
                result.Add(new FileGenerationResult(file.Name, Transform(file)));
            }
            catch (Exception exception)
            {
                result.Add(new FileGenerationResult(file.Name, exception));
            }

        return result;
    }
}