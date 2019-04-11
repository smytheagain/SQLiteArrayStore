Feature: MoreRealisticLoginDialog
	In order to authenticate a user
	As an IT admin person
	I want to force users to log in whenever it is most inconvenient

Scenario: Manipulate login dialog programmatically without a UI test framework
	Given a log in request
	When 'testUser' and 'T3stP@55w0rd' is entered into the login dialog
		And the user accepts the login dialog
	Then the authentication result is 'Authenticated'
