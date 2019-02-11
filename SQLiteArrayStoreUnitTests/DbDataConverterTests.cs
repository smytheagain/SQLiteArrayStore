using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLiteArrayStoreUnitTests
{
    [TestClass]
    public class DbDataConverterTests
    {
        [TestMethod]
        public void SerializeAndDeserializeDoubleArray()
        {
            double[] testArray = new double[] { 1.1, 1.342523, 12.2344, 45.243145, 100.34522 };

            byte[] bytes = SQLiteArrayStore.DbDataConverter.SerializeDoubleArray(testArray);

            double[] deSerialized = SQLiteArrayStore.DbDataConverter.DeSerializeBytesToDoubleArray(bytes);

            int i = 0;
            foreach (double item in testArray)
            {
                Assert.AreEqual(item, deSerialized[i]);
                i++;
            }
        }

        [TestMethod]
        public void SerializeAndDeserializeMultiDoubleArray()
        {
            double[,] testArray = new double[,] { { 1.1, 1.2344, 1.57522 }, { 1.342523, 45.243145, 67.8497 } };

            byte[] bytes = SQLiteArrayStore.DbDataConverter.SerializeMultiDimentionalDoubleArray(testArray);

            double[,] deSerialized = SQLiteArrayStore.DbDataConverter.DeSerializeBytesToMultiDimensionalDoubleArray(bytes);

            int i = 0;
            for (int x = 0; x < testArray.GetLength(0); x ++)
            {
                for (int y = 0; y < testArray.GetLength(1); y++)
                {
                    Assert.AreEqual(testArray[x, y], deSerialized[x, y]);
                    i++;
                }
            }
        }
    }
}
