Feature: Print step definition snippets for undefined steps

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
  
  Scenario: Undefined prints snippet
    Given a file named "features/wired.feature" with:
      """
        Scenario: Wired
          Given we're all wired

      """
    And Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then the output should contain
      """
      [Pending]
      [Given(@"^we're all wired$")]
      public void WereAllWired()
      {
      }
      """

  Scenario: Snippet with a table
    Given a file named "features/wired.feature" with:
      """
        Scenario: Wired
          Given we're all wired
            | who     |
            | Richard |
            | Matt    |
            | Aslak   |
      """
    And Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then the output should contain
      """
      [Pending]
      [Given(@"^we're all wired$")]
      public void WereAllWired(Table table)
      {
      }
      """

  Scenario: Snippet with a multiline string
    Given a file named "features/wired.feature" with:
      """
        Scenario: Wired
          Given we're all wired
            \"\"\"
              Lorem ipsum dolor sit amet, consectetur adipiscing elit.
              Morbi porttitor semper lobortis. Duis nec nibh felis, vitae tempor augue.
            \"\"\"
      """
    And Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then the output should contain
      """
      [Pending]
      [Given(@"^we're all wired$")]
      public void WereAllWired(string str)
      {
      }
      """

  Scenario: Snippet with a scenario outline
    Given a file named "features/wired.feature" with:
      """
        Scenario Outline: Wired
          Given we're all <something>

          Examples:
          | something |
          | wired     |
          | not wired |

      """
    And Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then the output should contain
      """
      [Pending]
      [Given(@"^we're all wired$")]
      public void WereAllWired()
      {
      }
      """
    And the output should contain
      """
      [Pending]
      [Given(@"^we're all not wired$")]
      public void WereAllNotWired()
      {
      }
      """

  Scenario: Snippet with Background
    Given a file named "features/wired.feature" with:
      """
        Background:
          Given something to do first

        Scenario: Wired
          Given we're all wired

      """
    And Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then the output should contain
      """
      [Pending]
      [Given(@"^we're all wired$")]
      public void WereAllWired()
      {
      }
      """
    And the output should contain
      """
      [Pending]
      [Given(@"^something to do first$")]
      public void SomethingToDoFirst()
      {
      }
      """

  Scenario: Snippet for step with trailing comma
    Given a file named "features/wired.feature" with:
      """
        Scenario: Comma separated
          Given the separator is ,

      """
    And Cuke4Nuke started with no step definition assemblies
    When I run cucumber -f pretty
    Then STDERR should be empty
    And the output should contain
      """
      [Pending]
      [Given(@"^the separator is ,$")]
      public void TheSeparatorIs()
      {
      }
      """

   Scenario: Snippet for step with double quotes
     Given a file named "features/wired.feature" with:
      """
        Scenario: Quotes
          Given I "love" quotes

      """
     And Cuke4Nuke started with no step definition assemblies
     When I run cucumber -f pretty
     Then the output should contain
      """
      [Given(@"^I ""love"" quotes$")]
      """
