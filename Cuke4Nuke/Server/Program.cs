using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Cuke4Nuke.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client;
            int port = 3901;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            TcpListener listener = new TcpListener(endPoint);
            listener.Start(0);
            Console.WriteLine("Listening on port " + port.ToString());
            while (true)
            {
                if (listener.Pending())
                {
                    client = listener.AcceptTcpClient();
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
