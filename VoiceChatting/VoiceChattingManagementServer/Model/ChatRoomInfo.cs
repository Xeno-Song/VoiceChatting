using VoiceChattingManagementServer.Core.Database;
using VoiceChattingManagementServer.Core.Database.Attributes;

namespace VoiceChattingManagementServer.Model
{
    public class ChatRoomInfo : Entity
    {
        [Id]
        [AutoIncrement]
        [Column(name: "id")]
        public int? Id { get; set; }

        [Column(name: "uuid")]
        public string Uuid { get; set; }

        [Column(name: "name")]
        public string Name { get; set; }

        [Column(name: "host_address")]
        public string HostAddress { get; set; }

        [Column(name: "host_port")]
        public int? HostPort { get; set; }
    }
}
