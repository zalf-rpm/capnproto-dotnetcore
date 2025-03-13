﻿namespace CapnpC.CSharp.Generator.Model;

internal class Enumerant
{
    private string _literal;
    public TypeDefinition TypeDefinition { get; set; }

    public string Literal
    {
        get => _literal;
        set => _literal = IdentifierRenamer.ToNonKeyword(value);
    }

    public string CsLiteral { get; set; }
    public ushort? Ordinal { get; set; }
    public int CodeOrder { get; set; }
}