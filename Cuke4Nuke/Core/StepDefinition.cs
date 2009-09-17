using System;
using System.Reflection;

using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Core
{
    public class StepDefinition : IEquatable<StepDefinition>
    {
        public const BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public string Pattern { get; private set; }
        public MethodInfo Method { get; private set; }
        public string Id { get { return Method.DeclaringType.FullName + "." + Method.Name; } }

        public StepDefinition(MethodInfo method)
        {
            if (!method.IsStatic)
                throw new ArgumentException("method " + method + " must be static");

            var attributes = GetStepDefinitionAttributes(method);

            if (attributes.Length == 0)
                throw new ArgumentException("method " + method + " does not have a step definition attribute");

            Pattern = attributes[0].Pattern;
            Method = method;
        }

        public static bool IsValidMethod(MethodInfo method)
        {
            if (!method.IsStatic)
                return false;

            return GetStepDefinitionAttributes(method).Length == 1;
        }

        static StepDefinitionAttribute[] GetStepDefinitionAttributes(MethodInfo method)
        {
            return (StepDefinitionAttribute[]) method.GetCustomAttributes(typeof (StepDefinitionAttribute), true);
        }

        public void Invoke(params string[] parameters)
        {
            Method.Invoke(null, parameters);
        }

        public bool Equals(StepDefinition other)
        {
            if (other == null)
                return false;

            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StepDefinition);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
