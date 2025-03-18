using System;
using System.Net;
using Capnp.Rpc;
using EchoServiceCapnp.Services;

namespace EchoServiceCapnp;

internal class Program
{
    private static void Main(string[] args)
    {
        using (var server = new TcpRpcServer())
        {
            server.AddBuffering();
            server.Main = new CapnpEchoService();
            server.StartAccepting(IPAddress.Loopback, 5002);
            Console.WriteLine("Press RETURN to stop listening");
            Console.ReadLine();
        }
    }
}