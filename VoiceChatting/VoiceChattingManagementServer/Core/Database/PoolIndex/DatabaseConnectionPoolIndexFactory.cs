using System;

namespace VoiceChattingManagementServer.Core.Database.PoolIndex
{
    public class DatabaseConnectionPoolIndexFactory
    {
        public enum DBType
        {
            MySQL,
            MariaDB,
        }

        public static IDatabaseConnectionPoolIndex Create(DBType type)
        {
            return type switch
            {
                DBType.MySQL => new MySqlConnectionPoolIndex(),
                _ => throw new ArgumentException($"Invalid database type. {((int)type).ToString()}"),
            };
        }
    }
}
