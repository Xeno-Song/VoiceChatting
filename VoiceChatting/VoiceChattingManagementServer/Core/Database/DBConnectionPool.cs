using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using VoiceChattingManagementServer.Core.Database.PoolIndex;

namespace VoiceChattingManagementServer.Core.Database
{
    public class DBConnectionPool : IDisposable
    {
        // Database configurations
        public string ServerAddress { get; set; } = "localhost";
        public string DatabaseName { get; set; } = "test";
        public string Username { get; set; } = "root";
        public string Password { get; set; } = "12345678";
        public DBType DatabaseType { get; set; } = DBType.MariaDB;

        // Pool configurations
        public int MinRedundency { get; set; } = 1;
        public int MaxRedundency { get; set; } = 8;

        private List<IDatabaseConnectionPoolIndex> poolList;
        private Task RedundencyCheckTask = null;

        public DBConnectionPool(string serverAddress, string databaseName, string username, string password, DBType databaseType)
            : this(serverAddress, databaseName, username, password, databaseType, 1, 8) { }

        public DBConnectionPool(string serverAddress, string databaseName, string username, string password, DBType databaseType, int redundencyConnections, int redundencyMax)
        {
            ServerAddress = serverAddress;
            DatabaseName = databaseName;
            Username = username;
            Password = password;
            DatabaseType = databaseType;
            MinRedundency = redundencyConnections;
            MaxRedundency = redundencyMax;
            poolList = new List<IDatabaseConnectionPoolIndex>();

            InitializeConnectionPool();
        }

        private void InitializeConnectionPool()
        {
            if (!CreateNewPoolIndex(MinRedundency))
            {
                throw new ArgumentException(
                    $"Could not connect to database." +
                    $"[server: {ServerAddress}, database: {DatabaseName}, user: {Username}, pw: {Password}, type: {DatabaseType.ToString()}]");
            }
        }

        public IDatabaseConnectionPoolIndex GetConnection()
        {
            int acquiredIndex = -1;

            lock (this)
            {
                acquiredIndex = poolList.FindIndex((poolIndex) => !poolIndex.IsUsing);
                poolList[acquiredIndex].AcquireAccessPrivileges();
            }

            return poolList[acquiredIndex];
        }

        private bool CreateNewPoolIndex(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                if (!CreateNewPoolIndex())
                {
                    Debug.WriteLine(
                        $"Could not connect to database." +
                        $"[server: {ServerAddress}, database: {DatabaseName}, user: {Username}, pw: {Password}, type: {DatabaseType.ToString()}]");

                    return false;
                }
            }

            return true;
        }

        private bool CreateNewPoolIndex()
        {
            var databaseConnection = DatabaseConnectionPoolIndexFactory.Create(DatabaseType);
            if (databaseConnection.ConnectToDatabase(ServerAddress, DatabaseName, Username, Password))
            {
                return false;
            }

            lock (this)
            {
                poolList.Add(databaseConnection);
            }

            if (RedundencyCheckTask == null || RedundencyCheckTask.IsCompleted)
            {
                RedundencyCheckTask = new Task(() => CheckRedundencyConnections());
            }

            return true;
        }

        private bool RemoveUnusingPoolIndex(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                if (!RemoveUnusingPoolIndex())
                    return false;
            }

            return true;
        }

        private bool RemoveUnusingPoolIndex()
        {
            IDatabaseConnectionPoolIndex poolIndex = null;

            lock(this)
            {
                var index = poolList.FindIndex((index) => !index.IsUsing);
                if (index != -1)
                {
                    poolIndex = poolList[index];
                    poolList.RemoveAt(index);
                }
            }

            if (poolIndex == null) return false;

            // TODO. pool index disconnection
            // poolIndex

            return true;
        }

        private void CheckRedundencyConnections()
        {
            lock (this)
            {
                int currentRedundency = poolList.FindAll((connection) => !connection.IsUsing).Count;
                if (currentRedundency < MinRedundency)
                {
                    if (!CreateNewPoolIndex(MinRedundency - currentRedundency))
                        throw new ArgumentException(
                            $"Could not connect to database." +
                            $"[server: {ServerAddress}, database: {DatabaseName}, user: {Username}, pw: {Password}, type: {DatabaseType.ToString()}]");
                }
                else if (currentRedundency > MaxRedundency)
                {
                    if (!RemoveUnusingPoolIndex(currentRedundency - MaxRedundency))
                    {
                        throw new InvalidOperationException(
                            $"Could not close database connection because all connection is using");
                    }
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
