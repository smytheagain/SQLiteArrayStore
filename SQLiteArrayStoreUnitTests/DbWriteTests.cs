﻿using System;
using SQLiteArrayStore;
using System.Collections.Generic;
using NUnit.Framework;

namespace SQLiteArrayStoreUnitTests
{
    [TestFixture]
    public class DbWriteTests
    {
        [Test]
        public void InsertNewArrayData()
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

        [Test]
        public void InsertNewAttribute()
        {
            using (SQLiteConnector connector = new SQLiteConnector(TestDataHelper.FilePath))
            {
                string uniqueAttribute = "attribute " + DateTime.Now.ToString("ddMMyyyy_HHmmss.f");
                string sql = $"INSERT INTO Attributes (AttributeName) values('{uniqueAttribute}')";

                connector.WriteTextData(sql);
            }
        }

        [Test]
        public void InsertNewArrayAttributeRelationship()
        {
            using (SQLiteConnector connector = new SQLiteConnector(TestDataHelper.FilePath))
            {
                string sql = $"INSERT INTO DataSeries_Attributes (DataSeries_Id, Attributes_AttributeName) values('3', 'Monomer')";

                connector.WriteTextData(sql);
            }
        }
    }
}
