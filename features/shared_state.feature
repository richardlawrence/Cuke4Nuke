Feature: Run .NET step definitions with shared state from Cucumber

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/adding.feature" with:
      """
        Scenario: Adding
          Given 2 cukes
          When I add 2 more cukes
          Then I should have 4 cukes

      """
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
    
  Scenario: Shared state in one step definition class
    Given Cuke4Nuke started with a step definition assembly containing:
      """
      int _cukeCount = 0;
      
      [Given(@"^(\d+) cukes$")]
      public void GivenSomeCukes(int cukes)
      {
        _cukeCount = cukes;
      }
      
      [When(@"^I add (\d+) more cukes$")]
      public void AddCukes(int cukes)
      {
        _cukeCount += cukes;
      }
      
      [Then(@"^I should have (\d+) cukes$")]
      public void ExpectCukes(int cukes)
      {
        if (_cukeCount != cukes)
        {
          throw new Exception("Expected value: " + cukes.ToString() + ". Actual value: " + _cukeCount.ToString() + ".");
        }
      }

      """
    When I run cucumber -f progress features
    # Then STDERR should be empty
    And it should pass with
      """
      ...

      1 scenario (1 passed)
      3 steps (3 passed)

      """
  