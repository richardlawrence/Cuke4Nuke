using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;

namespace Test
{
    [TestFixture]
    public class Server_Specification
    {
        Process serverProcess;
        int port = 3901;
        TcpClient client;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            // launch the Cuke4Nuke server in a separate process
            string serverExePath = @"..\..\..\Server\bin\Debug\Cuke4Nuke.Server.exe";
            serverProcess = Process.Start(serverExePath);

            // connect to the Cuke4Nuker server over TCP
            client = new TcpClient("localhost", port);
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            // kill the Cuke4Nuker server process, swallowing the exception to avoid the 
            // uncaught exception dialog
            try
            {
                serverProcess.Kill();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void ShouldAnswerOverTcp()
        {
            Assert.That(client.Connected);
        }

        [Test]
        [Timeout(5000)]
        public void ShouldRespondToListStepDefinitionsWithEmptyArray()
        {
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            writer.WriteLine("list_step_definitions");
            writer.Flush();
            string response = reader.ReadLine();
            Assert.AreEqual("[]", response);
        }
    }
}
