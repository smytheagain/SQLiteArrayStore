using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace SQLiteArrayStoreUnitTests
{
    [Binding]
    public class SQLiteDatabaseReadWriteSteps
    {
        private Dictionary<string, List<object>> databaseResults;

        private List<string> currentAttributes;

        [Given(@"I have the test database")]
        public void GivenIHaveTheTestDatabase()
        {
            DbReadTests existingReadTest = new DbReadTests();

            this.databaseResults = existingReadTest.DatabaseData;
        }
        
        [When(@"I read the attributes of record (.*)")]
        public void WhenIReadTheAttributesOfRecord(int recordNumber)
        {
            this.currentAttributes = new List<string>();

            for (int i = 0; i < databaseResults["Id"].Count; i++)
            {
                if (Convert.ToInt32(databaseResults["Id"][i]) == recordNumber)
                {
                    this.currentAttributes.Add(databaseResults["AttributeName"][i].ToString());
                }
            }

        }
        
        [Then(@"the results contain (.*)")]
        public void ThenTheResultsContainValue(string value)
        {
            Assert.IsTrue(currentAttributes.Contains(value));
        }
    }
}
