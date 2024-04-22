using EpsilonLib.Settings;

namespace CacheEditor.Components.TagTree
{
    public static class Settings
    {
        public static SettingDefinition TagTreeViewModeSetting = new SettingDefinition("TagTreeViewMode", TagTreeViewMode.Groups);
        public static SettingDefinition TagTreeGroupDisplaySetting = new SettingDefinition("TagTreeGroupDisplayMode", TagTreeGroupDisplayMode.TagGroupName);
        public static SettingDefinition ShowTagGroupAltNamesSetting = new SettingDefinition("ShowTagGroupAltNames", false);
        public static SettingDefinition BaseCacheWarningsSetting = new SettingDefinition("BaseCacheWarnings", true);
    }
}
