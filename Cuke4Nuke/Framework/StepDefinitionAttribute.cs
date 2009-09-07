using System;

namespace Cuke4Nuke.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class StepDefinitionAttribute : Attribute
    {
        public StepDefinitionAttribute(string pattern)
        {
            _pattern = pattern;
        }

        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
            }
        }
    }
}
