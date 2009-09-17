using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

using Cuke4Nuke.Core;
using Cuke4Nuke.Server;
using Cuke4Nuke.Specifications.Core;

using NUnit.Framework;

using Rhino.Mocks;

namespace Cuke4Nuke.Specifications.Server
{
    [TestFixture]
    public class NukeServer_Specification
    {
        StringWriter _outputWriter;
        Process _serverProcess;
        TcpClient _client;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var serverExePath = @"..\..\..\Server\bin\Debug\Cuke4Nuke.Server.exe";
            var startInfo = new ProcessStartInfo(serverExePath, "-p " + Listener_Specification.TestPort)
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

        [SetUp]
        public void SetUp()
        {
            _outputWriter = new StringWriter();
        }

        [Test]
        public void Help_option_should_show_usage_help()
        {
            var factory = MockRepository.GenerateMock<Factory>(new Options("-h"));

            new NukeServer(factory, _outputWriter);

            StringAssert.StartsWith("Usage:", _outputWriter.ToString());
        }

        [Test]
        public void Should_start_all_collaborators()
        {
            var options = new Options();
            var stepDefinitions = new List<StepDefinition>();
            var processor = new Processor(stepDefinitions);

            var factory = MockRepository.GenerateMock<Factory>(options);
            var loader = MockRepository.GenerateMock<Loader>(options);
            var listener = MockRepository.GenerateMock<Listener>(processor, options);

            factory.Expect(f => f.GetLoader()).Return(loader);
            factory.Expect(f => f.GetProcessor(Arg.Is(stepDefinitions))).Return(processor);
            factory.Expect(f => f.GetListener(Arg.Is(processor))).Return(listener);

            loader.Expect(l => l.Load()).Return(stepDefinitions);
            listener.Expect(l => l.Start());
            listener.Expect(l => l.Stop());

            new NukeServer(factory, _outputWriter);

            loader.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            listener.VerifyAllExpectations();
        }

        [Test]
        public void Should_allow_clients_to_connect_over_socket()
        {
            Assert.That(_client.Connected);
        }
    }
}