using System;
using System.Collections.Generic;
using System.Linq;

namespace CapnpC.CSharp.Generator.Model;

internal class DefinitionManager
{
    private readonly Dictionary<ulong, IDefinition> _id2def = new();

    public IEnumerable<GenFile> Files =>
        _id2def.Values.Where(d => d.Tag == TypeTag.File).Select(f => f as GenFile);

    public GenFile CreateFile(ulong id, bool isGenerated)
    {
        return CreateId<GenFile>(id, () => new GenFile(id, isGenerated));
    }

    public GenFile GetExistingFile(ulong id)
    {
        return GetId<GenFile>(id, TypeTag.File);
    }

    public TypeDefinition CreateTypeDef(ulong id, TypeTag tag, IHasNestedDefinitions decl)
    {
        return CreateId<TypeDefinition>(id, () => new TypeDefinition(tag, id, decl));
    }

    public TypeDefinition GetExistingTypeDef(ulong id, TypeTag tag)
    {
        var def = GetId<TypeDefinition>(id, tag);
        if (def.Tag == TypeTag.Unknown)
            def.Tag = tag;
        return def;
    }

    public Annotation CreateAnnotation(ulong id, IHasNestedDefinitions decl)
    {
        return CreateId<Annotation>(id, () => new Annotation(id, decl));
    }

    public Annotation GetExistingAnnotation(ulong id)
    {
        return GetId<Annotation>(id, TypeTag.Annotation);
    }

    public Constant CreateConstant(ulong id, IHasNestedDefinitions decl)
    {
        return CreateId<Constant>(id, () => new Constant(id, decl));
    }

    public Constant GetExistingConstant(ulong id)
    {
        return GetId<Constant>(id, TypeTag.Const);
    }

    public IDefinition GetExistingDef(ulong id, TypeTag tag)
    {
        return GetId<IDefinition>(id, tag);
    }

    private T CreateId<T>(ulong id, Func<IDefinition> creator)
        where T : class, IDefinition
    {
        if (_id2def.TryGetValue(id, out var d))
            throw new ArgumentException(
                nameof(id),
                $"Attempting to redefine {d.Tag.ToString()} {id.StrId()} (as {nameof(T)})."
            );
        var def = creator();
        _id2def.Add(id, def);
        return def as T;
    }

    private T GetId<T>(ulong id, TypeTag tag)
        where T : IDefinition
    {
        if (!_id2def.TryGetValue(id, out var anyDef))
            throw new ArgumentOutOfRangeException(
                $"Attempting to retrieve nonexistent node {id.StrId()}."
            );
        if (!(anyDef is T def) || (tag != TypeTag.Unknown && def.Tag != tag))
            throw new ArgumentOutOfRangeException(
                $"Attempting to retrieve {tag.ToString()} {id.StrId()}, but found {anyDef.Tag.ToString()} instead."
            );
        return def;
    }
}
