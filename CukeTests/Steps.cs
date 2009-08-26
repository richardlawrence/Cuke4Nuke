using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cuke4Nuke;

namespace CukeTests
{
    public class PackingSteps : StepDefinions
    {
        //        Given /^a packing list with an umbrella$/ do
        //  pending
        //end

        //Given /^I only need an umbrella when the chance of precipitation is greater than 50%
        //  pending
        //end

        //Given /^I'm travelling to a place where the chance of precipitation is 60%$/ do
        //  pending
        //end

        //When /^I generate a packing list$/ do
        //  pending
        //end

        //Then /^the list should include an umbrella$/ do
        //  pending
        //end

        //Given /^I'm travelling to a place where the chance of precipitation is 40%$/ do
        //  pending
        //end

        //Then /^the list should not include an umbrella$/ do
        //  pending
        //end

        // option 1
        [Given(@"^a packing list with an? (.*)$")]
        public void PackingListWitThing(string thing)
        {
        }

        // option 2
        public void DefineSteps()
        {
            Given(@"^a packing list with an? (.*)$", (string thing) => {
                // lambda body
            });
        }

    }
}
