using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class Listener
    {
        StepDefinitionRepository _repository = new StepDefinitionRepository();

        public Listener(int port, string[] assembliesToLoad)
        {
            foreach (string assemblyPath in assembliesToLoad)
            {
                _repository.Load(assemblyPath);
            }

            Start(port);
        }

        private void Start(int port)
        {
            TcpClient client;
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
                        writer.WriteLine(_repository.ListStepDefinitionsAsJson());
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
