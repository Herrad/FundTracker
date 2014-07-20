Feature: Calendar
	In order to administer my funds over time
	As a person with a wallet
	I want to change the date I'm looking at


Scenario: Viewing a wallet for today
	Given this wallet exists
         | Unique Name | 
         | my wallet   | 
	When I load the wallet with name "my wallet"
	Then the calander should have today's date selected
	

Scenario: Viewing a wallet for next month
	Given this wallet exists
         | Unique Name | 
         | my wallet   | 
	And I am administering this wallet
	When I view next month
	Then the calander should have the first day of next month selected
	

Scenario: Viewing a wallet for last month
	Given this wallet exists
         | Unique Name | 
         | my wallet   | 
	And I am administering this wallet
	When I view last month
	Then the calander should have the last day of last month selected