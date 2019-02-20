using NUnit.Framework;
using SQLiteArrayStore;
using System.Collections.Generic;
using System.IO;

namespace SQLiteArrayStoreUnitTests
{
    [TestFixture]
    public class DbReadTests
    {
        private static string filePath;

        public static string FilePath
        {
            get
            {
                if (filePath == null)
                {
                    filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, @"TestResources\test.db");
                }

                return filePath;
            }
        }

        private Dictionary<string, List<object>> testDb;

        public Dictionary<string, List<object>> DatabaseData
        {
            get
            {
                if (this.testDb == null)
                {
                    Dictionary<string, List<object>> results;

                    using (SQLiteConnector connector = new SQLiteConnector(FilePath))
                    {
                        results = connector.ReadData("SELECT ds.Id, ds.Name, ds.AcquisitionDate, ds.Data, a.AttributeName FROM DataSeries as ds INNER JOIN DataSeries_Attributes as da ON ds.Id = da.DataSeries_Id INNER JOIN Attributes as a ON a.AttributeName = da.Attributes_AttributeName");
                    }

                    this.testDb = results;
                }

                return this.testDb;
            }
        }

        [TestCase]
        public void ReadAttibuteData()
        {
            Dictionary<string, List<object>> results = this.DatabaseData;

            Assert.AreEqual("Monomer", results["AttributeName"][0]);
            Assert.AreEqual("Some other attribute", results["AttributeName"][1]);
            Assert.AreEqual("Dimer", results["AttributeName"][2]);
            Assert.AreEqual("Diverging baseline", results["AttributeName"][3]);
            Assert.AreEqual("Monomer", results["AttributeName"][4]);
        }

        [TestCase]
        public void ReadArrayData()
        {
            Dictionary<string, List<object>> results = this.DatabaseData;

            Assert.IsTrue(DbDataConverter.CompareTwoDimensionalDoubleArrays(DbDataConverter.ConvertTwoDoubleArrayToMultiDimArray(DbWriteTests.RetentionVols, DbWriteTests.RI), (double[,])results["Data"][4]));
        }
    }
}
