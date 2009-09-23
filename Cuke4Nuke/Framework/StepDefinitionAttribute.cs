using System;

namespace Cuke4Nuke.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class StepDefinitionAttribute : Attribute
    {
        public string Pattern { get; private set; }

        protected StepDefinitionAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
