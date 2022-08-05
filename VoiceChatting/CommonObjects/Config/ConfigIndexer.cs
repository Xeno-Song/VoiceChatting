using System;
using System.Collections.Generic;

namespace CommonObjects.Config
{
    internal class ConfigIndexer : IDisposable
    {
        public bool IsDisposed { get; set; }
        private Dictionary<string, IConfig> configDictionary;

        public IConfig this[string key]
        {
            get
            {
                if (IsDisposed) return null;

                if (configDictionary.ContainsKey(key) == false) return null;
                return configDictionary[key];
            }
            set
            {
                if (IsDisposed || value == null) return;

                if (configDictionary.ContainsKey(key))
                    configDictionary[key] = value;
                else configDictionary.Add(key, value);
            }
        }

        public ConfigIndexer()
        {
            configDictionary = new Dictionary<string, IConfig>();
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            IsDisposed = true;
            configDictionary.Clear();
            configDictionary = null;
        }
    }
}
