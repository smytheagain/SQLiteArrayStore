Feature: CloseDialogTest
	Testing ability to close a dialog within a specflow test

Scenario: Open and close the dialog
	Given I have the simple messagebox window open
	When I invoke the close method
	Then the message box closes

Scenario: Open dialog and click the ok button
	Given I have the simple messagebox window open
	When I invoke the ok button click event
	Then the message box closes
