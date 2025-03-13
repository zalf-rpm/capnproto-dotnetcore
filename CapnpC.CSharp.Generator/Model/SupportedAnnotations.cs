﻿using System;
using System.Linq;
using CapnpC.CSharp.Generator.Schema;

namespace CapnpC.CSharp.Generator.Model;

internal static class SupportedAnnotations
{
    public enum TypeVisibility
    {
        Public = 0,
        Internal = 1
    }

    public static string[] GetNamespaceAnnotation(Node.READER fileNode)
    {
        foreach (var annotation in fileNode.Annotations)
        {
            if (annotation.Id == AnnotationIds.Cs.Namespace)
                return annotation.Value.Text.Split(new string[1] { "." }, default);

            if (annotation.Id == AnnotationIds.Cxx.Namespace)
                return annotation.Value.Text.Split(new string[1] { "::" }, default);
        }

        return null;
    }

    public static string GetCsName(Schema.Field.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.Name)
                return annotation.Value.Text;

        return null;
    }

    public static string GetCsName(Schema.Enumerant.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.Name)
                return annotation.Value.Text;

        return null;
    }

    public static string GetCsName(Node.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.Name)
                return annotation.Value.Text;

        return null;
    }

    public static string GetCsName(Schema.Method.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.Name)
                return annotation.Value.Text;

        return null;
    }

    public static bool? GetNullableEnable(Node.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.NullableEnable && annotation.Value.which == Schema.Value.WHICH.Bool)
                return annotation.Value.Bool;

        return null;
    }

    public static bool? GetEmitNullableDirective(Node.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.EmitNullableDirective &&
                annotation.Value.which == Schema.Value.WHICH.Bool)
                return annotation.Value.Bool;

        return null;
    }

    public static bool? GetEmitDomainClassesAndInterfaces(Node.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.EmitDomainClassesAndInterfaces &&
                annotation.Value.which == Schema.Value.WHICH.Bool)
                return annotation.Value.Bool;

        return null;
    }

    public static TypeVisibility? GetTypeVisibility(Node.READER node)
    {
        foreach (var annotation in node.Annotations)
            if (annotation.Id == AnnotationIds.Cs.TypeVisibility && annotation.Value.which == Schema.Value.WHICH.Enum)
                return (TypeVisibility)annotation.Value.Enum;

        return null;
    }

    public static string GetHeaderText(SourceInfo sourceInfo)
    {
        if (sourceInfo.DocComment == null)
            return null;

        var lines = sourceInfo.DocComment
            .Split('\n')
            .Select(line => line.Trim())
            .SkipWhile(line => !line.Equals("$$embed", StringComparison.OrdinalIgnoreCase))
            .Skip(1);

        return string.Join(Environment.NewLine, lines);
    }

    private static class AnnotationIds
    {
        public static class Cxx
        {
            public const ulong Namespace = 0xb9c6f99ebf805f2c;
        }

        public static class Cs
        {
            public const ulong Namespace = 0xeb0d831668c6eda0;
            public const ulong NullableEnable = 0xeb0d831668c6eda1;
            public const ulong Name = 0xeb0d831668c6eda2;
            public const ulong EmitNullableDirective = 0xeb0d831668c6eda3;
            public const ulong EmitDomainClassesAndInterfaces = 0xeb0d831668c6eda4;
            public const ulong TypeVisibility = 0xeb0d831668c6eda6;
        }
    }
}