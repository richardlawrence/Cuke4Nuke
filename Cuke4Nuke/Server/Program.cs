using System;
using System.IO;
using System.IO.IsolatedStorage;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            var options = new Options(args);
            var objectFactory = new ObjectFactory();
            var loader = new Loader(options.AssemblyPaths, objectFactory);
            var processor = new Processor(loader, objectFactory);
            var listener = new Listener(processor, options.Port);
            log4net.Config.XmlConfigurator.Configure();

            new NukeServer(listener, options).Start();
        }
    }
}
