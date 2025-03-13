using System.Diagnostics;

namespace CapnpC.CSharp.Generator.Model;

internal class Constant : IDefinition
{
    public Constant(ulong id, IHasNestedDefinitions parent)
    {
        Trace.Assert(parent != null);
        Id = id;
        IsGenerated = (parent as IDefinition).IsGenerated;
        DeclaringElement = parent;
        parent.NestedDefinitions.Add(this);
    }

    public Value Value { get; set; }
    public ulong Id { get; }
    public bool IsGenerated { get; }
    public TypeTag Tag => TypeTag.Const;
    public IHasNestedDefinitions DeclaringElement { get; }
}