using System.Collections.Generic;

namespace VoiceChattingManagementServer.Core.Database
{
    public interface IDatabaseConnectionPoolIndex
    {
        /// <summary>
        /// Check database is connected
        /// </summary>
        public bool IsConnected { get; }

        /// <summary>
        /// Check database is using in other job
        /// </summary>
        public bool IsUsing { get; }

        /// <summary>
        /// Get access privileges from db connection index
        /// </summary>
        public void AcquireAccessPrivileges();

        /// <summary>
        /// Return access privileges and make to usable state in other process
        /// </summary>
        public void ReturnAccessPrivileges();

        /// <summary>
        /// Connect to database
        /// </summary>
        /// <param name="serverAddress">database server ip address</param>
        /// <param name="databaseName">database name to access</param>
        /// <param name="userName">access user id</param>
        /// <param name="password">access user password</param>
        /// <returns>If success return <see langword="true"/>, otherwise <see langword="false"/></returns>
        /// <exception cref="ArgumentException"></exception>
        bool ConnectToDatabase(string serverAddress, string databaseName, string userName, string password);

        int ExecuteNonQuery(string query);

        // bool ExecuteQuery(string query);

        List<Entity> ExcuteQuery<Entity>(string query)
            where Entity : new();
    }
}
