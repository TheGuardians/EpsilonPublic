using System.Collections.Generic;

namespace EpsilonLib.Menus
{
    public interface IMenuFactory
    {
        MenuItemViewModel GetMenu(MenuItemDefinition definition);
        IEnumerable<MenuItemViewModel> GetMenuBar(MenuBarDefinition definition);
    }
}
