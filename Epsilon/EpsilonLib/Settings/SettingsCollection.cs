using Newtonsoft.Json.Linq;
using System;

namespace EpsilonLib.Settings
{
    class SettingsCollection : ISettingsCollection
    {
        private JToken _node;

        public JObject Node => (JObject)_node;

        private SettingsService _service;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public SettingsCollection(SettingsService service, JToken node)
        {
            _service = service;
            _node = node;
        }

      
        public void Set(string key, object value)
        {
            _node[key] = JToken.FromObject(value, SettingsService.Serializer);
            SettingChanged?.Invoke(this, new SettingChangedEventArgs(this, key));
            _service.NotifySettingChanged(this, key);
        }

        public object Get(Type type, string key, object defaultValue = null)
        {
            return _node[key]?.ToObject(type, SettingsService.Serializer) ?? defaultValue;
        }

        public SettingsCollection GetCollection(string key)
        {
            var node = _node[key] ?? (_node[key] = new JObject());
            return new SettingsCollection(_service, node);
        }
    }
}
