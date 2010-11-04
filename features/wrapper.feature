Feature: Run Cuke4Nuke and Cucumber from a single command

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/wired.feature" with:
      """
      Feature: Test Feature
      
        Scenario: Wired
          Given we're all wired

      """
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
      
    Scenario: A passing step
      Given a step definition assembly containing:
        """
        public class GeneratedSteps
        {
          [Given("^we're all wired$")]
          public static void AllWired()
          {
          }
        }
        """
      When I run the cuke4nuke wrapper
      Then STDERR should be empty
      And it should pass with
        """
        .

        1 scenario (1 passed)
        1 step (1 passed)

        """
        
    Scenario: A failing step
      Given a step definition assembly containing:
        """
        public class GeneratedSteps
        {
          [Given("^we're all wired$")]
          public static void AllWired()
          {
            throw new Exception("message");
          }
        }
        """
      When I run the cuke4nuke wrapper
      Then it should fail