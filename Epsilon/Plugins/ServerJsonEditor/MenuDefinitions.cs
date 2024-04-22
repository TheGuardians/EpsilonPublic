using EpsilonLib.Menus;
using EpsilonLib.Shell;

namespace ServerJsonEditor
{
    static class MenuDefinitions
    {
        [ExportMenuItem]
        public static MenuItemDefinition ServerJsonEditorMenuItem = new CommandMenuItemDefinition<ShowServerJsonEditorCommand>(StandardMenus.ToolsMenu, null);
    }
}
