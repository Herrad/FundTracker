Feature: Recurring funds
	In order to manage my finances
	As a person with recurring changes
	I want to track the impact of funds being added and removed
	
Scenario: Adding recurring withdrawal changes 
	Given this wallet exists
	| Unique Name | Starting Funds	|
	| my wallet   | 100.00          |
	And my wallet has no recurring changes
	And I am administering this wallet
	When I add the following recurring withdrawal
	| Name | Amount |
	| debt | 50     |
	Then the outgoing total value is 50.00
	And the amount in the wallet is 50.00


Scenario: Adding recurring deposit changes 
	Given this wallet exists
	| Unique Name | Starting Funds	|
	| my wallet   | 100.00          |
	And my wallet has no recurring changes
	And I am administering this wallet
	When I add the following recurring deposit
	| Name | Amount |
	| debt | 50     |
	Then the incoming total value is 50.00
	And the amount in the wallet is 150.00


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
