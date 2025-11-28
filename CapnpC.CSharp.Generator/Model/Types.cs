using System;

namespace CapnpC.CSharp.Generator.Model;

internal static class Types
{
    public static readonly Type Void = new(TypeTag.Void);
    public static readonly Type Bool = new(TypeTag.Bool);
    public static readonly Type S8 = new(TypeTag.S8);
    public static readonly Type U8 = new(TypeTag.U8);
    public static readonly Type S16 = new(TypeTag.S16);
    public static readonly Type U16 = new(TypeTag.U16);
    public static readonly Type S32 = new(TypeTag.S32);
    public static readonly Type U32 = new(TypeTag.U32);
    public static readonly Type S64 = new(TypeTag.S64);
    public static readonly Type U64 = new(TypeTag.U64);
    public static readonly Type F32 = new(TypeTag.F32);
    public static readonly Type F64 = new(TypeTag.F64);
    public static readonly Type AnyPointer = new(TypeTag.AnyPointer);
    public static readonly Type StructPointer = new(TypeTag.StructPointer);
    public static readonly Type ListPointer = new(TypeTag.ListPointer);
    public static readonly Type CapabilityPointer = new(TypeTag.CapabilityPointer);
    public static readonly Type Data = new(TypeTag.Data);
    public static readonly Type Text = new(TypeTag.Text);
    public static readonly Type AnyEnum = new(TypeTag.AnyEnum);

    public static Type List(Type elementType)
    {
        return new Type(TypeTag.List) { ElementType = elementType };
    }

    public static Type FromDefinition(TypeDefinition def)
    {
        if (def.Tag == TypeTag.Unknown)
            throw new InvalidOperationException(
                "Oops, type definition is not yet valid, cannot create type"
            );

        return new Type(def.Tag) { Definition = def };
    }

    public static Type FromParameter(GenericParameter genericParameter)
    {
        return new Type(TypeTag.AnyPointer) { Parameter = genericParameter };
    }
}
