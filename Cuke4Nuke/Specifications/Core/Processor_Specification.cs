using System;
using System.Collections.Generic;

using Cuke4Nuke.Core;
using Cuke4Nuke.Framework;

using LitJson;

using NUnit.Framework;
using System.Text;
using System.Reflection;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Processor_Specification
    {
        StepDefinition _stepDefinition;
        StepDefinition _exceptionDefinition;
        StepDefinition _stepDefinitionWithOneStringParameter;
        StepDefinition _stepDefinitionWithMultipleStringParameters;
        StepDefinition _stepDefinitionWithMultipleStringParametersOverloaded;
        StepDefinition _stepDefinitionWithOneIntParameter;
        StepDefinition _stepDefinitionWithOneDoubleParameter;
        StepDefinition _stepDefinitionWithIntDoubleAndStringParameters;
        StepDefinition _pendingStepDefinition;
        private StepDefinition _stepDefinitionWithTableDiff;

        List<StepDefinition> _stepDefinitions;

        Processor _processor;

        static bool _methodCalled;
        static object[] _receivedParameters; // Methods that take parameters should copy the parameter values into this array, so that the test can verify the values
        
        [SetUp]
        public void SetUp()
        {
            _stepDefinition = new StepDefinition(Reflection.GetMethod(GetType(), "CheckMethodCalled"));
            _exceptionDefinition = new StepDefinition(Reflection.GetMethod(GetType(), "ThrowsException"));
            _stepDefinitionWithOneStringParameter = new StepDefinition(GetType().GetMethod("OneStringParameter"));
            _stepDefinitionWithMultipleStringParameters = new StepDefinition(GetType().GetMethod("MultipleStringParameters", new Type[] { typeof(string), typeof(string) }));
            _stepDefinitionWithMultipleStringParametersOverloaded = new StepDefinition(GetType().GetMethod("MultipleStringParameters", new Type[] { typeof(string), typeof(string), typeof(string) }));
            _stepDefinitionWithOneIntParameter = new StepDefinition(GetType().GetMethod("OneIntParameter"));
            _stepDefinitionWithOneDoubleParameter = new StepDefinition(GetType().GetMethod("OneDoubleParameter"));
            _stepDefinitionWithIntDoubleAndStringParameters = new StepDefinition(GetType().GetMethod("IntDoubleAndStringParameters"));
            _pendingStepDefinition = new StepDefinition(GetType().GetMethod("Pending"));
            _stepDefinitionWithTableDiff = new StepDefinition(GetType().GetMethod("TableDiff"));
            
            _stepDefinitions = new List<StepDefinition> { 
                _stepDefinition, 
                _exceptionDefinition, 
                _stepDefinitionWithOneStringParameter, 
                _stepDefinitionWithMultipleStringParameters, 
                _stepDefinitionWithMultipleStringParametersOverloaded, 
                _stepDefinitionWithOneIntParameter, 
                _stepDefinitionWithOneDoubleParameter, 
                _stepDefinitionWithIntDoubleAndStringParameters,
                _pendingStepDefinition,
                _stepDefinitionWithTableDiff
            };
            var loader = new MockLoader(_stepDefinitions);
            var objectFactory = new ObjectFactory();
            _processor = new Processor(loader, objectFactory);

            _methodCalled = false;
            _receivedParameters = null;
        }

        [Test]
        public void Step_matches_should_return_a_json_formatted_list()
        {
            var response = _processor.Process(@"[""step_matches"",{""name_to_match"":""The regex group 'cukes' should be captured""}]");
            
            Assert.That(response, Is.EqualTo(@"[""step_matches"",[{""id"":""Cuke4Nuke.Specifications.Core.Processor_Specification.OneStringParameter(System.String)"",""args"":[{""val"":""cukes"",""pos"":17}]}]]"));
        }

        [Test]
        public void Begin_scenario_should_return_success_json()
        {
            String request = @"[""begin_scenario"",null]";
            var response = _processor.Process(request);
            Assert.That(response, Is.EqualTo(@"[""success"",null]"));
        }

        [Test]
        public void End_scenario_should_return_success_json()
        {
            String request = @"[""end_scenario"",null]";
            var response = _processor.Process(request);
            Assert.That(response, Is.EqualTo(@"[""success"",null]"));
        }

        [Test]
        public void Invoke_with_a_valid_id_should_invoke_step_definition_method()
        {
            var request = CreateInvokeRequest(_stepDefinition.Id);
            var response = _processor.Process(request);

            AssertSuccessResponse(response);
            Assert.That(_methodCalled, Is.True);
        }

        [Test]
        public void Invoke_with_malformed_json_should_return_failed_response()
        {
            var response = _processor.Process(@"nonsense:{a}");

            AssertFailResponse(response, "Unexpected character encountered while parsing value: n. Line 1, position 1.");
        }

        [Test]
        public void Invoke_with_an_invalid_id_and_one_arg_should_return_failed_response()
        {
            var request = CreateInvokeRequest("invalid_id", "x");

            var response = _processor.Process(request);

            AssertFailResponse(response, "Could not find step with id 'invalid_id'");
        }

        [Test]
        public void Invoke_with_an_invalid_id_and_no_args_should_return_failed_response()
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
        public void Invoke_with_a_step_taking_one_string_parameter_should_pass_the_correct_parameter_value()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithOneStringParameter.Id, "first");
            var response = _processor.Process(request);
            AssertSuccessResponse(response);
            Assert.That(_receivedParameters.Length, Is.EqualTo(1));
            Assert.That(_receivedParameters[0], Is.InstanceOf(typeof(string)));
            Assert.That(_receivedParameters[0], Is.EqualTo("first"));
        }

        [Test]
        public void Invoke_with_a_step_taking_multiple_string_parameters_should_pass_the_correct_parameter_values()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithMultipleStringParameters.Id, "first", "second");
            var response = _processor.Process(request);
            AssertSuccessResponse(response);
            Assert.That(_receivedParameters.Length, Is.EqualTo(2));
            Assert.That(_receivedParameters[0], Is.InstanceOf(typeof(string)));
            Assert.That(_receivedParameters[0], Is.EqualTo("first"));
            Assert.That(_receivedParameters[1], Is.InstanceOf(typeof(string)));
            Assert.That(_receivedParameters[1], Is.EqualTo("second"));
        }

        [Test]
        public void Invoke_with_an_overloaded_method_taking_multiple_string_parameters_should_pass_the_correct_parameter_values()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithMultipleStringParametersOverloaded.Id, "first", "second", "third");
            var response = _processor.Process(request);
            AssertSuccessResponse(response);
            Assert.AreEqual(3, _receivedParameters.Length);
            Assert.IsInstanceOf(typeof(string), _receivedParameters[0]);
            Assert.AreEqual("first", _receivedParameters[0]);
            Assert.IsInstanceOf(typeof(string), _receivedParameters[1]);
            Assert.AreEqual("second", _receivedParameters[1]);
            Assert.IsInstanceOf(typeof(string), _receivedParameters[2]);
            Assert.AreEqual("third", _receivedParameters[2]);
        }

        [Test]
        public void Invoke_without_arguments_if_the_step_takes_arguments_should_return_failed_response()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithOneStringParameter.Id);
            var response = _processor.Process(request);
            AssertFailResponse(response, "Expected 1 argument(s); got 0", typeof(ArgumentException));
        }

        [Test]
        public void Invoke_with_arguments_if_the_step_does_not_take_arguments_should_return_failed_response()
        {
            var request = CreateInvokeRequest(_stepDefinition.Id, "first");
            var response = _processor.Process(request);
            AssertFailResponse(response, "Expected 0 argument(s); got 1", typeof(ArgumentException));
        }

        [Test]
        public void Invoke_with_wrong_number_of_arguments_should_return_failed_response()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithOneStringParameter.Id, "first", "second");
            var response = _processor.Process(request);
            AssertFailResponse(response, "Expected 1 argument(s); got 2", typeof(ArgumentException));
        }

        [Test]
        public void Invoke_with_a_step_taking_an_int_should_convert_and_pass_the_correct_value()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithOneIntParameter.Id, "42");
            var response = _processor.Process(request);
            AssertSuccessResponse(response);
            Assert.That(_receivedParameters.Length, Is.EqualTo(1));
            Assert.That(_receivedParameters[0], Is.InstanceOf(typeof(int)));
            Assert.That(_receivedParameters[0], Is.EqualTo(42));
        }

        [Test]
        public void Invoke_with_a_step_taking_a_double_should_convert_and_pass_the_correct_value()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithOneDoubleParameter.Id, "3.14");
            var response = _processor.Process(request);
            AssertSuccessResponse(response);
            Assert.That(_receivedParameters.Length, Is.EqualTo(1));
            Assert.That(_receivedParameters[0], Is.InstanceOf(typeof(double)));
            Assert.That(_receivedParameters[0], Is.EqualTo(3.14));
        }

        [Test]
        public void Invoke_with_a_step_taking_parameters_of_several_types_should_convert_and_pass_the_correct_values()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithIntDoubleAndStringParameters.Id, "42", "3.14", "foo");
            var response = _processor.Process(request);
            AssertSuccessResponse(response);
            Assert.That(_receivedParameters.Length, Is.EqualTo(3));
            Assert.That(_receivedParameters[0], Is.InstanceOf(typeof(int)));
            Assert.That(_receivedParameters[0], Is.EqualTo(42));
            Assert.That(_receivedParameters[1], Is.InstanceOf(typeof(double)));
            Assert.That(_receivedParameters[1], Is.EqualTo(3.14));
            Assert.That(_receivedParameters[2], Is.InstanceOf(typeof(string)));
            Assert.That(_receivedParameters[2], Is.EqualTo("foo"));
        }

        [Test]
        public void Invoke_of_pending_step_definition_should_return_pending_response()
        {
            var request = CreateInvokeRequest(_pendingStepDefinition.Id);
            var response = _processor.Process(request);

            AssertPendingResponse(response);
        }

        [Test]
        public void Invoke_of_step_definition_with_table_diff_should_return_diff_bang_response()
        {
            var request = CreateInvokeRequest(_stepDefinitionWithTableDiff.Id);
            var response = _processor.Process(request);

            AssertDiffBangResponse(response);
        }

        static string CreateInvokeRequest(string id, params string[] invokeArgs)
        {
            JsonData req = new JsonData();
            req.Add("invoke");
            JsonData parameters = new JsonData();
            parameters["id"] = id;
            JsonData args = new JsonData();
            args.SetJsonType(JsonType.Array); // To avoid invalid json if it is empty
            foreach(var arg in invokeArgs) {
                args.Add(arg);
            }
            parameters["args"] = args;
            req.Add(parameters);
            return JsonMapper.ToJson(req);
        }

        static void AssertSuccessResponse(string response)
        {
            Assert.That(response, Is.EqualTo(@"[""success"",null]"));
        }

        static void AssertPendingResponse(string response)
        {
            Assert.That(response, Is.EqualTo(@"[""pending"",null]"));
        }

        static void AssertFailResponse(string response, string message)
        {
            var jsonData = JsonMapper.ToObject(response);
            JsonAssert.IsArray(jsonData);
            Assert.That(jsonData[0].ToString(), Is.EqualTo("fail"));
            JsonAssert.HasString(jsonData[1], "message", message);
        }

        static void AssertFailResponse(string response, string message , Type exceptionType)
        {
            var jsonData = JsonMapper.ToObject(response);
            JsonAssert.IsArray(jsonData);
            Assert.That(jsonData[0].ToString(), Is.EqualTo("fail"));
            JsonAssert.HasString(jsonData[1], "message", message);
            JsonAssert.HasString(jsonData[1], "backtrace");
            JsonAssert.HasString(jsonData[1], "exception", exceptionType.ToString());
        }

        static void AssertDiffBangResponse(string response)
        {
            var jsonData = JsonMapper.ToObject(response);
            JsonAssert.IsArray(jsonData);
            Assert.That(jsonData[0].ToString(), Is.EqualTo("diff!"));
        }

        [Given("check method called")]
        public static void CheckMethodCalled()
        {
            _methodCalled = true;
        }

        [Given("throws exception")]
        public static void ThrowsException()
        {
            throw new Exception("inner test Exception");
        }

        [Given("^The regex group '(.*)' should be captured$")]
        public static void OneStringParameter(string str)
        {
            _receivedParameters = new object[] { str };
        }

        [Given("^The regex groups '(.*)' and '(.*)' should be captured$")]
        public static void MultipleStringParameters(string firstStr, string secondStr)
        {
            _receivedParameters = new object[] { firstStr, secondStr };
        }

        [Given("^The regex groups (.*) and (.*) should be captured, and so should (.*)$")]
        public static void MultipleStringParameters(string firstStr, string secondStr, string thirdStr)
        {
            _receivedParameters = new object[] { firstStr, secondStr, thirdStr };
        }

        [Given(@"^The number ([+-]?\d+) is an int$")]
        public static void OneIntParameter(int number) {
            _receivedParameters = new object[] { number };
        }

        [Given(@"^The number ([+-]?\d+\.\d*) is a double$")]
        public static void OneDoubleParameter(double number)
        {
            _receivedParameters = new object[] { number };
        }

        [Given(@"^The values ([+-]?\d+), ([+-]?\d+\.\d*), and '(.*)' are an int, a double and a string$")]
        public static void IntDoubleAndStringParameters(int intValue, double doubleValue, string stringValue)
        {
            _receivedParameters = new object[] { intValue, doubleValue, stringValue };
        }

        [Given("^a pending step$")]
        [Pending]
        public static void Pending()
        {
        }

        [Given("^a step with a table diff call$")]
        public static void TableDiff()
        {
            Table table1 = new Table();
            table1.Data.Add(new List<string> { "foo", "bar" });
            Table table2 = new Table();
            table1.Data.Add(new List<string> { "foo", "baz" });

            table1.AssertSameAs(table2);
        }

        class MockLoader : Loader
        {
            internal List<StepDefinition> StepDefinitions { get; private set; }

            public MockLoader(List<StepDefinition> stepDefinitions)
                : base(null, null)
            {
                StepDefinitions = stepDefinitions;
            }

            public override Repository Load()
            {
                var repository = new Repository(StepDefinitions, new List<BeforeHook>(), new List<AfterHook>());
                return repository;
            }
        }
    }
}