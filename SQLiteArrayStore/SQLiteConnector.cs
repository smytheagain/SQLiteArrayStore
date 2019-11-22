namespace SQLiteArrayStore
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;

    public class SQLiteConnector : IDisposable
    {
        public SQLiteConnection Connection { get; private set; }

        public SQLiteConnector(string databasePath)
        {
            if (File.Exists(databasePath))
            {
                string connectionStr = $"Data Source={databasePath}; Version=3;";
                this.Connection = new SQLiteConnection(connectionStr);
            }
            else
            {
                throw new FileNotFoundException("File not found.", databasePath);
            }
        }

        public Dictionary<string, List<object>> ReadData(string query)
        {
            if (query.ToUpper().StartsWith("SELECT"))
            {
                Dictionary<string, List<object>> results = new Dictionary<string, List<object>>();

                using (SQLiteCommand command = new SQLiteCommand(query, this.Connection))
                {
                    command.Connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        results.Add(reader.GetName(i), new List<object>());
                    }

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object dataValue = reader.GetValue(i);
                            if (dataValue.GetType() == typeof(byte[]))
                            {
                                results[reader.GetName(i)].Add(DbDataConverter.DeSerializeBytesToMultiDimensionalDoubleArray((byte[])dataValue));
                            }
                            else
                            {
                                results[reader.GetName(i)].Add(dataValue);
                            }
                        }
                    }

                    reader.Close();
                    command.Connection.Close();
                }

                return results;
            }
            else
            {
                throw new ArgumentException("Query must start with a 'SELECT' statement.", "query");
            }
        }

        public DataTable ReadDataAsTable(string query)
        {
            if (query.ToUpper().StartsWith("SELECT"))
            {
                DataTable results = new DataTable();

                using (SQLiteCommand command = new SQLiteCommand(query, this.Connection))
                {
                    command.Connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    results.Load(reader);

                    reader.Close();
                    command.Connection.Close();
                }

                return results;
            }
            else
            {
                throw new ArgumentException("Query must start with a 'SELECT' statement.", "query");
            }
        }

        public void WriteTextData(string query)
        {
            this.WriteDataContainingParameters(query, new KeyValuePair<string, byte[]>());
        }

        /// <summary>
        /// Inserts a record that contains serialized data into a SQLite database.
        /// </summary>
        /// <param name="query">'INSERT INTO' SQL query containing serialized parameters denoted by @ symbol (see example).</param>
        /// <param name="serializedParameters">Key value pair containing the parameter name and the serialized parameter data.</param>
        /// <example>
        /// Example query string:
        /// "INSERT INTO DataSeries (Name, AcquisitionDate, Data) values('Series name', 'date of acquisition', @serializedScatterSeries')"
        /// parameter = new KeyValuePair&lt;string, byte[]&gt;("serializedScatterSeries", serializedData).
        /// </example>
        public void WriteDataContainingParameters(string query, params KeyValuePair<string, byte[]>[] serializedParameters)
        {
            if (query.ToUpper().StartsWith("INSERT INTO"))
            {
                using (SQLiteCommand command = new SQLiteCommand(query, this.Connection))
                {
                    command.Connection.Open();
                    foreach (KeyValuePair<string, byte[]> parameter in serializedParameters)
                    {
                        command.Parameters.Add(parameter.Key, System.Data.DbType.Binary).Value = parameter.Value;
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter();

                    adapter.InsertCommand = command;
                    adapter.InsertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                throw new ArgumentException("Query must start with an 'INSERT INTO' statement.", "query");
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
