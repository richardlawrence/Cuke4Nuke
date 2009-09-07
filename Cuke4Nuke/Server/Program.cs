using System;
using NDesk.Options;
using System.Collections.Generic;

namespace Cuke4Nuke.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 3901;
            bool showHelp = false;
            List<string> assemblyPaths = new List<string>();

            var p = new OptionSet()
            {
                {
                    "p|port=", 
                    "the {PORT} the server should listen on.",
                    (int v) => port = v
                },
                {
                    "a|assembly=",
                    "an assembly to search for step definition methods.",
                    v => assemblyPaths.Add(v)
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

            Listener listener = new Listener(port, assemblyPaths.ToArray());
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
