using System;
using System.Collections.Generic;

using Cuke4Nuke.Core;
using Cuke4Nuke.Framework;

using LitJson;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Processor_Specification
    {
        StepDefinition _stepDefinition;
        StepDefinition _exceptionDefinition;
        List<StepDefinition> _stepDefinitions;
        Processor _processor;
        static bool _methodCalled;

        [SetUp]
        public void SetUp()
        {
            _stepDefinition = new StepDefinition(Reflection.GetMethod(GetType(), "Method"));
            _exceptionDefinition = new StepDefinition(Reflection.GetMethod(GetType(), "ThrowsException"));
            _stepDefinitions = new List<StepDefinition> { _stepDefinition, _exceptionDefinition };

            var loader = new MockLoader(_stepDefinitions);
            _processor = new Processor(loader);

            _methodCalled = false;
        }

        [Test]
        public void List_step_definitions_should_return_a_json_formatted_list()
        {
            var response = _processor.Process("list_step_definitions");

            Assert.That(response, Is.EqualTo(new Formatter().Format(_stepDefinitions)));
        }

        [Test]
        public void Invoke_with_a_valid_id_should_invoke_step_definition_method()
        {
            var request = CreateInvokeRequest(_stepDefinition.Id);
            var response = _processor.Process(request);

            Assert.That(response, Is.EqualTo("OK"));
            Assert.That(_methodCalled, Is.True);
        }

        [Test]
        public void Invoke_with_a_missing_id_should_return_failed_response()
        {
            var response = _processor.Process(@"invoke:{ }");

            AssertFailResponse(response, "Missing 'id' in request");
        }

        [Test]
        public void Invoke_with_malformed_json_should_return_failed_response()
        {
            var response = _processor.Process(@"invoke:{a}");

            AssertFailResponse(response, "Invalid json in request 'invoke:{a}': Invalid character 'a' in input string");
        }

        [Test]
        public void Invoke_with_an_invalid_id_should_return_failed_response()
        {
            var request = CreateInvokeRequest("invalid_id");

            var response = _processor.Process(request);

            AssertFailResponse(response, "Could not find step with id 'invalid_id'");
        }

        [Test]
        public void Invoke_with_a_method_throws_should_return_failed_response()
        {
            var request = CreateInvokeRequest(_exceptionDefinition.Id);

            var response = _processor.Process(request);

            AssertFailResponse(response, "inner test Exception", typeof(Exception));
        }

        [Test]
        public void Unknown_request_should_return_failed_json_message()
        {
            var response = _processor.Process("invalid_request");

            AssertFailResponse(response, "Invalid request 'invalid_request'");
        }

        static string CreateInvokeRequest(string id)
        {
            return @"invoke:{ ""id"" : """ + id + @""" }";
        }

        static void AssertFailResponse(string response, string message)
        {
            StringAssert.StartsWith("FAIL:", response);
            var jsonData = JsonMapper.ToObject(response.Substring(5));
            JsonAssert.IsObject(jsonData);
            JsonAssert.HasString(jsonData, "message", message);
        }

        static void AssertFailResponse(string response, string message , Type exceptionType)
        {
            AssertFailResponse(response, message);
            var jsonData = JsonMapper.ToObject(response.Substring(5));
            JsonAssert.IsObject(jsonData);
            JsonAssert.HasString(jsonData, "exception", exceptionType.ToString());
            JsonAssert.HasString(jsonData, "backtrace");
        }

        [Given("")]
        public static void Method()
        {
            _methodCalled = true;
        }

        [Given("")]
        public static void ThrowsException()
        {
            throw new Exception("inner test Exception");
        }

        class MockLoader : Loader
        {
            internal List<StepDefinition> StepDefinitions { get; private set; }

            public MockLoader(List<StepDefinition> stepDefinitions)
                : base(null)
            {
                StepDefinitions = stepDefinitions;
            }

            public override List<StepDefinition> Load()
            {
                return StepDefinitions;
            }
        }
    }
}