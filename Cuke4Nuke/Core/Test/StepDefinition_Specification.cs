using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;

namespace Cuke4Nuke.Core.Test
{
    [TestFixture]
    public class StepDefinition_Specification
    {
        [Test]
        public void ShouldHavePatternProperty()
        {
            StepDefinition sd = new StepDefinition();
            Assert.That(sd, Has.Property("Pattern"));
        }

        [Test]
        public void ShouldHaveIdProperty()
        {
            StepDefinition sd = new StepDefinition();
            Assert.That(sd, Has.Property("Id"));
        }

        [Test]
        public void ShouldHaveMethodProperty()
        {
            StepDefinition sd = new StepDefinition();
            Assert.That(sd, Has.Property("Method"));
        }

        [Test]
        public void ShouldTakePatternAndMethodInConstructor()
        {
            MethodInfo dummyMethod = this.GetType().GetMethod("DummyMethod");
            StepDefinition sd = new StepDefinition(".*", dummyMethod);
            Assert.That(sd.Pattern, Is.EqualTo(".*"));
            Assert.That(sd.Method, Is.EqualTo(dummyMethod));
        }

        public void DummyMethod() { }
    }
}
