using System;
using System.Collections.Generic;
using System.Reflection;

using Cuke4Nuke.Framework;
using NUnit.Framework;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class StepDefinition_Specification
    {
        protected const BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        MethodInfo _successMethod;
        MethodInfo _exceptionMethod;
        StepDefinition _stepDefinition;

        [SetUp]
        public void SetUp()
        {
            _successMethod = Reflection.GetMethod(typeof(ValidStepDefinitions), "Succeeds");
            _exceptionMethod = Reflection.GetMethod(typeof(ValidStepDefinitions), "ThrowsException");

            _stepDefinition = new StepDefinition(_successMethod);
        }

        [Test]
        public void Should_allow_method_with_a_Given_attribute()
        {
            AssertMethodIsValid("Given");
        }


        

        [Test]
        public void Should_allow_method_with_a_When_attribute()
        {
            AssertMethodIsValid("When");
        }

        [Test]
        public void Should_allow_method_with_a_Then_attribute()
        {
            AssertMethodIsValid("Then");
        }

        [Test]
        public void Should_allow_public_methods()
        {
            AssertMethodIsValid("Public");
        }

        [Test]
        public void Should_allow_internal_methods()
        {
            AssertMethodIsValid("Internal");
        }

        [Test]
        public void Should_allow_protected_methods()
        {
            AssertMethodIsValid("Protected");
        }

        [Test]
        public void Should_allow_private_methods()
        {
            AssertMethodIsValid("Private");
        }

        [Test]
        public void Should_allow_methods_with_arguments()
        {
            AssertMethodIsValid("WithArguments");
        }

        [Test]
        public void Should_allow_methods_without_arguments()
        {
            AssertMethodIsValid("WithoutArguments");
        }

        [Test]
        public void Should_allow_instance_methods()
        {
            AssertMethodIsValid("Instance");
        }

        [Test]
        public void Should_not_allow_methods_without_a_step_definition_attribute()
        {
            AssertMethodIsInvalid("NoAttribute");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_should_throw_if_a_method_does_not_have_a_step_definition_attribute()
        {
            var invalidMethod = GetInvalidMethod("NoAttribute");
            new StepDefinition(invalidMethod);
        }

        [Test]
        public void Method_property_should_be_set_from_constructor()
        {
            Assert.That(_stepDefinition.Method, Is.SameAs(_successMethod));
        }

        [Test]
        public void Id_property_should_be_fully_qualified_method_name_with_parameter_types()
        {
            var fullNameForParameterlessMethod = typeof(ValidStepDefinitions).FullName + "." + _stepDefinition.Method.Name + "()";
            Assert.That(_stepDefinition.Id, Is.EqualTo(fullNameForParameterlessMethod));

            var parameterizedStepDefinition = new StepDefinition(Reflection.GetMethod(typeof(ValidStepDefinitions), "WithArguments"));
            var fullNameForParameterizedMethod = typeof(ValidStepDefinitions).FullName + "." + parameterizedStepDefinition.Method.Name + "(Int32)";
            Assert.That(parameterizedStepDefinition.Id, Is.EqualTo(fullNameForParameterizedMethod));
        }

        [Test]
        public void Id_property_of_equivalent_step_definitions_should_be_equal()
        {
            var _equivalentStepDefinition = new StepDefinition(_successMethod);
            Assert.That(_equivalentStepDefinition.Id, Is.EqualTo(_stepDefinition.Id));
        }

        [Test]
        public void Successful_invocation_should_not_throw()
        {
            _stepDefinition.Invoke(null);
        }

        [Test]
        public void Successful_invocation_of_instance_should_not_throw()
        {
            var objectFactory = new ObjectFactory();
            objectFactory.AddClass(typeof(ValidStepDefinitions));
            objectFactory.CreateObjects();
            var stepDefinition = new StepDefinition(GetValidMethod("Instance"));
            stepDefinition.Invoke(objectFactory);
        }

        [Test]
        public void Pending_should_be_false_when_method_does_not_have_Pending_attribute()
        {
            var stepDefinition = new StepDefinition(GetValidMethod("Given"));
            Assert.That(stepDefinition.Pending, Is.False);
        }

        [Test]
        public void Pending_should_be_true_when_method_has_Pending_attribute()
        {
            var stepDefinition = new StepDefinition(GetValidMethod("Pending"));
            Assert.That(stepDefinition.Pending, Is.True);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Incorrect_parameter_count_should_throw_exception()
        {
            _stepDefinition.Invoke(null, "parameter to cause invocation failure");
        }

        [Test]
        [ExpectedException(typeof(TargetInvocationException))]
        public void Method_that_throws_should_result_in_a_TargetInvocationException_being_thrown()
        {
            var stepDefinition = new StepDefinition(_exceptionMethod);
            stepDefinition.Invoke(null);
        }

        public static void AssertMethodIsValid(string methodName)
        {
            var method = GetValidMethod(methodName);
            Assert.IsTrue(StepDefinition.IsValidMethod(method));
        }

        static void AssertMethodIsInvalid(string methodName)
        {
            var method = GetInvalidMethod(methodName);
            Assert.IsFalse(StepDefinition.IsValidMethod(method));
        }

        static MethodInfo GetValidMethod(string methodName)
        {
            return Reflection.GetMethod(typeof(ValidStepDefinitions), methodName);
        }

        static MethodInfo GetInvalidMethod(string methodName)
        {
            return Reflection.GetMethod(typeof(InvalidStepDefinitions), methodName);
        }

        public static List<MethodInfo> GetStepDefinitionMethods()
        {
            return GetStepDefinitionMethods(typeof(ValidStepDefinitions));
        }

        public static List<MethodInfo> GetStepDefinitionMethods(Type type)
        {
            var methods = type.GetMethods(BindingFlags.DeclaredOnly | MethodFlags);
            return new List<MethodInfo>(methods);
        }

        public class ValidStepDefinitions
        {
            [Given("pattern")]
            public static void Succeeds() { }

            [Given("")]
            public static void ThrowsException() { throw new Exception("inner test Exception"); }

            [Given("")]
            public static void Given() { }

            [When("")]
            public static void When() { }

            [Then("")]
            public static void Then() { }

            [Given("")]
            public static void Public() { }

            [Given("")]
            internal static void Internal() { }

            [Given("")]
            protected static void Protected() { }

            [Given("")]
            private static void Private() { }

            [Given("")]
            public static void WithArguments(int arg) { }

            [Given("")]
            public static void WithoutArguments() { }

            [Given("")]
            public void Instance() { }

            [Given("")]
            [Pending]
            public void Pending() { }
        }

        public class InvalidStepDefinitions
        {
            public static void NoAttribute() { }
        }
    }
}