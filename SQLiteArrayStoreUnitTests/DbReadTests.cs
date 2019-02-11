using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLiteArrayStore;

namespace SQLiteArrayStoreUnitTests
{
    [TestClass]
    public class DbReadTests
    {
        public static readonly string FilePath = @"C:\Users\jusmith\Source\Repos\SQLiteArrayStore\SQLiteArrayStore\test.db";

        [TestMethod]
        public void ReadData()
        {
            using (SQLiteConnector connector = new SQLiteConnector(FilePath))
            {
                Dictionary<string, List<object>> results = connector.ReadData("SELECT ds.Id, ds.Name, ds.AcquisitionDate, ds.Data, a.AttributeName FROM DataSeries as ds INNER JOIN DataSeries_Attributes as da ON ds.Id = da.DataSeries_Id INNER JOIN Attributes as a ON a.AttributeName = da.Attributes_AttributeName");

                Assert.AreEqual("Monomer", results["AttributeName"][0]);

                Assert.IsTrue(DbDataConverter.CompareTwoDimensionalDoubleArrays(DbDataConverter.ConvertTwoDoubleArrayToMultiDimArray(DbWriteTests.RetentionVols, DbWriteTests.RI), (double[,])results["Data"][4]));
            }
        }
    }
}
