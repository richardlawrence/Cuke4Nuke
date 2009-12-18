Feature: Print step definition snippets for undefined steps

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/wired.feature" with:
      """
        Scenario: Wired
          Given we're all wired

      """
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
  
  Scenario: Undefined prints snippet
    Given Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then the output should contain
      """
      [Given(@"^we're all wired$")]
      public void WereAllWired()
      {
          throw new NotImplementedException();
      }
      """