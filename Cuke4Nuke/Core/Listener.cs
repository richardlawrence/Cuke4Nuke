using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Cuke4Nuke.Core
{
    public class Listener
    {
        readonly IProcessor _processor;
        readonly int _port;
        bool _stopping;

        protected AutoResetEvent Started = new AutoResetEvent(false);
        protected AutoResetEvent Stopped = new AutoResetEvent(false);

        public EventHandler<LogEventArgs> MessageLogged;

        public Listener(IProcessor processor, int port)
        {
            _processor = processor;
            _port = port;
        }

        public virtual void Start()
        {
            Run();
        }

        public virtual void Stop()
        {
            _stopping = true;
            Log("Stopping.");
        }

        protected void Run()
        {
            var listener = StartTcpListener();

            Started.Set();

            while (!_stopping)
            {
                using (var client = WaitForClientToConnect(listener))
                {
                    if (!_stopping)
                    {
                        Process(client);
                        client.Close();
                    }
                }
            }

            listener.Stop();

            Stopped.Set();
        }

        TcpListener StartTcpListener()
        {
            var endPoint = new IPEndPoint(IPAddress.Any, _port);
            var listener = new TcpListener(endPoint);

            listener.Start(0);
            Log("Listening on port " + _port);

            return listener;
        }

        protected virtual TcpClient WaitForClientToConnect(TcpListener listener)
        {
            TcpClient client = null;

            Log("Waiting for client to connect.");

            while (!_stopping)
            {
                if (listener.Pending())
                {
                    client = listener.AcceptTcpClient();
                    Log("Connected to client.");
                    break;
                }
                Thread.Sleep(100);
            }

            return client;
        }

        protected virtual void Process(TcpClient client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            try
            {
                while (!_stopping)
                {
                    var request = GetRequest(reader);
                    if (request == null)
                    {
                        Log("Client disconnected.");
                        break;
                    }

                    var response = _processor.Process(request);
                    Write(response, writer);
                }
            }
            catch (IOException x)
            {
                Log("Exception: " + x.Message);
            }
        }

        protected virtual string GetRequest(StreamReader reader)
        {
            Log("Waiting for request.");
            var command = reader.ReadLine();
            Log("Received request <" + command + ">.");
            return command;
        }

        void Write(string response, StreamWriter writer)
        {
            Log("Responded with <" + response + ">.");
            writer.WriteLine(response);
            writer.Flush();
        }

        protected void Log(string message)
        {
            var handler = MessageLogged;

            if (handler != null)
            {
                handler(this, new LogEventArgs(message));
            }
        }
    }
}