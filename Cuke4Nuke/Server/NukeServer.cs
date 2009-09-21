using System.IO;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class NukeServer
    {
        private readonly Listener _listener;
        readonly TextWriter _output;
        readonly Options _options;

        public NukeServer(Listener listener, TextWriter output, Options options)
        {
            _listener = listener;
            _options = options;
            _output = output;
        }

        public void Start()
        {
            if (_options.ShowHelp)
            {
                ShowHelp();
            }
            else
            {
                Run();
            }
        }

        void Run()
        {
            _listener.MessageLogged += listener_LogMessage;
            _listener.Start();
            _listener.Stop();
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