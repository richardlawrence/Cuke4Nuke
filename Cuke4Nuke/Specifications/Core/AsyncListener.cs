using System.Threading;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    internal class AsyncListener : Listener
    {
        readonly Thread _thread;

        public int ReadTimeout { get; set; }

        public AsyncListener(IProcessor processor, int port)
            : base(processor, port)
        {
            _thread = new Thread(Run) { Name = "AsyncListener" };
        }

        public override void Start()
        {
            _thread.Start();
            Started.WaitOne();
        }

        public override void Stop()
        {
            base.Stop();

            Log("Waiting for stopped event.");
            Stopped.WaitOne();

            Log("Waiting for listener thread to complete.");
            _thread.Join();
        }
    }
}