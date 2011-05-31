using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Core
{
    public class Hook
    {
        protected Hook()
        {
            Tags = new List<string>();
        }

        public Hook(MethodInfo method)
        {
            if (!IsValidMethod(method))
            {
                throw new ArgumentException(String.Format("<{0}> is not a valid hook method.", method));
            }


            var hookAttribute = GetHookAttributes(method)[0];

            Tags = new List<string>();
            if (hookAttribute.Tag != null)
            {
                Tags.Add(Regex.Replace(hookAttribute.Tag, @"^@(.*)$", @"$1"));
            }

            Method = method;
        }

        public bool HasTags
        {
            get { return Tags.Count > 0; }
        }

        public MethodInfo Method { get; protected set; }

        public List<string> Tags { get; protected set; }

        public static bool IsValidMethod(MethodInfo method)
        {
            bool hasAHookAttribute = GetHookAttributes(method).Length == 1;
            bool hasNoParameters = method.GetParameters().Length == 0;
            return hasAHookAttribute && hasNoParameters;
        }

        private static HookAttribute[] GetHookAttributes(MethodInfo method)
        {
            return (HookAttribute[]) method.GetCustomAttributes(typeof (HookAttribute), true);
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

        public void Invoke(ObjectFactory objectFactory, string[] scenarioTags)
        {
            if (!HasTags || ScenarioHasMatchingTag(scenarioTags, Tags[0]))
            {
                Invoke(objectFactory);
            }
        }

        private bool ScenarioHasMatchingTag(string[] scenarioTags, string hookTag)
        {
            return (new List<string>(scenarioTags)).Contains(hookTag);
        }
    }
}