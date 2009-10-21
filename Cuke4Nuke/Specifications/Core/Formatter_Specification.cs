using System;
using System.Collections.Generic;

using Cuke4Nuke.Core;
using Cuke4Nuke.Framework;

using LitJson;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Formatter_Specification
    {
        Formatter _formatter;
        StepDefinition _stepDefinition;
        ICollection<StepDefinition> _stepDefinitions;

        [SetUp]
        public void SetUp()
        {
            _formatter = new Formatter();
            _stepDefinition = CreateStepDefinition();
            _stepDefinitions = new List<StepDefinition>();
        }

        [Test]
        public void ShouldFormatAnExceptionCorrectly()
        {
            var exception = CreateThrownException();

            var result = _formatter.Format(exception);

            var jsonData = GetJsonData(result);
            JsonAssert.IsArray(jsonData);
            Assert.That(jsonData[0].ToString(), Is.EqualTo("step_failed"));
            JsonAssert.HasString(jsonData[1], "message", exception.Message);
            JsonAssert.HasString(jsonData[1], "backtrace", exception.StackTrace);
        }

        static Exception CreateThrownException()
        {
            Exception exception;
            try
            {
                throw new Exception("message");
            }
            catch (Exception x)
            {
                exception = x;
            }
            return exception;
        }

        static JsonData GetJsonData(string result)
        {
            return JsonMapper.ToObject(result);
        }

        StepDefinition CreateStepDefinition()
        {
            var dummyMethod = GetType().GetMethod("DummyMethod");
            return new StepDefinition(dummyMethod);
        }

        [Given("")]
        public static void DummyMethod() { }
    }
}