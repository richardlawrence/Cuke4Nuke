using System;
using System.Collections.Generic;

namespace Cuke4Nuke.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HookAttribute : Attribute
    {
        public string Tag
        {
            get
            {
                return Tags.Count > 0 ? Tags[0] : null;
            }
        }

        public List<string> Tags { get; private set; }

        public HookAttribute()
        {
            Tags = new List<string>();
        }

        public HookAttribute(params string[] tags)
        {
            Tags = new List<string>(tags);
        }
    }
}