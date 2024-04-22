using System;

namespace EpsilonLib.Settings
{
    public interface IReadOnlySettingsCollection
    {
        event EventHandler<SettingChangedEventArgs> SettingChanged;

        object Get(Type type, string key, object defaultValue = null);
    }

    public interface ISettingsCollection : IReadOnlySettingsCollection
    {
        void Set(string key, object value);
    }

    public static class SettingsCollectionEx
    {
        public static T Get<T>(this ISettingsCollection collection, string key, T defaultValue = default)
        {
            return (T)collection.Get(typeof(T), key, defaultValue);
        }

        public static T Get<T>(this ISettingsCollection collection, SettingDefinition definition)
        {
            return (T)collection.Get(typeof(T), definition.Key, (T)definition.DefaultValue);
        }

        public static void Set<T>(this ISettingsCollection collection, string key, T value)
        {
            collection.Set(key, value);
        }
    }
}
