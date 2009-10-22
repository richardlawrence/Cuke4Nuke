using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    public class ObjectFactory_Specification
    {
        [Test]
        public void Should_have_AddClass_method()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
        }

        [Test]
        public void Should_have_CreateObjects_method()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
            objectFactory.CreateObjects();
        }
        
        [Test]
        public void Should_have_GetObject_method()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
            objectFactory.CreateObjects();
            Dummy dummy = (Dummy) objectFactory.GetObject(typeof(Dummy));
        }

        [Test]
        public void Should_return_null_from_GetObject_if_CreateObjects_not_called_first()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
            Assert.That(objectFactory.GetObject(typeof(Dummy)), Is.Null);
        }

        [Test]
        public void Should_return_same_object_from_multiple_GetObject_calls()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
            objectFactory.CreateObjects();
            Dummy dummy1 = (Dummy)objectFactory.GetObject(typeof(Dummy));
            dummy1.Value = "foo";
            Dummy dummy2 = (Dummy)objectFactory.GetObject(typeof(Dummy));
            Assert.That(dummy2.Value, Is.EqualTo("foo"));
        }
    }

    public class Dummy
    {
        public String Value { get; set; }
    }
}
