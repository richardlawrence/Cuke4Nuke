Feature: Run .NET Before and After hooks from Cucumber

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
    
  Scenario: Before hook
    Given a file named "features/adding.feature" with:
      """
        Scenario: Cuke count set in Before
          Then I should have 4 cukes

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        int _cukeCount = 0;
        
        [Before]
        public void Setup()
        {
          _cukeCount = 4;
        }
      
        [Then(@"^I should have (\d+) cukes$")]
        public void ExpectCukes(int cukes)
        {
          if (_cukeCount != cukes)
          {
            throw new Exception("Expected value: " + cukes.ToString() + ". Actual value: " + _cukeCount.ToString() + ".");
          }
        }
      }
      """
    When I run cucumber -f progress features
    Then STDERR should be empty
    And it should pass with
      """
      .

      1 scenario (1 passed)
      1 step (1 passed)

      """