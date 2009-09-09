using System;
using Cuke4Nuke.Framework;
using NUnit.Framework;

namespace Cuke4Nuke.TestStepDefinitions
{
    public class SimpleSteps
    {
        [Given("^nothing$")]
        public static void DoNothing()
        {
        }

        [Then("^it should pass.$")]
        public static void ItShouldPass()
        {
            Assert.Pass();
        }

        [Then("^it should fail.$")]
        public static void ItShouldFail()
        {
            Assert.Fail();
        }
    }
}
