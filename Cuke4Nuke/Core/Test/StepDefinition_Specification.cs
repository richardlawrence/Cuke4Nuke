using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

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
        public void ShouldTakePatternInConstructor()
        {
            StepDefinition sd = new StepDefinition(".*");
            Assert.That(sd.Pattern, Is.EqualTo(".*"));
        }

        [Test]
        public void ShouldTakeIdAndPatternInConstructor()
        {
            StepDefinition sd = new StepDefinition("id", ".*");
            Assert.That(sd.Id, Is.EqualTo("id"));
            Assert.That(sd.Pattern, Is.EqualTo(".*"));
        }
    }
}
