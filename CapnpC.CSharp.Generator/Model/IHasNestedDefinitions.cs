using System.Collections.Generic;
using System.Linq;

namespace CapnpC.CSharp.Generator.Model;

internal interface IHasNestedDefinitions
{
    IEnumerable<TypeDefinition> NestedTypes { get; }
    ICollection<IDefinition> NestedDefinitions { get; }
    ICollection<Constant> Constants { get; }
}

internal static class Extensions
{
    public static IEnumerable<TypeDefinition> GetNestedTypes(this IHasNestedDefinitions def)
    {
        return def.NestedDefinitions.Select(d => d as TypeDefinition).Where(d => d != null);
    }
}