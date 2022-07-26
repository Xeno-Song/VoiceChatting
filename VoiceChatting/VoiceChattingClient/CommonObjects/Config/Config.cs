using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VoiceChattingClient.CommonObjects.Config
{
    internal class Config : IConfig
    {
        [JsonIgnore]
        public string FilePath { get; set; }
        [JsonIgnore]
        public bool CreateIfNotExist { get; set; }

        [JsonIgnore]
        public object this[string key] {
            get {
                if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid key value");

                var propertyInfo = GetType().GetProperties().First(x => x.Name == key);
                if (propertyInfo == null) throw new ArgumentException("Cannot find key in properties");

                return propertyInfo.GetValue(this);
            }
            set {
                if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid key value");

                var propertyInfo = GetType().GetProperties().First(x => x.Name == key);
                if (propertyInfo == null) throw new ArgumentException("Cannot find key in properties");

                propertyInfo.SetValue(this, value);
            }
        }

        public bool Load()
        {
            if (File.Exists(FilePath) == false)
            {
                if (!CreateIfNotExist) return false;
                if (!Save()) return false;
                return true;
            }

            object data = JsonSerializer.Deserialize(File.ReadAllText(FilePath), GetType());
            PropertyInfo[] infos = GetType().GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.GetCustomAttribute<JsonIgnoreAttribute>() != null) continue;
                info.SetValue(this, info.GetValue(data, null), null);
            }

            return true;
        }

        public bool Save()
        {
            var baseDirectory = Directory.GetParent(FilePath);
            if (baseDirectory.Exists == false) return false;

            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, GetType(), serializerOptions));
            return true;
        }
    }
}
