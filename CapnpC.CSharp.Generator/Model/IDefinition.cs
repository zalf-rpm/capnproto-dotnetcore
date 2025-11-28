namespace CapnpC.CSharp.Generator.Model;

internal interface IDefinition
{
    ulong Id { get; }
    bool IsGenerated { get; }
    TypeTag Tag { get; }
    IHasNestedDefinitions DeclaringElement { get; }
}
