namespace EpsilonLib.Settings
{
    public class SettingChangedEventArgs
    {
        public IReadOnlySettingsCollection Collection { get; }
        public string Key { get; }

        public SettingChangedEventArgs(IReadOnlySettingsCollection collection, string key)
        {
            Collection = collection;
            Key = key;
        }
    }
}
