@web
Feature: Administering a wallet
	In order to store money
	As someone with incoming and outgoing transactions
	I want to be able to create a wallet

Scenario: Creating a wallet with a name
	When I create a wallet with the unique name starting with "my first wallet"
	Then I am taken to the display wallet page
	And the name starts with "my first wallet"

Scenario: Adding funds to a wallet
	Given I have created a wallet with a unique name starting with "my wallet"
	And my available funds are 50.00
	When I add 100.00 in funds to my wallet
	Then the amount in the wallet is 150.00

Scenario: Removing funds from a wallet
	Given I have created a wallet with a unique name starting with "my wallet"
	And my available funds are 50.00
	When I remove 25.00 in funds from my wallet
	Then the amount in the wallet is 25.00