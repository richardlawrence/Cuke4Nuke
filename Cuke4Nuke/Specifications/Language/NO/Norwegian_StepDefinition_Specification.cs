using Cuke4Nuke.Core;
using Cuke4Nuke.Framework.Languages.NO;
using NUnit.Framework;

namespace Cuke4Nuke.Specifications.language.no
{
    [TestFixture]
    public class Norwegian_StepDefinition_Specification 
    {

        [Test]
        public void Should_allow_method_with_a_Gitt_attribute()
        {
            AssertMethodIsValid("Gitt");
        }

        [Test]
        public void Should_allow_method_with_a_Når_attribute()
        {
            AssertMethodIsValid("Når");
        }

        [Test]
        public void Should_allow_method_with_a_Så_attribute()
        {
            AssertMethodIsValid("Så");
        }

        private void AssertMethodIsValid(string methodName)
        {
            var method = Reflection.GetMethod(typeof (ValidNorwegianSteps), methodName);
            Assert.IsTrue(StepDefinition.IsValidMethod(method));
        }


        public class ValidNorwegianSteps
        {
            [Gitt("test")]
            public static void Gitt() { }

            [Når("test")]
            public static void Når(){}

            [Så("test")]
            public  static  void Så(){}
        }
    }
}