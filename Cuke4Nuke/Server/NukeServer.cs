using System;
using System.IO;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class NukeServer
    {
        readonly TextWriter _output;
        readonly Options _options;

        static void Main(string[] args)
        {
            new NukeServer(new Factory(new Options(args)), Console.Out);
        }

        public NukeServer(Factory factory, TextWriter output)
        {
            _options = factory.Options;
            _output = output;
            if (_options.ShowHelp)
                ShowHelp();
            else
                Run(factory);
        }

        void Run(Factory factory)
        {
            var stepDefinitions = factory.GetLoader().Load();
            var processor = factory.GetProcessor(stepDefinitions);
            var listener = factory.GetListener(processor);

            listener.MessageLogged += listener_LogMessage;
            listener.Start();
            listener.Stop();
        }

        void ShowHelp()
        {
            Log("Usage: Cuke4Nuke.Server.exe [OPTIONS]");
            Log("Start the Cuke4Nuke server to invoke .NET Cucumber step definitions.");
            Log("");
            _options.Write(_output);
        }

        void listener_LogMessage(object sender, LogEventArgs e)
        {
            Log(e.Message);
        }

        void Log(string message)
        {
            _output.WriteLine(message);
        }
    }
}
