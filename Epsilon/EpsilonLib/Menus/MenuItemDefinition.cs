using System;

namespace EpsilonLib.Menus
{
    public class MenuItemDefinition
    {
        public string Text { get; protected set; }
        public object Parent { get; protected set; }
        public object Group { get; protected set; }
        public Func<MenuItemDefinition> PlaceAfter { get; protected set; }

        public MenuItemDefinition() { }

        public MenuItemDefinition(
            object parent, 
            object group, 
            string text, 
            Func<MenuItemDefinition> placeAfter = null) 
        {
            Parent = parent;
            Group = group;
            Text = text;
            PlaceAfter = placeAfter;
        }
    }
}
