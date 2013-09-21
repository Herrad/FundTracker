@web
Feature: Creating a wallet
	In order to store money
	As someone with incoming and outgoing transactions
	I want to be able to create a wallet

Scenario: Creating a wallet with a name
	When I create a wallet with the name "my first wallet"
	Then I am taken to the display wallet page
	And the name "my first wallet" is displayed
