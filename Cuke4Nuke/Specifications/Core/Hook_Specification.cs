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
