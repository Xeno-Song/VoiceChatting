using System;
using System.Collections.Generic;

namespace VoiceChattingManagementServer.Core.Database.PoolIndex
{
    public abstract class DatabaseConnectionPoolIndex : IDatabaseConnectionPoolIndex
    {
        public abstract bool IsConnected { get; }

        public bool IsUsing { get; private set; } = false;

        public void AcquireAccessPrivileges()
        {
            lock (this)
            {
                if (IsUsing) throw new InvalidOperationException("This database connection is using in other process");
                IsUsing = true;
            }
        }

        public void ReturnAccessPrivileges()
        {
            lock(this)
            {
                IsUsing = false;
            }
        }

        public abstract bool ConnectToDatabase(string serverAddress, string databaseName, string userName, string password);

        public abstract List<Entity> ExcuteQuery<Entity>(string query) where Entity : new();

        public abstract int ExecuteNonQuery(string query);
        public abstract void Dispose();
    }
}
