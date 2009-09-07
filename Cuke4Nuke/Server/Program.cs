using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

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
                    Console.WriteLine("Connected to client.");
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            while (true)
            {
                Console.WriteLine("Waiting for command.");
                string command = reader.ReadLine();
                Console.WriteLine("Received command <" + command + ">.");
                switch (command)
                {
                    case "list_step_definitions":
                        writer.WriteLine("[]");
                        writer.Flush();
                        break;
                    default:
                        writer.WriteLine("ERROR: Command not recognized.");
                        break;
                }
            }
        }
    }
}
