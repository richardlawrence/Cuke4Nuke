using System;
using System.Collections.Generic;

namespace Cuke4Nuke.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HookAttribute : Attribute
    {
        public HookAttribute()
        {
        }
        public HookAttribute(string tag)
        {
        }
    }
}
