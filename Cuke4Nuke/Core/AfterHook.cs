using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Core
{
    public class AfterHook : Hook
    {
        public AfterHook(MethodInfo method)
            : base(method)
        {
            if (!AfterHook.IsValidMethod(method))
            {
                throw new ArgumentException(String.Format("<{0}> is not a valid After hook method.", method));
            }
            Method = method;
        }

        public new static bool IsValidMethod(MethodInfo method)
        {
            bool hasAHookAttribute = GetHookAttributes(method).Length == 1;
            bool hasNoParameters = method.GetParameters().Length == 0;
            return hasAHookAttribute && hasNoParameters;
        }

        private static HookAttribute[] GetHookAttributes(MethodInfo method)
        {
            return (HookAttribute[])method.GetCustomAttributes(typeof(AfterAttribute), true);
        }
    }
}
