using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SQLiteArrayStore
{
    public class DbDataConverter
    {
        public static byte[] SerializeDoubleArray(double[] doubleArray)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            byte[] retval;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, doubleArray);
                retval = stream.ToArray();
            }

            return retval;
        }

        public static byte[] SerializeMultiDimentionalDoubleArray(double[,] multiDoubleArray)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            byte[] retval;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, multiDoubleArray);
                retval = stream.ToArray();
            }

            return retval;
        }

        public static double[] DeSerializeBytesToDoubleArray(byte[] byteArray)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            double[] retval;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                retval = (double[])formatter.Deserialize(stream);
            }

            return retval;
        }

        public static double[,] DeSerializeBytesToMultiDimensionalDoubleArray(byte[] byteArray)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            double[,] retval;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                retval = (double[,])formatter.Deserialize(stream);
            }

            return retval;
        }

        public static double[,] ConvertTwoDoubleArrayToMultiDimArray(double[] arr1, double[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                throw new ArgumentException($"Array lengths must match: arr1.Length = {arr1.Length}, arr2.Length = {arr2.Length}");
            }

            double[,] multiArray = new double[arr1.Length, 2];
            for (int i = 0; i < arr1.Length; i++)
            {
                multiArray[i, 0] = arr1[i];
                multiArray[i, 1] = arr2[i];
            }

            return multiArray;
        }

        public static bool CompareTwoDimensionalDoubleArrays(double[,] data1, double[,] data2)
        {
            return data1.Rank == data2.Rank &&
                    Enumerable.Range(0, data1.Rank).All(dimension => data1.GetLength(dimension) == data2.GetLength(dimension)) &&
                    data1.Cast<double>().SequenceEqual(data2.Cast<double>());
        }
    }
}
