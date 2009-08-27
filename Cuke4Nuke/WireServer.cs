using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke
{
    class WireServer
    {
        public void Start()
        {
            // start listening on he right socket.
            // loop waiting for commands. Call other methods on this class when commands come in
        }

        public void Stop()
        {
        }

        void ListStepDefinitions()
        {
            // ask the repository for the stepdefinitnions
            // create a string representation of the patterns (with an ID?)
            // send them back on the wire
        }

        // ? TODO: does cucumber leave us to figure out which stepdef matches the text, or give us the pattern key?
        // could be the regex or some other ID
        void InvokeStepDefinition(string id, string[] args)
        {
            // find in repo
            // invoke it (within try / catch)
            // return error code if it failed
        }
    }
}
