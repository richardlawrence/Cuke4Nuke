using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Net.Sockets;

namespace Test
{
    [TestFixture]
    public class Server_Specification
    {
        Process serverProcess;

        [SetUp]
        public void Setup()
        {
            string serverExePath = @"..\..\..\Server\bin\Debug\Cuke4Nuke.Server.exe";
            serverProcess = Process.Start(serverExePath);
        }

        [TearDown]
        public void Teardown()
        {
            serverProcess.CloseMainWindow();
        }

        [Test]
        public void ShouldAnswerOverTcp()
        {
            int port = 3901;
            TcpClient client = new TcpClient("localhost", port);
            Assert.That(client.Connected);
        }
    }
}
