using Epsilon.Commands;
using EpsilonLib.Menus;
using EpsilonLib.Shell;

namespace WpfApp20
{
    static class Menus
    {
        [ExportMenuItem]
        public static MenuItemDefinition GarbageCollectMenuItem = new CommandMenuItemDefinition<GarbageCollectCommand>(StandardMenus.ToolsMenu, null);

        [ExportMenuItem]
        public static MenuItemDefinition ToolsOptionsMenuItem = new CommandMenuItemDefinition<ShowOptionsCommand>(StandardMenus.ToolsMenu, null);

        [ExportMenuItem]
        public static MenuItemDefinition HelpAboutMenuItem = new CommandMenuItemDefinition<ShowAboutCommand>(StandardMenus.HelpMenu, null);
    }
}
