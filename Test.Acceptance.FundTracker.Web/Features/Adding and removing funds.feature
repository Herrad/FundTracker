Feature: Adding and removing funds
	In order to keep track of ad hoc transactions
	As a person with money
	I want to see when I've added and remved funds from my wallet

	@wip
Scenario: Removing funds 2 days ago 
	Given this wallet exists
	| Unique Name | Starting Funds |
	| my wallet   | 250.00          |
	And I removed 100 in funds 2 days ago for "dance lessons"
	When I view my withdrawals for 2 days ago
	Then I can see an entry for "dance lessons"

