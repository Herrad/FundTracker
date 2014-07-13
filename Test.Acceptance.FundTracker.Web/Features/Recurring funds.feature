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