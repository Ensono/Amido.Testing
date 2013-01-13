Feature: RestClientExtensionsTests
	In order to test the spec flow extensions
	etc. etc.


Scenario: Call Some Sites

	When I call Google at url http://www.google.co.uk
	Then the scenario context responses collection should include 1 requests
	And the last response should be set
	And the response http status code should be 200

	When I call Yahoo at url http://www.yahoo.co.uk
	Then the scenario context responses collection should include 2 requests
	And the last response should be set
	And the response http status code should be 200

	When I call Yahoo at url http://www.google.co.uk
	Then the scenario context responses collection should include 3 requests
	And the last response should be set
	And the response http status code should be 200

	When I call Yahoo at url http://www.google.co.uk
	Then the scenario context responses collection should include 4 requests
	And the last response should be set
	And the response http status code should be 200