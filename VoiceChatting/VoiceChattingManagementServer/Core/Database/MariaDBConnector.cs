using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;

namespace VoiceChattingManagementServer.Core.Database
{
    public class DBConnectionPool
    {
        public static string ServerAddress = "localhost";
        public static string DatabaseName = "test";
        public static string UserName = "root";
        public static string Password = "12345678";

        private bool CreateNewPoolIndex()
        {
            string connectionString = string.Format(
                "Server={0};Database={1};Uid={2};Pwd={3};",
                ServerAddress, DatabaseName, UserName, Password);

            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        
    }
}
