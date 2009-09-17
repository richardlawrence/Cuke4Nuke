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
        public void Format_of_a_StepDefinition_should_be_an_object_with_id_and_regex_values()
        {
            var result = _formatter.Format(_stepDefinition);
            var jsonData = GetJsonData(result);

            AssertJsonStepDefinition(jsonData);
        }

        [Test]
        public void Format_of_a_StepDefinition_collection_should_be_an_array_with_each_step_definition_as_a_child()
        {
            _stepDefinitions.Add(_stepDefinition);
            _stepDefinitions.Add(_stepDefinition);
            _stepDefinitions.Add(_stepDefinition);

            var result = _formatter.Format(_stepDefinitions);

            var jsonData = GetJsonData(result);
            JsonAssert.IsArray(jsonData);
            for (int i = 0; i < _stepDefinitions.Count; i++)
                AssertJsonStepDefinition(jsonData[i]);
        }

        [Test]
        public void Format_of_an_exception_should_be_FAIL_plus_a_json_object_with_message_and_backtrace_values()
        {
            var exception = CreateThrownException();

            var result = _formatter.Format(exception);

            StringAssert.StartsWith("FAIL:", result);
            var jsonData = GetJsonData(result.Substring(5));
            JsonAssert.HasString(jsonData, "message", exception.Message);
            JsonAssert.HasString(jsonData, "backtrace", exception.StackTrace);
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

        void AssertJsonStepDefinition(JsonData jsonData)
        {
            JsonAssert.IsObject(jsonData);
            JsonAssert.HasString(jsonData, "id", _stepDefinition.Id);
            JsonAssert.HasString(jsonData, "regexp", _stepDefinition.Pattern);
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