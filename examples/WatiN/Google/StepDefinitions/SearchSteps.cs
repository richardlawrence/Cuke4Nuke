using Cuke4Nuke.Framework;
using NUnit.Framework;
using WatiN.Core;

namespace Google.StepDefinitions
{
    public class SearchSteps
    {
        Browser _browser;

        [Before]
        public void SetUp()
        {
            _browser = new WatiN.Core.IE();
        }

        [After]
        public void TearDown()
        {
            if (_browser != null)
            {
                _browser.Dispose();
            }
        }

        [When(@"^(?:I'm on|I go to) the search page$")]
        public void GoToSearchPage()
        {
            _browser.GoTo("http://www.google.com/");
        }

        [When("^I search for \"(.*)\"$")]
        public void SearchFor(string query)
        {
            _browser.TextField(Find.ByName("q")).TypeText(query);
            _browser.Button(Find.ByName("btnG")).Click();
        }

        [Then("^I should be on the search page$")]
        public void IsOnSearchPage()
        {
            Assert.That(_browser.Title == "Google");
        }

        [Then("^I should see \"(.*)\" in the results$")]
        public void ResultsContain(string expectedResult)
        {
            Assert.That(_browser.ContainsText(expectedResult));
        }
    }
}
