using System;

namespace Cuke4Nuke.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 3901;
            Listener listener = new Listener(port);
        }
    }
}
