using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using LitJson;

namespace Cuke4Nuke.Core.Test
{
    [TestFixture]
    public class StepDefinitionRepository_Specification
    {
        [Test]
        public void ShouldHoldStepDefinitions()
        {
            StepDefinitionRepository sdr = new StepDefinitionRepository();
            StepDefinition sd = CreateStepDefinition();
            sdr.AddStepDefinition(sd);
            Assert.That(sdr.StepDefinitions, Has.Member(sd));
        }

        [Test]
        public void ShouldListStepDefinitionsAsJson()
        {
            StepDefinitionRepository sdr = new StepDefinitionRepository();
            StepDefinition sd = CreateStepDefinition();
            sdr.AddStepDefinition(sd);
            JsonData data = JsonMapper.ToObject(sdr.ListStepDefinitionsAsJson());

            Assert.That(data.IsArray);
            Assert.That(data.Count, Is.EqualTo(1));
            Assert.That((string) data[0]["id"], Is.EqualTo(sd.Id));
            Assert.That((string) data[0]["regexp"], Is.EqualTo(sd.Pattern));
        }

        private StepDefinition CreateStepDefinition()
        {
            MethodInfo dummyMethod = this.GetType().GetMethod("DummyMethod");
            return new StepDefinition(".*", dummyMethod);
        }

        public void DummyMethod() { }
    }
}
