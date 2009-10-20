using System;
using System.IO;
using System.IO.IsolatedStorage;

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

            using (var logger = new StreamWriter(new IsolatedStorageFileStream("Cuke4Nuke.log", FileMode.Append)))
            {
                logger.AutoFlush = true;
                logger.WriteLine(DateTime.Now.ToString() + "\tStarting logging...");
                new NukeServer(listener, logger, options).Start();
            }
        }
    }
}
