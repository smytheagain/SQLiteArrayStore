using NUnit.Framework;
using SQLiteArrayStore;
using System.Collections.Generic;

namespace SQLiteArrayStoreUnitTests
{
    [TestFixture]
    public class DbReadTests
    {
        private TestDataHelper Helper;

        public DbReadTests()
        {
            this.Helper = new TestDataHelper();
        }

        [TestCase]
        public void ReadAttibuteData()
        {
            Dictionary<string, List<object>> results = this.Helper.DatabaseSeriesData;

            Assert.AreEqual("Monomer", results["AttributeName"][0]);
            Assert.AreEqual("Some other attribute", results["AttributeName"][1]);
            Assert.AreEqual("Dimer", results["AttributeName"][2]);
            Assert.AreEqual("Diverging baseline", results["AttributeName"][3]);
            Assert.AreEqual("Monomer", results["AttributeName"][4]);
        }

        [TestCase]
        public void ReadArrayData()
        {
            Dictionary<string, List<object>> results = this.Helper.DatabaseSeriesData;

            Assert.IsTrue(DbDataConverter.CompareTwoDimensionalDoubleArrays(DbDataConverter.ConvertTwoDoubleArrayToMultiDimArray(TestDataHelper.RetentionVols, TestDataHelper.RI), (double[,])results["Data"][4]));
        }
    }
}
