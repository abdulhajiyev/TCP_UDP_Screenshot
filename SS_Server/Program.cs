using System;
using System.Net;
using System.Net.Sockets;

namespace SS_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = IPAddress.Loopback;
            var listener = new TcpListener(ip, 9290);


            listener.Start(100);
            Console.WriteLine("Listening on {0}", listener.LocalEndpoint);
        }
    }
}
