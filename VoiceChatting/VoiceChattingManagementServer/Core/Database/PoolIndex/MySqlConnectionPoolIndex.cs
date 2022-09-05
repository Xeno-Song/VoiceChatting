using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using VoiceChattingManagementServer.Core.Database.Attributes;

namespace VoiceChattingManagementServer.Core.Database.PoolIndex
{

    public class MySqlConnectionPoolIndex : DatabaseConnectionPoolIndex
    {
        /// <summary>
        /// Check database is connected
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                if (connection == null) return false;
                return connection.State == ConnectionState.Open;
            }
        }
        private MySqlConnection connection = null;

        /// <summary>
        /// Connect to database
        /// </summary>
        /// <param name="serverAddress">database server ip address</param>
        /// <param name="databaseName">database name to access</param>
        /// <param name="userName">access user id</param>
        /// <param name="password">access user password</param>
        /// <returns>If success return <see langword="true"/>, otherwise <see langword="false"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public override bool ConnectToDatabase(string serverAddress, string databaseName, string userName, string password)
        {
            lock (this)
            {
                try {

                    if (string.IsNullOrEmpty(serverAddress)) throw new ArgumentException("Database server address cannot be null or empty");
                    if (string.IsNullOrEmpty(databaseName)) throw new ArgumentException("Database name cannot be null or empty");
                    if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Database access username cannot be null or empty");

                    // Create MariaDB conection message
                    string connectionString = string.Format(
                        "Server={0};Database={1};Uid={2};Pwd={3};",
                        serverAddress, databaseName, userName, password);

                    // Try connection
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                }
                catch (Exception e)
                {
                    // If failed to connection
                    Debug.WriteLine(e.ToString());
                    return false;
                }
            }

            return true;
        }

        public override int ExecuteNonQuery(string query)
        {
            lock (this)
            {
                try
                {
                    if (!IsConnected)
                        throw new InvalidOperationException("Database wans't connected");

                    var command = new MySqlCommand(query);
                    return command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    return Int32.MinValue;
                }
            }
        }

        // public bool ExecuteQuery(string query)
        // {
        //     MySqlCommand command = new MySqlCommand(query, connection);
        //     MySqlDataReader reader = command.ExecuteReader();
        // 
        //     while (reader.Read())
        //     {
        //         Console.WriteLine("{0}: {1}", reader["Id"], reader["Name"]);
        //     }
        //     reader.Close();
        // 
        //     throw new NotImplementedException();
        // }

        public override List<Entity> ExcuteQuery<Entity>(string query)
        {
            DataSet dataSet = new DataSet();
            
            lock (this)
            {
                try
                {
                    if (!IsConnected)
                        throw new InvalidOperationException("Database wans't connected");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    adapter.Fill(dataSet);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to execute query : {query}");
                }
            }

            var propertyList = new List<PropertyInfo>(typeof(Entity).GetProperties());
            var sortedPropertyList = new List<Tuple<PropertyInfo, ColumnAttribute>>();

            if (dataSet.Tables.Count == 0) return null;
            if (dataSet.Tables[0].Rows.Count == 0) return null;
            if (dataSet.Tables[0].Rows[0].Table.Columns.Count == 0) return null;

            foreach (var index in dataSet.Tables[0].Rows[0].Table.Columns)
            {
                int propertyIndex = propertyList.FindIndex((info) =>
                {
                    var columnAtrribute = info.GetCustomAttribute<ColumnAttribute>();
                    if (columnAtrribute != null && columnAtrribute.Name == index.ToString())
                        return true;

                    return false;
                });

                if (propertyIndex == -1) sortedPropertyList.Add(null);
                else sortedPropertyList.Add(new Tuple<PropertyInfo, ColumnAttribute>(
                    propertyList[propertyIndex],
                    propertyList[propertyIndex].GetCustomAttribute<ColumnAttribute>()));
            }

            List<Entity> entityList = new List<Entity>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Entity value = new Entity();

                foreach (var propertyInfo in sortedPropertyList)
                    propertyInfo.Item1.SetValue(value, row[propertyInfo.Item2.Name]);

                entityList.Add(value);
            }

            return entityList;
        }

        public override void Dispose()
        {
            lock (this)
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
