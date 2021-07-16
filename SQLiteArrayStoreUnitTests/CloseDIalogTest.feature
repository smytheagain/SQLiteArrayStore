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

Scenario Outline: Open dialog and click ok with no direct access to dialog using automation framework
	Given The simple messagebox is opened without direct access to it
	And I am using '<framework>' automation framework
	When I use UI Automation to click ok
	Then the message box is no longer on screen

Examples: 
	| framework |
	| white     |
	| flaui     |
	| appium    |

Scenario: Open dialog and raise the ok button click event without direct access
	Given The simple messagebox is opened without direct access to it
	When I invoke the ok button click event using the window handle
	Then the message box also closes

@ignore
Scenario: Kick the click tests ass
	# Clicks the button on https://clickspeedtest.com/ as rapidly as it can.
	# Manually open the page then set the processID in the steps to the process ID of the browser (Tested on Chrome)
	Then Just Do It
