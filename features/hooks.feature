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
      Feature: Test Feature
      
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
      
  Scenario: After hook throws exception (how else do we know it's called?)
    Given a file named "features/adding.feature" with:
      """
      Feature: Test Feature

        Scenario: After hook defined
          Given a passing step

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        [After]
        public void Teardown()
        {
          throw new Exception();
        }

        [Given(@"^a passing step$")]
        public void PassingStep()
        {
        }
      }
      """
    When I run cucumber -f pretty features
    Then it should fail with:
    """
    foo
    """

  Scenario: Before hook throws exception
    Given a file named "features/adding.feature" with:
      """
      Feature: Test Feature

        Scenario: Before hook defined
          Given a passing step

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        [Before]
        public void Setup()
        {
          throw new Exception("Something is wrong.");
        }

        [Given(@"^a passing step$")]
        public void PassingStep()
        {
        }
      }
      """
    When I run cucumber -f pretty features
    Then it should fail with:
    """
    Feature: Test Feature

      Scenario: Before hook defined # features/adding.feature:3
      Something is wrong. (System.Exception from localhost:3901)
      features/wired.feature:3:in `Before'
        Given we're all wired # features/wired.feature:4

    Failing Scenarios:
    cucumber features/wired.feature:3 # Scenario: Wired

    1 scenario (1 failed)
    1 step (1 skipped)

    """

  Scenario: Tagged Before hook (string, positive)
    Given a file named "features/adding.feature" with:
      """
      Feature: Test Feature

        @my_tag
        Scenario: Was Before hook called?
          Then the Before hook should be called

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        bool _isBeforeCalled = false;

        [Before("@my_tag")]
        public void Setup()
        {
          _isBeforeCalled = true;
        }

        [Then(@"^the Before hook should be called$")]
        public void ExpectBeforeHookCalled()
        {
          Assert.True(_isBeforeCalled);
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

  Scenario: Tagged Before hook (string, negative)
    Given a file named "features/adding.feature" with:
      """
      Feature: Test Feature

        @my_tag
        Scenario: Was Before hook called?
          Then the Before hook should not be called

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        bool _isBeforeCalled = false;

        [Before("@not_my_tag")]
        public void Setup()
        {
          _isBeforeCalled = true;
        }

        [Then(@"^the Before hook should not be called$")]
        public void ExpectBeforeHookNotCalled()
        {
          Assert.False(_isBeforeCalled);
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
    
  Scenario: Tagged After hook (string, positive)
    Given a file named "features/adding.feature" with:
      """
      Feature: Test Feature

        @my_tag
        Scenario: After hook defined
          Given a passing step

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        [After("@my_tag")]
        public void TearDown()
        {
          throw new Exception();
        }

        [Given(@"^a passing step$")]
        public void APassingStep()
        {
        }
      }
      """
    When I run cucumber -f progress features
    Then it should fail

  Scenario: Tagged After hook (string, negative)
    Given a file named "features/adding.feature" with:
      """
      Feature: Test Feature
      
        @my_tag
        Scenario: After hook defined
          Given a passing step

      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        [After("@not_my_tag")]
        public void TearDown()
        {
          throw new Exception();
        }

        [Given(@"^a passing step$")]
        public void APassingStep()
        {
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

 