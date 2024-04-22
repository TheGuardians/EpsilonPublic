using EpsilonLib.Settings;

namespace Epsilon.Options
{
    public static class GeneralSettings
    {
        public const string CollectionKey = "General";

        public static SettingDefinition DefaultTagCacheSetting = new SettingDefinition("DefaultTagCache", "");
        public static SettingDefinition DefaultPakSetting = new SettingDefinition("DefaultModPackage", "");
        public static SettingDefinition StartupPositionLeftSetting = new SettingDefinition("StartupPositionLeft", "");
        public static SettingDefinition StartupPositionTopSetting = new SettingDefinition("StartupPositionTop", "");
        public static SettingDefinition StartupWidthSetting = new SettingDefinition("StartupWidth", "");
        public static SettingDefinition StartupHeightSetting = new SettingDefinition("StartupHeight", "");
        public static SettingDefinition AlwaysOnTopSetting = new SettingDefinition("AlwaysOnTop", "");
        public static SettingDefinition AccentColorSetting = new SettingDefinition("AccentColor", "#007ACC");
    }
}
