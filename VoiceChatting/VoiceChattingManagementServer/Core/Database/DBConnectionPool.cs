using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static VoiceChattingManagementServer.Core.Database.PoolIndex.DatabaseConnectionPoolIndexFactory;

namespace VoiceChattingManagementServer.Core.Database
{
    public class DBConnectionPool
    {
        public string ServerAddress = "localhost";
        public string DatabaseName = "test";
        public string UserName = "root";
        public string Password = "12345678";
        public DBType DatabaseType = DBType.MariaDB;

        private List<IDatabaseConnectionPoolIndex> poolList;

        private bool CreateNewPoolIndex()
        {
            return true;
        }
    }
}
