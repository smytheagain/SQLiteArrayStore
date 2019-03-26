Feature: CloseDialogTest
	Testing ability to close a dialog within a specflow test

Scenario: Open and close the dialog
	Given I have the simple messagebox window open
	When I invoke the close method
	Then the message box closes

Scenario: Open dialog and raise the ok button click event
	Given I have the simple messagebox window open
	When I invoke the ok button click event
	Then the message box closes

Scenario: Open dialog and click ok with no direct access to dialog
	Given The simple messagebox is opened without direct access to it
	When I use UI Automation to click ok
	Then the message box is no longer on screen
