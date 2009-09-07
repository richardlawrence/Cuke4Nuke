using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
using LitJson;

namespace Test
{
    [TestFixture]
    public class Server_Specification_UsingTestStepDefinitions
    {
        Process serverProcess;
        int port = 3902;
        TcpClient client;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            // launch the Cuke4Nuke server in a separate process
            string serverExePath = @"..\..\..\Server\bin\Debug\Cuke4Nuke.Server.exe";
            string stepDefinitionAssemblyPath = @"..\..\..\TestStepDefinitions\bin\Debug\Cuke4Nuke.TestStepDefinitions.dll";
            string commandLineArgs = "-p " + port + " -a \"" + stepDefinitionAssemblyPath + "\"";
            serverProcess = Process.Start(serverExePath, commandLineArgs);

            // connect to the Cuke4Nuke server over TCP
            client = new TcpClient("localhost", port);
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            // kill the Cuke4Nuke server process, swallowing the exception to avoid the 
            // uncaught exception dialog
            try
            {
                serverProcess.Kill();
            }
            catch (Exception)
            {
            }
        }

        private string SendCommand(string command)
        {
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            writer.WriteLine(command);
            writer.Flush();
            string response = reader.ReadLine();
            return response;
        }

        [Test]
        [Timeout(5000)]
        public void ShouldRespondToListStepDefinitionsWithJsonArray()
        {
            string response = SendCommand("list_step_definitions");
            JsonData data = JsonMapper.ToObject(response);
            Assert.That(data.IsArray);
        }

        [Test]
        [Timeout(5000)]
        public void ShouldRespondToListStepDefinitionsWithJsonArrayOf2Elements()
        {
            string response = SendCommand("list_step_definitions");
            JsonData data = JsonMapper.ToObject(response);
            Assert.AreEqual(2, data.Count);
        }
    }
}
