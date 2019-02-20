Feature: SQLiteDatabaseReadWrite
	In order to store and read double arrays
	As a reasearch scientist
	I want to be able to read and write array data to a SQLite database

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
