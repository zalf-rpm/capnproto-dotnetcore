using System.Net;

namespace Capnp.Net.Runtime.Tests.Util;

internal class TcpManager
{
    public static readonly TcpManager Instance = new();

    private readonly byte[] _nextAddress;
    private readonly int _nextPort = 50005;

    public TcpManager()
    {
        _nextAddress = new byte[] { 127, 0, 0, 1 };
    }

    public (IPAddress, int) GetLocalAddressAndPort()
    {
        if (++_nextAddress[2] == 0 && ++_nextAddress[1] == 0 && ++_nextAddress[0] == 0)
            _nextAddress[0] = 2;

        return (new IPAddress(_nextAddress), _nextPort);
    }
}
