Feature: Run .NET step definitions with shared state from Cucumber

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
    
  Scenario: Shared state in one step definition class
    Given a file named "features/adding.feature" with:
      """
        Scenario: Adding
          Given 2 cukes
          When I add 2 more cukes
          Then I should have 4 cukes

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
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
      }
      """
    When I run cucumber -f progress features
    Then STDERR should be empty
    And it should pass with
      """
      ...

      1 scenario (1 passed)
      3 steps (3 passed)

      """
    
  Scenario: Multiple scenarios each get their own state
    Given a file named "features/adding.feature" with:
      """
        Scenario: Adding with pickles
          Given 2 cukes
          And 2 pickles
          When I add 2 more cukes
          Then I should have 4 cukes
          And I should have 2 pickles
          
        Scenario: Adding without pickles
          Given 2 cukes
          When I add 2 more cukes
          Then I should have 4 cukes
          And I should have 0 pickles

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        int _cukeCount = 0;
        int _pickleCount = 0;
      
        [Given(@"^(\d+) cukes$")]
        public void GivenSomeCukes(int cukes)
        {
          _cukeCount = cukes;
        }
      
        [Given(@"^(\d+) pickles$")]
        public void GivenSomePickles(int pickles)
        {
          _pickleCount = pickles;
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
      
        [Then(@"^I should have (\d+) pickles$")]
        public void ExpectPickles(int pickles)
        {
          if (_pickleCount != pickles)
          {
            throw new Exception("Expected value: " + pickles.ToString() + ". Actual value: " + _pickleCount.ToString() + ".");
          }
        }
      }
      """
    When I run cucumber -f progress features
    Then STDERR should be empty
    And it should pass with
      """
      .........

      2 scenarios (2 passed)
      9 steps (9 passed)

      """
      
  Scenario: Shared state between two step definition classes
    Given a file named "features/adding.feature" with:
      """
        Scenario: Adding
          Given 2 cukes
          When I add 2 more cukes
          Then I should have 4 cukes

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class CukeJar
      {
        public int CukeCount { get; set; }
      }
      
      public class Steps1
      {
        CukeJar _cukeJar;
        
        public Steps1(CukeJar cukeJar)
        {
          _cukeJar = cukeJar;
        }

        [Given(@"^(\d+) cukes$")]
        public void GivenSomeCukes(int cukes)
        {
          _cukeJar.CukeCount = cukes;
        }

        [When(@"^I add (\d+) more cukes$")]
        public void AddCukes(int cukes)
        {
          _cukeJar.CukeCount += cukes;
        }
      }
      
      public class Steps2
      {
        CukeJar _cukeJar;
        
        public Steps2(CukeJar cukeJar)
        {
          _cukeJar = cukeJar;
        }
        
        [Then(@"^I should have (\d+) cukes$")]
        public void ExpectCukes(int cukes)
        {
          if (_cukeJar.CukeCount != cukes)
          {
            throw new Exception("Expected value: " + cukes.ToString() + ". Actual value: " + _cukeJar.CukeCount.ToString() + ".");
          }
        }
      }
      """
    When I run cucumber -f progress features
    Then STDERR should be empty
    And it should pass with
      """
      ...

      1 scenario (1 passed)
      3 steps (3 passed)

      """
