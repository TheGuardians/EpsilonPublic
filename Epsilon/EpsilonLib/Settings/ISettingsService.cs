using System;

namespace EpsilonLib.Settings
{
    public interface ISettingsService
    {
        event EventHandler<SettingChangedEventArgs> SettingChanged;

        ISettingsCollection GetCollection(string key);
    }
}
