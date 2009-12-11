Feature: Use table multi-line step arguments

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """

  Scenario: Table in a Given
    Given a file named "features/table.feature" with:
      """
      Scenario: Shopping list
        Given I have a shopping list with the following items:
          | item      | count |
          | cucumbers |   3   |
          | bananas   |   5   |
          | tomatoes  |   2   |
        When I buy everything on my list
        Then my cart should contain 10 items
      """
    And Cuke4Nuke started with a step definition assembly containing:
      """
      public class GeneratedSteps
      {
        Cuke4Nuke.Framework.Table _shoppingList;
        int _itemsInCart = 0;

        [Given(@"^I have a shopping list with the following items:$")]
        public void GivenAShoppingList(Cuke4Nuke.Framework.Table shoppingList)
        {
          _shoppingList = shoppingList;
        }

        [When(@"^I buy everything on my list$")]
        public void BuyEverything()
        {
          foreach (System.Collections.Generic.Dictionary<string, string> row in _shoppingList.Hashes)
          {
            _itemsInCart += Int32.Parse(row["count"]);
          }
        }

        [Then(@"^my cart should contain (\d+) items$")]
        public void CartContainsNItems(int itemCount)
        {
          if (_itemsInCart != itemCount)
          {
            throw new Exception("Expected value: " + itemCount.ToString() + ". Actual value: " + _itemsInCart.ToString() + ".");
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

  # Scenario: Table in a Then
  #   Given I need 3 cucumbers
  #   And I need 5 bananas
  #   And I need 2 tomatoes
  #   When I build my shopping list
  #   Then the shopping list should look like:
  #     | item      | count |
  #     | cucumbers |   3   |
  #     | bananas   |   5   |
  #     | tomatoes  |   2   |