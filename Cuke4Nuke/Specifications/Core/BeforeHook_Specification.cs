using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Framework;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    public class BeforeHook_Specification
    {
        [Test]
        public void Should_load_Before_method_successfully()
        {
            var method = Reflection.GetMethod(typeof(ValidHooks), "Before1");
            var hook = new BeforeHook(method);
            Assert.That(hook.Method, Is.EqualTo(method));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_not_load_After_method()
        {
            var method = Reflection.GetMethod(typeof(InvalidHooks), "After1");
            var hook = new BeforeHook(method);
        }

        public class ValidHooks
        {
            [Before]
            private void Before1() { }
        }

        public class InvalidHooks
        {
            [After]
            public void After1() { }
        }
    }
}
