Feature: Calendar
	In order to administer my funds over time
	As a person with a wallet
	I want to change the date I'm looking at


Scenario: Viewing a wallet for today
	Given this wallet exists
         | Unique Name | Starting Funds |
         | my wallet   | 0              |
	When I load the wallet with name "my wallet"
	Then the calander should have today's date selected
