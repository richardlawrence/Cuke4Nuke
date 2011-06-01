Feature: Tag logic
  In order to conveniently run subsets of features
  As a Cuker
  I want to select features using logical AND/OR of tags

  Background:
    Given a file named "features/tagulicious.feature" with:
    """
    Feature: Sample

      @one @three
      Scenario: Example
        Given passing

      @one
      Scenario: Another Example
        Given passing

      @three
      Scenario: Yet another Example
        Given passing

      @ignore
      Scenario: And yet another Example
    """

  Scenario: Before hooks ORing
    Given Cuke4Nuke started with a step definition assembly containing:
    """
    public class GeneratedSteps
    {
      [Before("@one,@three")]
      public void Setup()
      {
        throw new Exception("boom");
      }
    }
    """
    When I run cucumber -q features/tagulicious.feature
    Then it should fail with:
    """
    Feature: Sample

      @one @three
      Scenario: Example
      boom (RuntimeError)
      ./features/support/hooks.rb:2:in `Before'
        Given passing

      @one
      Scenario: Another Example
      boom (RuntimeError)
      ./features/support/hooks.rb:2:in `Before'
        Given passing

      @three
      Scenario: Yet another Example
      boom (RuntimeError)
      ./features/support/hooks.rb:2:in `Before'
        Given passing

      @ignore
      Scenario: And yet another Example

    Failing Scenarios:
    cucumber features/tagulicious.feature:4 # Scenario: Example
    cucumber features/tagulicious.feature:8 # Scenario: Another Example
    cucumber features/tagulicious.feature:12 # Scenario: Yet another Example

    4 scenarios (3 failed, 1 passed)
    3 steps (3 undefined)

    """

  Scenario: Before hooks ANDing
    Given Cuke4Nuke started with a step definition assembly containing:
    """
    public class GeneratedSteps
    {
      [Before("@one", "@three")]
      public void Setup()
      {
        throw new Exception("boom");
      }
    }
    """
    When I run cucumber -q features/tagulicious.feature
    Then it should fail with:
    """
    Feature: Sample

      @one @three
      Scenario: Example
      boom (RuntimeError)
      ./features/support/hooks.rb:2:in `Before'
        Given passing

      @one
      Scenario: Another Example
        Given passing

      @three
      Scenario: Yet another Example
        Given passing

      @ignore
      Scenario: And yet another Example

    Failing Scenarios:
    cucumber features/tagulicious.feature:4 # Scenario: Example

    4 scenarios (1 failed, 2 undefined, 1 passed)
    3 steps (3 undefined)

    """

  Scenario: Before hooks ANDing with a bad hook matching nothing
    Given Cuke4Nuke started with a step definition assembly containing:
    """
    public class GeneratedSteps
    {
      [Before("@one", "@notused")]
      public void Setup()
      {
        throw new Exception("boom");
      }
    }
    """
    When I run cucumber -q features/tagulicious.feature
    Then it should pass with:
    """
    Feature: Sample

      @one @three
      Scenario: Example
        Given passing

      @one
      Scenario: Another Example
        Given passing

      @three
      Scenario: Yet another Example
        Given passing

      @ignore
      Scenario: And yet another Example

    4 scenarios (3 undefined, 1 passed)
    3 steps (3 undefined)

    """

  Scenario: After hooks ORing
    Given Cuke4Nuke started with a step definition assembly containing:
    """
    public class GeneratedSteps
    {
      [After("@one,@three")]
      public void TearDown()
      {
        throw new Exception("boom");
      }
    }
    """
    When I run cucumber -q features/tagulicious.feature
    Then it should fail with:
    """
    Feature: Sample

      @one @three
      Scenario: Example
        Given passing
        boom (RuntimeError)
        ./features/support/hooks.rb:2:in `After'

      @one
      Scenario: Another Example
        Given passing
        boom (RuntimeError)
        ./features/support/hooks.rb:2:in `After'

      @three
      Scenario: Yet another Example
        Given passing
        boom (RuntimeError)
        ./features/support/hooks.rb:2:in `After'

      @ignore
      Scenario: And yet another Example

    Failing Scenarios:
    cucumber features/tagulicious.feature:4 # Scenario: Example
    cucumber features/tagulicious.feature:8 # Scenario: Another Example
    cucumber features/tagulicious.feature:12 # Scenario: Yet another Example

    4 scenarios (3 failed, 1 passed)
    3 steps (3 undefined)

    """

  Scenario: After hooks ANDing
    Given Cuke4Nuke started with a step definition assembly containing:
    """
    public class GeneratedSteps
    {
      [After("@one", "@three")]
      public void TearDown()
      {
        throw new Exception("boom");
      }
    }
    """
    When I run cucumber -q features/tagulicious.feature
    Then it should fail with:
    """
    Feature: Sample

      @one @three
      Scenario: Example
        Given passing
        boom (RuntimeError)
        ./features/support/hooks.rb:2:in `After'

      @one
      Scenario: Another Example
        Given passing

      @three
      Scenario: Yet another Example
        Given passing

      @ignore
      Scenario: And yet another Example

    Failing Scenarios:
    cucumber features/tagulicious.feature:4 # Scenario: Example

    4 scenarios (1 failed, 2 undefined, 1 passed)
    3 steps (3 undefined)

    """