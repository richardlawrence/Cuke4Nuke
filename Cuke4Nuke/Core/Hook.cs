using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Core
{
    public class Hook
    {
        public Hook(MethodInfo method)
        {
            if (!Hook.IsValidMethod(method))
            {
                throw new ArgumentException(String.Format("<{0}> is not a valid hook method.", method));
            }
            Method = method;
        }

        public MethodInfo Method { get; private set; }

        public static bool IsValidMethod(MethodInfo method)
        {
            bool hasAHookAttribute = GetHookAttributes(method).Length == 1;
            bool hasNoParameters = method.GetParameters().Length == 0;
            return hasAHookAttribute && hasNoParameters;
        }

        private static HookAttribute[] GetHookAttributes(MethodInfo method)
        {
            return (HookAttribute[])method.GetCustomAttributes(typeof(HookAttribute), true);
        }

        public void Invoke(ObjectFactory objectFactory)
        {
            try
            {
                object instance = null;
                if (!Method.IsStatic)
                {
                    instance = objectFactory.GetObject(Method.DeclaringType);
                }
                Method.Invoke(instance, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
