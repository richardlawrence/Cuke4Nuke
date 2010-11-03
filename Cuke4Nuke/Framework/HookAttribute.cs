using System;

namespace Cuke4Nuke.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HookAttribute : Attribute
    {
        public string Tag { get; private set; }

        public HookAttribute()
        {
        }

        public HookAttribute(string tag)
        {
            Tag = tag;
        }
    }
}