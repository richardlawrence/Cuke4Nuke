using System;
using NDesk.Options;

namespace Cuke4Nuke.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 3901;
            bool showHelp = false;

            var p = new OptionSet()
            {
                {
                    "p|port=", 
                    "the {PORT} the server should listen on.",
                    (int v) => port = v
                },
                {
                    "h|?|help",
                    "show this message and exit.",
                    v => showHelp = v != null
                }
            };
            p.Parse(args);

            if (showHelp)
            {
                ShowHelp(p);
                return;
            }

            Listener listener = new Listener(port);
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: Cuke4Nuke.Server.exe [OPTIONS]");
            Console.WriteLine("Start the Cuke4Nuke server to invoke .NET Cucumber step definitions.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
