using EpsilonLib.Commands;
using System;

namespace EpsilonLib.Menus
{
    public abstract class CommandMenuItemDefinition : MenuItemDefinition
    {
        public Type CommandType { get; set; }

        public CommandMenuItemDefinition(
            Type commandType,
            object parent,
            object group,
            Func<MenuItemDefinition> placeAfter = null)
        {
            CommandType = commandType;
            Parent = parent;
            Group = group;
            PlaceAfter = placeAfter;
        }
    }

    public class CommandMenuItemDefinition<T> : CommandMenuItemDefinition where T : CommandDefinition
    {
        public CommandMenuItemDefinition(
            object parent, 
            object group, 
            Func<MenuItemDefinition> placeAfter = null)  : base(typeof(T), parent, group, placeAfter)
        { 
        }
    }
}
