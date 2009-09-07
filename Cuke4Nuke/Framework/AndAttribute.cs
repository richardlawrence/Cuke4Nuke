using System;

namespace Cuke4Nuke.Framework
{
    public class AndAttribute : StepDefinitionAttribute
    {
        public AndAttribute(string pattern) : base(pattern)
        {
        }
    }
}