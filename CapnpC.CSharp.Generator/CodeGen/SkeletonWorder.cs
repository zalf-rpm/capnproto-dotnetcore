using Capnp.Rpc;

namespace CapnpC.CSharp.Generator.CodeGen;

internal abstract class SkeletonWorder : Skeleton<object>
{
    public const string SetMethodTableName = nameof(SetMethodTable);
    public const string ImplName = nameof(Impl);
}