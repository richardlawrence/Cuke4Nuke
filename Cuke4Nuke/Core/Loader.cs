using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuke4Nuke.Core
{
    public class Loader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEnumerable<string> _assemblyPaths;
        readonly ObjectFactory _objectFactory;

        public Loader(IEnumerable<string> assemblyPaths, ObjectFactory objectFactory)
        {
            _assemblyPaths = assemblyPaths;
            _objectFactory = objectFactory;
        }

        public virtual Repository Load()
        {
            var repository = new Repository();

            foreach (var assemblyPath in _assemblyPaths)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(StepDefinition.MethodFlags))
                    {
                        if (StepDefinition.IsValidMethod(method))
                        {
                            repository.StepDefinitions.Add(new StepDefinition(method));
                            _objectFactory.AddClass(method.ReflectedType);
                        }
                        if (BeforeHook.IsValidMethod(method))
                        {
                            repository.BeforeHooks.Add(new BeforeHook(method));
                            _objectFactory.AddClass(method.ReflectedType);
                        }
                        if (AfterHook.IsValidMethod(method))
                        {
                            repository.AfterHooks.Add(new AfterHook(method));
                            _objectFactory.AddClass(method.ReflectedType);
                        }
                    }
                }
            }

            return repository;
        }
    }
}