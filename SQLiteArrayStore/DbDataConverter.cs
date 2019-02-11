using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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
    }
}
