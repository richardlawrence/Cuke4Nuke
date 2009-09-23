using System;

using Cuke4Nuke.Framework;

namespace Cuke4Nuke.TestStepDefinitions
{
    public class ExternalSteps
    {
        [Given("^nothing$")]
        public static void DoNothing()
        {
        }

        [Then("^it should pass.$")]
        public static void ItShouldPass()
        {
        }

        [Then("^it should fail.$")]
        public static void ItShouldFail()
        {
            throw new Exception("intentional");
        }
    }
}
