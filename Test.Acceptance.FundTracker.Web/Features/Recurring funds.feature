Feature: Recurring funds
	In order to manage my finances
	As a person with recurring changes
	I want to track the impact of funds being added and removed
	

@wip
Scenario: Adding recurring fund changes
	Given this wallet exists
	| Unique Name | Starting Funds	|
	| my wallet   | 100.00          |
	And I am administering this wallet
	When I add a recurring withdrawal of 50.00
	Then a withdrawal tile is shown with the outgoing amount set to 50.00
	And the amount in the wallet is 50.00