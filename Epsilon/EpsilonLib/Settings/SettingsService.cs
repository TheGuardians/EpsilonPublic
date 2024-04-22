using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.Composition;
using System.IO;

namespace EpsilonLib.Settings
{
    [Export(typeof(ISettingsService))]
    class SettingsService : ISettingsService
    {
        private const string FilePath = "settings.json";
        private SettingsCollection _rootCollection;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public SettingsService()
        {
            _rootCollection = new SettingsCollection(this, new JObject());
            Load(FilePath);
        }

        public readonly static JsonSerializer Serializer = JsonSerializer.CreateDefault();
        static SettingsService()
        {
            Serializer.Formatting = Formatting.Indented;
            Serializer.Converters.Add(new StringEnumConverter());
        }

        public ISettingsCollection GetCollection(string key)
        {
            return _rootCollection.GetCollection(key);
        }

        private void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            using (JsonReader reader = new JsonTextReader(File.OpenText(filePath)))
            {
                reader.Read();
                _rootCollection = new SettingsCollection(this, JObject.ReadFrom(reader));
            }
        }

        private void Save(string filePath)
        {
            using (JsonWriter writer = new JsonTextWriter(File.CreateText(filePath)))
            {
                writer.Formatting = Formatting.Indented;
                _rootCollection.Node.WriteTo(writer);
            }
               
        }

        internal void NotifySettingChanged(SettingsCollection collection, string key)
        {
            Save(FilePath);
            SettingChanged?.Invoke(this, new SettingChangedEventArgs(collection, key));
        }
    }
}
