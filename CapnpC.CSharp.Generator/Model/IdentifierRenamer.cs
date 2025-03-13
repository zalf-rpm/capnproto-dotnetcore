﻿using Microsoft.CodeAnalysis.CSharp;

namespace CapnpC.CSharp.Generator.Model;

internal class IdentifierRenamer
{
    public static bool IsAnyKeyword(string str)
    {
        return SyntaxFacts.GetKeywordKind(str) != SyntaxKind.None
               || SyntaxFacts.GetContextualKeywordKind(str) != SyntaxKind.None;
    }

    public static string ToNonKeyword(string str)
    {
        // Capnp schema identifiers should be already valid, but could be a keyword
        if (IsAnyKeyword(str)) return $"@{str}";
        return str;
    }
}