namespace EpsilonLib.Settings
{
    public class SettingDefinition
    {
        public string Key { get; }
        public object DefaultValue { get; }

        public SettingDefinition(string key, object defaultValue)
        {
            Key = key;
            DefaultValue = defaultValue;
        }
    }
}
