using System;

namespace EpsilonLib.Menus
{
    public class TextMenuItemDefinition : MenuItemDefinition
    {
        public TextMenuItemDefinition(object parent, object group, string text, Func<MenuItemDefinition> placeAfter = null)
        {
            Parent = parent;
            Group = group;
            Text = text;
            PlaceAfter = placeAfter;
        }
    }
}
