Feature: Adding and removing funds
	In order to keep track of ad hoc transactions
	As a person with money
	I want to see when I've added and remved funds from my wallet
	
Scenario: Removing funds 2 days ago 
	Given this wallet exists
	| Unique Name | Starting Funds |
	| my wallet   | 250.00          |
	And I removed 100 in funds 2 days ago for "dance lessons"
	When I view my withdrawals for 2 days ago
	Then I can see an entry for "dance lessons"

Scenario: Incoming funds in 5 days 
	Given this wallet exists
	| Unique Name | Starting Funds |
	| my wallet   | 250.00          |
	And I have a deposit of 1000 due in 5 days for "payday"
	When I view my deposits for 5 days ahead
	Then I can see an entry for "payday"


Scenario: Multiple changes on different days
	Given this wallet exists
	| Unique Name | Starting Funds |
	| my wallet   | 250.00         |
	And I have a deposit of 1000 due in 5 days for "payday"
	And I have a deposit of 250 due in 2 days for "debt"
	When I view my deposits for 5 days ahead
	Then I can see an entry for "payday"
	And no entry for "debt" is present

