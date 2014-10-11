Feature: Administering a wallet 
	In order to store money
	As someone with incoming and outgoing transactions
	I want to be able to create a wallet

Scenario: Creating a wallet with a name
	When I create a wallet with the unique name starting with "my first wallet" 
	Then I am taken to the display wallet page
	And the name starts with "my first wallet"

Scenario: Adding funds to a wallet
	Given this wallet exists
	| Unique Name | 
	| my wallet   | 
	When I add 100.00 in funds to my wallet
	Then the amount in the wallet is 100.00

Scenario: Removing funds from a wallet
	Given this wallet exists
	| Unique Name |
	| my wallet   |
	When I remove 25.00 in funds from my wallet
	Then the amount in the wallet is -25.00

Scenario: Creating a wallet adds it to the database
	When I create a wallet with the unique name starting with "my wallet"
	Then the database contains a wallet with my name
