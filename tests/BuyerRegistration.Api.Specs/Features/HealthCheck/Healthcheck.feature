Feature: Healthcheck
	Confirm service is working

@mytag
Scenario: Check service health
	And my required headers
		| Key                   | Value |
		| x--source-platform | 10    |
		| x--client-id       | 1     |
		| x--client-ip       | 1     |
		| x--app-id          | 1     |
		| x--user-reference  | 1     |
	When I ask for a service healthcheck
	Then the response should be 200 OK