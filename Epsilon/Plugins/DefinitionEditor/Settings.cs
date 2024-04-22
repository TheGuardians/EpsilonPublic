using EpsilonLib.Settings;

namespace DefinitionEditor
{
    public static class Settings
    {
        public const string CollectionKey = "DefinitionEditor";

        public static SettingDefinition DisplayFieldTypesSetting = new SettingDefinition("DisplayFieldTypes", false);
        public static SettingDefinition DisplayFieldOffsetsSetting = new SettingDefinition("DisplayFieldOffsets", false);
        public static SettingDefinition CollapseBlocksSetting = new SettingDefinition("CollapseBlocks", false);
    }
}
