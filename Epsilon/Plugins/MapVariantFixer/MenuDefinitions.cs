using EpsilonLib.Menus;
using EpsilonLib.Shell;

namespace MapVariantFixer
{
    static class MenuDefinitions
    {
        [ExportMenuItem]
        public static MenuItemDefinition MapVariantFixerMenuItem = new CommandMenuItemDefinition<ShowMapVariantFixerCommand>(StandardMenus.ToolsMenu, null);
    }
}
