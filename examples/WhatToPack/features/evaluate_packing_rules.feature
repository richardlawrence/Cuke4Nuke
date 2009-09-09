Feature: Evaluate packing rules
	As a user, I want to know whether I should bring an umbrella on my trip.
	
	Scenario Outline: Bring an umbrella when it rains
		Given a packing list with an umbrella
		And I only need an umbrella when the chance of precipitation is greater than 50%
		And I'm travelling to a place where the chance of precipitation is <Actual Chance>%
		When I generate a packing list
		Then the list <Might> include an umbrella
		
		Examples:
		| Actual Chance | Might      |
		| 60            | should     |
		| 40            | should not |