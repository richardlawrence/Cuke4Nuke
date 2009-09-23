using System;
using System.Diagnostics;
using System.Net.Sockets;

using Cuke4Nuke.Specifications.Core;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Server
{
    [TestFixture]
    public class Program_Specification
    {
        Process _serverProcess;
        TcpClient _client;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var startInfo = new ProcessStartInfo("Cuke4Nuke.Server.exe", "-p " + Listener_Specification.TestPort)
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            _serverProcess = Process.Start(startInfo);

            _client = new TcpClient("localhost", Listener_Specification.TestPort);
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            try
            {
                _serverProcess.Kill();
            }
            catch (Exception)
            {
            }
            _client.Close();
        }

        [Test]
        public void Starting_server_process_should_start_the_server()
        {
            Assert.That(_client.Connected);
        }
    }
}
