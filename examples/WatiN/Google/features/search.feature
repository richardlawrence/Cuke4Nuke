Feature: Google search
    In order find things on the internet
    As a user
    I want to search for web pages with particular text in them
    
    Scenario: Load search page
        When I go to the search page
        Then I should be on the search page

    Scenario: Search
        Given I'm on the search page
        When I search for "richard lawrence"
        Then I should see "www.richardlawrence.info" in the results
