using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Core
{
    public class ObjectFactory
    {
        List<Type> _classes = new List<Type>();
        Dictionary<Type, object> _objects = new Dictionary<Type, object>();

        public void AddClass(Type type)
        {
            if (!_classes.Contains(type))
            {
                _classes.Add(type);
            }
        }

        public void CreateObjects()
        {
            foreach (Type type in _classes)
            {
                _objects.Add(type, Activator.CreateInstance(type));
            }
        }

        public object GetObject(Type type)
        {
            if (!_objects.ContainsKey(type))
            {
                return null;
            }

            return _objects[type];
        }
    }
}
