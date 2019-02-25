Feature: SQLiteDatabaseReadWrite
	In order to store and read chart series data
	As a reasearch scientist
	I want to be able to read and write array data to a SQLite database and associate attributes with them

Background:
	Given I have the test database

Scenario Outline: Read attribute value
	When I read the attributes of record <recordNum>
	Then the results contain <attributeName>

	Examples:
	| recordNum | attributeName        |
	| 1         | Monomer              |
	| 1         | Some other attribute |
	| 2         | Dimer                |
	| 2         | Diverging baseline   |
	| 3         | Monomer              |

Scenario: Read scatter series plot data
	When I read known series data from record 3
	Then it matches the example scatter data

Scenario: Write new scatter plot data
	When I add a new data series to the database
	Then a new data series record is added
	And it matches the example scatter data

Scenario: Add a new attribute
	When I add a new attribute called Low concentration
	Then the database contains an attribute called Low concentration

Scenario: Associate scatter plot with attribute
	When I associate data series 3 with Some other attribute
	And I read the attributes of record 3
	Then the results contain Some other attribute

Scenario: Can't add duplicate attribute tag
	When I add an existing attribute called Monomer
	Then a Unique constraint exception is thrown