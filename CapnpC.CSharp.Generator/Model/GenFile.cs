using System.Collections.Generic;

namespace CapnpC.CSharp.Generator.Model;

internal class GenFile : IDefinition, IHasNestedDefinitions
{
    public GenFile(ulong id, bool isGenerated)
    {
        Id = id;
        IsGenerated = isGenerated;
    }

    public string Name { get; set; }
    public string[] Namespace { get; set; }
    public bool? NullableEnable { get; set; }
    public bool EmitNullableDirective { get; set; }
    public bool EmitDomainClassesAndInterfaces { get; set; }
    public string HeaderText { get; set; }
    public SupportedAnnotations.TypeVisibility TypeVisibility { get; set; }
    public ulong Id { get; }
    public bool IsGenerated { get; }
    public TypeTag Tag => TypeTag.File;
    public IHasNestedDefinitions DeclaringElement => null;

    public IEnumerable<TypeDefinition> NestedTypes => this.GetNestedTypes();
    public ICollection<IDefinition> NestedDefinitions { get; } = new List<IDefinition>();
    public ICollection<Constant> Constants { get; } = new List<Constant>();
}
