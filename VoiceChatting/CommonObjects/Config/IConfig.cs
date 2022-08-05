using System.Text.Json.Serialization;

namespace CommonObjects.Config
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
