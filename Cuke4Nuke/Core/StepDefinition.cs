using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

using Cuke4Nuke.Framework;
using System.ComponentModel;

namespace Cuke4Nuke.Core
{
    public class StepDefinition : IEquatable<StepDefinition>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        private Regex _regex;
        public MethodInfo Method { get; private set; }
        public string Id { get; private set; }

        public StepDefinition(MethodInfo method)
        {
            var attributes = GetStepDefinitionAttributes(method);
            if (attributes.Length == 0)
            {
                throw new ArgumentException("method " + method + " does not have a step definition attribute");
            }
            _regex = new Regex(attributes[0].Pattern);

            Method = method;

            // Is there a better way of getting a fully qualified signature that includes the parameter types?
            var signatureWithReturnType = method.ToString();
            var positionOfSpaceAfterReturnType = signatureWithReturnType.IndexOf(' ');
            var signatureWithoutReturnType = signatureWithReturnType.Substring(positionOfSpaceAfterReturnType + 1);
            Id = method.DeclaringType.FullName + "." + signatureWithoutReturnType;
        }

        public static bool IsValidMethod(MethodInfo method)
        {
            return GetStepDefinitionAttributes(method).Length == 1;
        }

        static StepDefinitionAttribute[] GetStepDefinitionAttributes(MethodInfo method)
        {
            return (StepDefinitionAttribute[]) method.GetCustomAttributes(typeof (StepDefinitionAttribute), true);
        }

        public void Invoke(ObjectFactory objectFactory, params string[] args)
        {
            ParameterInfo[] parameters = Method.GetParameters();
            if (parameters.Length != args.Length)
            {
                throw new ArgumentException("Expected " + parameters.Length + " argument(s); got " + args.Length);
            }
            var typedArgs = new object[args.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(parameters[i].ParameterType);
                typedArgs[i] = converter.ConvertFromString(args[i]);
            }

            object instance = null;
            if (!Method.IsStatic)
            {
                instance = objectFactory.GetObject(Method.DeclaringType);
            }
            Method.Invoke(instance, typedArgs);
        }

        public bool Equals(StepDefinition other)
        {
            return other != null && other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StepDefinition);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        internal List<StepArgument> ArgumentsFrom(string stepName)
        {
            List<StepArgument> arguments = null;
            Match match = _regex.Match(stepName);
            if(match.Success)
            {
                arguments = new List<StepArgument>();
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    Group group = match.Groups[i];
                    arguments.Add(new StepArgument(group.Value, group.Index));
                }
            }
            return arguments;
        }
    }
}
