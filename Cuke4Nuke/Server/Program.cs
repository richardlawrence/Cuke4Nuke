using System;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new Options(args);
            var loader = new Loader(options.AssemblyPaths);
            var processor = new Processor(loader);
            var listener = new Listener(processor, options.Port);

            new NukeServer(listener, Console.Out, options).Start();
        }
    }
}
