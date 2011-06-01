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

            Tags = hookAttribute.Tags;
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

        public void Invoke(ObjectFactory objectFactory, string[] scenarioTags)
        {
            if (!HasTags || MatchesTags(scenarioTags))
            {
                Invoke(objectFactory);
            }
        }

        public bool MatchesTags(string[] scenarioTags)
        {
            var normalizedScenarioTags = NormalizeTags(new List<string>(scenarioTags));
            var predicate = PredicateBuilder.True<List<string>>();
            foreach (string tag in Tags)
            {
                if (tag.Contains(","))
                {
                    var nestedPredicate = PredicateBuilder.False<List<string>>();
                    var orTags = tag.Split(new string[] { ",", ", " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string orTag in orTags)
                    {
                        var orTagValue = orTag;
                        nestedPredicate = nestedPredicate.Or(tags => tags.Contains(NormalizeTag(orTagValue)));
                    }
                    predicate = predicate.And(nestedPredicate);
                }
                else
                {
                    var tagValue = tag;
                    predicate = predicate.And(tags => tags.Contains(NormalizeTag(tagValue)));
                }
            }
            return !HasTags || predicate.Compile().Invoke(normalizedScenarioTags);
        }

        private string NormalizeTag(string tag)
        {
            return Regex.Replace(tag, @"^@(.*)$", @"$1");
        }

        private List<string> NormalizeTags(List<string> tags)
        {
            return tags.ConvertAll(tag => NormalizeTag(tag));
        }
    }
}