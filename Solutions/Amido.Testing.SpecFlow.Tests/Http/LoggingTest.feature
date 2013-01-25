Feature: LoggingTest
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	When I call google
	Then the scenario context responses collection should include 1 requests
	And the last response should be set
	And the response http status code should be 200
