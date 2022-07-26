using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VoiceChattingClient.CommonObjects.Config
{
    internal interface IConfig
    {
        [JsonIgnore]
        string FilePath { get; set; }

        [JsonIgnore]
        bool CreateIfNotExist { get; set; }

        [JsonIgnore]
        object this[string key] { get; set; }

        bool Save();
        bool Load();
    }
}
