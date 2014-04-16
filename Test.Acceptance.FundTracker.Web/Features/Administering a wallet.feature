﻿@web
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

Scenario: View an existing wallet
	Given A wallet already exists called "existing wallet"
	When I load the wallet with name "existing wallet"
	Then I am taken to the display wallet page
	And the name starts with "existing wallet"

Scenario: Creating a wallet adds it to the database
	When I create a wallet with the unique name starting with "my wallet"
	Then the database contains a wallet with my name

@wip
#Scenario: Adding recurring fund changes
#	Given I have created a wallet with a unique name starting with "my wallet"
#	And my available funds are 100.00
#	When I add a recurring withdrawal of 50.00
#	Then a withdrawal tile is shown with the outgoing amount set to 50.00
#	And the amount in the wallet is 50.00