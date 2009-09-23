using System;
using System.Reflection;

namespace Cuke4Nuke.Specifications
{
    public static class Reflection
    {
        public static MethodInfo GetMethod(Type type, string MethodName)
        {
            const BindingFlags Flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return type.GetMethod(MethodName, Flags);
        }
    }
}
