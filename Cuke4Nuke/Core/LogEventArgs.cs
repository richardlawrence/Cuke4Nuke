using System;

namespace Cuke4Nuke.Core
{
    public class LogEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public LogEventArgs(string message)
        {
            Message = message;    
        }
    }
}