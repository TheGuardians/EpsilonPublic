using EpsilonLib.Commands;
using EpsilonLib.Menus;
using EpsilonLib.Shell;
using System.Windows;

namespace CryingCatPlugin
{
    class MenuDefinitions
    {
        [ExportMenuItem]
        public static MenuItemDefinition CatMenu = new MenuItemDefinition(StandardMenus.MainMenu, null, "Cats", placeAfter: () => StandardMenus.ToolsMenu);

        [ExportMenuItem]
        public static MenuItemDefinition MyMenuItem = new CommandMenuItemDefinition<CatCommand>(CatMenu, null);
    }

    [ExportCommandHandler]
    class CatCommandHandler : ICommandHandler<CatCommand>
    {
        public void ExecuteCommand(Command command)
        {
            MessageBox.Show("gruntbirthday sounds");
        }

        public void UpdateCommand(Command command)
        {
            
        }
    }
}
