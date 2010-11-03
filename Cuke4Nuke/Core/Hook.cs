using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Cuke4Nuke.Framework;
using log4net;

namespace Cuke4Nuke.Core
{
    public class Hook
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected Hook()
        {
        }

        public Hook(MethodInfo method)
        {
            if (!IsValidMethod(method))
            {
                throw new ArgumentException(String.Format("<{0}> is not a valid hook method.", method));
            }


            var hookAttribute = GetHookAttributes(method)[0];
            if (hookAttribute.Tag != null)
            {
                Tag = Regex.Replace(hookAttribute.Tag, @"^@(.*)$", @"$1");
            }

            Method = method;
        }

        public bool HasTags
        {
            get { return !string.IsNullOrEmpty(Tag); }
        }

        public MethodInfo Method { get; protected set; }

        public string Tag { get; set; }

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
            if (!HasTags || ScenarioHasMatchingTag(scenarioTags, Tag))
            {
                log.Debug(String.Format("Invoking hook {0}.{1}.", Method.DeclaringType.FullName, Method.ToString()));
                Invoke(objectFactory);
            }
            else
            {

                log.Debug(String.Format("Tag mismatch. Not invoking hook {0}.{1}.", Method.DeclaringType.FullName, Method.ToString()));
            }
        }

        private bool ScenarioHasMatchingTag(string[] scenarioTags, string hookTag)
        {
            return (new List<string>(scenarioTags)).Contains(hookTag);
        }
    }
}