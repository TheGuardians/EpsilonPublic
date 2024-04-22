using EpsilonLib.Menus;
using EpsilonLib.Shell;
using ModPackagePlugin.Commands;

namespace ModPackagePlugin
{
    public static class MenuDefinitions
    {
        [ExportMenuItem]
        public static MenuItemDefinition ModPackageMenu = new MenuItemDefinition(StandardMenus.MainMenu, null, "Mod Package", placeAfter: () => StandardMenus.ViewMenu);

        [ExportMenuItem]
        public static MenuItemDefinition CacheSelectionMenu = new CommandMenuItemDefinition<TagCacheCommandList>(ModPackageMenu, null);

        [ExportMenuItem]
        public static MenuItemDefinition ReloadModPackageMenuItem = new CommandMenuItemDefinition<ReloadModPackageCommand>(ModPackageMenu, null);

        [ExportMenuItem]
        public static MenuItemDefinition NewModPackageMenuItem = new CommandMenuItemDefinition<NewModPackageCommand>(StandardMenus.FileNewMenu, null);
    }
}
