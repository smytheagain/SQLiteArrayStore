Feature: CloseDialogTest
	Testing ability to close a dialog within a specflow test

Scenario: Open and close the dialog
	Given I have the simple messagebox window open
	When I invoke the close method
	Then the message box closes
