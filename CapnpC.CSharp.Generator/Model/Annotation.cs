using System.Diagnostics;

namespace CapnpC.CSharp.Generator.Model;

internal class Annotation : IDefinition
{
    public Annotation(ulong id, IHasNestedDefinitions parent)
    {
        Trace.Assert(parent != null);
        Id = id;
        IsGenerated = (parent as IDefinition).IsGenerated;
        DeclaringElement = parent;
        parent.NestedDefinitions.Add(this);
    }

    public Type Type { get; set; }
    public ulong Id { get; }
    public bool IsGenerated { get; }
    public TypeTag Tag => TypeTag.Annotation;
    public IHasNestedDefinitions DeclaringElement { get; }
}