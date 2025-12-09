using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CapnpC.CSharp.Generator.Model;

internal class TypeDefinition
    : AbstractType,
        IDefinition,
        IHasNestedDefinitions,
        IHasGenericParameters
{
    public TypeDefinition(TypeTag tag, ulong id, IHasNestedDefinitions parent)
    {
        Trace.Assert(parent != null);
        Tag = tag;
        Id = id;
        IsGenerated = (parent as IDefinition).IsGenerated;
        DeclaringElement = parent;
        if (tag == TypeTag.Group)
            ((TypeDefinition)parent).NestedGroups.Add(this);
        else
            parent.NestedDefinitions.Add(this);
    }

    public Method UsingMethod { get; set; }
    public string Name { get; set; }
    public string CsName { get; set; }
    public SpecialName SpecialName { get; set; }
    public DiscriminationInfo UnionInfo { get; set; }
    public List<Field> Fields { get; } = new();
    public List<Enumerant> Enumerants { get; } = new();
    public List<TypeDefinition> NestedGroups { get; } = new();
    public List<Method> Methods { get; } = new();
    public List<Type> Superclasses { get; } = new();
    public bool IsGeneric { get; set; }
    public ushort StructDataWordCount { get; set; }
    public ushort StructPointerCount { get; set; }

    public IEnumerable<TypeDefinition> DefinitionHierarchy
    {
        get
        {
            IHasNestedDefinitions cur = this;

            while (cur is TypeDefinition def)
            {
                yield return def;
                cur = def.DeclaringElement;
            }
        }
    }

    public GenFile File
    {
        get
        {
            IHasNestedDefinitions cur = this;
            while (cur is TypeDefinition def)
                cur = def.DeclaringElement;
            return cur as GenFile;
        }
    }

    public IEnumerable<GenericParameter> AllTypeParameters =>
        from def in DefinitionHierarchy
        from p in def.GetLocalTypeParameters()
        select p;

    public ulong Id { get; }
    public bool IsGenerated { get; }
    public IHasNestedDefinitions DeclaringElement { get; }
    public List<string> GenericParameters { get; } = new();
    public ICollection<IDefinition> NestedDefinitions { get; } = new List<IDefinition>();
    public IEnumerable<TypeDefinition> NestedTypes => this.GetNestedTypes();
    public ICollection<Constant> Constants { get; } = new List<Constant>();

    public class DiscriminationInfo
    {
        public DiscriminationInfo(ushort numOptions, uint tagOffset)
        {
            NumOptions = numOptions;
            TagOffset = tagOffset;
        }

        public ushort NumOptions { get; }
        public uint TagOffset { get; }
    }
}
