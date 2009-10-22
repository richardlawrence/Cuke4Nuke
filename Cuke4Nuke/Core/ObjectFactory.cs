using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Core
{
    public class ObjectFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        List<Type> _classes = new List<Type>();
        Dictionary<Type, object> _objects = new Dictionary<Type, object>();

        public void AddClass(Type type)
        {
            if (!_classes.Contains(type))
            {
                _classes.Add(type);
                log.DebugFormat("Added class of type <{0}>.", type);
            }
        }

        public void CreateObjects()
        {
            _objects.Clear();
            foreach (Type type in _classes)
            {
                _objects.Add(type, Activator.CreateInstance(type));
                log.DebugFormat("Creating instance of type <{0}>.", type);
            }
        }

        public object GetObject(Type type)
        {
            if (!_objects.ContainsKey(type))
            {
                log.DebugFormat("Instance of type <{0}> not found.", type);
                return null;
            }

            log.DebugFormat("Found instance of type <{0}>.", type);
            return _objects[type];
        }
    }
}
