using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Framework;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    public class AfterHook_Specification
    {
        [Test]
        public void Should_load_After_method_successfully()
        {
            var method = Reflection.GetMethod(typeof(ValidHooks), "After1");
            var hook = new AfterHook(method);
            Assert.That(hook.Method, Is.EqualTo(method));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_not_load_Before_method()
        {
            var method = Reflection.GetMethod(typeof(InvalidHooks), "Before1");
            var hook = new AfterHook(method);
        }

        public class ValidHooks
        {
            [After]
            public void After1() { }
        }

        public class InvalidHooks
        {
            [Before]
            private void Before1() { }
        }
    }
}