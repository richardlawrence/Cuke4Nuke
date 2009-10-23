using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using System.Reflection;
using System.Diagnostics;

namespace Cuke4Nuke.Core
{
    public class ObjectFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        List<Type> _classes = new List<Type>();
        IKernel _kernel;

        public void AddClass(Type type)
        {
            if (!_classes.Contains(type))
            {
                _classes.Add(type);
                log.DebugFormat("Added class of type <{0}>.", type);
                foreach (ConstructorInfo ci in type.GetConstructors())
                {
                    foreach (ParameterInfo pi in ci.GetParameters())
                    {
                        AddClass(pi.ParameterType);
                    }
                }
            }
        }

        public void CreateObjects()
        {
            _kernel = new DefaultKernel();
            foreach (Type type in _classes)
            {
                _kernel.AddComponent(type.ToString(), type);
                log.DebugFormat("Creating instance of type <{0}>.", type);
            }
        }

        public object GetObject(Type type)
        {
            if (_kernel == null || !_kernel.HasComponent(type))
            {
                log.DebugFormat("Instance of type <{0}> not found.", type);
                return null;
            }

            log.DebugFormat("Found instance of type <{0}>.", type);
            return _kernel[type];
        }

        public void DisposeObjects()
        {
            if (_kernel != null)
            {
                _kernel.Dispose();
            }
        }
    }
}
