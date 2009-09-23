using System;
using System.IO;
using System.Net.Sockets;

using Cuke4Nuke.Core;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Listener_Specification
    {
        public const int TestPort = 3902;
        
        static bool _logging;

        TcpClient _client;
        AsyncListener _listener;
        MockProcessor _processor;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _logging = false;
        }

        [SetUp]
        public void SetUp()
        {
            StartListener();
            StartClient();
        }

        [TearDown]
        public void TearDown()
        {
            StopClient();
            StopListener();
        }

        [Test]
        public void Should_read_request_from_client_socket()
        {
            ProcessRequest("hunky");

            Assert.That(_processor.Request, Is.EqualTo("hunky"));
        }

        [Test]
        public void Should_write_response_to_client_socket()
        {
            _processor.Response = "dory";
            var response = ProcessRequest("hunky");

            Assert.That(response, Is.EqualTo("dory"));
        }

        [Test]
        public void Should_rewait_for_clients_if_client_exits()
        {
            StopClient();
            StartClient();

            _processor.Response = "dory";
            var response = ProcessRequest("hunky");

            Assert.That(response, Is.EqualTo("dory"));
        }

        void StartClient()
        {
            _client = new TcpClient("localhost", TestPort);
        }

        void StopClient()
        {
            _client.Close();
        }

        void StartListener()
        {
            _processor = new MockProcessor();
            _listener = new AsyncListener(_processor, TestPort);
            _listener.MessageLogged += _listener_MessageLogged;
            _listener.Start();
        }

        void StopListener()
        {
            _listener.Stop();
        }

        string ProcessRequest(string request)
        {
            SendRequest(request);
            return GetResponse();
        }

        void SendRequest(string request)
        {
            var writer = new StreamWriter(_client.GetStream());
            writer.WriteLine(request);
            writer.Flush();
        }

        string GetResponse()
        {
            var reader = new StreamReader(_client.GetStream());
            return reader.ReadLine();
        }

        private static void _listener_MessageLogged(object sender, LogEventArgs e)
        {
            if (_logging)
            {
                Console.WriteLine(e.Message);
            }
        }

        class MockProcessor : IProcessor
        {
            public string Request;
            public string Response;

            public string Process(string request)
            {
                Request = request;
                return Response;
            }
        }
    }
}