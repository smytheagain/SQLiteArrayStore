using NUnit.Framework;
using SQLiteArrayStore;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using TechTalk.SpecFlow;

namespace SQLiteArrayStoreUnitTests
{
    [Binding]
    [Scope(Feature = "SQLiteDatabaseReadWrite")]
    public class SQLiteDatabaseReadWriteSteps
    {
        private Dictionary<string, List<object>> databaseResults;

        private List<string> currentAttributes;

        private double[,] scatterPlotData;

        private List<SQLiteException> exceptions;

        private void UpdateDatabaseResults()
        {
            TestDataHelper helper = new TestDataHelper();
            helper.SetDatabaseSeriesdataToNull();
            this.databaseResults = helper.DatabaseSeriesData;
        }

        [Given(@"I have the test database")]
        public void GivenIHaveTheTestDatabase()
        {
            this.UpdateDatabaseResults();
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

        [When(@"I read known series data from record 3")]
        public void WhenIReadKnownSeriesDataFromRecord()
        {
            this.scatterPlotData = (double[,])this.databaseResults["Data"][4];
        }

        [Then(@"it matches the example scatter data")]
        public void ThenItMatchesTheExampleScatterData()
        {
            Assert.IsTrue(DbDataConverter.CompareTwoDimensionalDoubleArrays(DbDataConverter.ConvertTwoDoubleArrayToMultiDimArray(TestDataHelper.RetentionVols, TestDataHelper.RI), this.scatterPlotData));
        }

        [When(@"I add a new data series to the database")]
        public void WhenIAddANewDataSeriesToTheDatabase()
        {
            double[,] multiArray = DbDataConverter.ConvertTwoDoubleArrayToMultiDimArray(TestDataHelper.RetentionVols, TestDataHelper.RI);

            byte[] serializedScatterSeries = DbDataConverter.SerializeMultiDimentionalDoubleArray(multiArray);
            string name = "Visual Studio Test";
            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.f");

            using (SQLiteConnector connector = new SQLiteConnector(TestDataHelper.FilePath))
            {
                string sql = $"INSERT INTO DataSeries (Name, AcquisitionDate, Data) values('{name}', '{date}', @serializedScatterSeries)";

                connector.WriteDataContainingParameters(sql, new KeyValuePair<string, byte[]>("serializedScatterSeries", serializedScatterSeries));
            }
        }

        [Then(@"a new data series record is added")]
        public void ThenANewDataSeriesRecordIsAdded()
        {
            int previousRecordCount = this.databaseResults["Id"].Select(i => Convert.ToInt32(i)).Distinct().Count();
            UpdateDatabaseResults();

            int newRecordCount = this.databaseResults["Id"].Select(i => Convert.ToInt32(i)).Distinct().Count();

            Assert.IsTrue(newRecordCount == previousRecordCount + 1);

            this.scatterPlotData = (double[,])this.databaseResults["Data"][5];
        }

        [When(@"I add a new attribute called (.*)")]
        public void WhenIAddANewAttribute(string attributeName)
        {
            using (SQLiteConnector connector = new SQLiteConnector(TestDataHelper.FilePath))
            {
                string sql = $"INSERT INTO Attributes (AttributeName) values('{attributeName}')";

                connector.WriteTextData(sql);
            }
        }

        [Then(@"the database contains an attribute called (.*)")]
        public void ThenTheDatabaseContainsAttribute(string attributeName)
        {
            List<string> attributesList = TestDataHelper.GetDatabaseAttributeNames(); //this.databaseResults["AttributeName"].Select(a => a.ToString()).Distinct();

            Assert.IsTrue(attributesList.Contains(attributeName));
        }

        [When(@"I associate data series (.*) with (.*)")]
        public void WhenIAssociateDataSeriesWithSomeOtherAttribute(int recordNum, string attributeName)
        {
            using (SQLiteConnector connector = new SQLiteConnector(TestDataHelper.FilePath))
            {
                string sql = $"INSERT INTO DataSeries_Attributes (DataSeries_Id, Attributes_AttributeName) values('{recordNum}', '{attributeName}')";

                connector.WriteTextData(sql);
            }

            UpdateDatabaseResults();
        }

        [When(@"I add an existing attribute called (.*)")]
        public void WhenIAddAnExistingAttributeCalledMonomer(string attributeName)
        {
            this.exceptions = new List<SQLiteException>();

            try
            {
                using (SQLiteConnector connector = new SQLiteConnector(TestDataHelper.FilePath))
                {
                    string duplicateAttribute = "Monomer";
                    string sql = $"INSERT INTO Attributes (AttributeName) values('{duplicateAttribute}')";

                    connector.WriteTextData(sql);
                }
            }
            catch (SQLiteException ex)
            {
                this.exceptions.Add(ex);
            }
        }

        [Then(@"a Unique constraint exception is thrown")]
        public void ThenAUniqueConstraintExceptionIsthrown()
        {
            bool uniqueContraintHit = false;

            if (this.exceptions.Count > 0)
            {
                foreach (SQLiteException ex in this.exceptions)
                {
                    if (ex.Message.Contains("UNIQUE constraint"))
                    {
                        uniqueContraintHit = true;
                        break;
                    }
                }
            }

            Assert.IsTrue(uniqueContraintHit);
        }

    }
}
