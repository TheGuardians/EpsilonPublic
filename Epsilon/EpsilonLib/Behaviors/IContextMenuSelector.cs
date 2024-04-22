using EpsilonLib.Menus;
using System.Windows.Controls;

namespace EpsilonLib.Behaviors
{
    public interface IContextMenuSelector
    {
        MenuItemDefinition SelectMenu(object target, ContextMenuEventArgs e);
    }
}
