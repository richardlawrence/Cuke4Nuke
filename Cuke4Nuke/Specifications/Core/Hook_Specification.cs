using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Framework;
using System.Reflection;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Hook_Specification
    {
        const BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        [Test]
        public void Should_allow_valid_methods()
        {
            var methods = typeof(ValidHooks).GetMethods(MethodFlags);
            foreach (MethodInfo method in methods)
            {
                Assert.IsTrue(Hook.IsValidMethod(method), String.Format("<{0}> is not a valid hook method.", method));
            }
        }

        [Test]
        public void Should_not_allow_invalid_methods()
        {
            var methods = typeof(InvalidHooks).GetMethods(MethodFlags);
            foreach (MethodInfo method in methods)
            {
                Assert.IsFalse(Hook.IsValidMethod(method), String.Format("<{0}> is a valid hook method.", method));
            }
        }

        [Test]
        public void Constructor_should_take_a_method_for_Method_property()
        {
            var method = Reflection.GetMethod(typeof(ValidHooks), "Before1");
            var hook = new Hook(method);
            Assert.That(hook.Method, Is.EqualTo(method));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_should_throw_given_invalid_method()
        {
            var method = Reflection.GetMethod(typeof(InvalidHooks), "Before1");
            var hook = new Hook(method);
        }

        [Test]
        public void Should_invoke_method_successfully()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(ValidHooks));
            objectFactory.CreateObjects();
            var method = Reflection.GetMethod(typeof(ValidHooks), "Before1");
            var hook = new Hook(method);
            hook.Invoke(objectFactory);
        }

        [Test]
        public void Should_invoke_tagged_hook_when_scenario_has_matching_tag()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(ValidHooks));
            objectFactory.CreateObjects();
            var method = Reflection.GetMethod(typeof(ValidHooks), "BeforeWithTagThrowsException");
            var hook = new Hook(method);
            Assert.Throws<Exception>(() => hook.Invoke(objectFactory, new string[] {"my_tag"}));
        }

        [Test]
        public void Should_not_invoke_tagged_hook_when_scenario_has_no_matching_tag()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(ValidHooks));
            objectFactory.CreateObjects();
            var method = Reflection.GetMethod(typeof(ValidHooks), "BeforeWithTagThrowsException");
            var hook = new Hook(method);
            hook.Invoke(objectFactory, new string[] { "not_my_tag" });
        }

        [Test]
        public void Should_not_invoke_tagged_hook_when_scenario_has_no_tags()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(ValidHooks));
            objectFactory.CreateObjects();
            var method = Reflection.GetMethod(typeof(ValidHooks), "BeforeWithTagThrowsException");
            var hook = new Hook(method);
            hook.Invoke(objectFactory, new string[0]);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Invoke_should_throw_when_method_throws()
        {
            ObjectFactory objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(ValidHooks));
            objectFactory.CreateObjects();
            var method = Reflection.GetMethod(typeof(ValidHooks), "ThrowsException");
            var hook = new Hook(method);
            hook.Invoke(objectFactory);
        }

        [Test]
        public void Constructor_should_get_tag_from_attribute()
        {
            var method = Reflection.GetMethod(typeof(ValidHooks), "BeforeWithTag");
            var hook = new Hook(method);
            Assert.That(hook.Tags[0], Is.EqualTo("my_tag"));
        }

        [Test]
        public void Constructor_should_set_HasTags_to_true_when_tags_on_attribute()
        {
            var method = Reflection.GetMethod(typeof(ValidHooks), "BeforeWithTag");
            var hook = new Hook(method);
            Assert.That(hook.HasTags, Is.True);
        }

        [Test]
        public void Constructor_should_set_HasTags_to_false_when_no_tags_on_attribute()
        {
            var method = Reflection.GetMethod(typeof(ValidHooks), "Before1");
            var hook = new Hook(method);
            Assert.That(hook.HasTags, Is.False);
        }

        public class ValidHooks
        {
            [Before]
            private void Before1() { }

            [Before]
            internal void Before2() { }

            [Before]
            protected void Before3() { }

            [Before]
            public void Before4() { }
            
            [Before]
            public static void Before5() { }

            [Before("@my_tag")]
            public static void BeforeWithTag() { }

            [Before("@my_tag", "@another_tag", "a_third_tag")]
            public static void BeforeWithMultipleTags() { }

            [Before]
            private void ThrowsException()
            {
                throw new Exception();
            }

            [Before("@my_tag")]
            private void BeforeWithTagThrowsException()
            {
                throw new Exception();
            }
        }

        public class InvalidHooks
        {
            // has a parameter
            [Before]
            public void Before1(object arg) { }

            // doesn't have an attribute
            public void Before2() { }

            // has a step definition attribute
            [Given("")]
            public void Before3() { }
        }
    }
}
