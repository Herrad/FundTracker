@web
Feature: Administering a wallet
	In order to store money
	As someone with incoming and outgoing transactions
	I want to be able to create a wallet

Scenario: Creating a wallet with a name
	When I create a wallet with the name "my first wallet"
	Then I am taken to the display wallet page
	And the name "my first wallet" is displayed

Scenario: Adding funds to a wallet
	Given I have created a wallet called "my wallet"
	And my available funds are 0
	When I add 100.00 in funds to my wallet
	Then the amount in the wallet is 100.00
 
 Scenario: Wallet does not display value from query string