using EpsilonLib.Menus;
using EpsilonLib.Shell;
using TagToolShellPlugin.Commands;

namespace TagToolShellPlugin
{
    public static class MenuDefinitions
    {
        [ExportMenuItem]
        public static MenuItemDefinition ShowShellWindowMenuItem = new CommandMenuItemDefinition<ShowShellWindowCommand>(StandardMenus.ViewMenu, "CacheEditor.Tools");
    }
}
