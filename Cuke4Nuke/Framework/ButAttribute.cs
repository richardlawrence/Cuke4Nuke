using System;

namespace Cuke4Nuke.Framework
{
    public class ButAttribute : StepDefinitionAttribute
    {
        public ButAttribute(string pattern) : base(pattern)
        {
        }
    }
}