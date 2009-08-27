using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke
{
    class Program
    {   
        static void Main(string[] args)
        {
            // 1. read config from commad line args / .config file to tell us which stepdef assemblies to load
            // 2. load those assemblies into our appdomain
            // 3. reflect on the classes to find subclasses of StepDefinitions
            // 4. run all the DefineSteps methods on the StepDefinitions subclasses
            // 5. start the socket server and await instructions
        }
    }
}
