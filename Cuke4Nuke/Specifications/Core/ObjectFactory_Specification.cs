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

        [Test]
        public void Should_have_DisposeObjects_method()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
            objectFactory.CreateObjects();
            objectFactory.DisposeObjects();
        }

        [Test]
        public void Should_return_null_from_GetObject_after_DisposeObjects_is_called()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(Dummy));
            objectFactory.CreateObjects();
            Assert.That(objectFactory.GetObject(typeof(Dummy)), Is.Not.Null);
            objectFactory.DisposeObjects();
            Assert.That(objectFactory.GetObject(typeof(Dummy)), Is.Null);
        }

        [Test]
        public void Should_successfully_create_an_object_without_a_parameterless_constructor()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(DummyBox));
            objectFactory.CreateObjects();
            DummyBox dummyBox = (DummyBox)objectFactory.GetObject(typeof(DummyBox));
        }
    }

    public class Dummy
    {
        public String Value { get; set; }
    }

    public class DummyBox
    {
        Dummy _dummy;

        public DummyBox(Dummy dummy)
        {
            _dummy = dummy;
        }
    }
}
