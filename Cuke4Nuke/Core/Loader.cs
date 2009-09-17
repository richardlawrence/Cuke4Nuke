using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuke4Nuke.Core
{
    public class Loader
    {
        readonly Options _options;

        public Loader(Options options)
        {
            _options = options;
        }

        public virtual List<StepDefinition> Load()
        {
            var stepDefinitions = new List<StepDefinition>();

            foreach (var assemblyPath in _options.AssemblyPaths)
                Load(stepDefinitions, Assembly.LoadFrom(assemblyPath));

            return stepDefinitions;
        }

        static void Load(ICollection<StepDefinition> stepDefinitions, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
                Load(stepDefinitions, type);
        }

        static void Load(ICollection<StepDefinition> stepDefinitions, Type type)
        {
            foreach (var method in type.GetMethods(StepDefinition.MethodFlags))
                Load(stepDefinitions, method);
        }

        static void Load(ICollection<StepDefinition> stepDefinitions, MethodInfo method)
        {
            if (StepDefinition.IsValidMethod(method))
                stepDefinitions.Add(new StepDefinition(method));
        }
    }
}