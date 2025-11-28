namespace Capnp.Rpc;

internal class RpcProtocolErrorException : System.Exception
{
    public RpcProtocolErrorException(string reason)
        : base(reason) { }
}
