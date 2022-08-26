using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;

namespace VoiceChattingManagementServer.Core.Database
{
    public class DBConnectionPoolIndex
    {
        /// <summary>
        /// Check database is connected
        /// </summary>
        public bool IsConnected {
            get
            {
                if (connection == null) return false;
                return connection.State == System.Data.ConnectionState.Open;
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
        private bool ConnectToDatabase(string serverAddress, string databaseName, string userName, string password)
        {
            if (string.IsNullOrEmpty(serverAddress)) throw new ArgumentException("Database server address cannot be null or empty");
            if (string.IsNullOrEmpty(databaseName)) throw new ArgumentException("Database name cannot be null or empty");
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Database access username cannot be null or empty");

            // Create MariaDB conection message
            string connectionString = string.Format(
                "Server={0};Database={1};Uid={2};Pwd={3};",
                serverAddress, databaseName, userName, password);

            // Try connection
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception e)
            {
                // If failed to connection
                Debug.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public int ExecuteNonQuery(string query)
        {
            var command = new MySqlCommand(query);
            return command.ExecuteNonQuery();
        }

        public bool ExecuteQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0}: {1}", reader["Id"], reader["Name"]);
            }
            reader.Close();

            return true;
        }
    }
}
