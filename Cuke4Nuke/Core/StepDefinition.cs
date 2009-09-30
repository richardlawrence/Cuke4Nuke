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
        public string Id { get; private set; }

        public StepDefinition(MethodInfo method)
        {
            if (!method.IsStatic)
            {
                throw new ArgumentException("method " + method + " must be static");
            }

            var attributes = GetStepDefinitionAttributes(method);
            if (attributes.Length == 0)
            {
                throw new ArgumentException("method " + method + " does not have a step definition attribute");
            }
            Pattern = attributes[0].Pattern;

            Method = method;

            // Is there a better way of getting a fully qualified signature that includes the parameter types?
            var signatureWithReturnType = method.ToString();
            var positionOfSpaceAfterReturnType = signatureWithReturnType.IndexOf(' ');
            var signatureWithoutReturnType = signatureWithReturnType.Substring(positionOfSpaceAfterReturnType + 1);
            Id = method.DeclaringType.FullName + "." + signatureWithoutReturnType;
        }

        public static bool IsValidMethod(MethodInfo method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            return GetStepDefinitionAttributes(method).Length == 1;
        }

        static StepDefinitionAttribute[] GetStepDefinitionAttributes(MethodInfo method)
        {
            return (StepDefinitionAttribute[]) method.GetCustomAttributes(typeof (StepDefinitionAttribute), true);
        }

        public void Invoke(params object[] parameters)
        {
            Method.Invoke(null, parameters);
        }

        public bool Equals(StepDefinition other)
        {
            if (other == null)
            {
                return false;
            }

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
