using VoiceChattingManagementServer.Core.Database.Attributes;

namespace VoiceChattingManagementServer.Model
{
    public class ChatRoomInfo
    {
        [Id]
        [AutoIncrement]
        [Column(name: "id")]
        public int id;

        [Column(name: "uuid")]
        public string uuid;

        [Column(name: "name")]
        public string name;

        [Column(name: "host_address")]
        public string hostAddress;

        [Column(name: "host_port")]
        public int hostPort;
    }
}
